using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ShopUI
{
    public class ShopCommand : IRocketCommand
    {
        public string Name
        {
            get { return "shop"; }

        }
        public string Help
        {
            get { return "shop"; }
        }
        public string Syntax
        {
            get { return ""; }
        }
        public AllowedCaller AllowedCaller
        {
            get { return Rocket.API.AllowedCaller.Player; }
        }
        public bool RunFromConsole
        {
            get { return false; }
        }
        public List<string> Aliases
        {
            get { return new List<string>(); }
        }
        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "shop" };
            }
        }
        public List<ushort> Rewards = new List<ushort>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
           
            UnturnedPlayer player = (UnturnedPlayer)caller;
            Main.playersayfa.Add(player, 1);
            Main.KitGöster(player, 1);
        }
    }
}
