using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net.Mime;
using System.Windows.Forms;
using LF2Dashboard.ScoreBoard;

namespace LF2Dashboard.StatsPanel
{
	class ScoreBoard
	{
		public List<RealPlayer> Players;
		public TableLayoutPanel Panel;
		public TableLayoutPanel TopPanel;
		public TableLayoutPanel MidPanel;
		public TableLayoutPanel MidTopPanel;
		public TableLayoutPanel[] PlayerRowPanels;
		public TableLayoutPanel BotPanel;
		public TableLayoutPanel MidMotherPanel;
		public TimeSpan Time;
		public Label TimLabel;

		public ScoreBoard(TableLayoutPanel panel)
		{
			Time = TimeSpan.Zero;
			Panel = panel;
			Players = new List<RealPlayer>();
		}

		public void BuildTable()
		{
			RealPlayer ImaginaryPlayer = new RealPlayer();
			TopPanel = new TableLayoutPanel();
			MidPanel = new TableLayoutPanel();
			MidTopPanel = new TableLayoutPanel();
			PlayerRowPanels = new TableLayoutPanel[Players.Count];
			BotPanel = new TableLayoutPanel();
			MidMotherPanel = new TableLayoutPanel();

			List<TableLayoutPanel> Panels = new List<TableLayoutPanel>();
			Panels.Add(Panel);
			Panels.Add(TopPanel);
			Panels.Add(MidMotherPanel);
			Panels.Add(MidPanel);
			Panels.Add(MidTopPanel);
			for (int i = 0; i < PlayerRowPanels.Length; i++)
			{
				PlayerRowPanels[i] = new TableLayoutPanel();
				Panels.Add(PlayerRowPanels[i]);
			}
			Panels.Add(BotPanel);
			for (int i = 0; i < Panels.Count; i++)
			{
				Panels[i].BorderStyle = BorderStyle.None;
				Panels[i].Margin = Padding.Empty;
				Panels[i].Dock = DockStyle.Fill;
			}

			Panel.Controls.Clear();
			Panel.RowCount = 0;
			Panel.ColumnCount = 0;
			Panel.RowStyles.Clear();
			Panel.ColumnStyles.Clear();
			Panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 22));
			Panel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
			Panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 27));
			Panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
			Panel.MaximumSize = new Size(1000, 56 + Players.Count*45 + 27);

			MidMotherPanel.ColumnCount = 3;
			MidMotherPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			MidMotherPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 9));
			MidMotherPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,50));
			MidMotherPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 9));
			MidMotherPanel.Controls.Add(MidPanel, 1, 0);

			MidTopPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

			MidPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
			MidPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
			MidPanel.MaximumSize = new Size(1000, 394);


			Panel.Controls.Add(TopPanel, 0, 0);
			Panel.Controls.Add(MidMotherPanel, 0, 1);
			TopPanel.Controls.Add(new Label
			{
				Text = "Summary",
				Anchor = AnchorStyles.Left | AnchorStyles.Right,
				BackColor = Color.Transparent,
				ForeColor = Color.White,
				Font = new Font(new FontFamily("Arial"), 12, FontStyle.Bold),
				TextAlign = ContentAlignment.MiddleCenter
			});
			MidPanel.Controls.Add(MidTopPanel, 0, 0);


			BotPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize, 50));
			BotPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
			BotPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
			BotPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 71));
			BotPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 8));

			BotPanel.ColumnCount = 4;
			BotPanel.RowCount = 1;

			TimLabel = new Label
			{
				Anchor = AnchorStyles.Left | AnchorStyles.Right,
				BackColor = Color.Black,
				ForeColor = Color.White,
				TextAlign = ContentAlignment.MiddleCenter
			};
			TimLabel.Font = new Font(TimLabel.Font.FontFamily, 9, FontStyle.Bold);

			BotPanel.Controls.Add(TimLabel, 2, 0);
			BotPanel.Controls.Add(new Label
			{
				Text = "Time: ",
				Anchor = AnchorStyles.Left | AnchorStyles.Right,
				BackColor = Color.Transparent,
				ForeColor = Color.White,
				TextAlign = ContentAlignment.MiddleRight
			}, 1, 0);

			LinkLabel lable = new LinkLabel()
			{
				Text = "http://www.lf-empire.de/forum/",
				Links =
				{
					new LinkLabel.Link()
					{LinkData = "http://www.lf-empire.de/forum/"}
				},

				Anchor = AnchorStyles.Left | AnchorStyles.Right,
				BackColor = Color.Transparent,
				ForeColor = Color.White,
				TextAlign = ContentAlignment.MiddleLeft,
				ActiveLinkColor = Color.White,
				VisitedLinkColor = Color.White,
				LinkColor = Color.White,
				LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
		};
			lable.LinkClicked += LableOnLinkClicked;

			BotPanel.Controls.Add(lable, 0, 0);

			ImaginaryPlayer.ImaginaryName.Text = "Player";
			ImaginaryPlayer.Kill.Text = "Kill";
			ImaginaryPlayer.Attack.Text = "Attack";
			ImaginaryPlayer.HpLost.Text = "HP Lost";
			ImaginaryPlayer.MpUsage.Text = "MP Usage";
			ImaginaryPlayer.Picking.Text = "Picking";
			ImaginaryPlayer.Status.Text = "Status";

			var ImaginLabels = new List<Label>();

			ImaginLabels.Add(ImaginaryPlayer.ImaginaryName);
			ImaginLabels.Add(ImaginaryPlayer.Kill);
			ImaginLabels.Add(ImaginaryPlayer.Attack);
			ImaginLabels.Add(ImaginaryPlayer.HpLost);
			ImaginLabels.Add(ImaginaryPlayer.MpUsage);
			ImaginLabels.Add(ImaginaryPlayer.Picking);
			ImaginLabels.Add(ImaginaryPlayer.Status);

			for (int i = 0; i < ImaginLabels.Count; i++)
			{
				ImaginLabels[i].BackColor = Color.Transparent;
				ImaginLabels[i].Font = new Font(new FontFamily("Arial"),10);
				ImaginLabels[i].AutoSize = false;
				ImaginLabels[i].MaximumSize = new Size(500,18);
			}

			ImaginaryPlayer.Kill.ForeColor = ImaginaryPlayer.MightyColors[4];
			ImaginaryPlayer.Attack.ForeColor = ImaginaryPlayer.MightyColors[4];
			ImaginaryPlayer.HpLost.ForeColor = ImaginaryPlayer.MightyColors[5];
			ImaginaryPlayer.MpUsage.ForeColor = ImaginaryPlayer.MightyColors[5];
			ImaginaryPlayer.Picking.ForeColor = ImaginaryPlayer.MightyColors[6];
			ImaginaryPlayer.Status.ForeColor = ImaginaryPlayer.MightyColors[7];

			MidTopPanel.ColumnCount = 7;
			MidTopPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 19));
			MidTopPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13));
			MidTopPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13));
			MidTopPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13));
			MidTopPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13));
			MidTopPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13));
			MidTopPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12));

			int col = 0;
			MidTopPanel.Controls.Add(ImaginaryPlayer.ImaginaryName, col, 0);
			col++;
			MidTopPanel.Controls.Add(ImaginaryPlayer.Kill, col, 0);
			col++;
			MidTopPanel.Controls.Add(ImaginaryPlayer.Attack, col, 0);
			col++;
			MidTopPanel.Controls.Add(ImaginaryPlayer.HpLost, col, 0);
			col++;
			MidTopPanel.Controls.Add(ImaginaryPlayer.MpUsage, col, 0);
			col++;
			MidTopPanel.Controls.Add(ImaginaryPlayer.Picking, col, 0);
			col++;
			MidTopPanel.Controls.Add(ImaginaryPlayer.Status, col, 0);


			for (int row = 0; row < Players.Count; row++)
			{
				col = 0;
				PlayerRowPanels[row].Controls.Add(Players[row].Picture, col, 0);
				col++;
				PlayerRowPanels[row].Controls.Add(Players[row].Name, col, 0);
				col++;
				PlayerRowPanels[row].Controls.Add(Players[row].Kill, col, 0);
				col++;
				PlayerRowPanels[row].Controls.Add(Players[row].ImaginaryGray1, col, 0);
				col++;
				PlayerRowPanels[row].Controls.Add(Players[row].Attack,col, 0);
				col++;
				PlayerRowPanels[row].Controls.Add(Players[row].ImaginaryGray2, col, 0);
				col++;
				PlayerRowPanels[row].Controls.Add(Players[row].HpLost, col, 0);
				col++;
				PlayerRowPanels[row].Controls.Add(Players[row].ImaginaryGray3, col, 0);
				col++;
				PlayerRowPanels[row].Controls.Add(Players[row].MpUsage, col, 0);
				col++;
				PlayerRowPanels[row].Controls.Add(Players[row].ImaginaryGray4, col, 0);
				col++;
				PlayerRowPanels[row].Controls.Add(Players[row].Picking, col, 0);
				col++;
				PlayerRowPanels[row].Controls.Add(Players[row].ImaginaryGray5, col, 0);
				col++;
				PlayerRowPanels[row].Controls.Add(Players[row].Status, col, 0);

				PlayerRowPanels[row].ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9));
				PlayerRowPanels[row].ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10));

				PlayerRowPanels[row].ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12));
				PlayerRowPanels[row].ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
				PlayerRowPanels[row].ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12));
				PlayerRowPanels[row].ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
				PlayerRowPanels[row].ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12));
				PlayerRowPanels[row].ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
				PlayerRowPanels[row].ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12));
				PlayerRowPanels[row].ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
				PlayerRowPanels[row].ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12));
				PlayerRowPanels[row].ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
				PlayerRowPanels[row].ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12));

				PlayerRowPanels[row].Dock = DockStyle.Fill;
				MidPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
				MidPanel.Controls.Add(PlayerRowPanels[row], 0, row + 1);
			}



			Panel.Controls.Add(BotPanel, 0, 2);

			MidMotherPanel.CellPaint += MidPanelOnCellPaint;
			MidPanel.CellPaint += MidPanelOnCellPaint;
			MidTopPanel.CellPaint += MidPanelOnCellPaint;
			for (int i = 0; i < Players.Count; i++)
			{
				PlayerRowPanels[i].CellPaint += MidPanelOnCellPaint;
			}

			TopPanel.CellPaint += TopPanelOnCellPaint;
			BotPanel.CellPaint += TopPanelOnCellPaint;



			var wow = 1;




		}

		private void LableOnLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(e.Link.LinkData as string);
		}

		private void TopPanelOnCellPaint(object sender, TableLayoutCellPaintEventArgs e)
		{
			Color TopColor = Color.FromArgb(255, 17, 40, 108);
			using (var b = new SolidBrush(TopColor))
			{
				e.Graphics.FillRectangle(b, e.CellBounds);
			}
		}

		private void MidPanelOnCellPaint(object sender, TableLayoutCellPaintEventArgs e)
		{
			Color MidColor = Color.FromArgb(255, 50, 77, 154);
			using (var b = new SolidBrush(MidColor))
			{
				e.Graphics.FillRectangle(b, e.CellBounds);
			}
		}


		public void Update(TimeSpan time)
		{
			Time = time;
			TimLabel.Text = Time.ToString();
			foreach (var realPlayer in Players)
			{
				realPlayer.Update();
			}
		}
	}
}
