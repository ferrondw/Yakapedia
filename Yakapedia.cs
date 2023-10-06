using System.Security.Cryptography;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;

public static class Yakapedia
{
    private static string CurrentSaveFile
    {
        get
        {
            return Path.Combine(Application.persistentDataPath, "Saves", Application.productName.Replace(' ', '_') + "_SaveFile_" + (PlayerPrefs.HasKey("SelectedSave") ? PlayerPrefs.GetInt("SelectedSave") : 0) + ".bin");
        }
    }

    private static string GetSaveFilePath(int index)
    {
        return Path.Combine(Application.persistentDataPath, "Saves", Application.productName.Replace(' ', '_') + "_SaveFile_" + index + ".bin");
    }

    private static readonly string JsonEncryptionKey = GenerateDeviceSpecificKey();

    private static Dictionary<string, object> values;
    private static StringBuilder stringBuilder = new();

    #region BetterPlayerPrefs

    #region Public Get / Set Methods
    /// <summary>
    /// Saves a string value into the currently selected save file. You can use Yakapedia.GetString to retrieve this value.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    /// <param name="value">The value to set.</param>
    public static void SetString(string key, string value)
    {
        InitValues();
        SetData(key, value);
    }

    /// <summary>
    /// Returns a string value corresponding to key in the currently selected save file if it exists.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    /// <param name="defaultValue">The value to retrieve.</param>
    /// <returns></returns>
    public static string GetString(string key, string defaultValue = default)
    {
        InitValues();
        return GetData<string>(key, defaultValue);
    }

    /// <summary>
    /// Saves an int value into the currently selected save file. You can use Yakapedia.GetInt to retrieve this value.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    /// <param name="value">The value to set.</param>
    public static void SetInt(string key, int value)
    {
        InitValues();
        SetData(key, value);
    }

    /// <summary>
    /// Returns an int value corresponding to key in the currently selected save file if it exists.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    /// <param name="defaultValue">The value to retrieve.</param>
    /// <returns></returns>
    public static int GetInt(string key, int defaultValue = default)
    {
        InitValues();
        return GetData<int>(key, defaultValue);
    }

    /// <summary>
    /// Saves a float value into the currently selected save file. You can use Yakapedia.GetFloat to retrieve this value.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    /// <param name="value">The value to set.</param>
    public static void SetFloat(string key, float value)
    {
        InitValues();
        SetData<float>(key, value);
    }

    /// <summary>
    /// Returns a float value corresponding to key in the currently selected save file if it exists.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    /// <param name="defaultValue">The value to retrieve.</param>
    /// <returns></returns>
    public static float GetFloat(string key, float defaultValue = default)
    {
        InitValues();
        return GetData(key, defaultValue);
    }

    /// <summary>
    /// Saves a bool value into the currently selected save file. You can use Yakapedia.GetBool to retrieve this value.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    /// <param name="value">The value to set.</param>
    public static void SetBool(string key, bool value)
    {
        InitValues();
        values[key] = value ? "1" : "0";
        SaveData();
    }

    /// <summary>
    /// Returns a bool value corresponding to key in the currently selected save file if it exists.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    /// <param name="defaultValue">The value to retrieve.</param>
    /// <returns></returns>
    public static bool GetBool(string key, bool defaultValue = false)
    {
        InitValues();
        if (values.TryGetValue(key, out object value))
        {
            string valueStr = (string)value;
            return valueStr == "1";
        }
        return defaultValue;
    }

    /// <summary>
    /// Toggles a bool value into the currently selected save file.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    public static void ToggleBool(string key)
    {
        SetBool(key, !GetBool(key));

    }

    /// <summary>
    /// Saves a Vector2 value into the currently selected save file. You can use Yakapedia.GetVector2 to retrieve this value.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    /// <param name="value">The value to set.</param>
    public static void SetVector2(string key, Vector2 value)
    {
        InitValues();
        string vector2String = $"{value.x},{value.y}";
        SetString(key, vector2String);
    }

    /// <summary>
    /// Returns a Vector2 value corresponding to key in the currently selected save file if it exists.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    /// <param name="defaultValue">The value to retrieve.</param>
    /// <returns></returns>
    public static Vector2 GetVector2(string key, Vector2 defaultValue = default)
    {
        InitValues();
        string vector2String = GetString(key);
        if (!string.IsNullOrEmpty(vector2String))
        {
            string[] vector2Parts = vector2String.Split(',');
            if (vector2Parts.Length == 2 &&
                float.TryParse(vector2Parts[0], out float x) &&
                float.TryParse(vector2Parts[1], out float y))
            {
                return new Vector2(x, y);
            }
        }
        return defaultValue;
    }

    /// <summary>
    /// Saves a Vector3 value into the currently selected save file. You can use Yakapedia.GetVector3 to retrieve this value.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    /// <param name="value">The value to set.</param>
    public static void SetVector3(string key, Vector3 value)
    {
        InitValues();
        string vector3String = $"{value.x},{value.y},{value.z}";
        SetString(key, vector3String);
    }

    /// <summary>
    /// Returns a Vector3 value corresponding to key in the currently selected save file if it exists.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    /// <param name="defaultValue">The value to retrieve.</param>
    /// <returns></returns>
    public static Vector3 GetVector3(string key, Vector3 defaultValue = default)
    {
        InitValues();
        string vector3String = GetString(key);
        if (!string.IsNullOrEmpty(vector3String))
        {
            string[] vector3Parts = vector3String.Split(',');
            if (vector3Parts.Length == 3 &&
                float.TryParse(vector3Parts[0], out float x) &&
                float.TryParse(vector3Parts[1], out float y) &&
                float.TryParse(vector3Parts[2], out float z))
            {
                return new Vector3(x, y, z);
            }
        }
        return defaultValue;
    }

    /// <summary>
    /// Saves a Quaternion value into the currently selected save file. You can use Yakapedia.GetQuaternion to retrieve this value.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    /// <param name="value">The value to set.</param>
    public static void SetQuaternion(string key, Quaternion value)
    {
        InitValues();
        string quaternionString = $"{value.x},{value.y},{value.z},{value.w}";
        SetString(key, quaternionString);
    }

    /// <summary>
    /// Returns a Quaternion value corresponding to key in the currently selected save file if it exists.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    /// <param name="defaultValue">The value to retrieve.</param>
    /// <returns></returns>
    public static Quaternion GetQuaternion(string key, Quaternion defaultValue = default)
    {
        InitValues();
        string quaternionString = GetString(key);
        if (!string.IsNullOrEmpty(quaternionString))
        {
            string[] quaternionParts = quaternionString.Split(',');
            if (quaternionParts.Length == 4 &&
                float.TryParse(quaternionParts[0], out float x) &&
                float.TryParse(quaternionParts[1], out float y) &&
                float.TryParse(quaternionParts[2], out float z) &&
                float.TryParse(quaternionParts[3], out float w))
            {
                return new Quaternion(x, y, z, w);
            }
        }
        return defaultValue;
    }

    /// <summary>
    /// Saves a Color value into the currently selected save file. You can use Yakapedia.GetColor to retrieve this value.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    /// <param name="value">The value to set.</param>
    public static void SetColor(string key, Color value)
    {
        InitValues();
        string colorString = ColorUtility.ToHtmlStringRGBA(value);
        SetString(key, colorString);
    }

    /// <summary>
    /// Returns a Color value corresponding to key in the currently selected save file if it exists.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    /// <param name="defaultValue">The value to retrieve.</param>
    /// <returns></returns>
    public static Color GetColor(string key, Color defaultValue = default)
    {
        InitValues();
        string colorString = GetString(key);
        if (!string.IsNullOrEmpty(colorString))
        {
            Color color;
            if (ColorUtility.TryParseHtmlString(colorString, out color))
            {
                return color;
            }
        }
        return defaultValue;
    }

    /// <summary>
    /// Saves an int array value into the currently selected save file. You can use Yakapedia.GetIntArray to retrieve this value.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    /// <param name="value">The value to set.</param>
    public static void SetIntArray(string key, int[] value)
    {
        InitValues();
        string array = string.Join(",", value);
        SetString(key, array);
    }

    /// <summary>
    /// Returns an int array value corresponding to key in the currently selected save file if it exists.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    /// <param name="defaultValue">The value to retrieve.</param>
    /// <returns>An int array from the selected save file using the key.</returns>
    public static int[] GetIntArray(string key, int[] defaultValue = default)
    {
        InitValues();
        string[] stringArray = GetString(key).Split(',');
        int[] intArray = new int[stringArray.Length];
        for (int i = 0; i < stringArray.Length; i++)
        {
            intArray[i] = int.Parse(stringArray[i]);
        }
        return intArray;
    }

    /// <summary>
    /// Saves a float array value into the currently selected save file. You can use Yakapedia.GetFloatArray to retrieve this value.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    /// <param name="value">The value to set.</param>
    public static void SetFloatArray(string key, float[] value)
    {
        InitValues();
        string array = string.Join(",", value);
        SetString(key, array);
    }

    /// <summary>
    /// Returns a float array value corresponding to key in the currently selected save file if it exists.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    /// <param name="defaultValue">The value to retrieve.</param>
    /// <returns>A float array from the selected save file using the key.</returns>
    public static float[] GetFloatArray(string key, float[] defaultValue = default)
    {
        InitValues();
        string[] stringArray = GetString(key).Split(',');
        float[] floatArray = new float[stringArray.Length];
        for (int i = 0; i < stringArray.Length; i++)
        {
            floatArray[i] = int.Parse(stringArray[i]);
        }
        return floatArray;
    }

    /// <summary>
    /// Saves a string array value into the currently selected save file. You can use Yakapedia.GetStringArray to retrieve this value.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    /// <param name="value">The value to set.</param>
    public static void SetStringArray(string key, string[] value)
    {
        InitValues();
        string arrayString = string.Join("\0", value);
        SetString(key, arrayString);
    }

    /// <summary>
    /// Returns a string array value corresponding to key in the currently selected save file if it exists.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    /// <param name="defaultValue">The value to retrieve.</param>
    /// <returns>A string array from the selected save file using the key.</returns>
    public static string[] GetStringArray(string key, string[] defaultValue = default)
    {
        InitValues();
        string arrayString = GetString(key);
        if (!string.IsNullOrEmpty(arrayString))
        {
            return arrayString.Split('\0');
        }
        return defaultValue;
    }

    /// <summary>
    /// Saves a bool array value into the currently selected save file. You can use Yakapedia.GetBoolArray to retrieve this value.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    /// <param name="value">The value to set.</param>
    public static void SetBoolArray(string key, bool[] value)
    {
        InitValues();
        string arrayString = string.Join(",", value);
        SetString(key, arrayString);
    }

    /// <summary>
    /// Returns a bool array value corresponding to key in the currently selected save file if it exists.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    /// <param name="defaultValue">The value to retrieve.</param>
    /// <returns>A bool array from the selected save file using the key.</returns>
    public static bool[] GetBoolArray(string key, bool[] defaultValue = default)
    {
        InitValues();
        string arrayString = GetString(key);
        if (!string.IsNullOrEmpty(arrayString))
        {
            string[] boolStrings = arrayString.Split(',');
            bool[] boolArray = new bool[boolStrings.Length];
            for (int i = 0; i < boolStrings.Length; i++)
            {
                if (bool.TryParse(boolStrings[i], out bool boolValue))
                {
                    boolArray[i] = boolValue;
                }
                else
                {
                    boolArray[i] = defaultValue != null ? defaultValue[i] : false;
                }
            }
            return boolArray;
        }
        return defaultValue;
    }

    /// <summary>
    /// Saves a quaternion array value into the currently selected save file. You can use Yakapedia.GetQuaternionArray to retrieve this value.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    /// <param name="value">The value to set.</param>
    public static void SetQuaternionArray(string key, Quaternion[] value)
    {
        InitValues();
        string arrayString = string.Join(";", value);
        SetString(key, arrayString);
    }

    /// <summary>
    /// Returns a quaternion array value corresponding to key in the currently selected save file if it exists.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    /// <param name="defaultValue">The value to retrieve.</param>
    /// <returns>A quaternion array from the selected save file using the key.</returns>
    public static Quaternion[] GetQuaternionArray(string key, Quaternion[] defaultValue = default)
    {
        InitValues();
        string arrayString = GetString(key);
        if (!string.IsNullOrEmpty(arrayString))
        {
            string[] quaternionStrings = arrayString.Split(';');
            Quaternion[] quaternionArray = new Quaternion[quaternionStrings.Length];
            for (int i = 0; i < quaternionStrings.Length; i++)
            {
                string[] quaternionParts = quaternionStrings[i].Split(',');
                if (quaternionParts.Length == 4 &&
                    float.TryParse(quaternionParts[0], out float x) &&
                    float.TryParse(quaternionParts[1], out float y) &&
                    float.TryParse(quaternionParts[2], out float z) &&
                    float.TryParse(quaternionParts[3], out float w))
                {
                    quaternionArray[i] = new Quaternion(x, y, z, w);
                }
                else
                {
                    quaternionArray[i] = defaultValue != null ? defaultValue[i] : Quaternion.identity;
                }
            }
            return quaternionArray;
        }
        return defaultValue;
    }

    /// <summary>
    /// Saves a color array value into the currently selected save file. You can use Yakapedia.GetColorArray to retrieve this value.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    /// <param name="value">The value to set.</param>
    public static void SetColorArray(string key, Color[] value)
    {
        InitValues();
        string arrayString = string.Join(";", value);
        SetString(key, arrayString);
    }

    /// <summary>
    /// Returns a color array value corresponding to key in the currently selected save file if it exists.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    /// <param name="defaultValue">The value to retrieve.</param>
    /// <returns>A color array from the selected save file using the key.</returns>
    public static Color[] GetColorArray(string key, Color[] defaultValue = default)
    {
        InitValues();
        string arrayString = GetString(key);
        if (!string.IsNullOrEmpty(arrayString))
        {
            string[] colorStrings = arrayString.Split(';');
            Color[] colorArray = new Color[colorStrings.Length];
            for (int i = 0; i < colorStrings.Length; i++)
            {
                if (ColorUtility.TryParseHtmlString(colorStrings[i], out Color color))
                {
                    colorArray[i] = color;
                }
                else
                {
                    colorArray[i] = defaultValue != null ? defaultValue[i] : Color.white;
                }
            }
            return colorArray;
        }
        return defaultValue;
    }

    /// <summary>
    /// Saves a Vector2 array value into the currently selected save file. You can use Yakapedia.GetVector2Array to retrieve this value.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    /// <param name="value">The value to set.</param>
    public static void SetVector2Array(string key, Vector2[] value)
    {
        InitValues();
        string arrayString = string.Join(";", value);
        SetString(key, arrayString);
    }

    /// <summary>
    /// Returns a Vector2 array value corresponding to key in the currently selected save file if it exists.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    /// <param name="defaultValue">The value to retrieve.</param>
    /// <returns>A Vector2 array from the selected save file using the key.</returns>
    public static Vector2[] GetVector2Array(string key, Vector2[] defaultValue = default)
    {
        InitValues();
        string arrayString = GetString(key);
        if (!string.IsNullOrEmpty(arrayString))
        {
            string[] vector2Strings = arrayString.Split(';');
            Vector2[] vector2Array = new Vector2[vector2Strings.Length];
            for (int i = 0; i < vector2Strings.Length; i++)
            {
                string[] vector2Parts = vector2Strings[i].Split(',');
                if (vector2Parts.Length == 2 &&
                    float.TryParse(vector2Parts[0], out float x) &&
                    float.TryParse(vector2Parts[1], out float y))
                {
                    vector2Array[i] = new Vector2(x, y);
                }
                else
                {
                    vector2Array[i] = defaultValue != null ? defaultValue[i] : Vector2.zero;
                }
            }
            return vector2Array;
        }
        return defaultValue;
    }

    /// <summary>
    /// Saves a Vector3 array value into the currently selected save file. You can use Yakapedia.GetVector3Array to retrieve this value.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    /// <param name="value">The value to set.</param>
    public static void SetVector3Array(string key, Vector3[] value)
    {
        InitValues();
        string arrayString = string.Join(";", value);
        SetString(key, arrayString);
    }

    /// <summary>
    /// Returns a Vector3 array value corresponding to key in the currently selected save file if it exists.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    /// <param name="defaultValue">The value to retrieve.</param>
    /// <returns>A Vector3 array from the selected save file using the key.</returns>
    public static Vector3[] GetVector3Array(string key, Vector3[] defaultValue = default)
    {
        InitValues();
        string arrayString = GetString(key);
        if (!string.IsNullOrEmpty(arrayString))
        {
            string[] vector3Strings = arrayString.Split(';');
            Vector3[] vector3Array = new Vector3[vector3Strings.Length];
            for (int i = 0; i < vector3Strings.Length; i++)
            {
                string[] vector3Parts = vector3Strings[i].Split(',');
                if (vector3Parts.Length == 3 &&
                    float.TryParse(vector3Parts[0], out float x) &&
                    float.TryParse(vector3Parts[1], out float y) &&
                    float.TryParse(vector3Parts[2], out float z))
                {
                    vector3Array[i] = new Vector3(x, y, z);
                }
                else
                {
                    vector3Array[i] = defaultValue != null ? defaultValue[i] : Vector3.zero;
                }
            }
            return vector3Array;
        }
        return defaultValue;
    }

    /// <summary>
    /// Checks if the key exists in the currently selected save file.
    /// </summary>
    /// <param name="key">The key for the data entry.</param>
    /// <returns>True if the key exists; otherwise, false.</returns>
    public static bool HasKey(string key)
    {
        InitValues();
        return values.ContainsKey(key);
    }

    /// <summary>
    /// Deletes the data entry with the specified key.
    /// </summary>
    /// <param name="key">The key of the data entry to delete.</param>
    public static void DeleteKey(string key)
    {
        InitValues();
        values.Remove(key);
        SaveData();
    }

    #endregion

    #region Public Save File Methods
    /// <summary>
    /// Select a save file, if it does not exist it will be made. This also reloads the current scene so all variables can be properly loaded in.
    /// </summary>
    /// <param name="index">Index of the save file that needs to be loaded</param>
    public static void SelectSaveFile(int index)
    {
        PlayerPrefs.SetInt("SelectedSave", index);

        //InitValues checks if your dictionary is empty, which it is not when loading a new save file, so this just forces the dictionary to update with the new save file.
        //This took me a few hours to figure out i'm about to cry
        values = new Dictionary<string, object>();
        if (File.Exists(CurrentSaveFile))
        {
            using (StreamReader reader = new(CurrentSaveFile))
            {
                string encryptedData = reader.ReadToEnd();
                string decryptedData = RijndealDecrypt(encryptedData, JsonEncryptionKey);
                Dictionary<string, object> data;
                if (decryptedData != null)
                {
                    data = JsonConvert.DeserializeObject<Dictionary<string, object>>(decryptedData);

                }
                else
                {
                    data = new();
                }

                foreach (KeyValuePair<string, object> pair in data)
                {
                    values[pair.Key] = pair.Value;
                }
            }
        }
        else
        {
            SaveData();
        }

        ReloadCurrentScene();
    }

    /// <summary>
    /// Deletes a save file based on index, and then loads another save file, which by default is 0.
    /// </summary>
    /// <param name="index">Index number of the save file to delete.</param>
    /// <param name="saveToLoad">Which save to load once the other one is deleted.</param>
    public static void DeleteSaveFile(int index, int saveToLoad = 0)
    {
        if (File.Exists(GetSaveFilePath(index)))
        {
            File.Delete(GetSaveFilePath(index));
        }

        SelectSaveFile(saveToLoad);
        ReloadCurrentScene();
    }

    /// <summary>
    /// Deletes the current save file.
    /// </summary>
    /// <param name="saveToLoad">Which save to load once the current one is deleted.</param>
    public static void DeleteCurrentSaveFile(int saveToLoad = 0)
    {
        if (File.Exists(CurrentSaveFile))
        {
            File.Delete(CurrentSaveFile);
        }

        SelectSaveFile(saveToLoad);
        ReloadCurrentScene();
    }

    /// <summary>
    /// Checks if a save file is empty or does not exist at all.
    /// </summary>
    /// <param name="index">The index of the save file to check</param>
    /// <returns>True if the save file does not exist or is empty; otherwise, false.</returns>
    public static bool IsSaveFileEmpty(int index)
    {
        string path = GetSaveFilePath(index);

        if (File.Exists(path))
        {
            using (StreamReader reader = new(path))
            {
                string data = reader.ReadToEnd();
                if (data == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        return true;
    }
    #endregion

    private static void InitValues()
    {
        if (values == null)
        {
            values = new Dictionary<string, object>();
            if (File.Exists(CurrentSaveFile))
            {
                using (StreamReader reader = new(CurrentSaveFile))
                {
                    string encryptedData = reader.ReadToEnd();
                    string decryptedData = RijndealDecrypt(encryptedData, JsonEncryptionKey);
                    Dictionary<string, object> data;
                    if (decryptedData != null)
                    {
                        data = JsonConvert.DeserializeObject<Dictionary<string, object>>(decryptedData);

                    }
                    else
                    {
                        data = new();
                    }

                    foreach (KeyValuePair<string, object> pair in data)
                    {
                        values[pair.Key] = pair.Value;
                    }
                }
            }
            else
            {
                SaveData();
            }
        }
    }

    private static void SetData<T>(string key, T value)
    {
        values[key] = value;
        SaveData();
    }

    private static T GetData<T>(string key, T defaultValue)
    {
        if (values.ContainsKey(key))
        {
            object val = values[key];
            return (T)Convert.ChangeType(val, typeof(T));
        }
        return defaultValue;
    }

    private static void SaveData()
    {
        string jsonData = JsonConvert.SerializeObject(values);
        string encryptedData = RijndealEncrypt(jsonData, JsonEncryptionKey);

        if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "Saves")))
        {
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "Saves"));
        }
        File.WriteAllText(CurrentSaveFile, encryptedData);
    }

    private static string GenerateDeviceSpecificKey()
    {
        string deviceId = SystemInfo.deviceUniqueIdentifier;


        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(deviceId));
            StringBuilder stringBuilder = new();

            for (int i = 0; i < hashedBytes.Length; i++)
            {
                stringBuilder.Append(hashedBytes[i].ToString("x2")); // hexadecimal format (this is a reminder for myself that this is possible)
            }

            string deviceSpecificKey = stringBuilder.ToString();

            deviceSpecificKey = deviceSpecificKey.Substring(0, 16);
            deviceSpecificKey = deviceSpecificKey.Insert(2, "-");
            deviceSpecificKey += "@";

            return deviceSpecificKey;
        }
    }

    private static string RijndealEncrypt(string plain, string password)
    {
        byte[] encrypted;
        RijndaelManaged rijndael = SetupRijndaelManaged;
        Rfc2898DeriveBytes deriveBytes = new(password, 32);
        byte[] salt = new byte[32];
        salt = deriveBytes.Salt;
        byte[] bufferKey = deriveBytes.GetBytes(32);

        rijndael.Key = bufferKey;
        rijndael.GenerateIV();

        using (ICryptoTransform encrypt = rijndael.CreateEncryptor(rijndael.Key, rijndael.IV))
        {
            byte[] dest = encrypt.TransformFinalBlock(Encoding.UTF8.GetBytes(plain), 0, Encoding.UTF8.GetBytes(plain).Length);
            List<byte> compile = new(salt);
            compile.AddRange(rijndael.IV);
            compile.AddRange(dest);
            encrypted = compile.ToArray();
        }

        return Convert.ToBase64String(encrypted);
    }

    private static string RijndealDecrypt(string encrypted, string password)
    {
        byte[] decrypted;

        RijndaelManaged rijndael = SetupRijndaelManaged;

        List<byte> compile = new(Convert.FromBase64String(encrypted));

        List<byte> salt = compile.GetRange(0, 32);
        List<byte> iv = compile.GetRange(32, 32);
        rijndael.IV = iv.ToArray();

        Rfc2898DeriveBytes deriveBytes = new(password, salt.ToArray());
        byte[] bufferKey = deriveBytes.GetBytes(32);
        rijndael.Key = bufferKey;

        byte[] plain = compile.GetRange(32 * 2, compile.Count - (32 * 2)).ToArray();

        using (ICryptoTransform decrypt = rijndael.CreateDecryptor(rijndael.Key, rijndael.IV))
        {
            byte[] dest = decrypt.TransformFinalBlock(plain, 0, plain.Length);
            decrypted = dest;
        }

        return Encoding.UTF8.GetString(decrypted);
    }

    private static RijndaelManaged SetupRijndaelManaged
    {
        get
        {
            RijndaelManaged rijndael = new()
            {
                BlockSize = 256,
                KeySize = 256,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };
            return rijndael;
        }
    }
    #endregion

    #region Vibration (Android only)

#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
    public static AndroidJavaClass unityPlayer;
    public static AndroidJavaObject currentActivity;
    public static AndroidJavaObject vibrator;
#endif

    /// <summary>
    /// Starts vibration
    /// </summary>
    public static void StartVibration()
    {
        if (IsAndroid())
            vibrator.Call("vibrate");
        else
            Handheld.Vibrate();
    }

    /// <summary>
    /// Stops vibration
    /// </summary>
    public static void StopVibration()
    {
        if (IsAndroid())
            vibrator.Call("cancel");
    }

    /// <summary>
    /// Starts vibration for a fixed amount of milliseconds
    /// </summary>
    /// <param name="milliseconds">Amount of milliseonds to vibrate for</param>
    public static void Vibrate(long milliseconds)
    {
        if (IsAndroid())
            vibrator.Call("vibrate", milliseconds);
        else
            Handheld.Vibrate();
    }

    /// <summary>
    /// Starts vibration with a pattern, i really don't know how this works so i'm not going to explain further
    /// </summary>
    /// <param name="pattern"></param>
    /// <param name="repeat"></param>
    public static void Vibrate(long[] pattern, int repeat)
    {
        if (IsAndroid())
            vibrator.Call("vibrate", pattern, repeat);
        else
            Handheld.Vibrate();
    }

    /// <summary>
    /// Checks if you are on Android
    /// </summary>
    /// <returns>True if you are on Android; otherwise, false.</returns>
    public static bool IsAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
	return true;
#else
        return false;
#endif
    }
    #endregion

    #region MethodExtenders
    /// <summary>
    /// Checks if an item is present in the array.
    /// </summary>
    /// <typeparam name="T">The type of items in the array.</typeparam>
    /// <param name="array">The array to check.</param>
    /// <param name="item">The item to check for.</param>
    /// <returns>True if the item is present in the array; otherwise, false.</returns>
    public static bool ArrayContainsItem<T>(this T[] array, T item)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (EqualityComparer<T>.Default.Equals(array[i], item))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Sorts the array in ascending order.
    /// </summary>
    /// <typeparam name="T">The type of items in the array.</typeparam>
    /// <param name="array">The array to sort.</param>
    /// <returns>A new array with the elements sorted in ascending order.</returns>
    public static T[] SortArray<T>(this T[] array)
    {
        T[] newArray = new T[array.Length];
        Array.Copy(array, newArray, array.Length);
        Array.Sort(newArray);
        return newArray;
    }


    /// <summary>
    /// Inverts the input color (Maybe use this for better readability, idk im not smart)
    /// </summary>
    /// <param name="inputColor">Color to invert</param>
    /// <returns>The inverse of the input color</returns>
    public static Color InvertColor(this Color inputColor)
    {
        Color inverseColor = new(1 - inputColor.r, 1 - inputColor.g, 1 - inputColor.b, inputColor.a);
        return inverseColor;
    }

    /// <summary>
    /// Destroys all child objects from a parent object.
    /// </summary>
    /// <param name="parent">Parent object to remove all child objects from.</param>
    public static void DestroyAllChildren(this Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            UnityEngine.Object.Destroy(parent.GetChild(0).gameObject);
        }
    }

    /// <summary>
    /// Rounds the given value to the nearest multiple of the specified increment.
    /// </summary>
    /// <param name="value">The value to be rounded.</param>
    /// <param name="increment">The increment to round to.</param>
    /// <returns>The rounded value.</returns>
    public static float RoundToNearest(this float value, float increment)
    {
        return Mathf.Round(value / increment) * increment;
    }

    /// <summary>
    /// Finds the closest object of type T to the specified position.
    /// </summary>
    /// <typeparam name="T">The type of MonoBehaviour to search for.</typeparam>
    /// <param name="position">The position to measure the distance from.</param>
    /// <returns>The closest object of type T to the specified position, or null if none are found.</returns>
    public static T GetClosestObjectOfType<T>(this Transform position) where T : MonoBehaviour
    {
        T[] objectsOfType = UnityEngine.Object.FindObjectsOfType<T>();
        if (objectsOfType.Length == 0) return null;
        T closest = objectsOfType[0];
        float closestDistance = Vector2.Distance(position.position, closest.transform.position);
        for (int i = 1; i < objectsOfType.Length; i++)
        {
            float distance = Vector2.Distance(position.position, objectsOfType[i].transform.position);
            if (distance < closestDistance)
            {
                closest = objectsOfType[i];
                closestDistance = distance;
            }
        }
        return closest;
    }

    /// <summary>
    /// Clamps the given angle value between the specified minimum and maximum angles.
    /// </summary>
    /// <param name="angle">The angle to be clamped.</param>
    /// <param name="min">The minimum allowed angle.</param>
    /// <param name="max">The maximum allowed angle.</param>
    /// <returns>The clamped angle value.</returns>
    public static float ClampAngle(this float angle, float min, float max)
    {
        angle = Mathf.Repeat(angle, 360f);
        if (angle > 180f)
        {
            angle -= 360f;
        }
        return Mathf.Clamp(angle, min, max);
    }

    /// <summary>
    /// Retrieves all components of type T found in the GameObject hierarchy under the given root GameObject.
    /// </summary>
    /// <typeparam name="T">The type of component to search for.</typeparam>
    /// <param name="root">The root GameObject to search under.</param>
    /// <returns>An array of components of type T found in the GameObject hierarchy under the root.</returns>
    public static T[] GetAllComponentsUnderParent<T>(this GameObject parent)
    {
        List<T> list = new();
        foreach (Transform transform in parent.GetComponentsInChildren<Transform>())
        {
            T component = transform.GetComponent<T>();
            if (component != null)
            {
                list.Add(component);
            }
        }
        return list.ToArray();
    }

    /// <summary>
    /// Shuffles the elements in the given array into a random order.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The array to be shuffled.</param>
    public static void ShuffleArray<T>(this T[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            T temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }

    /// <summary>
    /// Creates an AudioSource component with specified settings and attaches it to a game object.
    /// </summary>
    /// <param name="gameObject">The game object to attach the AudioSource to.</param>
    /// <param name="clip">The AudioClip to set on the AudioSource.</param>
    /// <param name="volume">The volume of the AudioSource.</param>
    /// <param name="loop">Whether the AudioSource should loop the audio.</param>
    /// <param name="playOnAwake">Whether the AudioSource should play the clip when it is instantiated.</param>
    /// <returns>The created AudioSource component.</returns>
    public static AudioSource CreateAudioSource(this GameObject gameObject, AudioClip clip, float volume = 1, bool loop = false, bool playOnAwake = false)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.loop = loop;
        source.playOnAwake = playOnAwake;
        return source;
    }

    /// <summary>
    /// Plays a random sound effect from an array of AudioClips using an AudioSource component.
    /// </summary>
    /// <param name="source">The AudioSource component to play the sound effect.</param>
    /// <param name="clips">The array of AudioClips to choose from.</param>
    public static void PlayRandomSoundEffect(this AudioSource source, AudioClip[] clips)
    {
        if (clips.Length == 0)
        {
            Debug.LogWarning("The array of AudioClips is empty.");
            return;
        }

        int randomIndex = UnityEngine.Random.Range(0, clips.Length);
        source.PlayOneShot(clips[randomIndex]);
    }

    /// <summary>
    /// Fades the volume of an AudioSource component over a specified duration.
    /// </summary>
    /// <param name="source">The AudioSource component to fade.</param>
    /// <param name="targetVolume">The target volume to fade to.</param>
    /// <param name="duration">The duration of the fade in seconds.</param>
    /// <param name="playBeforeFade">Plays the AudioSource before it begins fading the volume</param>
    public static IEnumerator FadeAudioVolume(this AudioSource source, float targetVolume, float duration, bool playBeforeFade = false)
    {
        if (playBeforeFade)
        {
            source.Play();
        }

        float startVolume = source.volume;
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            float elapsedTime = Time.time - startTime;
            float t = elapsedTime / duration;
            source.volume = Mathf.Lerp(startVolume, targetVolume, t);
            yield return null;
        }

        source.volume = targetVolume;
    }

    /// <summary>
    /// Fades the pitch of an AudioSource component over a specified duration.
    /// </summary>
    /// <param name="source">The AudioSource component to fade.</param>
    /// <param name="targetPitch">The target pitch to fade to.</param>
    /// <param name="duration">The duration of the fade in seconds.</param>
    /// <param name="playBeforeFade">Plays the AudioSource before it begins fading the pitch</param>
    public static IEnumerator FadeAudioPitch(this AudioSource source, float targetPitch, float duration, bool playBeforeFade = false)
    {
        if (playBeforeFade)
        {
            source.Play();
        }

        float startPitch = source.pitch;
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            float elapsedTime = Time.time - startTime;
            float t = elapsedTime / duration;
            source.pitch = Mathf.Lerp(startPitch, targetPitch, t);
            yield return null;
        }

        source.pitch = targetPitch;
    }

    /// <summary>
    /// Adds a unique item to the array if it's not already present.
    /// </summary>
    /// <typeparam name="T">The type of items in the array.</typeparam>
    /// <param name="array">The original array.</param>
    /// <param name="itemToAdd">The item to add.</param>
    /// <returns>A new array with the added item if it was not already present; otherwise, the original array.</returns>
    public static T[] AddUniqueToArray<T>(this T[] array, T itemToAdd)
    {
        if (!ArrayContainsItem(array, itemToAdd))
        {
            T[] newArray = new T[array.Length + 1];

            for (int i = 0; i < array.Length; i++)
            {
                newArray[i] = array[i];
            }

            newArray[newArray.Length - 1] = itemToAdd;

            array = newArray;
        }

        return array;
    }

    /// <summary>
    /// Converts an integer value to a Roman numeral string representation. (0 - 3999)
    /// </summary>
    /// <param name="value">The integer value to convert.</param>
    /// <returns>The Roman numeral string representation of the integer value.</returns>
    public static string ConvertToRomanNumeral(this int value)
    {
        if (value < 0 || value > 3999)
        {
            Debug.LogError("Value must be between 0 and 3999");
            return "";
        }

        string[] romanNumeralSymbols = { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
        int[] romanNumeralValues = { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };

        stringBuilder.Length = 0;

        for (int i = 0; i < romanNumeralSymbols.Length; i++)
        {
            while (value >= romanNumeralValues[i])
            {
                stringBuilder.Append(romanNumeralSymbols[i]);
                value -= romanNumeralValues[i];
            }
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// Converts a hexadecimal color string to a Color.
    /// </summary>
    /// <param name="hex">The hexadecimal color string to convert.</param>
    /// <returns>Color using the hex string.</returns>
    public static Color HexToColor(this string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color col);
        return col;
    }

    /// <summary>
    /// Converts a Color value to a hexadecimal color string.
    /// </summary>
    /// <param name="color">The Color value to convert.</param>
    /// <param name="includeAlpha">Optional. Determines whether to include the alpha channel in the hexadecimal string. Default is false.</param>
    /// <returns>The hexadecimal color string of the Color.</returns>
    public static string ColorToHex(this Color color, bool includeAlpha = false)
    {
        string hex = includeAlpha ? ColorUtility.ToHtmlStringRGBA(color) : ColorUtility.ToHtmlStringRGB(color);
        return hex;
    }

    /// <summary>
    /// Rotates the transform to look at the target
    /// </summary>
    /// <param name="transform">The transform to rotate</param>
    /// <param name="target">The target point to look at</param>
    public static void LookAt2D(this Transform transform, Vector2 target)
    {
        Vector2 direction = target - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public static void RandomizeRotation2D(this Transform transform)
    {
        transform.rotation = new Quaternion(0, 0, UnityEngine.Random.Range(0, 360), 0);
    }

    public static void ToggleActive(this GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
    #endregion

    #region Miscellaneous

    /// <summary>
    /// Converts a float to a string representation of minutes and seconds e.g. (12m, 45s).
    /// </summary>
    /// <param name="value">The float value to convert.</param>
    /// <returns>The string representation of minutes and seconds.</returns>
    public static string FloatToMinutesSeconds(float value)
    {
        int totalSeconds = Mathf.RoundToInt(value);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    /// <summary>
    /// Converts a float to a string representation of hours and minutes e.g. (21h, 37m)
    /// </summary>
    /// <param name="value">The float value to convert.</param>
    /// <returns>The string representation of minutes and seconds.</returns>
    public static string FloatToHoursMinutes(float value)
    {
        int totalSeconds = Mathf.RoundToInt(value);
        int hours = totalSeconds / 3600;
        int minutes = Mathf.RoundToInt((totalSeconds % 3600) / 60f);

        return string.Format("{0:00}:{1:00}", hours, minutes);
    }

    /// <summary>
    /// Generates a random bool value (true or false).
    /// </summary>
    /// <returns>A random bool value.</returns>
    public static bool RandomBool()
    {
        return UnityEngine.Random.value < 0.5f;
    }

    /// <summary>
    /// Generates a random direction as a Vector2 with the specified magnitude.
    /// </summary>
    /// <param name="magnitude">The magnitude of the generated direction vector.</param>
    /// <returns>A random direction vector with the specified magnitude.</returns>
    public static Vector2 RandomDirection(float magnitude = 1f)
    {
        float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        return direction * magnitude;
    }

    /// <summary>
    /// Checks if there is an active internet connection.
    /// </summary>
    /// <returns>True if there is an active internet connection; otherwise, false.</returns>
    public static bool InternetConnectionReachable()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }

    /// <summary>
    /// Takes a screenshot of the screen and saves it into a custom folder in the persistentDataPath.
    /// The screenshot filename includes the current date and the product name (title of the game).
    /// </summary>
    /// <param name="screenSizeMultiplier">How many times bigger the screenshot is then the current screen resolution.</param>
    public static void TakeScreenshot(int screenSizeMultiplier = 1)
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "Screenshots");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        string filename = Path.Combine(folderPath, Application.productName + "_Screenshot_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png");
        ScreenCapture.CaptureScreenshot(filename, screenSizeMultiplier);
        Debug.Log("Screenshot saved to: " + filename);
    }

    /// <summary>
    /// Just returns the date, not the time.
    /// </summary>
    /// <returns>Date</returns>
    public static string GetDate()
    {
        return DateTime.Now.ToString("dd-MM-yyyy");
    }

    /// <summary>
    /// Just returns the time, not the date.
    /// </summary>
    /// <returns>Time</returns>
    public static string GetTime()
    {
        return DateTime.Now.ToString("HH:mm:ss");
    }

    /// <summary>
    /// Checks if the user's finger is currently touching any UI element on a specific layer.
    /// </summary>
    /// <param name="layerName">The name of the layer to check against.</param>
    /// <returns>True if the finger is touching a UI element on the specified layer; otherwise, false.</returns>
    public static bool IsFingerTouchingLayer(string layerName)
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Touch touch = Input.GetTouch(0);
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = touch.position;
            GraphicRaycaster[] uiRaycasters = UnityEngine.Object.FindObjectsOfType<GraphicRaycaster>();
            foreach (GraphicRaycaster raycaster in uiRaycasters)
            {
                if (raycaster.gameObject.layer == LayerMask.NameToLayer(layerName))
                {
                    List<RaycastResult> results = new List<RaycastResult>();
                    raycaster.Raycast(pointerEventData, results);
                    if (results.Count > 0)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Checks if the cursor position is currently touching any UI element on a specific layer.
    /// </summary>
    /// <param name="layerName">The name of the layer to check against.</param>
    /// <returns>True if the cursor is touching a UI element on the specified layer; otherwise, false.</returns>
    public static bool IsCursorTouchingLayer(string layerName)
    {
        Vector2 touch = Input.mousePosition;
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = touch;
        GraphicRaycaster[] uiRaycasters = UnityEngine.Object.FindObjectsOfType<GraphicRaycaster>();
        foreach (GraphicRaycaster raycaster in uiRaycasters)
        {
            if (raycaster.gameObject.layer == LayerMask.NameToLayer(layerName))
            {
                List<RaycastResult> results = new List<RaycastResult>();
                raycaster.Raycast(pointerEventData, results);
                if (results.Count > 0)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Reloads the current scene.
    /// </summary>
    public static void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Loads a scene based on a relative index offset.
    /// </summary>
    /// <param name="offset">The relative index offset to determine the scene to load.
    ///     Positive values load scenes ahead in the build index.
    ///     Negative values load scenes before in the build index.
    ///     Zero reloads the current scene.</param>
    public static void LoadSceneWithRelativeOffset(int offset = 0)
    {
        if (SceneManager.GetActiveScene().buildIndex + offset < 0 || SceneManager.GetActiveScene().buildIndex + offset > SceneManager.sceneCount)
        {
            Debug.LogWarning("Scene buildIndex out of bounds");
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + offset);
        }
    }

    /// <summary>
    /// Determines a boolean value based on a probability percentage.
    /// </summary>
    /// <param name="probability">The probability percentage between 0 and 100.</param>
    /// <returns>True if the random value falls within the probability range; otherwise, false.</returns>
    public static bool Probability(float probability)
    {
        float percent = probability / 100f;

        if (percent >= 1.0f)
        {
            return true;
        }
        else if (percent <= 0.0f)
        {
            return false;
        }
        else
        {
            return UnityEngine.Random.value < percent;
        }
    }

    /// <summary>
    /// Executes onTimerComplete after duration seconds.
    /// </summary>
    /// <param name="duration">How many seconds before onTimerComplete is executed</param>
    /// <param name="onTimerComplete">The method to execute after duration seconds</param>
    /// <returns></returns>
    public static IEnumerator TimedAction(float duration, Action onTimerComplete)
    {
        yield return new WaitForSeconds(duration);
        onTimerComplete?.Invoke();
    }

    /// <summary>
    /// Converts a frequency in hertz to a pitch value.
    /// </summary>
    /// <param name="frequency">The frequency in hertz.</param>
    /// <returns>The pitch value corresponding to the frequency.</returns>
    public static float FrequencyToPitch(float frequency)
    {
        return Mathf.Log(frequency / 440f, 2f) * 12f;
    }

    /// <summary>
    /// Converts a pitch value to a frequency in hertz.
    /// </summary>
    /// <param name="pitch">The pitch value.</param>
    /// <returns>The frequency in hertz corresponding to the pitch value.</returns>
    public static float PitchToFrequency(float pitch)
    {
        return 440f * Mathf.Pow(2f, pitch / 12f);
    }

    /// <summary>
    /// Sets the volume of all AudioSources in the scene.
    /// </summary>
    /// <param name="volume">The volume value to set (0 to 1).</param>
    public static void SetGlobalVolume(float volume)
    {
        AudioSource[] allSources = UnityEngine.Object.FindObjectsOfType<AudioSource>();
        foreach (AudioSource source in allSources)
        {
            source.volume = Mathf.Clamp01(volume);
        }
    }

    /// <summary>
    /// Stops ALL AudioSources in the scene.
    /// </summary>
    public static void StopAllAudioSources()
    {
        AudioSource[] allSources = UnityEngine.Object.FindObjectsOfType<AudioSource>();
        foreach (AudioSource source in allSources)
        {
            source.Stop();
        }
    }
    #endregion

    #region Logger
    private static string LogFilePath
    {
        get
        {
            string logDirectory = Path.Combine(Application.persistentDataPath, "Logs");
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            return Path.Combine(logDirectory, GetDate() + ".txt");
        }
    }

    public static void Log(string message)
    {
        string logMessage = message;

        string sceneName = SceneManager.GetActiveScene().name;
        string currentTime = GetTime();

        logMessage = $"{currentTime} (Scene: {sceneName}, Version: {Application.version}) | {message}";

        using (StreamWriter writer = new StreamWriter(LogFilePath, true))
        {
            writer.WriteLine(logMessage);
        }
        Debug.Log($"<color='#FF0000'>Y</color><color='#FF1302'>a</color><color='#FF2705'>k</color><color='#FF3B07'>a</color><color='#FF4F0A'>p</color><color='#FF620C'>e</color><color='#FF760F'>d</color><color='#FF8A11'>i</color><color='#FF9E14'>a</color> <color='#fff'>Logger</color> | {message}");
    }

    public static Dictionary<string, string> GetAllLogs()
    {
        Dictionary<string, string> logFiles = new Dictionary<string, string>();

        string logDirectory = Path.Combine(Application.persistentDataPath, "Logs");
        if (!Directory.Exists(logDirectory))
        {
            return logFiles;
        }

        string[] logFileNames = Directory.GetFiles(logDirectory, "*.txt");
        foreach (string fileName in logFileNames)
        {
            string fileDate = Path.GetFileNameWithoutExtension(fileName);
            string fileData = File.ReadAllText(fileName);
            logFiles[fileDate] = fileData;
        }

        return logFiles;
    }
    #endregion
}