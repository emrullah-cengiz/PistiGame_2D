using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class DataSaveHandler<T> where T : struct
{
    private readonly string SaveFilePath;

    public DataSaveHandler(string saveFilePath)
    {
        SaveFilePath = saveFilePath;
    }

    public async UniTask<T?> Load()
    {
        try
        {
            var json = await File.ReadAllTextAsync(SaveFilePath);

            var data = JsonUtility.FromJson<T?>(json);
            if (data != null) 
                return data;
            
            Debug.LogError("Json deserialize failed, returning null. Json string: " + json);
            return null;
        }
        catch (FileNotFoundException)
        {
            Debug.Log("Save file not found, returning null..");
            return null;
        }
    }

    public async UniTask Save(T data)
    {
        var json = JsonUtility.ToJson(data, true);
        await File.WriteAllTextAsync(SaveFilePath, json);
    }
}