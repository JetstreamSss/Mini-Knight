
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image heart1;
    public Image heart2;
    public Image heart3;

    public void UpdateHearts(int hp)
    {
        heart1.enabled = hp >= 1;
        heart2.enabled = hp >= 2;
        heart3.enabled = hp >= 3;
    }
}
