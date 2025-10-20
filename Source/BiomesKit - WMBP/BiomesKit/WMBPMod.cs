using HarmonyLib;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace BiomesKit;

public class WMBPMod : Mod
{
	public static WMBPSettings settings;

    public static bool worldBeautificationToggle = true;
    public static bool WorldBeautificationToggle
    {
        get
        {
            return worldBeautificationToggle;
        }
        set
        {
            worldBeautificationToggle = value;
            foreach (PlanetLayer planetLayer in Find.WorldGrid.PlanetLayers.Values) {
                planetLayer.WorldDrawLayers.Find(f => f.GetType() == typeof(WorldDrawLayer_Hills))?.SetDirty();
            }

        }
    }

    public WMBPMod(ModContentPack content)
		: base(content)
	{
		settings = GetSettings<WMBPSettings>();
		new Harmony("zal.wmbp").PatchAll();
	}

	public override void DoSettingsWindowContents(Rect inRect)
	{
		base.DoSettingsWindowContents(inRect);
		settings.DoWindowContents(inRect);
	}

	public override string SettingsCategory()
	{
		return "WMBP_ModName".Translate();
	}
}