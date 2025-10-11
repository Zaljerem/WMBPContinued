using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using BiomesKit;


namespace BiomesKitPatches
{
    [HarmonyPatch(typeof(PlaySettings), nameof(PlaySettings.DoPlaySettingsGlobalControls))]
    public static class Patch_PlaySettings_DoPlaySettingsGlobalControls
    {
        public static void Postfix(WidgetRow row, bool worldView)
        {
            if (worldView is false || WMBPMod.settings.enabled is false)
            {
                return;
            }
            bool showWorldLayers = WMBPMod.WorldBeautificationToggle;
            var toggleTexture = ContentFinder<Texture2D>.Get("UI/Icons/WMB_Toggle");
            row.ToggleableIcon(ref showWorldLayers, toggleTexture, "WMBP_ToggleWorldMapBeautification".Translate(), SoundDefOf.Mouseover_ButtonToggle);
            if (showWorldLayers != WMBPMod.WorldBeautificationToggle)
            {
                WMBPMod.WorldBeautificationToggle = showWorldLayers;
            }
        }
    }
}
