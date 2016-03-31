using System;
using System.Diagnostics;

namespace LF2Dashboard
{
	class ComPlayer : Player
	{
		public ComPlayer(Process gameProcess, int px) : base(gameProcess, px)
		{
			GameProcess = gameProcess;
			PlayerAddress = BitConverter.ToInt32(
				Hooker.ReadMemory(GameProcess, AddressTable.Computer[px], 4), 0);
			Name = "Com" + (px+1);
		}
	}
}
