using Il2CppSystem.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using UnhollowerRuntimeLib;
using UnityEngine;
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

        public static void UpdatePlayerNameplate(Player player, bool OriginalName = false)
        {
            if (NicknameManager.Contains(player.prop_APIUser_0.id))
            {
                player.prop_VRCPlayer_0.nameplate.uiName.text = !OriginalName ? NicknameManager.GetModifiedName(player.field_Private_APIUser_0.id) : player.field_Private_APIUser_0.displayName;
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
            foreach (Text t in VRCUiManagerInstance.menuContent.GetComponentsInChildren<Text>())
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

    }
}
