using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private Vector2 offset; // Use Vector2 for 2D calculations

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>(); // Optional: Use if you want to add visual feedback (like transparency) during drag
    }

    // Called when the drag starts
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Optionally make the object semi-transparent during drag (use CanvasGroup)
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0.6f;
        }

        // Calculate the offset between the mouse position and the center of the UI element
        Vector3 globalMousePosition = eventData.position;
        Vector2 localMousePosition;

        // Convert the global mouse position to the local position of the UI element relative to the Canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, globalMousePosition, canvas.worldCamera, out localMousePosition);

        // Calculate the offset (difference) between the mouse and the center of the RectTransform
        offset = rectTransform.localPosition - new Vector3(localMousePosition.x, localMousePosition.y, rectTransform.localPosition.z);
    }

    // Called while dragging the UI element
    public void OnDrag(PointerEventData eventData)
    {
        // Get the global mouse position
        Vector3 globalMousePosition = eventData.position;
        Vector2 localPosition;

        // Convert the global mouse position to the local position of the UI element relative to the Canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, globalMousePosition, canvas.worldCamera, out localPosition);

        // Apply the offset to keep the element centered on the cursor
        rectTransform.localPosition = new Vector3(localPosition.x, localPosition.y, rectTransform.localPosition.z) + new Vector3(offset.x, offset.y, 0f);
    }

    // Called when the drag ends
    public void OnEndDrag(PointerEventData eventData)
    {
        // Optionally restore transparency after drag ends
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
        }
    }
}