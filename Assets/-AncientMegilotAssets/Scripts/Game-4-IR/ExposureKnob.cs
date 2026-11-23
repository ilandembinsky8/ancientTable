using UnityEngine;
using UnityEngine.EventSystems;

public class ExposureKnob : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform knobVisual;

    [SerializeField] private float minimumAngle = 135f;
    [SerializeField] private float maximumAngle = -135f;

    [SerializeField, Min(1f)]private float dragDistance = 200f;
    [SerializeField, Range(0f, 1f)]private float value = 0f;

    //properties
    public float Value => value;
    public bool IsDragging { get; private set; }
    public bool AllowMouseInput { get; set; } = true;
    
    //private variables
    private RectTransform dragArea;
    private Quaternion initialRotation;
    private float startMouseY;
    private float startValue;

    private void Awake()
    {
        dragArea = (RectTransform)transform;
        initialRotation = knobVisual.localRotation;
        SetValue(0f);
    }

    public void SetValue(float newValue)
    {
        value = Mathf.Clamp01(newValue);

        float angle = Mathf.Lerp(minimumAngle, maximumAngle, value);

        knobVisual.localRotation =
            initialRotation * Quaternion.Euler(0f, 0f, angle);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!AllowMouseInput)
            return;

        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if (!TryGetLocalPosition(eventData, out Vector2 position))
            return;

        IsDragging = true;
        startMouseY = position.y;
        startValue = value;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!AllowMouseInput || !IsDragging)
            return;

        if (!TryGetLocalPosition(eventData, out Vector2 position))
            return;

        float change = (position.y - startMouseY) / dragDistance;
        SetValue(startValue + change);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        IsDragging = false;
    }

    private void OnDisable()
    {
        IsDragging = false;
    }

    private bool TryGetLocalPosition(
        PointerEventData eventData,
        out Vector2 position)
    {
        return RectTransformUtility.ScreenPointToLocalPointInRectangle(
            dragArea,
            eventData.position,
            eventData.pressEventCamera,
            out position);
    }
}