using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsBackground;

    private GameObject settingsControls;
    private Canvas confirmation;

    public void OpenSettings()
    {
        settingsBackground.SetActive(true);
        if (settingsControls != null)
            return;

        var overlay = MenuWidgets.Overlay("Progress Settings", 20);
        settingsControls = overlay.gameObject;
        var vibration = MenuWidgets.Button(overlay.transform, VibrationLabel(), new Vector2(-155, 62),
            null, anchor: new Vector2(0.5f, 0f));
        vibration.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt(ProgressStore.VibrationKey, ProgressStore.VibrationEnabled ? 0 : 1);
            PlayerPrefs.Save();
            vibration.GetComponentInChildren<Text>().text = VibrationLabel();
        });
        MenuWidgets.Button(overlay.transform, "Сбросить прогресс", new Vector2(155, 62),
            AskReset, true, new Vector2(0.5f, 0f));
    }

    public void CloseSettings()
    {
        settingsBackground.SetActive(false);
        Destroy(settingsControls);
    }

    private static string VibrationLabel() => ProgressStore.VibrationEnabled ? "Вибрация: вкл." : "Вибрация: выкл.";

    private void AskReset()
    {
        if (confirmation != null)
            return;

        confirmation = MenuWidgets.Overlay("Confirm Progress Reset", 50);
        string levels = string.Join(", ", ProgressStore.Data.unlockedLevels.OrderBy(level => level));
        var panel = MenuWidgets.Modal(confirmation.transform,
            $"Сбросить прогресс?\nРекорд: {ProgressStore.Data.bestScore} · Уровни: {levels}\nНастройки вернутся к исходным.");
        MenuWidgets.Button(panel, "Отмена", new Vector2(-135, -64), () => Destroy(confirmation));
        MenuWidgets.Button(panel, "Удалить", new Vector2(135, -64), () =>
        {
            if (ProgressStore.ResetProgress())
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            else
                panel.GetComponentInChildren<Text>().text = "Не удалось удалить сохранение. Попробуй ещё раз.";
        }, true);
    }
}
