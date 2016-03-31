using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace LF2Dashboard
{
	internal class Hooker
	{
		public static byte[] ReadMemory(Process p, int address, uint size)
		{
			//var hProcWow = OpenProcess(ProcessAccessFlags.All, true, p.Id);
			var hProcWow = OpenProcess(0x0010, 1, (uint)p.Id);

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