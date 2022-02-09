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
     * JsonSerializer�����b�v����N���X�B
     * JsonSerializer���g�p����CRUD
     * �t�@�C���̏������ރ��\�b�h�𑵂���B�^�C�~���O�͗��p����N���X�B
     */
    /// <summary>
    /// json�I�u�W�F�N�g�̐���
    /// </summary>
    /// <typeparam name="T">�Y������Json�t�H�[�}�b�g�N���X</typeparam>
    /// <param name="jsonClass">�f�[�^��ێ�����json�t�H�[�}�b�g�N���X</param>
    /// <returns>json�I�u�W�F�N�g</returns>
    public static string GenerateJsonObject<T>(T jsonClass)
    {
        var json = JsonSerializer.ToJsonString(jsonClass);

        return json;
    }

    /// <summary>
    /// �t�@�C���������킹���f�o�C�X���Ƃ̃t�@�C���ۑ���̐�΃p�X�𐶐�����B
    /// </summary>
    /// <param name="fileName">�ۑ�����t�@�C����</param>
    /// <returns>��΃p�X</returns>
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
    /// �Y�������΃p�X�ɁA�f�[�^���������ށB
    /// </summary>
    /// <param name="fileName">�ۑ�����t�@�C����</param>
    /// <param name="json">json�I�u�W�F�N�g : string</param>
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
    /// StreamingAssets����e�L�X�g�f�[�^��ǂݍ����string�ŕԂ��B
    /// </summary>
    /// <param name="fileName">�g���q���܂ރt�@�C����</param>
    /// <returns>�e�L�X�g�f�[�^</returns>
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