using UnityEngine;
using UnityEngine.UI;

public class VFXVolumeController : MonoBehaviour
{
    public Slider vfxSlider;

    private void Start()
    {
        vfxSlider.SetValueWithoutNotify(MusicManager.ReadVolume("SFXVolume"));
        vfxSlider.onValueChanged.AddListener(SetVolume);
    }

    private void SetVolume(float value) => MusicManager.SaveVolume("SFXVolume", value);
}
