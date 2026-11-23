using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform), typeof(Rigidbody2D))]
public class CTScanner : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Collider2D _scanTrigger;

    public event Action<Collider2D, bool> RegionContactChanged;

    private RectTransform _handle;
    private RectTransform _track;
    private Vector2 _startPosition;
    private float _dragOffset;
    private int? _dragPointer;
    private bool _scanning;

    private void Awake()
    {
        CachePosition();
    }

    public void SetScanning(bool scanning)
    {
        _scanning = scanning;
        _dragPointer = null;
        _scanTrigger.enabled = scanning;
    }

    public void ResetPosition()
    {
        CachePosition();
        _handle.anchoredPosition = _startPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!_scanning || _dragPointer.HasValue || !TryGetTrackPoint(eventData, out Vector2 point))
            return;

        _dragPointer = eventData.pointerId;
        _dragOffset = _handle.localPosition.x - point.x;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_dragPointer != eventData.pointerId || !TryGetTrackPoint(eventData, out Vector2 point))
            return;

        Vector3 position = _handle.localPosition;
        position.x = Mathf.Clamp(point.x + _dragOffset, _track.rect.xMin, _track.rect.xMax);
        _handle.localPosition = position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_dragPointer == eventData.pointerId)
            _dragPointer = null;
    }

    private bool TryGetTrackPoint(PointerEventData eventData, out Vector2 point)
    {
        return RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _track, eventData.position, eventData.pressEventCamera, out point);
    }

    private void CachePosition()
    {
        if (_handle != null)
            return;

        _handle = (RectTransform)transform;
        _track = (RectTransform)_handle.parent;
        _startPosition = _handle.anchoredPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_scanning)
            RegionContactChanged?.Invoke(other, true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_scanning)
            RegionContactChanged?.Invoke(other, false);
    }
}
