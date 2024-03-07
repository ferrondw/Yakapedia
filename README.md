![Yakapedia Logo](Yakapedia.png)
**Classes that make coding in Unity (C#) MUCH easier! (includes a logger, save system, method extenders, vibration and more!)**

## Information
This project is long from complete, and is still going strong on the development part. That said there are some bugs here and there, so apologies for that!
Anyways a LOT of effort was put into these scripts, so I hope y'all enjoy it!

## Prerequisites
You know, I'm not THAT smart, so Yakapedia uses [Json.Net (Newtonsoft.Json)](https://www.newtonsoft.com/json) in order to serialize stuff so it can be saved into a file!

## Installation
Just download and open the .unitypackage file (you can find it in releases), then import everything into your Unity project!
Alternatively, you can clone this repository and import all the scripts manually.

## Usage
Once you got the package in your project, you can call Yakapedia methods from other scripts.
Since Yakapedia is a namespace, there are 2 ways it can be used:
```cs
void Start()
{
  Yakapedia.Logger.Log("Me when Yakapedia: (❁´◡`❁)");
}
```

or...

```cs
using Yakapedia;

void Start()
{
  Logger.Log("Me when Yakapedia: (❁´◡`❁)");
}
```

## Method List

### BetterPlayerPrefs
- ``Set<T>(string key, T value = default)``
- ``Get<T>(string key, T defaultValue = default)``
- ``ToggleBool(string key)``
- ``HasKey(string key)``
- ``DeleteKey(string key)``
- ``SelectSaveFile(int index)``
- ``DeleteSaveFile(int index, int saveToLoad = 0)``
- ``DeleteCurrentSaveFile(int saveToLoad = 0)``
- ``IsSaveFileEmpty(int index)``

### Vibration
- ``Start()``
- ``Stop()``
- ``Vibrate(long milliseconds)``
- ``Vibrate(long[] pattern, int repeat)``
- ``IsAndroid()``

### MethodExtenders
- ``ArrayContainsItem<T>(this T[] array, T item)``
- ``SortArray<T>(this T[] array)``
- ``InvertColor(this Color inputColor)``
- ``DestroyAllChildren(this Transform parent)``
- ``RoundToNearest(this float value, float increment)``
- ``GetClosestObjectOfType<T>(this Transform position) where T : MonoBehaviour``
- ``ClampAngle(this float angle, float min, float max)``
- ``GetAllComponentsUnderParent<T>(this GameObject parent)``
- ``ShuffleArray<T>(this T[] array)``
- ``CreateAudioSource(this GameObject gameObject, AudioClip clip, float volume = 1, bool loop = false, bool playOnAwake = false)``
- ``PlayRandomSoundEffect(this AudioSource source, AudioClip[] clips)``
- ``FadeAudioVolume(this AudioSource source, float targetVolume, float duration, bool playBeforeFade = false)``
- ``FadeAudioPitch(this AudioSource source, float targetPitch, float duration, bool playBeforeFade = false)``
- ``AddUniqueToArray<T>(this T[] array, T itemToAdd)``
- ``ConvertToRomanNumeral(this int value)``
- ``HexToColor(this string hex)``
- ``ColorToHex(this Color color, bool includeAlpha = false)``
- ``LookAt2D(this Transform transform, Vector2 target)``
- ``RandomizeRotation2D(this Transform transform)``
- ``ToggleActive(this GameObject gameObject)``

### Miscellaneous
- ``FloatToMinutesSeconds(float value)``
- ``FloatToHoursMinutes(float value)``
- ``RandomBool()``
- ``RandomDirection(float magnitude = 1f)``
- ``InternetConnectionReachable()``
- ``TakeScreenshot(int screenSizeMultiplier = 1)``
- ``GetDate()``
- ``GetTime()``
- ``IsFingerTouchingLayer(string layerName)``
- ``IsCursorTouchingLayer(string layerName)``
- ``ReloadCurrentScene()``
- ``LoadSceneWithRelativeOffset(int offset = 0)``
- ``Probability(float probability)``
- ``TimedAction(float duration, Action onTimerComplete)``
- ``FrequencyToPitch(float frequency)``
- ``PitchToFrequency(float pitch)``
- ``SetGlobalVolume(float volume)``
- ``StopAllAudioSources()``

### Logger
- ``Log(string message)``
- ``GetAllLogs()``

### Encryption
- ``RijndealEncrypt(byte[] plain, string password)``
- ``RijndealDecrypt(byte[] encrypted, string password)``
