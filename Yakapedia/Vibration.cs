using UnityEngine;

namespace Yakapedia
{
    public static class Vibration
    {
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
        public static void Start()
        {
            if (IsAndroid())
            {
                vibrator.Call("vibrate");
            }
            else
            {
                Handheld.Vibrate();
            }
        }

        /// <summary>
        /// Stops vibration
        /// </summary>
        public static void Stop()
        {
            if (IsAndroid())
            {
                vibrator.Call("cancel");
            }
        }

        /// <summary>
        /// Starts vibration for a fixed amount of milliseconds
        /// </summary>
        /// <param name="milliseconds">Amount of milliseonds to vibrate for</param>
        public static void Vibrate(long milliseconds)
        {
            if (IsAndroid())
            {
                vibrator.Call("vibrate", milliseconds);
            }
            else
            {
                Handheld.Vibrate();
            }
        }

        /// <summary>
        /// Starts vibration with a pattern, i really don't know how this works so i'm not going to explain further
        /// </summary>
        /// <param name="pattern">An array representing the pattern of vibration in milliseconds.</param>
        /// <param name="repeat">The number of times to repeat the vibration pattern.</param>
        public static void Vibrate(long[] pattern, int repeat)
        {
            if (IsAndroid())
            {
                vibrator.Call("vibrate", pattern, repeat);
            }
            else
            {
                Handheld.Vibrate();
            }
        }

        /// <summary>
        /// Checks if you are on Android.
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
    }
}