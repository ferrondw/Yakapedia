# Yakapedia
**A large C# script with a lot of useful methods (includes a custom logger, save system, audio control, vibration, converters and more!)**

## Installation
Just download and drag the .cs scripts into your project, that's it!

## Usage
Once you got the script in your project, you can call Yakapedia methods from other scripts, such as making screenshots, checking if there is internet etc.
Examples:
```cs
Yakapedia.SetInt("interestingInt", 34); // Save 34 to the save file!! >:0

public int boringInt = 0;
boringInt = Yakapedia.GetInt("interestingInt") // boringInt is now 34! :D

Yakapedia.SelectSaveFile(1); // Hop over to another save file, clean and empty like by brain :/

Yakapedia.DeleteKey("interestingInt"); // You can't do this :0 It doesn't exist in this save file!!

Yakapedia.Log("I'm very sad that \"interestingInt\" can't be deleted :(");
```

## Content
Since there are a lot of scripts, they are listed here in their corresponding regions (without explanation, the names mostly speak for themselves, I also won't list any private methods):

#### BetterPlayerPrefs
##### Public Get / Set Methods
- SetString(string key, string value)
- GetString(string key, string defaultValue = default)
- SetInt(string key, int value)
- GetInt(string key, int defaultValue = default)
- SetFloat(string key, float value)
- GetFloat(string key, float defaultValue = default)
- SetBool(string key, bool value)
- GetBool(string key, bool defaultValue = false)
- ToggleBool(string key)
- SetVector2(string key, Vector2 value)
- GetVector2(string key, Vector2 defaultValue = default)
- SetVector3(string key, Vector3 value)
- GetVector3(string key, Vector3 defaultValue = default)
- SetQuaternion(string key, Quaternion value)
- GetQuaternion(string key, Quaternion defaultValue = default)
- SetColor(string key, Color value)
- GetColor(string key, Color defaultValue = default)
- SetIntArray(string key, int[] value)
- GetIntArray(string key, int[] defaultValue = default)
- SetFloatArray(string key, float[] value)
- GetFloatArray(string key, float[] defaultValue = default)
- SetStringArray(string key, string[] value)
- GetStringArray(string key, string[] defaultValue = default)
- SetBoolArray(string key, bool[] value)
- GetBoolArray(string key, bool[] defaultValue = default)
- SetQuaternionArray(string key, Quaternion[] value)
- GetQuaternionArray(string key, Quaternion[] defaultValue = default)
- SetColorArray(string key, Color[] value)
- GetColorArray(string key, Color[] defaultValue = default)
- SetVector2Array(string key, Vector2[] value)
- GetVector2Array(string key, Vector2[] defaultValue = default)
- SetVector3Array(string key, Vector3[] value)
- GetVector3Array(string key, Vector3[] defaultValue = default)
- HasKey(string key)
- DeleteKey(string key)

##### Public Save File Methods
- SelectSaveFile(int index)
- DeleteSaveFile(int index, int saveToLoad = 0)
- DeleteCurrentSaveFile(int saveToLoad = 0)
- IsSaveFileEmpty(int index)

#### Vibration (Android only)
- StartVibration()
- StopVibration()
- Vibrate(long milliseconds)
- Vibrate(long[] pattern, int repeat)
- IsAndroid()

#### MethodExtenders
- ArrayContainsItem<T>(this T[] array, T item)
- SortArray<T>(this T[] array)
- InvertColor(this Color inputColor)
- DestroyAllChildren(this Transform parent)
- RoundToNearest(this float value, float increment)
- GetClosestObjectOfType<T>(this Transform position) where T : MonoBehaviour
- ClampAngle(this float angle, float min, float max)
- GetAllComponentsUnderParent<T>(this GameObject parent)
- ShuffleArray<T>(this T[] array)
- CreateAudioSource(this GameObject gameObject, AudioClip clip, float volume = 1, bool loop = false, bool playOnAwake = false)
- PlayRandomSoundEffect(this AudioSource source, AudioClip[] clips)
- FadeAudioVolume(this AudioSource source, float targetVolume, float duration, bool playBeforeFade = false)
- FadeAudioPitch(this AudioSource source, float targetPitch, float duration, bool playBeforeFade = false)
- AddUniqueToArray<T>(this T[] array, T itemToAdd)
- ConvertToRomanNumeral(this int value)
- HexToColor(this string hex)
- ColorToHex(this Color color, bool includeAlpha = false)
- RandomizeRotation2D(this Transform transform)
- ToggleActive(this GameObject gameObject)

#### Miscellaneous
- FloatToMinutesSeconds(float value)
- FloatToHoursMinutes(float value)
- RandomBool()
- RandomDirection(float magnitude = 1f)
- InternetConnectionReachable()
- TakeScreenshot(int screenSizeMultiplier = 1)
- GetDate()
- GetTime()
- IsFingerTouchingLayer(string layerName)
- IsCursorTouchingLayer(string layerName)
- ReloadCurrentScene()
- LoadSceneWithRelativeOffset(int offset = 0)
- Probability(float probability)
- TimedAction(float duration, Action onTimerComplete)
- FrequencyToPitch(float frequency)
- PitchToFrequency(float pitch)
- SetGlobalVolume(float volume)
- StopAllAudioSources()

#### Logger
- Log(string message)
- GetAllLogs()