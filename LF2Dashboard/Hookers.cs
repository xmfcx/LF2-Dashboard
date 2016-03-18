using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace LF2Dashboard
{
	internal class Hookers
	{
		#region Keyboard Hook

		public const int WH_KEYBOARD_LL = 13;
		public const int WM_KEYDOWN = 0x0100;
		public const int WM_KEYUP = 0x0101;
		public static LowLevelKeyboardProc suchProc = HookCallback;
		public static IntPtr keyboardHook = IntPtr.Zero;

		public static IntPtr SetHook(LowLevelKeyboardProc proc)
		{
			using (Process curProcess = Process.GetCurrentProcess())
			using (ProcessModule curModule = curProcess.MainModule)
			{
				return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
					GetModuleHandle(curModule.ModuleName), 0);
			}
		}

		public delegate IntPtr LowLevelKeyboardProc(
			int nCode, IntPtr keyState, IntPtr lParam);

		public static IntPtr HookCallback(
			int nCode, IntPtr keyState, IntPtr lParam)
		{
			if (nCode >= 0 && keyState == (IntPtr) WM_KEYDOWN)
			{
				IntPtr hwnd2 = GetForegroundWindow();
				StringBuilder windowtitle = new StringBuilder(256);
				if (GetWindowText(hwnd2, windowtitle, windowtitle.Capacity) > 0)
					if ((windowtitle.ToString() == "Little Fighter 2"))
					{
						int vkCode = Marshal.ReadInt32(lParam);
					}
			}
			if (nCode >= 0 && keyState == (IntPtr) WM_KEYUP)
			{
				IntPtr hwnd2 = GetForegroundWindow();
				StringBuilder windowtitle = new StringBuilder(256);
				if (GetWindowText(hwnd2, windowtitle, windowtitle.Capacity) > 0)
					if ((windowtitle.ToString() == "Little Fighter 2"))
					{
						int vkCode = Marshal.ReadInt32(lParam);
					}
			}
			return CallNextHookEx(keyboardHook, nCode, keyState, lParam);
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr SetWindowsHookEx(int idHook,
			LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
			IntPtr wParam, IntPtr lParam);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetModuleHandle(string lpModuleName);

		#endregion

		#region Window Title Returner

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString,
			int nMaxCount);

		#endregion

		#region AsyncKeys

		[DllImport("user32.dll")]
		public static extern short GetAsyncKeyState(Keys vKey);

		#endregion

		#region Memory Writer Main

		//EditLf2Mem[Flags]
		public enum ProcessAccessFlags : uint
		{
			All = 0x001F0FFF,
		}

		[DllImport("kernel32.dll")]
		public static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess,
			[MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize,
			out int lpNumberOfBytesWritten);

		[DllImport("kernel32.dll")]
		public static extern Int32 CloseHandle(IntPtr hProcess);

		#region Memory Writer Functions

		public static void WriteMem(Process p, int address, long v)
		{
			var hProcWow = OpenProcess(ProcessAccessFlags.All, false, p.Id);
			var val = new[] {(byte) v};
			int wtf;
			WriteProcessMemory(hProcWow, new IntPtr(address), val, (UInt32) val.LongLength, out wtf);
			CloseHandle(hProcWow);
		}

		public static void WriteMemWow(Process p, int address, long v, int length)
		{
			var hProcWow = OpenProcess(ProcessAccessFlags.All, false, p.Id);
			var val = new[] {(byte) v};
			int wtf;
			WriteProcessMemory(hProcWow, new IntPtr(address), val, (UInt32) length, out wtf);
			CloseHandle(hProcWow);
		}

		//Better Memory Writer
		public static void SetPort(IntPtr gameHandle, IntPtr writeAddress, int i)
		{
			var array = BitConverter.GetBytes(i);
			int bytesWritten;
			WriteProcessMemory(gameHandle, writeAddress, array, (uint) array.Length, out bytesWritten);
		}

		#endregion

		#endregion

		#region Suspender

		[Flags]
		public enum ThreadAccess
		{
			SUSPEND_RESUME = (0x0002),
		}

		[DllImport("kernel32.dll")]
		private static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

		[DllImport("kernel32.dll")]
		private static extern uint SuspendThread(IntPtr hThread);

		[DllImport("kernel32.dll")]
		private static extern int ResumeThread(IntPtr hThread);

		public static void SuspendProcess(Process process)
		{
			if (process.ProcessName == string.Empty)
				return;
			foreach (ProcessThread pT in process.Threads)
			{
				IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint) pT.Id);
				if (pOpenThread == IntPtr.Zero)
				{
					continue;
				}
				SuspendThread(pOpenThread);
				CloseHandle(pOpenThread);
			}
		}

		public static void ResumeProcess(Process process)
		{
			if (process.ProcessName == string.Empty)
				return;
			foreach (ProcessThread pT in process.Threads)
			{
				IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint) pT.Id);
				if (pOpenThread == IntPtr.Zero)
				{
					continue;
				}
				int suspendCount;
				do
				{
					suspendCount = ResumeThread(pOpenThread);
				} while (suspendCount > 0);
				CloseHandle(pOpenThread);
			}
		}

		#endregion

		#region Freezer

		[DllImport("user32.dll")]
		public static extern bool BlockInput(bool block);

		public static void FreezeMouse()
		{
			BlockInput(true);
		}

		public static void ThawMouse()
		{
			BlockInput(false);
		}

		#endregion

		#region key-mous Simulation

		[DllImport("user32.dll")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		[DllImport("user32.dll")]
		public static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

		[DllImport("user32.dll")]
		internal static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray), In] Input[] pInputs, int cbSize);

		internal struct Input
		{
			public UInt32 Type;
			public MOUSEKEYBDHARDWAREINPUT Data;
		}

		[StructLayout(LayoutKind.Explicit)]
		internal struct MOUSEKEYBDHARDWAREINPUT
		{
			[FieldOffset(0)] public MOUSEINPUT Mouse;
			[FieldOffset(0)] public KEYBDINPUT Keyb;
		}

		internal struct KEYBDINPUT
		{
			public int wVk;
			public int wScan;
			public int dwFlags;
			public int time;
			public IntPtr dwExtraInfo;
		}

		internal struct MOUSEINPUT
		{
			public Int32 X;
			public Int32 Y;
			public UInt32 MouseData;
			public UInt32 Flags;
			public UInt32 Time;
			public IntPtr ExtraInfo;
		}

		public static void ClickOnPoint(IntPtr wndHandle, Point clientPoint)
		{
			var oldPos = Cursor.Position;
			// get screen coordinates
			ClientToScreen(wndHandle, ref clientPoint);
			// set cursor on coords, and press mouse
			Cursor.Position = new Point(clientPoint.X, clientPoint.Y);
			var inputMouseDown = new Input();
			inputMouseDown.Type = 0; // input type mouse
			inputMouseDown.Data.Mouse.Flags = 0x0002; // left button down
			var inputMouseUp = new Input();
			inputMouseUp.Type = 0; // input type mouse
			inputMouseUp.Data.Mouse.Flags = 0x0004; // left button up
			Cursor.Position = new Point(clientPoint.X, clientPoint.Y);
			var inputs = new[] {inputMouseDown};
			SendInput((uint) inputs.Length, inputs, Marshal.SizeOf(typeof (Input)));
			// return mouse
			Cursor.Position = oldPos;
		}

		#endregion




		public static byte[] ReadMemory(Process p, int address, uint size)
		{
			//var hProcWow = OpenProcess(ProcessAccessFlags.All, true, p.Id);
			var hProcWow = OpenProcess((uint)(0x0010), 1, (uint)p.Id);

			byte[] buffer = new byte[size];
			IntPtr bytesRead;
			ReadProcessMemory(hProcWow, new IntPtr(address), buffer, size, out bytesRead);
			return buffer;
		}
		


		[DllImport("kernel32.dll")]
		public static extern IntPtr OpenProcess(UInt32 dwDesiredAccess, Int32 bInheritHandle, UInt32 dwProcessId);

		[DllImport("kernel32.dll")]
		public static extern Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer,
			UInt32 size, out IntPtr lpNumberOfBytesRead);


	}
}