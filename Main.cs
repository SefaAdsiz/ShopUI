using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket;
using Rocket.API.Serialisation;
using Rocket.Core;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Framework.Utilities;
using SDG.Unturned;
using UnityEngine;
using static SDG.Provider.SteamGetInventoryResponse;

namespace ShopUI
{
    public class Main : RocketPlugin<Config>
    {
        public static Main main;
        protected override void Load()
        {
            main = this;
            U.Events.OnPlayerConnected += Events_OnPlayerConnected;
            EffectManager.onEffectButtonClicked += butonbasildi;
            base.Load();
        }
        protected override void Unload()
        {
            U.Events.OnPlayerConnected -= Events_OnPlayerConnected;
            EffectManager.onEffectButtonClicked -= butonbasildi;
            base.Unload();
        }
        public int GetMaxPage(UnturnedPlayer player)
        {

            UnturnedChat.Say((GetProductUsers(player).Count / 12).ToString());
            if(GetProductUsers(player).Count % 12 !=  0)
            {
                return (GetProductUsers(player).Count / 12) + 1;

            }
            return GetProductUsers(player).Count / 12;

        }
        private void butonbasildi(Player player, string buttonName)
        {
            var unturnedPlayer = UnturnedPlayer.FromPlayer(player);
            if (buttonName.Contains("satinal"))
            {
                string kelime = buttonName;

                var keli = kelime.Remove(0, 7);

                Getin(unturnedPlayer, playersayfa[unturnedPlayer], Convert.ToInt32(keli));
            }
            if (buttonName == "cikisyap")
            {
                unturnedPlayer.Player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, false);
                unturnedPlayer.Player.setPluginWidgetFlag(EPluginWidgetFlags.ForceBlur, false);
                EffectManager.askEffectClearByID(27845, unturnedPlayer.SteamPlayer().transportConnection);
                Main.playersayfa.Remove(unturnedPlayer);

            }
            if (buttonName == "ileri")
            {

                if (GetMaxPage(unturnedPlayer) > playersayfa[unturnedPlayer])
                {
                    playersayfa[unturnedPlayer] = playersayfa[unturnedPlayer] + 1;
                    KitGöster(unturnedPlayer, playersayfa[unturnedPlayer]);
                }
                else
                {
                    playersayfa[unturnedPlayer] = GetMaxPage(unturnedPlayer);
                    KitGöster(unturnedPlayer, playersayfa[unturnedPlayer]);
                }

            }
            if (buttonName == "ileri (1)")
            {

                if (playersayfa[unturnedPlayer] > 1)
                {
                    playersayfa[unturnedPlayer] = playersayfa[unturnedPlayer] - 1;
                    KitGöster(unturnedPlayer, playersayfa[unturnedPlayer]);

                }
                else
                {
                    playersayfa[unturnedPlayer] = 1;
                    KitGöster(unturnedPlayer, 1);
                }


            }
        }
        public static Dictionary<UnturnedPlayer, int> playersayfa = new Dictionary<UnturnedPlayer, int>();

        public void Getin(UnturnedPlayer player, int index, int indexinindex)
        {
            var get_range = GetProductUsers(player);
            var kontrol = get_range.Where(x => x.index == index);
            if (kontrol != null)
            {
                var aa = kontrol.ElementAt(indexinindex - 1);
                if(player.Experience < Convert.ToUInt64(aa.Price))
                {
                    UnturnedChat.Say(player, "Paran yetersiz");
                    return;
                }
                player.Experience -= Convert.ToUInt32(aa.Price);
                UnturnedChat.Say(player, "Başarı ile satın alındı");
                player.GiveItem(aa.Id, 1);
            }
            else
            {
            }


          

        }
        private void Events_OnPlayerConnected(UnturnedPlayer player)
        {
          

        }
        public static void KitGöster(UnturnedPlayer player, int index)
        {
            int sayi = 1;

            var kontrol = GetProductUsers(player).Where(x => x.index == index);
            if (kontrol != null)
            {
                EffectManager.askEffectClearByID(27845, player.SteamPlayer().transportConnection);
                EffectManager.sendUIEffect(27845, 27845, player.SteamPlayer().transportConnection, false);
                player.Player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, true);
                player.Player.setPluginWidgetFlag(EPluginWidgetFlags.ForceBlur, true);

                foreach (var kt in kontrol)
                {
                    string name = "itemisim";
                    string fiyat = "fiyat";
                    string satinal = "satinal";
                    string itemisim = ((ItemAsset)Assets.find(EAssetType.ITEM, kt.Id)).itemName;
                    EffectManager.sendUIEffectText(27845, player.SteamPlayer().transportConnection, false, name + sayi.ToString(), itemisim);
                    EffectManager.sendUIEffectText(27845, player.SteamPlayer().transportConnection, false, fiyat + sayi.ToString(), kt.Price);
                    EffectManager.sendUIEffectText(27845, player.SteamPlayer().transportConnection, false, "sayfano", index.ToString());
                    EffectManager.sendUIEffectImageURL(27845, player.SteamPlayer().transportConnection, false, "itemresim" + sayi.ToString(),kt.ImageUrl);

                    sayi++;

                }
                int yeni_sayi = 12 - sayi;
                for (int i = 12 - yeni_sayi; i < 15; i++)
                {
                    EffectManager.sendUIEffectVisibility(27845, player.SteamPlayer().transportConnection, false, "panel" + i.ToString(), false);
                }

            }
        }


        public Dictionary<UnturnedPlayer, List<ProductUser>> dic = new Dictionary<UnturnedPlayer, List<ProductUser>>();
        public Dictionary<UnturnedPlayer, int> maxindex = new Dictionary<UnturnedPlayer, int>();

        public static List<ProductUser> GetProductUsers(UnturnedPlayer player)
        {
            int count = 0;
            int indexs = 1;
            int sayi = 1;
            List<ProductUser> list = new List<ProductUser>();
            foreach (var itemler in main.Configuration.Instance.Products)
            {

                foreach (RocketPermissionsGroup group in R.Permissions.GetGroups(player, true))
                {
                    if (group.Id == itemler.Permissions)
                    {
                     


                        count++;
                        sayi++;

                        if (count == 13)
                        {
                            indexs++;
                            count = 0;
                        }
                        list.Add(new ProductUser
                        {
                            Id = itemler.Id,
                            Price = itemler.Price,
                            index = indexs,
                            ImageUrl = itemler.ImageUrl

                        });
                    }
                }
            }
            return list;
        }
    }
}
