using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

using Utf8Json;

public static class JsonManager
{
    /*
     * JsonSerializerをラップするクラス。
     * JsonSerializerを使用したCRUD
     * ファイルの書き込むメソッドを揃える。タイミングは利用するクラス。
     */
    /// <summary>
    /// jsonオブジェクトの生成
    /// </summary>
    /// <typeparam name="T">該当するJsonフォーマットクラス</typeparam>
    /// <param name="jsonClass">データを保持したjsonフォーマットクラス</param>
    /// <returns>jsonオブジェクト</returns>
    public static string GenerateJsonObject<T>(T jsonClass)
    {
        var json = JsonSerializer.ToJsonString(jsonClass);

        return json;
    }

    /// <summary>
    /// ファイル名を合わせたデバイスごとのファイル保存先の絶対パスを生成する。
    /// </summary>
    /// <param name="fileName">保存するファイル名</param>
    /// <returns>絶対パス</returns>
    public static string GetPersistentDataPath(string fileName)
    {
        var path = Path.Combine(Application.persistentDataPath, fileName);

        return path;
    }
    public static string GetStreamingDataPath(string fileName)
    {
        var path = Path.Combine(Application.streamingAssetsPath, fileName);

        return path;
    }

    /// <summary>
    /// 該当する絶対パスに、データを書き込む。
    /// </summary>
    /// <param name="fileName">保存するファイル名</param>
    /// <param name="json">jsonオブジェクト : string</param>
    public static void WriteJsonData(string fileName, string json)
    {
        var path = GetPersistentDataPath(fileName);

        File.WriteAllText(path, json);
    }

    public static string ReadJsonData(string fileName)
    {
        var path = GetPersistentDataPath(fileName);

        if (!File.Exists(path))
        {
            var errorMsg = fileName + " is not exits !";
            Debug.LogError(errorMsg);
            return "-1";
        }

        var json = File.ReadAllText(path);

        return json;
    }
    public static string ReadJsonDataResources(string fileName)
    {
        var data = Resources.Load(fileName) as TextAsset;
        string te = data.text;

        StringBuilder sb = new StringBuilder();
        foreach (var text in te.Split('\n'))
        {
            sb.Append(text);
        }
        var json = sb.ToString();

        return json;
    }

    /// <summary>
    /// StreamingAssetsからテキストデータを読み込んでstringで返す。
    /// </summary>
    /// <param name="fileName">拡張子を含むファイル名</param>
    /// <returns>テキストデータ</returns>
    public static string ReadJsonDataStreamingAssets(string fileName)
    {
        var path = GetStreamingDataPath(fileName);

        if (!File.Exists(path))
        {
            var errorMsg = fileName + " is not exits !";
            Debug.LogError(errorMsg);
            return "-1";
        }

        var json = File.ReadAllText(path);

        return json;
    }

    public static T ExpandJsonData<T>(string json)
    {
        var data = JsonSerializer.Deserialize<T>(json);

        return data;
    }
}