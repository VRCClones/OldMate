using Harmony;
using MelonLoader;
using System.Collections;
using System.Linq;
using System.Reflection;
using UIExpansionKit;
using UnityEngine;
using VRC;

namespace OldMate
{

    public static class BuildInfo
    {
        public const string Name = "OldMate";
        public const string Author = "dave-kun";
        public const string Company = null;
        public const string Version = "1.3.0";
        public const string DownloadLink = "https://github.com/dave-kun/OldMate";
    }

    public class OldMateMod : MelonMod
    {

        private const string ModCategory = "OldMate";
        private const string UpdateDelayPref = "Update Delay";
        private static float NextQuickMenuUpdate;
        private static float NextMenuContentUpdate;

        public override void OnApplicationStart()
        {
            MelonPrefs.RegisterCategory(ModCategory, "OldMate");
            MelonPrefs.RegisterFloat(ModCategory, UpdateDelayPref, 1.0f, "The amount of seconds to wait before updating display names in menus.");
            if (MelonHandler.Mods.Any(it => it.Info.SystemType.Name == nameof(UiExpansionKitMod)))
            {
                typeof(UiExpansionKitSupport).GetMethod(nameof(UiExpansionKitSupport.Initialize), BindingFlags.Static | BindingFlags.Public)!.Invoke(null, new object[0]);
            }
            NicknameManager.LoadNicknames();
            MelonCoroutines.Start(Initialize());
        }

        private IEnumerator Initialize()
        {
            while (ReferenceEquals(NetworkManager.field_Internal_Static_NetworkManager_0, null))
                yield return null;

            MelonLogger.Log("Initializing OldMate.");
            NetworkManagerHooks.Initialize();
            NetworkManagerHooks.OnJoin += OnPlayerJoined;
        }

        public void OnPlayerJoined(Player player)
        {
            if (player != null)
            {
                if (NicknameManager.Contains(player.field_Private_APIUser_0.id))
                {
                    VRChatAPI.UpdatePlayerNameplate(player);
                }
            }
        }

        public override void OnUpdate()
        {
            if (RoomManager.field_Internal_Static_ApiWorld_0 == null && RoomManager.field_Internal_Static_ApiWorldInstance_0 == null)
                return;

            if (QuickMenu.prop_QuickMenu_0.gameObject.activeInHierarchy)
            {
                if (Time.time > NextQuickMenuUpdate)
                {
                    var players = PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0;
                    for (int i = 0; i < players.Count; i++)
                    {
                        if (players[i] != null)
                        {
                            VRChatAPI.UpdatePlayerNameplate(players[i], false);
                        }
                    }
                    VRChatAPI.UpdateQuickMenuText();
                    NextQuickMenuUpdate = Time.time + MelonPrefs.GetFloat(ModCategory, UpdateDelayPref);
                }
            }

            if (VRChatAPI.VRCUiManagerInstance.menuContent.activeInHierarchy)
            {
                if (Time.time > NextMenuContentUpdate)
                {
                    VRChatAPI.UpdateMenuContentText();
                    NextMenuContentUpdate = Time.time + MelonPrefs.GetFloat(ModCategory, UpdateDelayPref);
                }
            }
        }

    }
}
