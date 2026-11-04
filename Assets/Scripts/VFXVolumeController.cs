using UnityEngine;
using UnityEngine.UI;

public class VFXVolumeController : MonoBehaviour
{
    public Slider vfxSlider;

    void Start()
    {
        vfxSlider.onValueChanged.AddListener(SetVFXVolume);
        SetVFXVolume(vfxSlider.value);
    }

    void SetVFXVolume(float value)
    {
        AudioListener.volume = value;
    }
}