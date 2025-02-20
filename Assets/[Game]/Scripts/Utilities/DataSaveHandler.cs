using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class DataSaveHandler<T> where T : class, new()
{
    private readonly string SaveFilePath;

    public DataSaveHandler(string saveFilePath)
    {
        SaveFilePath = saveFilePath;
    }

    public async UniTask<T> Load()
    {
        var json = await File.ReadAllTextAsync(SaveFilePath);
        return JsonUtility.FromJson<T>(json) ?? new T();
    }

    public async UniTask Save(T data)
    {
        var json = JsonUtility.ToJson(data, true);
        await File.WriteAllTextAsync(SaveFilePath, json);
    }
}