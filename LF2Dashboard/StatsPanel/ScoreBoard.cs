using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using LF2Dashboard.Properties;

namespace LF2Dashboard.StatsPanel
{
	class ScoreBoard
	{
		public bool StreamIsOn;
		public bool StreamIsOnChanged;
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
		public Dictionary<string, int> MuhDic;
		private LinkLabel lable;
		private RealPlayer imaginaryPlayer;
		public Form MotherForm;

		public ScoreBoard(TableLayoutPanel panel, Form form)
		{
			MotherForm = form;
			Time = TimeSpan.Zero;
			Panel = panel;
			Players = new List<RealPlayer>();
		}

		public void BuildTable()
		{
			MuhDic = new Dictionary<string, int>();
			MuhDic.Add("PanelRow1Size", 22);
			MuhDic.Add("PlayerRowSize", 50);
			MuhDic.Add("TitlesRowSize", 34);
			MuhDic.Add("MidMotherWidth", 8);
			MuhDic.Add("maxHeight", 120 + Players.Count*45);


			imaginaryPlayer = new RealPlayer();
			TopPanel = new TableLayoutPanel();
			MidPanel = new TableLayoutPanel();
			MidTopPanel = new TableLayoutPanel();
			PlayerRowPanels = new TableLayoutPanel[Players.Count];
			BotPanel = new TableLayoutPanel();
			MidMotherPanel = new TableLayoutPanel();

			Panel.MaximumSize = new Size(1000, MuhDic["maxHeight"]);
			List<TableLayoutPanel> panels = new List<TableLayoutPanel>();
			panels.Add(Panel);
			panels.Add(TopPanel);
			panels.Add(MidMotherPanel);
			panels.Add(MidPanel);
			panels.Add(MidTopPanel);
			for (int i = 0; i < PlayerRowPanels.Length; i++)
			{
				PlayerRowPanels[i] = new TableLayoutPanel();
				panels.Add(PlayerRowPanels[i]);
			}
			panels.Add(BotPanel);
			for (int i = 0; i < panels.Count; i++)
			{
				panels[i].BorderStyle = BorderStyle.None;
				panels[i].Margin = Padding.Empty;
				panels[i].Dock = DockStyle.Fill;
			}

			Panel.Controls.Clear();
			Panel.RowCount = 0;
			Panel.ColumnCount = 0;
			Panel.RowStyles.Clear();
			Panel.ColumnStyles.Clear();
			Panel.RowStyles.Add(new RowStyle(SizeType.Absolute, MuhDic["PanelRow1Size"]));
			Panel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
			Panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 27));
			Panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

			MidMotherPanel.ColumnCount = 3;
			MidMotherPanel.RowStyles.Add(new RowStyle(SizeType.Percent,1));
			MidMotherPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, MuhDic["MidMotherWidth"]));
			MidMotherPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,50));
			MidMotherPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, MuhDic["MidMotherWidth"]));
			MidMotherPanel.Controls.Add(MidPanel, 1, 0);

			MidTopPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

			MidPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, MuhDic["TitlesRowSize"]));
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
			BotPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30));
			BotPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
			BotPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40));
			BotPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 71));
			BotPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 8));

			BotPanel.ColumnCount = 5;
			BotPanel.RowCount = 1;

			PictureBox favIconKawaii = new PictureBox();
			favIconKawaii.Image = Resources.favicon;
			favIconKawaii.SizeMode = PictureBoxSizeMode.CenterImage;
			favIconKawaii.Size=new Size(16, 16);
			favIconKawaii.Margin = Padding.Empty;
			favIconKawaii.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
				|AnchorStyles.Right;
			favIconKawaii.BackColor = Color.Transparent;
			BotPanel.Controls.Add(favIconKawaii, 0, 0);

			TimLabel = new Label
			{
				Anchor = AnchorStyles.Left | AnchorStyles.Right,
				BackColor = Color.Black,
				ForeColor = Color.White,
				TextAlign = ContentAlignment.MiddleCenter
			};
			TimLabel.Font = new Font(TimLabel.Font.FontFamily, 9, FontStyle.Bold);

			BotPanel.Controls.Add(TimLabel, 3, 0);
			BotPanel.Controls.Add(new Label
			{
				Text = "Time: ",
				Anchor = AnchorStyles.Left | AnchorStyles.Right|AnchorStyles.Top|AnchorStyles.Bottom,
				BackColor = Color.Transparent,
				ForeColor = Color.White,
				TextAlign = ContentAlignment.MiddleRight,
				MinimumSize = new Size(3,3),
				Margin = Padding.Empty
			}, 2, 0);

			lable = new LinkLabel
			{
				Text = "http://www.lf-empire.de/forum/",
				Links =
				{
					new LinkLabel.Link {LinkData = "http://www.lf-empire.de/forum/"}
				},

				Anchor = AnchorStyles.Left | AnchorStyles.Right,
				BackColor = Color.Transparent,
				ForeColor = Color.White,
				TextAlign = ContentAlignment.MiddleLeft,
				ActiveLinkColor = Color.White,
				VisitedLinkColor = Color.White,
				LinkColor = Color.White,
				LinkBehavior = LinkBehavior.NeverUnderline
			};
			lable.LinkClicked += LableOnLinkClicked;

			BotPanel.Controls.Add(lable, 1, 0);

			imaginaryPlayer.ImaginaryName.Text = "Player";
			imaginaryPlayer.Kill.Text = "Kill";
			imaginaryPlayer.Attack.Text = "Attack";
			imaginaryPlayer.HpLost.Text = "HP Lost";
			imaginaryPlayer.MpUsage.Text = "MP Usage";
			imaginaryPlayer.Picking.Text = "Picking";
			imaginaryPlayer.Status.Text = "Status";

			var imaginLabels = new List<Label>();

			imaginLabels.Add(imaginaryPlayer.ImaginaryName);
			imaginLabels.Add(imaginaryPlayer.Kill);
			imaginLabels.Add(imaginaryPlayer.Attack);
			imaginLabels.Add(imaginaryPlayer.HpLost);
			imaginLabels.Add(imaginaryPlayer.MpUsage);
			imaginLabels.Add(imaginaryPlayer.Picking);
			imaginLabels.Add(imaginaryPlayer.Status);

			for (int i = 0; i < imaginLabels.Count; i++)
			{
				imaginLabels[i].BackColor = Color.Transparent;
				imaginLabels[i].Font = new Font(new FontFamily("Arial"),10);
				imaginLabels[i].AutoSize = false;
				imaginLabels[i].MaximumSize = new Size(500,18);
			}

			imaginaryPlayer.Kill.ForeColor = imaginaryPlayer.MightyColors[4];
			imaginaryPlayer.Attack.ForeColor = imaginaryPlayer.MightyColors[4];
			imaginaryPlayer.HpLost.ForeColor = imaginaryPlayer.MightyColors[5];
			imaginaryPlayer.MpUsage.ForeColor = imaginaryPlayer.MightyColors[5];
			imaginaryPlayer.Picking.ForeColor = imaginaryPlayer.MightyColors[6];
			imaginaryPlayer.Status.ForeColor = imaginaryPlayer.MightyColors[7];

			MidTopPanel.ColumnCount = 7;
			MidTopPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 19));
			MidTopPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13));
			MidTopPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13));
			MidTopPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13));
			MidTopPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13));
			MidTopPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13));
			MidTopPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12));

			int col = 0;
			MidTopPanel.Controls.Add(imaginaryPlayer.ImaginaryName, col, 0);
			col++;
			MidTopPanel.Controls.Add(imaginaryPlayer.Kill, col, 0);
			col++;
			MidTopPanel.Controls.Add(imaginaryPlayer.Attack, col, 0);
			col++;
			MidTopPanel.Controls.Add(imaginaryPlayer.HpLost, col, 0);
			col++;
			MidTopPanel.Controls.Add(imaginaryPlayer.MpUsage, col, 0);
			col++;
			MidTopPanel.Controls.Add(imaginaryPlayer.Picking, col, 0);
			col++;
			MidTopPanel.Controls.Add(imaginaryPlayer.Status, col, 0);


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
				MidPanel.RowStyles.Add(new RowStyle(SizeType.Percent, MuhDic["PlayerRowSize"]));
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
			Color topColor = Color.FromArgb(255, 17, 40, 108);
			using (var b = new SolidBrush(topColor))
			{
				e.Graphics.FillRectangle(b, e.CellBounds);
			}
		}

		private void MidPanelOnCellPaint(object sender, TableLayoutCellPaintEventArgs e)
		{
			Color midColor = Color.FromArgb(255, 50, 77, 154);
			using (var b = new SolidBrush(midColor))
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
			if (StreamIsOnChanged)
			{
				if (StreamIsOn)
				{
					MidMotherPanel.ColumnStyles[0] = new ColumnStyle(SizeType.Absolute, 1);
					MidMotherPanel.ColumnStyles[2] = new ColumnStyle(SizeType.Absolute, 1);
					Panel.MaximumSize = new Size(1000, Players.Count*24 + 91);
					lable.Text = "LFE";
					Panel.RowStyles[0] = new RowStyle(SizeType.Absolute, 0);
					Panel.Size = new Size(Panel.Size.Width, 52 + Players.Count*24);
					for (int i = 0; i < MidPanel.RowStyles.Count; i++)
					{
						MidPanel.RowStyles[i] = new RowStyle(SizeType.Absolute, 24);
					}
					Panel.Dock = DockStyle.None;
					Panel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

					imaginaryPlayer.ImaginaryName.Text = "Player";
					imaginaryPlayer.Kill.Text = "Kill";
					imaginaryPlayer.Attack.Text = "Atk";
					imaginaryPlayer.HpLost.Text = "HP";
					imaginaryPlayer.MpUsage.Text = "MP";
					imaginaryPlayer.Picking.Text = "Pick";
					imaginaryPlayer.Status.Text = "Stat";

					MidTopPanel.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, 8);
					MidTopPanel.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 9);
					MidTopPanel.ColumnStyles[2] = new ColumnStyle(SizeType.Percent, 9);
					MidTopPanel.ColumnStyles[3] = new ColumnStyle(SizeType.Percent, 9);
					MidTopPanel.ColumnStyles[4] = new ColumnStyle(SizeType.Percent, 9);
					MidTopPanel.ColumnStyles[5] = new ColumnStyle(SizeType.Percent, 6);
					MidTopPanel.ColumnStyles[6] = new ColumnStyle(SizeType.Percent, 3);
					for (int i = 0; i < Players.Count; i++)
					{
						PlayerRowPanels[i].ColumnStyles[0] = new ColumnStyle(SizeType.Percent, 4);
						PlayerRowPanels[i].ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 7);
						PlayerRowPanels[i].ColumnStyles[10] = new ColumnStyle(SizeType.Percent, 8);
						PlayerRowPanels[i].ColumnStyles[12] = new ColumnStyle(SizeType.Percent, 4);
					}
					MotherForm.Size = new Size(475, 284);
					MotherForm.BackColor = Color.FromArgb(255, 2, 0, 44);
				}
				else
				{
					MotherForm.BackColor = Color.Black;
					MidMotherPanel.ColumnStyles[0] = new ColumnStyle(SizeType.Absolute, MuhDic["MidMotherWidth"]);
					MidMotherPanel.ColumnStyles[2] = new ColumnStyle(SizeType.Absolute, MuhDic["MidMotherWidth"]);
					Panel.MaximumSize = new Size(1000, MuhDic["maxHeight"]);
					lable.Text = "http://www.lf-empire.de/forum/";
					Panel.RowStyles[0] = new RowStyle(SizeType.Absolute, MuhDic["PanelRow1Size"]);
					MidPanel.RowStyles[0] = new RowStyle(SizeType.Absolute, MuhDic["TitlesRowSize"]);
					for (int i = 1; i < MidPanel.RowStyles.Count; i++)
					{
						MidPanel.RowStyles[i] = new RowStyle(SizeType.Percent, MuhDic["PlayerRowSize"]);
					}
					Panel.Dock = DockStyle.Fill;

					imaginaryPlayer.ImaginaryName.Text = "Player";
					imaginaryPlayer.Kill.Text = "Kill";
					imaginaryPlayer.Attack.Text = "Attack";
					imaginaryPlayer.HpLost.Text = "HP Lost";
					imaginaryPlayer.MpUsage.Text = "MP Usage";
					imaginaryPlayer.Picking.Text = "Picking";
					imaginaryPlayer.Status.Text = "Status";

					MidTopPanel.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, 19);
					MidTopPanel.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 13);
					MidTopPanel.ColumnStyles[2] = new ColumnStyle(SizeType.Percent, 13);
					MidTopPanel.ColumnStyles[3] = new ColumnStyle(SizeType.Percent, 13);
					MidTopPanel.ColumnStyles[4] = new ColumnStyle(SizeType.Percent, 13);
					MidTopPanel.ColumnStyles[5] = new ColumnStyle(SizeType.Percent, 13);
					MidTopPanel.ColumnStyles[6] = new ColumnStyle(SizeType.Percent, 12);
					for (int i = 0; i < Players.Count; i++)
					{
						PlayerRowPanels[i].ColumnStyles[0] = new ColumnStyle(SizeType.Percent, 9);
						PlayerRowPanels[i].ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 10);
						PlayerRowPanels[i].ColumnStyles[10] = new ColumnStyle(SizeType.Percent, 12);
						PlayerRowPanels[i].ColumnStyles[12] = new ColumnStyle(SizeType.Percent, 12);
					}
					MotherForm.Size = new Size(550, Panel.MaximumSize.Height);
				}
				StreamIsOnChanged = false;
			}
		}
	}
}
