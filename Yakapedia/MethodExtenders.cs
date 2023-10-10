using System.Collections.Generic;
using System.Collections;
using System.Text;
using UnityEngine;
using System;

namespace Yakapedia
{
    public static class MethodExtenders
    {
        private static StringBuilder stringBuilder = new();

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
    }
}