using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Yakapedia
{
    public static class Logger
    {
        private static string LogFilePath
        {
            get
            {
                string logDirectory = Path.Combine(Application.persistentDataPath, "Logs");
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                return Path.Combine(logDirectory, Miscellaneous.GetDate() + ".txt");
            }
        }

        /// <summary>
        /// Logs a message into a file.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public static void Log(string message)
        {
            string logMessage = message;

            string sceneName = SceneManager.GetActiveScene().name;
            string currentTime = Miscellaneous.GetTime();

            logMessage = $"{currentTime} (Scene: {sceneName}, Version: {Application.version}) | {message}";

            using (StreamWriter writer = new StreamWriter(LogFilePath, true))
            {
                writer.WriteLine(logMessage);
            }
            Debug.Log($"<color='#FF0000'>Y</color><color='#FF1302'>a</color><color='#FF2705'>k</color><color='#FF3B07'>a</color><color='#FF4F0A'>p</color><color='#FF620C'>e</color><color='#FF760F'>d</color><color='#FF8A11'>i</color><color='#FF9E14'>a</color> <color='#fff'>Logger</color> | {message}");
        }

        /// <summary>
        /// Retrieves ALL the logs from the Logs folder.
        /// </summary>
        /// <returns>string, string dictionary containing ALL the logs.</returns>
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
    }
}