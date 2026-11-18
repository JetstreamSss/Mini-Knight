using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

internal static class MenuWidgets
{
    private static readonly Color Ink = new(0.09f, 0.10f, 0.13f);
    private static readonly Color ButtonColor = new(0.25f, 0.26f, 0.32f);
    private static readonly Vector2 Center = new(0.5f, 0.5f);

    public static Canvas Overlay(string name, int order)
    {
        var obj = new GameObject(name, typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        var canvas = obj.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = order;
        var scaler = obj.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1280, 720);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 1f;
        return canvas;
    }

    public static RectTransform Modal(Transform parent, string message)
    {
        var shade = Element("Backdrop", parent, Center, Vector2.zero, Vector2.zero);
        shade.anchorMin = Vector2.zero;
        shade.anchorMax = Vector2.one;
        shade.gameObject.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.8f);
        var panel = Element("Dialog", shade, Center, Vector2.zero, new Vector2(650, 355));
        panel.gameObject.AddComponent<Image>().color = Ink;
        Label(panel, message, Center, new Vector2(0, 95), new Vector2(620, 105));
        return panel;
    }

    public static Text Label(Transform parent, string value, Vector2 anchor, Vector2 position, Vector2 size)
    {
        var text = Element(value, parent, anchor, position, size).gameObject.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 30;
        text.text = value;
        text.color = Color.white;
        text.alignment = TextAnchor.MiddleCenter;
        text.raycastTarget = false;
        var outline = text.gameObject.AddComponent<Outline>();
        outline.effectColor = Ink;
        return text;
    }

    public static Button Button(Transform parent, string title, Vector2 position, UnityAction action,
        bool destructive = false, Vector2? anchor = null)
    {
        var rect = Element(title, parent, anchor ?? Center, position, new Vector2(245, 64));
        var image = rect.gameObject.AddComponent<Image>();
        image.color = destructive ? new Color(0.62f, 0.15f, 0.20f) : ButtonColor;
        var button = rect.gameObject.AddComponent<Button>();
        button.targetGraphic = image;
        if (action != null)
            button.onClick.AddListener(action);
        Label(rect, title, Center, Vector2.zero, rect.sizeDelta).fontSize = 24;
        return button;
    }

    private static RectTransform Element(string name, Transform parent, Vector2 anchor, Vector2 position, Vector2 size)
    {
        var rect = new GameObject(name, typeof(RectTransform)).GetComponent<RectTransform>();
        rect.SetParent(parent, false);
        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
        return rect;
    }
}
