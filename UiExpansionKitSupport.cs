using UIExpansionKit.API;
using UnityEngine;
using UnityEngine.UI;
using static OldMate.NicknameManager;

namespace OldMate
{
    public static class UiExpansionKitSupport
    {

        public static void Initialize()
        {
            ExpansionKitApi.RegisterSimpleMenuButton(ExpandedMenu.UserDetailsMenu, "Remove Nickname", RemoveNicknameUserDetailsMenu);
            ExpansionKitApi.RegisterSimpleMenuButton(ExpandedMenu.UserDetailsMenu, "Update Nickname", UpdateNicknameUserDetailsMenu);
        }

        private static void UpdateNicknameUserDetailsMenu()
        {
            UIExpansionKit.API.BuiltinUiUtils.ShowInputPopup("Type a nickname", "", InputField.InputType.Standard, false, "Accept", (name, _, __) =>
            {
                var Menu = GameObject.Find("Screens").transform.Find("UserInfo");
                var UserInfo = Menu.transform.GetComponentInChildren<VRC.UI.PageUserInfo>();
                if (Menu != null && UserInfo != null)
                {
                    Nickname n = new Nickname();
                    n.UserId = UserInfo?.field_Public_APIUser_0?.id;
                    n.OriginalName = UserInfo?.field_Public_APIUser_0?.displayName;
                    n.ModifiedName = name;
                    UpdateNickname(n);
                    var Player = VRChatAPI.GetPlayerFromId(UserInfo.field_Public_APIUser_0.id);
                    if (Player != null)
                    {
                        VRChatAPI.UpdatePlayerNameplate(Player);
                    }
                    VRChatAPI.UpdateQuickMenuText();
                    VRChatAPI.UpdateMenuContentText();
                }
            });
        }

        private static void RemoveNicknameUserDetailsMenu()
        {
            var Menu = GameObject.Find("Screens").transform.Find("UserInfo");
            var UserInfo = Menu.transform.GetComponentInChildren<VRC.UI.PageUserInfo>();
            if (Menu != null && UserInfo != null)
            {
                var Player = VRChatAPI.GetPlayerFromId(UserInfo.field_Public_APIUser_0.id);
                if (Player != null)
                {
                    VRChatAPI.UpdatePlayerNameplate(Player, true);
                }
                RemoveNickname(UserInfo.field_Public_APIUser_0.id);
                VRChatAPI.UpdateQuickMenuText(true);
                VRChatAPI.UpdateMenuContentText(true);
            }
        }

    }
}
