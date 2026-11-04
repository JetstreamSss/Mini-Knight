
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsBackground;

    public void OpenSettings()
    {
        settingsBackground.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsBackground.SetActive(false);
    }
}
