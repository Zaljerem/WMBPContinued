using System.Collections;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace BiomesKit;

public class WorldDrawLayer_Beautification : WorldDrawLayer
{
    public override bool Visible {
        get
        {
            if (base.Visible) {
                if (WMBPMod.settings.enabled &&
                    WMBPMod.WorldBeautificationToggle) {
                    return true;
                }
            }

            return false;
        }
    }

    public override IEnumerable Regenerate() {
        foreach (object item in base.Regenerate()) {
            yield return item;
        }

        Rand.PushState();
        Rand.Seed = Find.World.info.Seed;

        List<Tile> worldGrid = planetLayer.Tiles;
        for (int i = 0; i < worldGrid.Count; i++) {
            Tile singleTile = worldGrid[i];
            SurfaceTile surfaceTile = null;
            // Other planetLayers may not have a SurfaceTile
            if (worldGrid[i] is SurfaceTile)
                surfaceTile = (SurfaceTile)worldGrid[i];

            Vector3 tileCenter = Find.WorldGrid.GetTileCenter(worldGrid[i].tile);


            if (singleTile.PrimaryBiome?.GetModExtension<BiomesKitControls>() is { } modExt) {
                bool noRoads = !singleTile.PrimaryBiome.allowRoads || surfaceTile?.potentialRoads == null;
                bool noRivers = !singleTile.PrimaryBiome.allowRivers || surfaceTile?.potentialRivers == null;


                // --- Unique Hills ---
                if (noRoads && noRivers) {
                    if (modExt.uniqueHills) {
                        Material hillMaterial = null;


                        switch (singleTile.hilliness) {
                            case Hilliness.Flat:
                                hillMaterial = null;
                                break;
                            case Hilliness.SmallHills:
                                hillMaterial = singleTile.temperature < modExt.snowpilesBelow
                                    ? modExt.SmallSnowpilesMat
                                    : modExt.SmallHillsMat;
                                break;
                            case Hilliness.LargeHills:
                                hillMaterial = singleTile.temperature < modExt.snowpilesBelow
                                    ? modExt.LargeSnowpilesMat
                                    : modExt.LargeHillsMat;
                                break;
                            case Hilliness.Mountainous:
                                hillMaterial = SnowSuffix(singleTile.temperature, modExt.mountainsSemiSnowyBelow,
                                    modExt.mountainsSnowyBelow, modExt.mountainsVerySnowyBelow,
                                    modExt.mountainsFullySnowyBelow) switch {
                                    0 => modExt.MountainsMat,
                                    1 => modExt.Mountains_SemiSnowyMat,
                                    2 => modExt.Mountains_SnowyMat,
                                    3 => modExt.Mountains_VerySnowyMat,
                                    4 => modExt.Mountains_FullySnowyMat,
                                    _ => null
                                };
                                break;
                            case Hilliness.Impassable:
                                hillMaterial = SnowSuffix(singleTile.temperature, modExt.impassableSemiSnowyBelow,
                                    modExt.impassableSnowyBelow, modExt.impassableVerySnowyBelow,
                                    modExt.impassableFullySnowyBelow) switch {
                                    0 => modExt.ImpassableMat,
                                    1 => modExt.Impassable_SemiSnowyMat,
                                    2 => modExt.Impassable_SnowyMat,
                                    3 => modExt.Impassable_VerySnowyMat,
                                    4 => modExt.Impassable_FullySnowyMat,
                                    _ => null
                                };
                                break;
                        }

                        if (hillMaterial != null) {
                            var subMesh = GetSubMesh(hillMaterial);

                            WorldRendererUtility.PrintQuadTangentialToPlanet(tileCenter, tileCenter,
                                singleTile.Layer.AverageTileSize * modExt.materialSizeMultiplier, 0.005f, subMesh,
                                false,
                                0.01f, false);
                            WorldRendererUtility.PrintTextureAtlasUVs(Rand.Range(0, 2), Rand.Range(0, 2), 2, 2,
                                subMesh);
                        }
                    }
                    else {
                        string hillPath = singleTile.hilliness switch {
                            Hilliness.SmallHills => "WorldMaterials/BiomesKit/Default/Hills/SmallHills",
                            Hilliness.LargeHills => "WorldMaterials/BiomesKit/Default/Hills/LargeHills",
                            Hilliness.Mountainous => "WorldMaterials/BiomesKit/Default/Hills/Mountains",
                            Hilliness.Impassable => "WorldMaterials/BiomesKit/Default/Hills/Impassable",
                            _ => null
                        };

                        if (hillPath != null) {
                            Material mat = MaterialPool.MatFrom(hillPath, ShaderDatabase.WorldOverlayTransparentLit,
                                3515);
                            var subMesh = GetSubMesh(mat);
                            WorldRendererUtility.PrintQuadTangentialToPlanet(tileCenter, tileCenter,
                                singleTile.Layer.AverageTileSize, 0.005f, subMesh, false, Rand.Range(0f, 360f),
                                false);
                            WorldRendererUtility.PrintTextureAtlasUVs(Rand.Range(0, 2), Rand.Range(0, 2), 2, 2,
                                subMesh);
                        }
                    }


                    // --- Forested Plains ---
                    if (modExt.forested && singleTile.hilliness == Hilliness.Flat) {
                        Material forestMaterial = modExt.Forest_Mat;

                        if (singleTile.rainfall < modExt.forestSparseBelow) {
                            forestMaterial = modExt.Forest_SparseMat;
                        }
                        else if (singleTile.rainfall > modExt.forestDenseAbove) {
                            forestMaterial = modExt.Forest_DenseMat;
                        }

                        if (singleTile.temperature < modExt.forestSnowyBelow) {
                            if (singleTile.rainfall < modExt.forestSparseBelow) {
                                forestMaterial = modExt.Forest_SnowySparseMat;
                            }
                            else if (singleTile.rainfall > modExt.forestDenseAbove) {
                                forestMaterial = modExt.Forest_SnowyDenseMat;
                            }
                            else {
                                forestMaterial = modExt.Forest_SnowyMat;
                            }
                        }

                        if (forestMaterial != null) {
                            var subMesh = GetSubMesh(forestMaterial);
                            WorldRendererUtility.PrintQuadTangentialToPlanet(tileCenter, tileCenter,
                                singleTile.Layer.AverageTileSize * modExt.materialSizeMultiplier, 0.005f, subMesh,
                                false,
                                0.01f,
                                false);
                            WorldRendererUtility.PrintTextureAtlasUVs(Rand.Range(0, 2), Rand.Range(0, 2), 2, 2,
                                subMesh);
                        }
                    }
                }

                // --- Custom Material Overlay ---
                if (modExt.materialPath != "World/MapGraphics/Default") {
                    Material mat = MaterialPool.MatFrom(modExt.materialPath, ShaderDatabase.WorldOverlayTransparentLit,
                        3515);
                    var subMesh = GetSubMesh(mat);
                    WorldRendererUtility.PrintQuadTangentialToPlanet(tileCenter, tileCenter,
                        singleTile.Layer.AverageTileSize * modExt.materialSizeMultiplier, 0.005f, subMesh, false, 0.01f,
                        false);
                    WorldRendererUtility.PrintTextureAtlasUVs(Rand.Range(0, 2), Rand.Range(0, 2), 2, 2, subMesh);
                }
            }
            else if (WMBPMod.settings.displayDefault) {
                //Fallback for if biome does not have any textures.
                //Will revert to base game graphics
                //Setting Enabled by default
                string hillPath = singleTile.hilliness switch {
                    Hilliness.SmallHills => "WorldMaterials/BiomesKit/Default/Hills/SmallHills",
                    Hilliness.LargeHills => "WorldMaterials/BiomesKit/Default/Hills/LargeHills",
                    Hilliness.Mountainous => "WorldMaterials/BiomesKit/Default/Hills/Mountains",
                    Hilliness.Impassable => "WorldMaterials/BiomesKit/Default/Hills/Impassable",
                    _ => null
                };

                if (hillPath != null) {
                    Material mat = MaterialPool.MatFrom(hillPath, ShaderDatabase.WorldOverlayTransparentLit,
                        3515);
                    var subMesh = GetSubMesh(mat);
                    WorldRendererUtility.PrintQuadTangentialToPlanet(tileCenter, tileCenter,
                        singleTile.Layer.AverageTileSize, 0.005f, subMesh, false, Rand.Range(0f, 360f),
                        false);
                    WorldRendererUtility.PrintTextureAtlasUVs(Rand.Range(0, 2), Rand.Range(0, 2), 2, 2, subMesh);
                }
            }
        }

        Rand.PopState();
        FinalizeMesh(MeshParts.All);

        yield break; // must yield because Regenerate is IEnumerable
    }


    private static int SnowSuffix(float temp, float semi, float snowy, float verySnowy, float fullySnowy) {
        if (temp < fullySnowy) return 4;
        if (temp < verySnowy) return 3;
        if (temp < snowy) return 2;
        if (temp < semi) return 1;
        return 0;
    }
}