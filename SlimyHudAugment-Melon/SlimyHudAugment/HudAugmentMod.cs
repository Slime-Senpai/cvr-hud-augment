using ABI_RC.Core.AudioEffects;
using ABI_RC.Core.InteractionSystem;
using ABI_RC.Core.UI;
using MelonLoader;
using UnityEngine;
using System.Reflection;
using HarmonyLib;

namespace SlimyHubAugment
{
    public class HudAugmentMod : MelonMod
    {
        public override void OnApplicationStart()
        {
            // Plugin startup logic
            MelonLogger.Msg($"Plugin SlimyHudAugment is loaded in version 1.0.0!");
            MelonLogger.Msg($"Plugin SlimyHudAugment requires you to use a custom HUD UI. Otherwise the mod is useless.");

            var _harmonyInstance = HarmonyInstance;

            MethodInfo original = AccessTools.Method(typeof(CohtmlHud), "RegisterEvents");

            MethodInfo patch = AccessTools.Method(typeof(HudAugmentPatcher), "RegisterEvents_PostFixPatch");

            _harmonyInstance.Patch(original, null, new HarmonyMethod(patch));

            MethodInfo original2 = AccessTools.Method(typeof(ViewManager), "PushList");

            MethodInfo patch2 = AccessTools.Method(typeof(HudAugmentPatcher), "PushList_PostFixPatch");

            _harmonyInstance.Patch(original2, null, new HarmonyMethod(patch2));
        }

        public static void PlayAudio(string sound)
        {
            MelonLogger.Msg($"PlayAudio triggered with sound {sound}");
            InterfaceAudio.PlayModule(sound);
        }
    }
}
