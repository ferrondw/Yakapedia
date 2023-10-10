using System.Security.Cryptography;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;
using UnityEngine;
using System.IO;
using System;

namespace Yakapedia
{
    public static class BetterPlayerPrefs
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

        private static Dictionary<string, object> values = new Dictionary<string, object>();

        /// <summary>
        /// Saves a value into the currently selected save file.
        /// </summary>
        /// <typeparam name="T">Type to set</typeparam>
        /// <param name="key">The key for the data entry.</param>
        /// <param name="value">The value to set.</param>
        public static void Set<T>(string key, T value = default)
        {
            if (values == null)
            {
                GetDictionary();
            }

            values[key] = value;
            SaveData();
        }

        /// <summary>
        /// Returns a value corresponding to key in the currently selected save file if it exists.
        /// </summary>
        /// <typeparam name="T">Type to get</typeparam>
        /// <param name="key">The key for the data entry.</param>
        /// <param name="defaultValue">The default value to return if there is no entry using the key.</param>
        public static T Get<T>(string key, T defaultValue = default)
        {
            if (values == null)
            {
                GetDictionary();
            }

            if (values.ContainsKey(key))
            {
                object val = values[key];
                return (T)Convert.ChangeType(val, typeof(T));
            }

            Logger.Log("Dictionary doesn't contain the key!");
            return defaultValue;
        }

        /// <summary>
        /// Toggles a bool value from the currently selected save file.
        /// </summary>
        /// <param name="key">The key for the data entry.</param>
        public static void ToggleBool(string key)
        {
            Set<bool>(key, !Get<bool>(key));
        }

        /// <summary>
        /// Checks if the key exists in the currently selected save file.
        /// </summary>
        /// <param name="key">The key for the data entry.</param>
        /// <returns>True if the key exists; otherwise, false.</returns>
        public static bool HasKey(string key)
        {
            if (values == null)
            {
                GetDictionary();
            }
            return values.ContainsKey(key);
        }

        /// <summary>
        /// Deletes the data entry with the specified key.
        /// </summary>
        /// <param name="key">The key of the data entry to delete.</param>
        public static void DeleteKey(string key)
        {
            if (values == null)
            {
                GetDictionary();
            }
            values.Remove(key);
            SaveData();
        }

        /// <summary>
        /// Select a save file, if it does not exist it will be made. This also reloads the current scene so all variables can be properly loaded in.
        /// </summary>
        /// <param name="index">Index of the save file that needs to be loaded</param>
        public static void SelectSaveFile(int index)
        {
            Logger.Log($"Selected save file with index: {index}");

            PlayerPrefs.SetInt("SelectedSave", index);
            GetDictionary();

            Miscellaneous.ReloadCurrentScene();
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

            Logger.Log($"Deleted save file with index: {index}");
            SelectSaveFile(saveToLoad);
            Miscellaneous.ReloadCurrentScene();
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

            Logger.Log($"Deleted current save file");
            SelectSaveFile(saveToLoad);
            Miscellaneous.ReloadCurrentScene();
        }

        /// <summary>
        /// Checks if a save file is empty or does not exist at all.
        /// </summary>
        /// <param name="index">The index of the save file to check</param>
        /// <returns>True if the save file does not exist or is empty; otherwise, false.</returns>
        public static bool IsSaveFileEmpty(int index)
        {
            if (File.Exists(GetSaveFilePath(index)))
            {
                byte[] data = File.ReadAllBytes(GetSaveFilePath(index));
                if (data == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private static void SaveData()
        {
            byte[] jsonData = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(values));
            byte[] encryptedData = Encryption.RijndealEncrypt(jsonData, JsonEncryptionKey);

            if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "Yakapedia", "Saves")))
            {
                Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "Yakapedia", "Saves"));
            }
            File.WriteAllBytes(CurrentSaveFile, encryptedData);
            Logger.Log("Saved data to file");
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
                    stringBuilder.Append(hashedBytes[i].ToString("x2"));
                }

                string deviceSpecificKey = stringBuilder.ToString();

                deviceSpecificKey = deviceSpecificKey.Substring(0, 16);
                deviceSpecificKey = deviceSpecificKey.Insert(2, "-");
                deviceSpecificKey += "@";

                return deviceSpecificKey;
            }
        }

        private static void GetDictionary()
        {
            values = new Dictionary<string, object>();
            if (File.Exists(CurrentSaveFile))
            {
                byte[] encryptedData = File.ReadAllBytes(CurrentSaveFile);
                byte[] decryptedData = Encryption.RijndealDecrypt(encryptedData, JsonEncryptionKey);
                Dictionary<string, object> data;
                if (decryptedData != null)
                {
                    data = JsonConvert.DeserializeObject<Dictionary<string, object>>(Encoding.UTF8.GetString(decryptedData));
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
            else
            {
                SaveData();
            }
        }
    }
}