using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace LF2Dashboard.ScoreBoard
{
	class RealPlayer
	{
		public PictureBox Picture;
		public CustomLabel Name;
		public Label ImaginaryName;
		public Label ImaginaryGray1;
		public Label ImaginaryGray2;
		public Label ImaginaryGray3;
		public Label ImaginaryGray4;
		public Label ImaginaryGray5;
		public Label Kill;
		public Label Attack;
		public Label HpLost;
		public Label MpUsage;
		public Label Picking;
		public Label Status;
		public Color[] TeamColors;
		public Color[] MightyColors;
		public Player Player;
		public struct HasBest
		{
			public bool Kill;
			public bool Attack;
			public bool HpLost;
			public bool MpUsage;
			public bool Picking;
		}

		public struct Scores
		{
			public int Kill;
			public int Attack;
			public int HpLost;
			public int MpUsage;
			public int Picking;
		}

		public HasBest Has;
		public Scores MyScores;

		public RealPlayer(Player player)
		{
			Has = new HasBest();
			Player = player;
			SetControls();
			Picture.Size = new Size(1, 1);
			Picture.Margin = Padding.Empty;
		}
		public RealPlayer()
		{
			SetControls();
		}

		public void SetControls()
		{
			MyScores = new Scores();
			Picture = new PictureBox();
			Name = new CustomLabel();
			Kill = new Label();
			Attack = new Label();
			HpLost = new Label();
			MpUsage = new Label();
			Picking = new Label();
			Status = new Label();
			ImaginaryName = new Label();
			ImaginaryGray1 = new Label();
			ImaginaryGray2 = new Label();
			ImaginaryGray3 = new Label();
			ImaginaryGray4 = new Label();
			ImaginaryGray5 = new Label();
			TeamColors = new Color[10];
			TeamColors[0] = Color.FromArgb(255, 251, 251, 251);
			TeamColors[1] = Color.FromArgb(255, 79, 155, 255);
			TeamColors[2] = Color.FromArgb(255, 255, 79, 79);
			TeamColors[3] = Color.FromArgb(255, 60, 173, 15);
			TeamColors[4] = Color.FromArgb(255, 255, 211, 76);
			TeamColors[5] = Color.FromArgb(255, 68, 68, 68);
			TeamColors[6] = Color.FromArgb(255, 0, 30, 70);
			TeamColors[7] = Color.FromArgb(255, 70, 1, 1);
			TeamColors[8] = Color.FromArgb(255, 21, 65, 3);
			TeamColors[9] = Color.FromArgb(255, 154, 87, 0);

			MightyColors = new Color[9];
			MightyColors[0] = Color.FromArgb(255, 255, 170, 170);
			MightyColors[1] = Color.FromArgb(255, 240, 240, 170);
			MightyColors[2] = Color.FromArgb(255, 170, 245, 170);
			MightyColors[3] = Color.FromArgb(255, 255, 152, 152);

			MightyColors[4] = Color.FromArgb(255, 228, 177, 148);
			MightyColors[5] = Color.FromArgb(255, 228, 223, 148);
			MightyColors[6] = Color.FromArgb(255, 110, 196, 155);
			MightyColors[7] = Color.FromArgb(255, 193, 199, 255);

			MightyColors[8] = Color.FromArgb(255, 133, 255, 133);

			Kill.ForeColor = MightyColors[0];
			Attack.ForeColor = MightyColors[0];
			HpLost.ForeColor = MightyColors[1];
			MpUsage.ForeColor = MightyColors[1];
			Picking.ForeColor = MightyColors[2];

			List<Control> cons = new List<Control>();
			cons.Add(Picture);
			cons.Add(Name);
			cons.Add(Kill);
			cons.Add(Attack);
			cons.Add(HpLost);
			cons.Add(MpUsage);
			cons.Add(Picking);
			cons.Add(Status);
			cons.Add(ImaginaryName);
			cons.Add(ImaginaryGray1);
			cons.Add(ImaginaryGray2);
			cons.Add(ImaginaryGray3);
			cons.Add(ImaginaryGray4);
			cons.Add(ImaginaryGray5);
			foreach (var control in cons)
			{
				control.Anchor = AnchorStyles.Left | AnchorStyles.Right;
				control.Margin = Padding.Empty;
				control.BackColor = Color.Transparent;
			}
			List<Label> labels = new List<Label>();
			labels.Add(Name);
			labels.Add(Kill);
			labels.Add(Attack);
			labels.Add(HpLost);
			labels.Add(MpUsage);
			labels.Add(Picking);
			labels.Add(Status);
			labels.Add(ImaginaryName);
			labels.Add(ImaginaryGray1);
			labels.Add(ImaginaryGray2);
			labels.Add(ImaginaryGray3);
			labels.Add(ImaginaryGray4);
			labels.Add(ImaginaryGray5);

			Kill.Font = new Font(Kill.Font, FontStyle.Bold);
			Attack.Font = new Font(Attack.Font, FontStyle.Bold);
			HpLost.Font = new Font(HpLost.Font, FontStyle.Bold);
			MpUsage.Font = new Font(MpUsage.Font, FontStyle.Bold);
			Picking.Font = new Font(Picking.Font, FontStyle.Bold);

			foreach (var label in labels)
			{
				label.MaximumSize = new Size(500, 15);
				label.Margin = Padding.Empty;
				label.AutoSize = false;
				if (label != Name)
				{
					label.BackColor = Color.Black;
					label.TextAlign = ContentAlignment.MiddleCenter;
				}
				else
				{
					label.Font = new Font(new FontFamily("Arial"), 8f);
					label.MaximumSize = new Size(1000, 11);
				}
			}
			ImaginaryName.ForeColor = Color.White;


			ImaginaryName.BackColor = Color.Transparent;
			ImaginaryGray1.BackColor = Color.FromArgb(255, 44, 47, 61);
			ImaginaryGray2.BackColor = Color.FromArgb(255, 44, 47, 61);
			ImaginaryGray3.BackColor = Color.FromArgb(255, 44, 47, 61);
			ImaginaryGray4.BackColor = Color.FromArgb(255, 44, 47, 61);
			ImaginaryGray5.BackColor = Color.FromArgb(255, 44, 47, 61);
			Picture.SizeMode = PictureBoxSizeMode.Zoom;
			Picture.Anchor = AnchorStyles.Left | AnchorStyles.Right 
				|AnchorStyles.Top | AnchorStyles.Bottom;
			Picture.Dock = DockStyle.None;
			Picture.MinimumSize = new Size(3,3);
		}

		public void Update()
		{
			MyScores.Kill = Player.Kills;
			MyScores.Attack = Player.Attack;
			MyScores.HpLost = Player.HpLost;
			MyScores.MpUsage = Player.MpUsage;
			MyScores.Picking = Player.Picking;

			Status.ForeColor = Player.IsAlive ? MightyColors[8] : MightyColors[3];
			Name.Text = Player.Name;
			Kill.Text = Player.Kills.ToString();
			Attack.Text = Player.Attack.ToString();
			HpLost.Text = Player.HpLost.ToString();
			MpUsage.Text = Player.MpUsage.ToString();
			Picking.Text = Player.Picking.ToString();

			if (Has.Kill) Kill.Text += "☆";
			if (Has.Attack) Attack.Text += "☆";
			if (Has.HpLost) HpLost.Text += "☆";
			if (Has.MpUsage) MpUsage.Text += "☆";
			if (Has.Picking) Picking.Text += "☆";


			Status.Text = Player.IsAlive ? "Alive" : "Dead";
			Picture.Image = Player.Char.Pic;
			Name.OutlineWidth = 1.3f;
			switch (Player.Team)
			{
				case 1:
					Name.ForeColor = TeamColors[1];
					Name.OutlineForeColor = TeamColors[6];
					break;
				case 2:
					Name.ForeColor = TeamColors[2];
					Name.OutlineForeColor = TeamColors[7];
					break;
				case 3:
					Name.ForeColor = TeamColors[3];
					Name.OutlineForeColor = TeamColors[8];
					break;
				case 4:
					Name.ForeColor = TeamColors[4];
					Name.OutlineForeColor = TeamColors[9];
					break;
				default:
					Name.ForeColor = TeamColors[0];
					Name.OutlineForeColor = TeamColors[5];
					break;
			}
		}

		public class CustomLabel : Label
		{
			public CustomLabel()
			{
				OutlineForeColor = Color.Green;
				OutlineWidth = 2;
			}
			public Color OutlineForeColor { get; set; }
			public float OutlineWidth { get; set; }
			protected override void OnPaint(PaintEventArgs e)
			{
				e.Graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);
				using (GraphicsPath gp = new GraphicsPath())
				using (Pen outline = new Pen(OutlineForeColor, OutlineWidth)
				{ LineJoin = LineJoin.Round })
				using (StringFormat sf = new StringFormat())
				using (Brush foreBrush = new SolidBrush(ForeColor))
				{
					gp.AddString(Text, Font.FontFamily, (int)Font.Style,
							Font.Size, ClientRectangle, sf);
					e.Graphics.ScaleTransform(1.3f, 1.35f);
					e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
					e.Graphics.DrawPath(outline, gp);
					e.Graphics.FillPath(foreBrush, gp);
				}
			}
		}
	}
}
