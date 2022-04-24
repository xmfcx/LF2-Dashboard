#nullable enable
using System;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LF2Dashboard
{
  public partial class FormMain : Form
  {
    public FormMain()
    {
      _dashboardIsOnline = false;
      InitializeComponent();
    }

    private Game _game;
    private bool _dashboardIsOnline;

    private void FormMain_Load(object sender, EventArgs e)
    {
      BackColor = Color.Black;
      timerWorker.Enabled = true;
      timerWorker.Interval = 1;
      AddressTable.Init();

      ReinitializeDashboard();
    }

    private void timerWorker_Tick(object sender, EventArgs e)
    {
      var lf2Process = TryGetLf2Process();
      var lf2IsOff = lf2Process == null;
      if (!lf2IsOff)
      {
        ReinitializeDashboard();
      }
      else
      {
        _game.Update();
      }
    }

    void ReinitializeDashboard()
    {
      Regex regex = new Regex(@"lf2(.*?)");
      foreach (Process p in Process.GetProcesses("."))
      {
        var name = p.ProcessName.ToLower();
        if (name != "lf2dashboard" && regex.Match(name).Success)
        {
          _game = new Game(p, tableLayoutPanel1, this);
          _lf2IsOn = true;
          break;
        }

        _lf2IsOn = false;
      }
    }

    private Process? TryGetLf2Process()
    {
      Process? lf2Process = null;
      var regex = new Regex(@"lf2(.*?)");
      foreach (var p in Process.GetProcesses("."))
      {
        var name = p.ProcessName.ToLower();
        if (name == "lf2dashboard" || !regex.Match(name).Success)
          continue;
        lf2Process = p;
      }

      return lf2Process;
    }

    private void FormMain_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (_lf2IsOn)
      {
        if (e.KeyChar.ToString().ToLower() == "s")
        {
          if (_game.GameIsOn)
          {
            if (_game.Board.StreamIsOn)
            {
              _game.StreamIsOn = false;
              _game.Board.StreamIsOn = false;
            }
            else
            {
              _game.StreamIsOn = true;
              _game.Board.StreamIsOn = true;
              Console.Write("IT'S ON!");
            }

            _game.Board.StreamIsOnChanged = true;
          }
        }
      }
    }
  }
}