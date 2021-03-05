using Harmony;
using System;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine.UI;
using VRC;

namespace OldMate
{
    public static class VRChatAPI
    {

        public delegate Player GetPlayerFromIdMethod(string id);
        public static GetPlayerFromIdMethod GetPlayerFromIdMethodDelegate;

        private static MethodInfo VRCUiManagerInstanceMethodInfo;

        public static VRCUiManager VRCUiManagerInstance
        {
            get
            {
                if (VRCUiManagerInstanceMethodInfo == null)
                {
                    VRCUiManagerInstanceMethodInfo = typeof(VRCUiManager).GetMethods().First(x => (x.ReturnType == typeof(VRCUiManager)));
                }
                return (VRCUiManager)VRCUiManagerInstanceMethodInfo.Invoke(null, Array.Empty<object>());
            }
        }

        public static GetPlayerFromIdMethod GetPlayerFromId
        {
            get
            {
                if (GetPlayerFromIdMethodDelegate == null)
                {
                    var targetMethod = typeof(PlayerManager).GetMethods(BindingFlags.Public | BindingFlags.Static).Where(it => it.GetParameters().Length == 1 && it.GetParameters()[0].ParameterType.ToString() == "System.String").Last();
                    GetPlayerFromIdMethodDelegate = (GetPlayerFromIdMethod)Delegate.CreateDelegate(typeof(GetPlayerFromIdMethod), targetMethod);
                }
                return GetPlayerFromIdMethodDelegate;
            }
        }

        public static void Patch()
        {
            HarmonyInstance harmonyInstance = HarmonyInstance.Create("OldMatePatches");
            harmonyInstance.Patch(typeof(TMP_Text).GetProperty("text").GetSetMethod(), new HarmonyMethod(typeof(VRChatAPI).GetMethod("OnTmpTextSet", BindingFlags.Static | BindingFlags.NonPublic)));
            harmonyInstance.Patch(typeof(Text).GetProperty("text").GetSetMethod(), new HarmonyMethod(typeof(VRChatAPI).GetMethod("OnUnityTextGet", BindingFlags.Static | BindingFlags.NonPublic)));
        }

        private static void OnTmpTextSet(ref string __0)
        {
            if (__0 != null)
            {
                foreach (var nickname in NicknameManager.nicknames)
                {
                    __0 = __0.Replace(nickname.OriginalName, nickname.ModifiedName);
                }
            }
        }

        private static void OnUnityTextGet(ref string __0)
        {
            if (__0 != null)
            {
                foreach (var nickname in NicknameManager.nicknames)
                {
                    __0 = __0.Replace(nickname.OriginalName, nickname.ModifiedName);
                }
            }
        }

        public static void UpdatePlayerNameplate(Player player, bool OriginalName = false)
        {
            if (NicknameManager.Contains(player?.prop_APIUser_0?.id))
            {
                player.prop_VRCPlayer_0.field_Public_PlayerNameplate_0.field_Public_TextMeshProUGUI_0.text = !OriginalName ? NicknameManager.GetModifiedName(player.field_Private_APIUser_0.id) : player.field_Private_APIUser_0.displayName;
            }
        }

        public static void UpdateQuickMenuText(bool OriginalName = false)
        {
            foreach (Text t in QuickMenu.prop_QuickMenu_0.gameObject.GetComponentsInChildren<Text>())
            {
                NicknameManager.nicknames.ForEach(nickname =>
                {
                    if (t.text.Equals(!OriginalName ? nickname.OriginalName : nickname.ModifiedName))
                    {
                        t.text = t.text.Replace(!OriginalName ? nickname.OriginalName : nickname.ModifiedName, !OriginalName ? nickname.ModifiedName : nickname.OriginalName);
                    }
                });
            }
        }

        public static void UpdateMenuContentText(bool OriginalName = false)
        {
            foreach (Text t in VRCUiManagerInstance.field_Public_GameObject_0.GetComponentsInChildren<Text>())
            {
                NicknameManager.nicknames.ForEach(nickname =>
                {
                    if (t.text.Equals(!OriginalName ? nickname.OriginalName : nickname.ModifiedName))
                    {
                        t.text = t.text.Replace(!OriginalName ? nickname.OriginalName : nickname.ModifiedName, !OriginalName ? nickname.ModifiedName : nickname.OriginalName);
                    }
                });
            }
        }

    }
}
