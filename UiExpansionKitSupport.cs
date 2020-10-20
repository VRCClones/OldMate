using UIExpansionKit.API;
using UnityEngine;
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
            VRChatAPI.ShowInputPopup("Type a nickname", "Accept", delegate (string name)
            {
                var Menu = GameObject.Find("Screens").transform.Find("UserInfo");
                var UserInfo = Menu.transform.GetComponentInChildren<VRC.UI.PageUserInfo>();
                if (Menu != null && UserInfo != null)
                {
                    Nickname n = new Nickname();
                    n.UserId = UserInfo?.user?.id;
                    n.OriginalName = UserInfo?.user?.displayName;
                    n.ModifiedName = name;
                    UpdateNickname(n);
                    var Player = VRChatAPI.GetPlayerFromId(UserInfo.user.id);
                    if (Player != null)
                    {
                        VRChatAPI.UpdatePlayerNameplate(Player);
                    }
                }
            });
        }

        private static void RemoveNicknameUserDetailsMenu()
        {
            var Menu = GameObject.Find("Screens").transform.Find("UserInfo");
            var UserInfo = Menu.transform.GetComponentInChildren<VRC.UI.PageUserInfo>();
            if (Menu != null && UserInfo != null)
            {
                var Player = VRChatAPI.GetPlayerFromId(UserInfo.user.id);
                if (Player != null)
                {
                    VRChatAPI.UpdatePlayerNameplate(Player, true);
                }
                VRChatAPI.UpdateMenuContentText();
                RemoveNickname(UserInfo.user.id);
            }
        }

    }
}
