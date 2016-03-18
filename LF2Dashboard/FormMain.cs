using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
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
			var lf2Process = Process.GetProcessesByName("lf2").FirstOrDefault();
			if (lf2Process != null)
			{
				game = new Game(lf2Process, tableLayoutPanel1);
				lf2IsOn = true;
			}
			else
			{
				lf2IsOn = false;
			}
		}

		private void CheckLf2()
		{
			var lf2Process = Process.GetProcessesByName("lf2").FirstOrDefault();
			if (lf2Process == null)
			{
				lf2IsOn = false;
			}
		}
	}
}
