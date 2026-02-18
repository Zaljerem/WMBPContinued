using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace BiomesKit;

[StaticConstructorOnStartup]
public static class StartupCheck
{
	static StartupCheck()
	{
		foreach (BiomeDef item in DefDatabase<BiomeDef>.AllDefsListForReading.Where((BiomeDef x) => x.HasModExtension<BiomesKitControls>()))
		{
			BiomesKitControls modExtension = item.GetModExtension<BiomesKitControls>();
			HashSet<BiomeDef> hashSet = new HashSet<BiomeDef>();
			foreach (BiomeDef spawnOnBiome in modExtension.spawnOnBiomes)
			{
				if (!hashSet.Add(spawnOnBiome))
				{
					Log.Warning("[BiomesKit] XML Config Error: " + item?.ToString() + ": spawnOnBiomes includes " + spawnOnBiome?.ToString() + " twice.");
				}
			}
			if (modExtension.materialPath != "World/MapGraphics/Default")
			{
				MaterialPool.MatFrom(modExtension.materialPath, ShaderDatabase.WorldOverlayTransparentLit, modExtension.materialLayer);
			}
			if (modExtension.materialLayer >= 3560)
			{
				Log.Warning("[BiomesKit] XML Config Error: " + item?.ToString() + ": The materialLayer is set to 3560 or higher, making the material display on top of the selection indicator.");
			}
			if (!modExtension.allowOnLand && !modExtension.allowOnWater)
			{
				Log.Warning("[BiomesKit] XML Config Error: " + item?.ToString() + ": Biome is disallowed on both land and water and will never spawn.");
			}
			if (modExtension.minTemperature > modExtension.maxTemperature)
			{
				Log.Warning("[BiomesKit] XML Config Error: " + item?.ToString() + ": minTemperature set above maxTemperature.");
			}
			if (modExtension.minNorthLatitude > modExtension.maxNorthLatitude)
			{
				Log.Warning("[BiomesKit] XML Config Error: " + item?.ToString() + ": minNorthLatitude set above maxNorthLatitude.");
			}
			if (modExtension.minSouthLatitude > modExtension.maxSouthLatitude)
			{
				Log.Warning("[BiomesKit] XML Config Error: " + item?.ToString() + ": minSouthLatitude set above maxSouthLatitude.");
			}
			if ((int)modExtension.minHilliness > (int)modExtension.maxHilliness)
			{
				Log.Warning("[BiomesKit] XML Config Error: " + item?.ToString() + ": minHilliness set above maxHilliness.");
			}
			if (modExtension.minElevation > modExtension.maxElevation)
			{
				Log.Warning("[BiomesKit] XML Config Error: " + item?.ToString() + ": minElevation set above maxElevation.");
			}
			if (modExtension.minRainfall > modExtension.maxRainfall)
			{
				Log.Warning("[BiomesKit] XML Config Error: " + item?.ToString() + ": minRainfall set above maxRainfall.");
			}
			if (modExtension.frequency > 100)
			{
				Log.Warning("[BiomesKit] XML Config Error: " + item?.ToString() + ": frequency set above 100. Frequency accepts values 1-100. Setting Frequency higher than that is not supported.");
			}
			if (!modExtension.usePerlin && modExtension.useAlternativePerlinSeedPreset)
			{
				Log.Warning("[BiomesKit] XML Config Error: " + item?.ToString() + ": usePerlin is false but useAlternativePerlinSeedPreset is true. useAlternativePerlinSeedPreset should be false if usePerlin is set to false.");
			}
			if (!modExtension.usePerlin && modExtension.perlinCustomSeed.HasValue)
			{
				Log.Warning("[BiomesKit] XML Config Error: " + item?.ToString() + ": usePerlin is false but perlinCustomSeed is assigned. perlinCustomSeed will not be read if usePerlin is set to false.");
			}

			ExtensionCheck(item.defName, modExtension);
		}
	}

	private static void ExtensionCheck(string biomeDefName, BiomesKitControls modExt) {
		if (modExt.uniqueHills) {
			string path = "WorldMaterials/BiomesKit/" + biomeDefName + "/Hills/";

			if (CanHaveRegularHill(modExt.snowpilesBelow)) {
				modExt.SmallHillsMat = TryGetMaterial(path, "SmallHills", true) ??
				                       GetDefaultMaterial("SmallHills");
				modExt.LargeHillsMat = TryGetMaterial(path, "LargeHills", true) ??
				                       GetDefaultMaterial("LargeHills");
			}

			if (GetTextureViable(modExt.snowpilesBelow)) {
				modExt.SmallSnowpilesMat = TryGetMaterial(path, "SmallSnowpiles", true);
				modExt.LargeSnowpilesMat = TryGetMaterial(path, "LargeSnowpiles", true);
			}

			if (CanHaveRegularHill(modExt.mountainsSnowyBelow)) {
				modExt.MountainsMat = TryGetMaterial(path, "Mountains", true);
			}

			if (GetTextureViable(modExt.mountainsSnowyBelow))
				modExt.Mountains_SnowyMat = TryGetMaterial(path, "Mountains_Snowy", true) ??
				                             GetDefaultMaterial("Mountains");
			if (GetTextureViable(modExt.mountainsFullySnowyBelow))
				modExt.Mountains_FullySnowyMat = TryGetMaterial(path, "Mountains_FullySnowy", true) ??
				                                  GetDefaultMaterial("Mountains");
			if (GetTextureViable(modExt.mountainsVerySnowyBelow))
				modExt.Mountains_VerySnowyMat = TryGetMaterial(path, "Mountains_VerySnowy", true) ??
				                                 GetDefaultMaterial("Mountains");
			if (GetTextureViable(modExt.mountainsSemiSnowyBelow))
				modExt.Mountains_SemiSnowyMat = TryGetMaterial(path, "Mountains_SemiSnowy", true) ??
				                                 GetDefaultMaterial("Mountains");

			if (CanHaveRegularHill(modExt.impassableSnowyBelow)) {
				modExt.ImpassableMat = TryGetMaterial(path, "Impassable", true);
			}

			if (GetTextureViable(modExt.impassableSnowyBelow))
				modExt.Impassable_SnowyMat = TryGetMaterial(path, "Impassable_Snowy", true) ??
				                              GetDefaultMaterial("Impassable");
			if (GetTextureViable(modExt.impassableFullySnowyBelow))
				modExt.Impassable_FullySnowyMat = TryGetMaterial(path, "Impassable_FullySnowy", true) ??
				                                   GetDefaultMaterial("Impassable");
			if (GetTextureViable(modExt.impassableVerySnowyBelow))
				modExt.Impassable_VerySnowyMat = TryGetMaterial(path, "Impassable_VerySnowy", true) ??
				                                  GetDefaultMaterial("Impassable");
			if (GetTextureViable(modExt.impassableSemiSnowyBelow))
				modExt.Impassable_SemiSnowyMat = TryGetMaterial(path, "Impassable_SemiSnowy", true) ??
				                                  GetDefaultMaterial("Impassable");
		}

		if (modExt.forested) {
			string path = "WorldMaterials/BiomesKit/" + biomeDefName + "/Forest/Forest";
			modExt.Forest_Mat = TryGetMaterial(path, "", false);

			if (GetTextureViable(modExt.forestSnowyBelow)) {
				modExt.Forest_SnowyMat = TryGetMaterial(path, "_Snowy", false);
				if (GetForestDenseTextureViable(modExt.forestDenseAbove)) {
					modExt.Forest_SnowyDenseMat = TryGetMaterial(path, "_SnowyDense", false);
				}
				if (GetTextureViable(modExt.forestSparseBelow)) {
					modExt.Forest_SnowySparseMat = TryGetMaterial(path, "_SnowySparse", false);
				}
			}

			if (GetTextureViable(modExt.forestSparseBelow)) {
				modExt.Forest_SparseMat = TryGetMaterial(path, "_Sparse", false);
			}

			if (GetForestDenseTextureViable(modExt.forestDenseAbove)) {
				modExt.Forest_DenseMat = TryGetMaterial(path, "_Dense", false);
			}
		}
	}


	/// <summary>
	/// Checks to see if a terrain/hill variant is possible.
	/// Default value is -9999
	/// </summary>
	/// <returns>true if the texture should exist</returns>
	private static bool GetTextureViable(float temperatureViable) {
		return !Mathf.Approximately(temperatureViable, -9999f);
	}

	/// <summary>
	/// forestDense is unique in that it uses a positive 9999
	/// </summary>
	private static bool GetForestDenseTextureViable(float temperatureViable) {
		return !Mathf.Approximately(temperatureViable, 9999f);
	}

	/// <summary>
	/// Colder biomes use the temperature 999 for their hillSnowBelow temperature.
	/// If this is the case, it doesn't need a regular hill texture
	/// </summary>
	private static bool CanHaveRegularHill(float temperatureViable) {
		return !Mathf.Approximately(temperatureViable, 999f);
	}

	private static Material TryGetMaterial(string path, string addition, bool isHill) {
		return MaterialPool.MatFrom(path + addition,
			ShaderDatabase.WorldOverlayTransparentLit,
			isHill ? 3600 : 3515);
	}

	/// <returns>default texture path</returns>
	private static Material GetDefaultMaterial(string path) {
		return MaterialPool.MatFrom("WorldMaterials/BiomesKit/Default/Hills/" + path,
			ShaderDatabase.WorldOverlayTransparentLit,
			3515);
	}
}
