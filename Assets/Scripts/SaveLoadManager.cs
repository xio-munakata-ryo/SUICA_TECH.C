using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveLoadManager
{
    private static string SaveDirectory
        => Path.Combine(Application.persistentDataPath, "SaveData");

    private static void EnsureDirectory()
    {
        if (!Directory.Exists(SaveDirectory))
        {
            Directory.CreateDirectory(SaveDirectory);
            Debug.Log($"Save directory created: {SaveDirectory}");
        }
    }

    private static string GetFilePath(string fileName)
    {
        EnsureDirectory();
        return Path.Combine(SaveDirectory, fileName + ".json");
    }

    // ===== セーブ（上書き） =====
    public static void Save<T>(string fileName, T data)
    {
        string path = GetFilePath(fileName);

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json); // 存在すれば上書き、なければ新規作成

        Debug.Log($"Saved to: {path} data:{json}");
    }

    // ===== ロード =====
    public static T Load<T>(string fileName, T defaultValue = default)
    {
        string path = GetFilePath(fileName);

        if (!File.Exists(path))
        {
            Debug.LogWarning($"Save file not found: {path}");
            return defaultValue;
        }

        string json = File.ReadAllText(path);

        try
        {
            return JsonUtility.FromJson<T>(json);
        }
        catch(System.Exception ex)
        {
            Debug.LogWarning(ex.Message);
            // データ連結部の問題により、ゲームの進行ができなくならないように、Throwをしない
            return defaultValue;
        }
    }

    // ===== 存在チェック =====
    public static bool Exists(string fileName)
    {
        string path = GetFilePath(fileName);
        return File.Exists(path);
    }

    // ===== 削除 =====
    public static void Delete(string fileName)
    {
        string path = GetFilePath(fileName);

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"Deleted: {path}");
        }
    }
}
