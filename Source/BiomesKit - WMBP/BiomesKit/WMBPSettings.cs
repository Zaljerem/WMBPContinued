using UnityEngine;
using Verse;

namespace BiomesKit;

public class WMBPSettings : ModSettings
{
	
	public bool enabled = true;

	public void DoWindowContents(Rect wrect)
	{
		Listing_Standard listing_Standard = new Listing_Standard();
		listing_Standard.Begin(wrect);		
		listing_Standard.CheckboxLabeled("WMBP_Enabled".Translate(), ref enabled);
		listing_Standard.End();
	}

	public override void ExposeData()
	{
		base.ExposeData();		
		Scribe_Values.Look(ref enabled, "enabled", true);
	}
}
