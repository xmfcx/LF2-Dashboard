using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace LF2Dashboard
{
  class Player
  {
    protected Process GameProcess;
    protected int PlayerAddress;
    public int DataAddress;
    public int Kills;
    public int Attack;
    public int HpLost;
    public int MpUsage;
    public int Picking;
    public int Owner;
    public int Enemy;
    public int Team;
    public int Invincible;
    public int Hp;
    public int HpDark;
    public int HpMax;
    public int Mp;
    public bool IsActive;
    public bool IsAlive;
    public Character Char;

    public string Name;

    public Player(Process gameProcess, int px)
    {
      GameProcess = gameProcess;
      PlayerAddress = BitConverter.ToInt32(
        Hooker.ReadMemory(GameProcess, AddressTable.Player[px], 4), 0);

      Name = GetString(11, AddressTable.Names[px]);
    }

    public void Update()
    {
      Kills = GetInt32(AddressTable.Kills);
      Attack = GetInt32(AddressTable.Attack);
      HpLost = GetInt32(AddressTable.HpLost);
      MpUsage = GetInt32(AddressTable.MpUsage);
      Picking = GetInt32(AddressTable.Picking);
      Owner = GetInt32(AddressTable.Owner);
      Enemy = GetInt32(AddressTable.Enemy);
      Team = GetInt32(AddressTable.Team);
      Invincible = GetInt32(AddressTable.Invincible);
      Hp = GetInt32(AddressTable.Hp);
      HpDark = GetInt32(AddressTable.HpDark);
      HpMax = GetInt32(AddressTable.HpMax);
      Mp = GetInt32(AddressTable.Mp);
      DataAddress = GetInt32(AddressTable.PDataPointer);
    }

    protected int GetInt32(int offset)
    {
      return BitConverter.ToInt32(Hooker.ReadMemory(GameProcess, PlayerAddress + offset, 4), 0);
    }

    protected string GetString(uint size, int address)
    {
      var array = Hooker.ReadMemory(GameProcess, address, size);
      return Encoding.Default.GetString(array).Trim('\0');
    }
  }
}