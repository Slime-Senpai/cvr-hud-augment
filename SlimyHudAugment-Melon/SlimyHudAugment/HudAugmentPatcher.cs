using ABI_RC.Core.UI;
using ABI_RC.Core.InteractionSystem;
using ABI_RC.Core.Networking.IO.Social;
using cohtml;
using System;
using MelonLoader;
using System.Collections.Generic;

namespace SlimyHubAugment
{
    class HudAugmentPatcher
    {
        private static CohtmlView _hudView;


        static void RegisterEvents_PostFixPatch(CohtmlHud __instance, CohtmlView ___hudView)
        {
            MelonLogger.Msg("Adding an event to CohtmlHud RegisterEvents");
            ___hudView.View.BindCall("SL1PlayAudio", new Action<string>(HudAugmentMod.PlayAudio));

            _hudView = ___hudView;
        }

        static void RequestFriendsListTask_PostFixPatch(List<Friend_t> ____friends)
        {
            if (!_hudView)
            {
                MelonLogger.Error($"_hudView is null, impossible to send friends to hud");

                return;
            }

            _hudView.View.TriggerEvent("SL1LoadFriends", ____friends);
        }
    }
}
