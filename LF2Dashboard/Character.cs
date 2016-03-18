using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LF2Dashboard
{
	class Character
	{
		public string Name;
		public Bitmap Pic;
		public int Address;

		public Character(string name, Bitmap pic, int address)
		{
			Name = name;
			Pic = pic;
			Address = address;
		}

		public Character()
		{
		}
	}
}
