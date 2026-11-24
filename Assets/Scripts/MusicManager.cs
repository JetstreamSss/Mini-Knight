using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameSound
{
    Jump, Land, Swing, Hit, Pickup, MeleeWarning, ChargeWarning, ShotWarning,
    BossWarning, EnemySwing, Charge, EnemyShot, Death, Click, Win, Lose
}

public class MusicManager : MonoBehaviour
{
    private const string AssetPath = "SoundPass/";
    private const string SettingsPrefix = "MiniKnight.Audio.";
    private const float FadeSeconds = 0.85f;
    private const float MusicGain = 0.55f;

    private static MusicManager instance;
    private AudioMixer mixer;
    private AudioMixerSnapshot playing;
    private AudioMixerSnapshot paused;
    private AudioSource menuTrack;
    private AudioSource levelTrack;
    private AudioSource[] voices;
    private AudioClip[] clips;
    private Coroutine fade;
    private AudioSource targetTrack;
    private int nextVoice;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        new GameObject("Mini Knight Audio").AddComponent<MusicManager>();
    }

    private void Awake()
    {
        if (instance != null)
        {
            // Old scenes may still contain the former menu-only music component.
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        mixer = Resources.Load<AudioMixer>(AssetPath + "MiniKnight");
        if (mixer == null)
        {
            Debug.LogError("Mini Knight AudioMixer could not be loaded.");
            enabled = false;
            return;
        }

        mixer.updateMode = AudioMixerUpdateMode.UnscaledTime;
        playing = mixer.FindSnapshot("Playing");
        paused = mixer.FindSnapshot("Paused");

        AudioMixerGroup music = mixer.FindMatchingGroups("Music Volume")[0];
        AudioMixerGroup sfx = mixer.FindMatchingGroups("SFX")[0];
        menuTrack = MakeTrack(music, "MenuTheme");
        levelTrack = MakeTrack(music, "LevelTheme");
        voices = new AudioSource[12];
        for (int i = 0; i < voices.Length; i++)
            voices[i] = MakeSource(sfx);

        clips = new AudioClip[System.Enum.GetValues(typeof(GameSound)).Length];
        foreach (GameSound sound in System.Enum.GetValues(typeof(GameSound)))
            clips[(int)sound] = Resources.Load<AudioClip>(AssetPath + sound);
    }

    private void Start()
    {
        ApplyVolume("MusicVolume", ReadVolume("MusicVolume"));
        ApplyVolume("SFXVolume", ReadVolume("SFXVolume"));
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void OnDestroy()
    {
        if (instance != this)
            return;

        SceneManager.sceneLoaded -= OnSceneLoaded;
        instance = null;
    }

    private AudioSource MakeSource(AudioMixerGroup group)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.outputAudioMixerGroup = group;
        source.spatialBlend = 0f;
        return source;
    }

    private AudioSource MakeTrack(AudioMixerGroup group, string clipName)
    {
        AudioSource source = MakeSource(group);
        source.clip = Resources.Load<AudioClip>(AssetPath + clipName);
        source.loop = true;
        source.volume = 0f;
        return source;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetPaused(false);
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            foreach (Button button in root.GetComponentsInChildren<Button>(true))
                button.onClick.AddListener(PlayClick);
        }

        bool menuMusic = scene.name == "MainMenu" || scene.name == "WinScene";
        AudioSource wanted = menuMusic ? menuTrack : levelTrack;
        if (wanted != targetTrack)
        {
            targetTrack = wanted;
            if (fade != null)
                StopCoroutine(fade);
            fade = StartCoroutine(Crossfade(wanted, menuMusic ? levelTrack : menuTrack));
        }

        if (scene.name == "WinScene")
            Play(GameSound.Win);
    }

    private static void PlayClick() => Play(GameSound.Click);

    private IEnumerator Crossfade(AudioSource incoming, AudioSource outgoing)
    {
        if (!incoming.isPlaying)
            incoming.Play();

        float initialIn = incoming.volume;
        float initialOut = outgoing.volume;

        for (float elapsed = 0f; elapsed < FadeSeconds; elapsed += Time.unscaledDeltaTime)
        {
            float progress = Mathf.SmoothStep(0f, 1f, elapsed / FadeSeconds);
            incoming.volume = Mathf.Lerp(initialIn, MusicGain, progress);
            outgoing.volume = Mathf.Lerp(initialOut, 0f, progress);
            yield return null;
        }

        incoming.volume = MusicGain;
        outgoing.volume = 0f;
        outgoing.Stop();
        fade = null;
    }

    public static void Play(GameSound sound)
    {
        if (instance == null || instance.voices == null)
            return;

        MusicManager audio = instance;
        AudioClip clip = audio.clips[(int)sound];
        if (clip == null)
            return;

        AudioSource voice = audio.voices[audio.nextVoice];
        foreach (AudioSource candidate in audio.voices)
        {
            if (candidate.isPlaying)
                continue;

            voice = candidate;
            break;
        }

        audio.nextVoice = (audio.nextVoice + 1) % audio.voices.Length;
        voice.Stop();
        voice.clip = clip;
        voice.pitch = sound == GameSound.Win || sound == GameSound.Lose
            ? 1f
            : Random.Range(0.92f, 1.08f);
        voice.volume = 0.72f;
        voice.Play();
    }

    public static void SetPaused(bool isPaused)
    {
        if (instance != null)
            (isPaused ? instance.paused : instance.playing)?.TransitionTo(0.22f);
    }

    public static float ReadVolume(string parameter)
    {
        float defaultVolume = parameter == "MusicVolume" ? 0.65f : 0.85f;
        return Mathf.Clamp01(PlayerPrefs.GetFloat(SettingsPrefix + parameter, defaultVolume));
    }

    public static void SaveVolume(string parameter, float volume)
    {
        volume = Mathf.Clamp01(volume);
        ApplyVolume(parameter, volume);
        PlayerPrefs.SetFloat(SettingsPrefix + parameter, volume);
    }

    private static void ApplyVolume(string parameter, float volume)
    {
        float decibels = 20f * Mathf.Log10(Mathf.Max(0.0001f, volume));
        if (instance != null && instance.mixer != null)
            instance.mixer.SetFloat(parameter, decibels);
    }

    private void OnApplicationPause(bool isPaused)
    {
        if (isPaused)
            PlayerPrefs.Save();
    }

    private void OnApplicationQuit() => PlayerPrefs.Save();
}
