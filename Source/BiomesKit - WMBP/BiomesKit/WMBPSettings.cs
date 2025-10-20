using System.EnterpriseServices.CompensatingResourceManager;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace BiomesKit;

public class WMBPSettings : ModSettings
{
	
    public bool enabled = true;
	
    private bool _cachedEnabled;

    public bool displayDefault = true;
	
    private bool _cachedDisplay;

    public void DoWindowContents(Rect wrect)
    {
        Listing_Standard listing_Standard = new Listing_Standard();
        listing_Standard.Begin(wrect);		
        listing_Standard.CheckboxLabeled("WMBP_Enabled".Translate(), ref enabled);
        if (_cachedEnabled != enabled) {
            if (Current.ProgramState == ProgramState.Playing) {
                //If in game need to refresh hill layers
                foreach (PlanetLayer planetLayer in Find.WorldGrid.PlanetLayers.Values) {
                    planetLayer.WorldDrawLayers.Find(f => f.GetType() == typeof(WorldDrawLayer_Hills))?.SetDirty();
                }
            }
            _cachedEnabled = enabled;
        }
        listing_Standard.CheckboxLabeled("WMBP_DisplayDefault".Translate(), ref displayDefault);
        if (_cachedDisplay != displayDefault) {
            if (Current.ProgramState == ProgramState.Playing) {
                //If in game need to refresh beautify layers
                foreach (PlanetLayer planetLayer in Find.WorldGrid.PlanetLayers.Values) {
                    planetLayer.WorldDrawLayers.Find(f => f.GetType() == typeof(WorldDrawLayer_Beautification))?.SetDirty();
                }
            }
            _cachedDisplay = displayDefault;
        }
        listing_Standard.End();
    }

    public override void ExposeData()
    {
        base.ExposeData();		
        Scribe_Values.Look(ref enabled, "enabled", true);
        _cachedEnabled = enabled;
        Scribe_Values.Look(ref displayDefault, "displayDefault", true);
        _cachedDisplay = displayDefault;
    }
}