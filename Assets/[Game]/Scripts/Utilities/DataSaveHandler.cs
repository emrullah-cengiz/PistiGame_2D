using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class DataSaveHandler<T>
{
    private readonly string SaveFilePath;

    public DataSaveHandler(string saveFilePath)
    {
        SaveFilePath = saveFilePath;
    }

    public async UniTask<(bool status, T data)> Load()
    {
        try
        {
            var json = await File.ReadAllTextAsync(SaveFilePath);

            if (string.IsNullOrWhiteSpace(json)) 
                return (false, default);
            
            var data = JsonUtility.FromJson<T>(json);

            return (true, data);
        }
        catch (FileNotFoundException)
        {
            Debug.Log("Save file not found, returning null..");
            return (false, default);
        }
    }

    public async UniTask Save(T data)
    {
        var json = JsonUtility.ToJson(data, true);
        await File.WriteAllTextAsync(SaveFilePath, json);
    }
}