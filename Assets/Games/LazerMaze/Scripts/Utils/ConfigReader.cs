using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ConfigReader
{
    public static async Task<LevelConfig> ReadConfigAsync(string addressableKey)
    {
        var handle = Addressables.LoadAssetAsync<TextAsset>(addressableKey);
        await handle.Task;

        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("Erreur de chargement de l'Addressable : " + addressableKey);
            return null;
        }

        string json = handle.Result.text;
        return ParseConfig(json);
    }

    private static LevelConfig ParseConfig(string json)
    {
        LevelConfigData config = JsonUtility.FromJson<LevelConfigData>(json);

        return LevelConfig.FromLevelConfigData(config);
    }
}
