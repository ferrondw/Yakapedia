using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Yakapedia
{
    public static class Miscellaneous
    {
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
    }
}