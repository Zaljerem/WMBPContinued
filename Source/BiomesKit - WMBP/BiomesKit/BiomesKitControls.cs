using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace BiomesKit;

public class BiomesKitControls : DefModExtension
{
	public List<BiomeDef> spawnOnBiomes = new List<BiomeDef>();

	public int? biomePriority;
	
	//field is not utilized in any mods
	public string materialPath = "World/MapGraphics/Default";

	public bool forested;

	public Material Forest_Mat;
	
	public Material Forest_SparseMat;

	public Material Forest_DenseMat;

	public Material Forest_SnowyMat;

	public Material Forest_SnowySparseMat;
	
	public Material Forest_SnowyDenseMat;

	public bool uniqueHills;

	public Material SmallHillsMat;
	
	public Material LargeHillsMat;
	
	public Material MountainsMat;
	
	public Material ImpassableMat;

	public float forestSnowyBelow = -9999f;

	public float forestSparseBelow = -9999f;

	public float forestDenseAbove = 9999f;

	//field is not utilized in any mods
	public int materialLayer = 3515;

	public float smallHillSizeMultiplier = 1.5f;

	public float largeHillSizeMultiplier = 2f;

	public float mountainSizeMultiplier = 1.4f;

	public float impassableSizeMultiplier = 1.3f;

	public float materialSizeMultiplier = 1f;

	public float materialOffset = 1f;

	public bool materialRandomRotation = true;

	public Hilliness materialMinHilliness;

	public Hilliness materialMaxHilliness;

	public bool allowOnWater;

	public bool allowOnLand = true;

	public bool setNotWaterCovered;

	public int minimumWaterNeighbors;

	public int minimumLandNeighbors;

	public bool needRiver;

	public bool randomizeHilliness;

	public float minTemperature = -999f;

	public float maxTemperature = 999f;

	public float minElevation = -9999f;

	public float maxElevation = 9999f;

	public float? setElevation;

	public float minNorthLatitude = -9999f;

	public float maxNorthLatitude = -9999f;

	public float minSouthLatitude = -9999f;

	public float maxSouthLatitude = -9999f;

	public Hilliness minHilliness = Hilliness.Flat;

	public Hilliness maxHilliness = Hilliness.Impassable;

	public BiomesKitHilliness? spawnHills;

	public BiomesKitHilliness? setHills;

	public BiomesKitHilliness? minRandomHills = BiomesKitHilliness.Flat;

	public BiomesKitHilliness? maxRandomHills = BiomesKitHilliness.Mountainous;

	public float snowpilesBelow = -9999f;
	
	public Material SmallSnowpilesMat;
	
	public Material LargeSnowpilesMat;

	public float mountainsSemiSnowyBelow = -9999f;
	
	public Material Mountains_SemiSnowyMat;

	public float mountainsSnowyBelow = -9999f;
	
	public Material Mountains_SnowyMat;

	public float mountainsVerySnowyBelow = -9999f;
	
	public Material Mountains_VerySnowyMat;

	public float mountainsFullySnowyBelow = -9999f;
	
	public Material Mountains_FullySnowyMat;

	public float impassableSemiSnowyBelow = -9999f;
	
	public Material Impassable_SemiSnowyMat;

	public float impassableSnowyBelow = -9999f;
	
	public Material Impassable_SnowyMat;

	public float impassableVerySnowyBelow = -9999f;
	
	public Material Impassable_VerySnowyMat;

	public float impassableFullySnowyBelow = -9999f;
	
	public Material Impassable_FullySnowyMat;

	public float minRainfall = -9999f;

	public float maxRainfall = 9999f;

	public int frequency = 100;

	public bool useAlternativePerlinSeedPreset;

	public bool usePerlin;

	public int? perlinCustomSeed;

	public float perlinCulling = 0.99f;

	public double perlinFrequency;

	public double perlinLacunarity;

	public double perlinPersistence;

	public int perlinOctaves;
}
