using UnityEngine;
using UnityEngine.EventSystems;

public class HoldButton : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public PlayerController player;
    public float direction = 1f;   // -1 влево, 1 вправо

    public void OnPointerDown(PointerEventData eventData)
    {
        if (player != null) player.SetTouchMove(direction);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (player != null) player.SetTouchMove(0f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (player != null) player.SetTouchMove(0f);
    }
}