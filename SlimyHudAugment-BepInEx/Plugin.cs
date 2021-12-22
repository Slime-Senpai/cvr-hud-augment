using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using ABI_RC.Core.UI;
using ABI_RC.Core.AudioEffects;
using ABI_RC.Core.InteractionSystem;
using ABI_RC.Core.Networking.IO.Social;
using cohtml;
using System;
using System.Reflection;

namespace SlimyHudAugment
{
    [BepInPlugin("fr.slimesenpai.plugins.hudaugment", "SlimyHudAugment", "1.0.0")]
    [BepInProcess("ChilloutVR.exe")]
    public class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log;

        private void Awake()
        {
            Plugin.Log = base.Logger;
            // Plugin startup logic
            Plugin.Log.LogInfo($"Plugin SlimyHudAugment is loaded in version 1.0.0!");
            Plugin.Log.LogInfo($"Plugin SlimyHudAugment requires you to use a custom HUD UI. Otherwise the mod is useless.");

            var _harmonyInstance = new Harmony("fr.slimesenpai.plugins.hudaugment.patch");

            MethodInfo original = AccessTools.Method(typeof(CohtmlHud), "RegisterEvents");

            MethodInfo patch = AccessTools.Method(typeof(HudAugmentPatcher), "RegisterEvents_PostFixPatch");

            _harmonyInstance.Patch(original, null, new HarmonyMethod(patch));

            MethodInfo original2 = AccessTools.Method(typeof(ViewManager), "PushList");

            MethodInfo patch2 = AccessTools.Method(typeof(HudAugmentPatcher), "PushList_PostFixPatch");

            _harmonyInstance.Patch(original2, null, new HarmonyMethod(patch2));
        }

        public static void PlayAudio(string sound)
        {
            Plugin.Log.LogInfo($"PlayAudio triggered with sound {sound}");
            InterfaceAudio.PlayModule(sound);
        }
    }

    class HudAugmentPatcher
    {
        private static CohtmlView _hudView;


        static void RegisterEvents_PostFixPatch(CohtmlHud __instance, CohtmlView ___hudView)
        {
            Plugin.Log.LogInfo("Adding an event to CohtmlHud RegisterEvents");
            ___hudView.View.BindCall("SL1PlayAudio", new Action<string>(Plugin.PlayAudio));

            _hudView = ___hudView;
        }

        static void PushList_PostFixPatch(ViewManager __instance, ViewManager.UiEventTypes t)
        {
            if (t != ViewManager.UiEventTypes.FriendsList) return;

            if (!_hudView) {
                Plugin.Log.LogError($"_hudView is null, impossible to send friends to hud");

                return;
            }

			_hudView.View.TriggerEvent("SL1LoadFriends", Friends.List);
        }
    }
}
