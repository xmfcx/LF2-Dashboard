using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LF2Dashboard
{
	static class AddressTable
	{
		public static readonly int Kills = 0x358;
		public static readonly int Attack = 0x348;
		public static readonly int HpLost = 0x34C;
		public static readonly int MpUsage = 0x350;
		public static readonly int Picking = 0x35C;
		public static readonly int Owner = 0x354;
		public static readonly int Enemy = 0x360;
		public static readonly int Team = 0x364;
		public static readonly int Invincible = 0x8;
		public static readonly int Hp = 0x2FC;
		public static readonly int HpDark = 0x300;
		public static readonly int HpMax = 0x304;
		public static readonly int Mp = 0x308;
		public static readonly int PDataPointer = 0x368;

		public static readonly int DataPointer = 0x4592D4;

		public static int[] Player;
		public static int[] Computer;
		public static int[] Names;
		public static int[] ActivePlayers;
		public static int[] SelectedPlayers;
		public static int[] PlayerInGame;
		public static int[] CPlayerInGame;
		public static int[] DataFiles;

		public static readonly int GameState = 0x44D020;
		public static readonly int Time = 0x450BBC;
		public static readonly int TotalTime = 0x450B8C;

		public static void SetTable()
		{
			Player = new int[8];
			Computer = new int[8];
			Names = new int[11];
			ActivePlayers = new int[8];
			SelectedPlayers = new int[8];
			PlayerInGame = new int[8];
			CPlayerInGame = new int[8];
			DataFiles = new int[65];
			for (int i = 0; i < 8; i++)
			{
				Player[i] = 0x458C94 + i * 4;
				Computer[i] = 0x458CBC + i * 4;
				ActivePlayers[i] = 0x458B04 + i;
				SelectedPlayers[i] = 0x451288 + i*4;
				PlayerInGame[i] = 0x458B04 + i;
				CPlayerInGame[i] = 0x458B0E + i;
			}
			for (int i = 0; i < 11; i++)
			{
				Names[i] = 0x44FCC0 + i * 11;
			}
		}

		public static void FillPointers(Process gameProcess)
		{
			int dataAddress = BitConverter.ToInt32(
				Hookers.ReadMemory(gameProcess, DataPointer, 4), 0);
			for (int i = 0; i < DataFiles.Length; i++)
			{
				DataFiles[i] = BitConverter.ToInt32(
				Hookers.ReadMemory(gameProcess, dataAddress + i * 4, 4), 0);
			}
		}
	}
}
