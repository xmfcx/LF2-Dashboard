using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net.Mime;
using System.Windows.Forms;
using LF2Dashboard.Properties;
using LF2Dashboard.ScoreBoard;

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
		public Dictionary<string, int> muhDic;
		private LinkLabel lable;
		private RealPlayer ImaginaryPlayer;
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
			muhDic = new Dictionary<string, int>();
			muhDic.Add("PanelRow1Size", 22);
			muhDic.Add("PlayerRowSize", 50);
			muhDic.Add("TitlesRowSize", 34);
			muhDic.Add("MidMotherWidth", 8);
			muhDic.Add("maxHeight", 120 + Players.Count*45);


			ImaginaryPlayer = new RealPlayer();
			TopPanel = new TableLayoutPanel();
			MidPanel = new TableLayoutPanel();
			MidTopPanel = new TableLayoutPanel();
			PlayerRowPanels = new TableLayoutPanel[Players.Count];
			BotPanel = new TableLayoutPanel();
			MidMotherPanel = new TableLayoutPanel();

			Panel.MaximumSize = new Size(1000, muhDic["maxHeight"]);
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
			Panel.RowStyles.Add(new RowStyle(SizeType.Absolute, muhDic["PanelRow1Size"]));
			Panel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
			Panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 27));
			Panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

			MidMotherPanel.ColumnCount = 3;
			MidMotherPanel.RowStyles.Add(new RowStyle(SizeType.Percent,1));
			MidMotherPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, muhDic["MidMotherWidth"]));
			MidMotherPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,50));
			MidMotherPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, muhDic["MidMotherWidth"]));
			MidMotherPanel.Controls.Add(MidPanel, 1, 0);

			MidTopPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

			MidPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, muhDic["TitlesRowSize"]));
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
				LinkBehavior = LinkBehavior.NeverUnderline
			};
			lable.LinkClicked += LableOnLinkClicked;

			BotPanel.Controls.Add(lable, 1, 0);

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
				MidPanel.RowStyles.Add(new RowStyle(SizeType.Percent, muhDic["PlayerRowSize"]));
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

					ImaginaryPlayer.ImaginaryName.Text = "Player";
					ImaginaryPlayer.Kill.Text = "Kill";
					ImaginaryPlayer.Attack.Text = "Atk";
					ImaginaryPlayer.HpLost.Text = "HP";
					ImaginaryPlayer.MpUsage.Text = "MP";
					ImaginaryPlayer.Picking.Text = "Pick";
					ImaginaryPlayer.Status.Text = "Stat";

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

				}
				else
				{
					MidMotherPanel.ColumnStyles[0] = new ColumnStyle(SizeType.Absolute, muhDic["MidMotherWidth"]);
					MidMotherPanel.ColumnStyles[2] = new ColumnStyle(SizeType.Absolute, muhDic["MidMotherWidth"]);
					Panel.MaximumSize = new Size(1000, muhDic["maxHeight"]);
					lable.Text = "http://www.lf-empire.de/forum/";
					Panel.RowStyles[0] = new RowStyle(SizeType.Absolute, muhDic["PanelRow1Size"]);
					MidPanel.RowStyles[0] = new RowStyle(SizeType.Absolute, muhDic["TitlesRowSize"]);
					for (int i = 1; i < MidPanel.RowStyles.Count; i++)
					{
						MidPanel.RowStyles[i] = new RowStyle(SizeType.Percent, muhDic["PlayerRowSize"]);
					}
					Panel.Dock = DockStyle.Fill;

					ImaginaryPlayer.ImaginaryName.Text = "Player";
					ImaginaryPlayer.Kill.Text = "Kill";
					ImaginaryPlayer.Attack.Text = "Attack";
					ImaginaryPlayer.HpLost.Text = "HP Lost";
					ImaginaryPlayer.MpUsage.Text = "MP Usage";
					ImaginaryPlayer.Picking.Text = "Picking";
					ImaginaryPlayer.Status.Text = "Status";

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
				}
				MotherForm.Size = new Size(MotherForm.Width, Panel.MaximumSize.Height);
				StreamIsOnChanged = false;
			}
		}
	}
}
