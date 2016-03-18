using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LF2Dashboard
{
	class ComPlayer : Player
	{
		public ComPlayer(Process gameProcess, int px) : base(gameProcess, px)
		{
			GameProcess = gameProcess;
			playerAddress = BitConverter.ToInt32(
				Hookers.ReadMemory(GameProcess, AddressTable.Computer[px], 4), 0);
			Name = "Com" + (px+1);
		}
	}
}
