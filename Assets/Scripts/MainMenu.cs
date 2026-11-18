using System;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private GameObject levelMenu;

    public void PlayGame()
    {
        if (levelMenu != null)
            return;

        var overlay = MenuWidgets.Overlay("Level Selection", 30);
        levelMenu = overlay.gameObject;
        var panel = MenuWidgets.Modal(overlay.transform, $"Выбор уровня\nРекорд: {ProgressStore.Data.bestScore}");
        var levels = (LevelData[])LevelData.All.Clone();
        Array.Sort(levels, (left, right) => left.number.CompareTo(right.number));
        for (int index = 0; index < levels.Length; index++)
        {
            var level = levels[index];
            var unlocked = ProgressStore.Data.unlockedLevels.Contains(level.number);
            var button = MenuWidgets.Button(panel, unlocked ? $"Уровень {level.number}" : $"{level.number} — закрыт",
                new Vector2(0, 40 - index * 62), level.Play);
            ((RectTransform)button.transform).sizeDelta = new Vector2(245, 56);
            button.interactable = unlocked;
        }

        MenuWidgets.Button(panel, "Назад", new Vector2(0, -143), () => Destroy(levelMenu));
    }

    public void QuitGame() => Application.Quit();
}
