﻿using System;
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
        var name = p.ProcessName.ToLower();
        if (name != "lf2dashboard" && regex.Match(name).Success)
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
      var regex = new Regex(@"lf2(.*?)");
      foreach (var p in Process.GetProcesses("."))
      {
        var name = p.ProcessName.ToLower();
        if (name == "lf2dashboard" || !regex.Match(name).Success)
          continue;
        lf2IsOn = true;
        return;
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
            }
            else
            {
              game.StreamIsOn = true;
              game.Board.StreamIsOn = true;
              Console.Write("IT'S ON!");
            }

            game.Board.StreamIsOnChanged = true;
          }
        }
      }
    }
  }
}