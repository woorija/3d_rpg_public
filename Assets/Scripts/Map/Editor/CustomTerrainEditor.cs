using UnityEditor;
using UnityEngine;

public static class FakeTerrainSettingContextMenu
{
    [MenuItem("CONTEXT/Terrain/Apply Fake Terrain Settings")]
    static void ApplyFakeTerrainSettings(MenuCommand command)
    {
        Terrain terrain = (Terrain)command.context;
        if (terrain == null)
        {
            return;
        }

        var terrainData = terrain.terrainData;
        Vector3 originSize = terrainData.size;
        Undo.RecordObject(terrain, "Apply Fake Terrain Settings");

        terrain.allowAutoConnect = false;
        terrain.heightmapPixelError = 40f;
        terrain.basemapDistance = 150f;
        terrain.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        terrain.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;

        terrain.bakeLightProbesForTrees = false;
        terrain.detailObjectDistance = 10f;
        terrain.detailObjectDensity = 0.5f;
        terrain.treeMaximumFullLODCount = 10;
        terrain.treeDistance = 25f;
        terrain.treeBillboardDistance = 10f;

        terrainData.wavingGrassSpeed = 0f;
        terrainData.wavingGrassStrength = 0f;
        terrainData.wavingGrassAmount = 0f;

        terrainData.SetDetailResolution(128, 8);

        ResampleHeightmap(terrainData, 129);
        ResampleAlphamap(terrainData, 128);
        terrainData.baseMapResolution = 256;

        terrainData.size = originSize;

        EditorUtility.SetDirty(terrain);
        EditorUtility.SetDirty(terrainData);

        Debug.Log("더미 땅 데이터 적용 완료");
    }
    [MenuItem("CONTEXT/Terrain/Apply Map Terrain Settings")]
    static void ApplyMapTerrainSettings(MenuCommand command)
    {
        Terrain terrain = (Terrain)command.context;
        if (terrain == null)
        {
            return;
        }

        var terrainData = terrain.terrainData;
        Vector3 originSize = terrainData.size;
        Undo.RecordObject(terrain, "Apply Map Terrain Settings");

        terrain.allowAutoConnect = false;
        terrain.heightmapPixelError = 5f;
        terrain.basemapDistance = 1000f;
        terrain.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
        terrain.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.BlendProbesAndSkybox;

        terrain.bakeLightProbesForTrees = false;
        terrain.detailObjectDistance = 80f;
        terrain.detailObjectDensity = 1f;
        terrain.treeMaximumFullLODCount = 50;
        terrain.treeDistance = 150f;
        terrain.treeBillboardDistance = 60f;

        terrainData.wavingGrassSpeed = 0.5f;
        terrainData.wavingGrassStrength = 0.5f;
        terrainData.wavingGrassAmount = 0.5f;

        terrainData.SetDetailResolution(1024, 32);

        ResampleHeightmap(terrainData, 513);
        ResampleAlphamap(terrainData, 512);
        terrainData.baseMapResolution = 1024;

        terrainData.size = originSize;

        EditorUtility.SetDirty(terrain);
        EditorUtility.SetDirty(terrainData);

        Debug.Log("맵 데이터 적용 완료");
    }
    static void ResampleHeightmap(TerrainData _terrainData, int _newResolution)
    {
        if (_terrainData == null)
        {
            return;
        }
        int originResolution = _terrainData.heightmapResolution;
        if(originResolution == _newResolution)
        {
            return;
        }

        var originHeights = _terrainData.GetHeights(0, 0, originResolution, originResolution);
        var newHeights = new float[_newResolution, _newResolution];

        float ratio = (float)(originResolution - 1) / (_newResolution - 1);

        for (int y = 0; y < _newResolution; y++)
        {
            float srcY = y * ratio;
            int y0 = Mathf.FloorToInt(srcY);
            int y1 = Mathf.Min(y0 + 1, originResolution - 1);
            float ty = srcY - y0;

            for (int x = 0; x < _newResolution; x++)
            {
                float srcX = x * ratio;
                int x0 = Mathf.FloorToInt(srcX);
                int x1 = Mathf.Min(x0 + 1, originResolution - 1);
                float tx = srcX - x0;

                // Bilinear interpolation
                float h00 = originHeights[y0, x0];
                float h10 = originHeights[y0, x1];
                float h01 = originHeights[y1, x0];
                float h11 = originHeights[y1, x1];

                float hx0 = Mathf.Lerp(h00, h10, tx);
                float hx1 = Mathf.Lerp(h01, h11, tx);
                newHeights[y, x] = Mathf.Lerp(hx0, hx1, ty);
            }
        }

        _terrainData.heightmapResolution = _newResolution;
        _terrainData.SetHeights(0, 0, newHeights);
    }

    static void ResampleAlphamap(TerrainData _terrainData, int _newResolution)
    {
        if (_terrainData == null)
        {
            return;
        }
        int originResolution = _terrainData.alphamapResolution;
        if (originResolution == _newResolution)
        {
            return;
        }

        int layerCount = _terrainData.alphamapLayers;
        var originAlpha = _terrainData.GetAlphamaps(0, 0, originResolution, originResolution);
        var newAlpha = new float[_newResolution, _newResolution, layerCount];

        float ratio = (float)(originResolution - 1) / (_newResolution - 1);

        for (int y = 0; y < _newResolution; y++)
        {
            float srcY = y * ratio;
            int y0 = Mathf.FloorToInt(srcY);
            int y1 = Mathf.Min(y0 + 1, originResolution - 1);
            float ty = srcY - y0;

            for (int x = 0; x < _newResolution; x++)
            {
                float srcX = x * ratio;
                int x0 = Mathf.FloorToInt(srcX);
                int x1 = Mathf.Min(x0 + 1, originResolution - 1);
                float tx = srcX - x0;

                for (int layer = 0; layer < layerCount; layer++)
                {
                    float a00 = originAlpha[y0, x0, layer];
                    float a10 = originAlpha[y0, x1, layer];
                    float a01 = originAlpha[y1, x0, layer];
                    float a11 = originAlpha[y1, x1, layer];

                    float ax0 = Mathf.Lerp(a00, a10, tx);
                    float ax1 = Mathf.Lerp(a01, a11, tx);
                    newAlpha[y, x, layer] = Mathf.Lerp(ax0, ax1, ty);
                }
            }
        }

        _terrainData.alphamapResolution = _newResolution;
        _terrainData.SetAlphamaps(0, 0, newAlpha);
    }
}
