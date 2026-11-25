using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class LevelLoader : MonoBehaviour
{
    private LevelData level;
    private float remaining;
    private int displayedSeconds = -1;
    private int defeatedBosses;
    private bool finished;
    private bool timedOut;
    private UnityEngine.UI.Text timer;

    public static LevelLoader Current { get; private set; }

    private void Awake()
    {
        Current = this;
        level = LevelData.Find(gameObject.scene.name);
        remaining = level.timeLimit;
        var hud = MenuWidgets.Overlay("Level HUD", 2);
        hud.transform.SetParent(transform, false);
        timer = MenuWidgets.Label(hud.transform, "", new Vector2(0.5f, 1f), new Vector2(0, -42), new Vector2(500, 55));
        UpdateTimer();
    }

    private IEnumerator Start()
    {
        foreach (var wave in level.waves)
        {
            if (wave.delay > 0f)
                yield return new WaitForSeconds(wave.delay);

            if (finished || timedOut)
                yield break;

            foreach (var point in wave.spawnPoints)
                Instantiate(wave.enemyPrefab, point, Quaternion.identity);
        }
    }

    private void Update()
    {
        if (finished || timedOut)
            return;

        remaining = Mathf.Max(0f, remaining - Time.deltaTime);
        UpdateTimer();
        if (remaining > 0f)
            return;

        timedOut = true;
        MusicManager.Play(GameSound.Lose);
        MusicManager.SetPaused(true);
        Time.timeScale = 0f;
        var overlay = MenuWidgets.Overlay("Time expired", 40);
        var panel = MenuWidgets.Modal(overlay.transform, "Время вышло");
        MenuWidgets.Button(panel, "Заново", new Vector2(-135, -64), level.Play);
        MenuWidgets.Button(panel, "В меню", new Vector2(135, -64), () =>
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        });
    }

    private void UpdateTimer()
    {
        int seconds = Mathf.CeilToInt(remaining);
        if (seconds == displayedSeconds)
            return;

        displayedSeconds = seconds;
        timer.text = $"{seconds / 60:00}:{seconds % 60:00}";
    }

    public bool ReportBossDefeated() => ReportGoal(LevelGoal.DefeatBoss, ++defeatedBosses);

    public bool ReportGoal(LevelGoal goal, int amount)
    {
        if (finished || timedOut || level.goal != goal || amount < level.goalAmount)
            return false;

        finished = true;
        ProgressStore.Complete(level);
        return true;
    }

    private void OnDestroy()
    {
        if (Current == this)
            Current = null;
    }
}
