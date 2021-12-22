using ABI_RC.Core.UI;
using ABI_RC.Core.InteractionSystem;
using ABI_RC.Core.Networking.IO.Social;
using cohtml;
using System;

namespace SlimyHubAugment
{
    class HudAugmentPatcher
    {
        private static CohtmlView _hudView;


        static void RegisterEvents_PostFixPatch(CohtmlHud __instance, CohtmlView ___hudView)
        {
            HudAugmentMod.Logger.Msg("Adding an event to CohtmlHud RegisterEvents");
            ___hudView.View.BindCall("SL1PlayAudio", new Action<string>(HudAugmentMod.PlayAudio));

            _hudView = ___hudView;
        }

        static void PushList_PostFixPatch(ViewManager __instance, ViewManager.UiEventTypes t)
        {
            if (t != ViewManager.UiEventTypes.FriendsList) return;

            if (!_hudView)
            {
                HudAugmentMod.Logger.Error($"_hudView is null, impossible to send friends to hud");

                return;
            }

            _hudView.View.TriggerEvent("SL1LoadFriends", Friends.List);
        }
    }
}
