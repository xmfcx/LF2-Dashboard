﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LF2Dashboard
{
	public partial class FormMain : Form
	{
		public FormMain()
		{
			InitializeComponent();
		}
		private Game game;
		private bool lf2IsOn;

		private void FormMain_Load(object sender, EventArgs e)
		{
			BackColor = Color.Black;
			timerWorker.Enabled = true;
			timerWorker.Interval = 100;
			AddressTable.SetTable();

			Restart();
		}

		private void timerWorker_Tick(object sender, EventArgs e)
		{
			CheckLf2();
			if (lf2IsOn)
			{
				game.Update();
				if (game.GameIsOn)
				{
				}
				else
				{
				}
			}
			else
			{
				Restart();
			}
		}
		
		

		void Restart()
		{
			Regex regex = new Regex(@"lf2(.*?)");
			foreach (Process p in Process.GetProcesses("."))
			{
				if (regex.Match(p.ProcessName).Success)
				{
					game = new Game(p, tableLayoutPanel1, this);
					lf2IsOn = true;
					break;
				}
				lf2IsOn = false;
			}
		}

		private void CheckLf2()
		{
			Regex regex = new Regex(@"lf2(.*?)");
			bool thereIsNoLf2 = true;
			foreach (Process p in Process.GetProcesses("."))
			{
				if (regex.Match(p.ProcessName).Success)
				{
					thereIsNoLf2 = false;
					break;
				}
			}
			if (thereIsNoLf2)
			{
				lf2IsOn = false;
			}
		}

		private void FormMain_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (lf2IsOn)
			{
				if (e.KeyChar.ToString().ToLower() == "s")
				{
					if (game.GameIsOn)
					{
						if (game.Board.StreamIsOn)
						{
							game.StreamIsOn = false;
							game.Board.StreamIsOn = false;
							Size = new Size(573, Size.Height);
						}
						else
						{
							game.StreamIsOn = true;
							game.Board.StreamIsOn = true;
							Size = new Size(387, Size.Height);
							Console.Write("IT'S ON!");
						}
						game.Board.StreamIsOnChanged = true;
					}
				}
			}
		}
	}
}
