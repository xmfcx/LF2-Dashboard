﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using LF2Dashboard.Properties;
using LF2Dashboard.StatsPanel;

namespace LF2Dashboard
{
	class Game
	{
		public Character[] Chars;
		private readonly TableLayoutPanel panel;
		public ScoreBoard Board;
		public Process GameProcess;
		public Player[] Players;
		public ComPlayer[] CPlayers;
		public int PlayerAmount;
		public bool GameIsOn;
		public int GameState;
		public bool GameOpenedFully;
		public int ActiveAmount;
		private bool weHaveTable;
		public TimeSpan GameSession;
		public bool StreamIsOn;
		public Form MotherForm;

		public Game(Process gameProcess, TableLayoutPanel panel, Form form)
		{
			MotherForm = form;
			this.panel = panel;
			GameProcess = gameProcess;
			Initialise();
		}

		public void Update()
		{
			GameState = BitConverter.ToInt16(
				Hooker.ReadMemory(GameProcess, AddressTable.GameState, 2), 0);
			GameIsOn = GameState == 0;

			if (GameState == 1 && !GameOpenedFully)
			{
				Initialise();
				GameOpenedFully = true;
			}

			#region Players
			ActiveAmount = 0;
			for (int i = 0; i < 8; i++)
			{
				Players[i].Update();
				CPlayers[i].Update();
				if (GameIsOn)
				{
					byte activity = Hooker.ReadMemory(GameProcess, AddressTable.PlayerInGame[i], 1)[0];
					Players[i].IsActive = activity == 1;
					activity = Hooker.ReadMemory(GameProcess, AddressTable.CPlayerInGame[i], 1)[0];
					CPlayers[i].IsActive = activity == 1;
				}
				else
				{
					Players[i].IsActive = false;
					CPlayers[i].IsActive = false;
				}
				if (GameIsOn)
				{
					if (Players[i].IsActive)
					{
						ActiveAmount++;
						Players[i].IsAlive = Players[i].Hp > 0;

						foreach (var character in Chars)
						{
							if (Players[i].DataAddress == character.Address)
							{
								Players[i].Char = character;
							}
						}
					}
					if (CPlayers[i].IsActive)
					{
						ActiveAmount++;
						CPlayers[i].IsAlive = CPlayers[i].Hp > 0;
						foreach (var character in Chars)
						{
							if (CPlayers[i].DataAddress == character.Address)
							{
								CPlayers[i].Char = character;
							}
						}
					}
				}
			} 
			#endregion

			if (GameIsOn)
			{
				var timyTime = BitConverter.ToInt32(
				Hooker.ReadMemory(GameProcess, AddressTable.Time, 4), 0);
				GameSession = TimeSpan.FromSeconds(timyTime / 30);

				if (!weHaveTable)
				{
					BuildTable();
					Board.StreamIsOn = StreamIsOn;
					MotherForm.Size = new Size(MotherForm.Width,Board.Panel.MaximumSize.Height);
					Board.StreamIsOnChanged = true;
					weHaveTable = true;
				}
				else
				{
					RealPlayer.Scores bestScore = new RealPlayer.Scores();
					foreach (var realPlayer in Board.Players)
					{
						bestScore.Kill = Comparer(realPlayer.MyScores.Kill, bestScore.Kill);
						bestScore.Attack = Comparer(realPlayer.MyScores.Attack, bestScore.Attack);
						bestScore.HpLost = Comparer(realPlayer.MyScores.HpLost, bestScore.HpLost);
						bestScore.MpUsage = Comparer(realPlayer.MyScores.MpUsage, bestScore.MpUsage);
						bestScore.Picking = Comparer(realPlayer.MyScores.Picking, bestScore.Picking);
					}
					foreach (var realPlayer in Board.Players)
					{
						realPlayer.Has.Kill = realPlayer.MyScores.Kill == bestScore.Kill;
						realPlayer.Has.Attack = realPlayer.MyScores.Attack == bestScore.Attack;
						realPlayer.Has.HpLost = realPlayer.MyScores.HpLost == bestScore.HpLost;
						realPlayer.Has.MpUsage = realPlayer.MyScores.MpUsage == bestScore.MpUsage;
						realPlayer.Has.Picking = realPlayer.MyScores.Picking == bestScore.Picking;
					}
				}
			}
			else
			{
				weHaveTable = false;
			}
			if (weHaveTable)
			{
				Board.Update(GameSession);
			}
		}

		private int Comparer(int a, int b)
		{
			return a > b ? a : b;
		}

		public void BuildTable()
		{
			Board = new ScoreBoard(panel, MotherForm);
			foreach (var player in Players)
			{
				if (player.IsActive)
				{
					Board.Players.Add(new RealPlayer(player));
				}
			}
			foreach (var player in CPlayers)
			{
				if (player.IsActive)
				{
					Board.Players.Add(new RealPlayer(player));
				}
			}
			Board.BuildTable();
			Board.Panel.Refresh();
		}
		
		public string Log()
		{
			string log = "";
			foreach (var player in Players)
			{
				if (player.IsActive)
				{
					log += "Name: " + player.Name + "  ";
					log += "Char: " + player.Char.Name + "  ";

					log += "Kills: " + player.Kills + "  ";
					log += "Attack: " + player.Attack + "  ";
					log += "HpLost: " + player.HpLost + "  ";
					log += "MpUsage: " + player.MpUsage + "  ";
					log += "Picking: " + player.Picking + "  ";
					log += "Owner: " + player.Owner + "  ";
					//log += "Enemy: " + player.Enemy + "  ";
					log += "Team: " + player.Team + "  ";
					log += "IsAlive: " + player.IsAlive + "  ";
					log += "\r\n";
				}
			}
			foreach (var player in CPlayers)
			{
				if (player.IsActive)
				{
					log += "Name: " + player.Name + "  ";
					log += "Char: " + player.Char.Name + "  ";

					log += "Kills: " + player.Kills + "  ";
					log += "Attack: " + player.Attack + "  ";
					log += "HpLost: " + player.HpLost + "  ";
					log += "MpUsage: " + player.MpUsage + "  ";
					log += "Picking: " + player.Picking + "  ";
					log += "Owner: " + player.Owner + "  ";
					//log += "Enemy: " + player.Enemy + "  ";
					log += "Team: " + player.Team + "  ";
					log += "IsAlive: " + player.IsAlive + "  ";
					log += "\r\n";
				}
			}
			return log;
		}

		public void Initialise()
		{
			PlayerAmount = 8;
			Players = new Player[PlayerAmount];
			CPlayers = new ComPlayer[PlayerAmount];
			for (int i = 0; i < PlayerAmount; i++)
			{
				Players[i] = new Player(GameProcess, i);
				CPlayers[i] = new ComPlayer(GameProcess, i);
			}
			AddressTable.FillPointers(GameProcess);

			Chars = new Character[24];
			for (int i = 0; i < Chars.Length; i++)
			{
				Chars[i] = new Character {Address = AddressTable.DataFiles[i]};
			}
			SetCharList();
		}

		private void SetCharList()
		{
			Chars[0].Name = "Template";
			Chars[1].Name = "Julian";
			Chars[2].Name = "Firzen";
			Chars[3].Name = "LouisEX";
			Chars[4].Name = "Bat";
			Chars[5].Name = "Justin";
			Chars[6].Name = "Knight";
			Chars[7].Name = "Jan";
			Chars[8].Name = "Monk";
			Chars[9].Name = "Sorcerer";
			Chars[10].Name = "Jack";
			Chars[11].Name = "Mark";
			Chars[12].Name = "Hunter";
			Chars[13].Name = "Bandit";
			Chars[14].Name = "Deep";
			Chars[15].Name = "John";
			Chars[16].Name = "Henry";
			Chars[17].Name = "Rudolf";
			Chars[18].Name = "Louis";
			Chars[19].Name = "Firen";
			Chars[20].Name = "Freeze";
			Chars[21].Name = "Dennis";
			Chars[22].Name = "Woody";
			Chars[23].Name = "Davis";

			Chars[0].Pic = Resources.template_s;
			Chars[1].Pic = Resources.julian_s;
			Chars[2].Pic = Resources.firzen_s;
			Chars[3].Pic = Resources.louisEX_s;
			Chars[4].Pic = Resources.bat_s;
			Chars[5].Pic = Resources.justin_s;
			Chars[6].Pic = Resources.knight_s;
			Chars[7].Pic = Resources.jan_s;
			Chars[8].Pic = Resources.monk_s;
			Chars[9].Pic = Resources.sorcerer_s;
			Chars[10].Pic = Resources.jack_s;
			Chars[11].Pic = Resources.mark_s;
			Chars[12].Pic = Resources.hunter_s;
			Chars[13].Pic = Resources.bandit_s;
			Chars[14].Pic = Resources.deep_s;
			Chars[15].Pic = Resources.john_s;
			Chars[16].Pic = Resources.henry_s;
			Chars[17].Pic = Resources.rudolf_s;
			Chars[18].Pic = Resources.louis_s;
			Chars[19].Pic = Resources.firen_s;
			Chars[20].Pic = Resources.freeze_s;
			Chars[21].Pic = Resources.dennis_s;
			Chars[22].Pic = Resources.woody_s;
			Chars[23].Pic = Resources.davis_s;
		}
	}
}
