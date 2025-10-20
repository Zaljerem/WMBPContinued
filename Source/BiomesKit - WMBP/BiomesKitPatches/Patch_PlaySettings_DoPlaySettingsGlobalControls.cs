using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using BiomesKit;


namespace BiomesKitPatches
{
    [HarmonyPatch(typeof(PlaySettings), "DoWorldViewControls")]
    public static class Patch_PlaySettings_DoWorldViewControls
    {
        public static void Postfix(WidgetRow row)
        {
            if (!WMBPMod.settings.enabled)
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