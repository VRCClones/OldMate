using MelonLoader;
using System.Collections;
using System.Linq;
using System.Reflection;
using UIExpansionKit;
using VRC;

namespace OldMate
{

    public static class BuildInfo
    {
        public const string Name = "OldMate";
        public const string Author = "dave-kun";
        public const string Company = null;
        public const string Version = "1.4.0";
        public const string DownloadLink = "https://github.com/dave-kun/OldMate";
    }

    public class OldMateMod : MelonMod
    {

        private const string ModCategory = "OldMate";

        public override void OnApplicationStart()
        {
            if (MelonHandler.Mods.Any(it => it.Info.SystemType.Name == nameof(UiExpansionKitMod)))
            {
                typeof(UiExpansionKitSupport).GetMethod(nameof(UiExpansionKitSupport.Initialize), BindingFlags.Static | BindingFlags.Public)!.Invoke(null, new object[0]);
            }
            NicknameManager.LoadNicknames();
            MelonCoroutines.Start(Initialize());
            VRChatAPI.Patch();
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

    }
}
