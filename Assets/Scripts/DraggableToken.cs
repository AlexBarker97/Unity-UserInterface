using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class DraggableToken : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Image image;

    private Vector2 offset;
    public bool selectedRed;
    public bool selectedYellow;
    public bool selectedBlue;

    [SerializeField] private RectTransform region1; // Region 1 RectTransform
    [SerializeField] private float radius1; // Radius of Region 1

    [SerializeField] private RectTransform region2; // Region 2 RectTransform
    [SerializeField] private float radius2; // Radius of Region 2

    [SerializeField] private RectTransform region3; // Region 3 RectTransform
    [SerializeField] private float radius3; // Radius of Region 3

    [SerializeField] private GameObject nextObject;

    [SerializeField] private GameObject redSlider;
    [SerializeField] private GameObject yellowSlider;
    [SerializeField] private GameObject blueSlider;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        image = GetComponent<Image>();

        if (image == null)
        {
            Debug.LogError("DraggableToken requires an Image component.");
        }

        selectedRed = false;
        selectedYellow = false;
        selectedBlue = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (image != null)
        {
            // Make the image semi-transparent during dragging
            Color tempColor = image.color;
            tempColor.a = 0.6f;
            image.color = tempColor;
        }

        Vector3 globalMousePosition = eventData.position;
        Vector2 localMousePosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, globalMousePosition, canvas.worldCamera, out localMousePosition);

        offset = rectTransform.localPosition - new Vector3(localMousePosition.x, localMousePosition.y, rectTransform.localPosition.z);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 globalMousePosition = eventData.position;
        Vector2 localPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, globalMousePosition, canvas.worldCamera, out localPosition);

        rectTransform.localPosition = new Vector3(localPosition.x, localPosition.y, rectTransform.localPosition.z) + new Vector3(offset.x, offset.y, 0f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (image != null)
        {
            // Restore the image's transparency
            Color tempColor = image.color;
            tempColor.a = 1f;
            image.color = tempColor;
        }

        // Check each region and trigger corresponding effect
        if (IsWithinRegion(rectTransform.localPosition, region1.localPosition, radius1))
        {
            OnDropInRegion1();
        }
        else if (IsWithinRegion(rectTransform.localPosition, region2.localPosition, radius2))
        {
            OnDropInRegion2();
        }
        else if (IsWithinRegion(rectTransform.localPosition, region3.localPosition, radius3))
        {
            OnDropInRegion3();
        }
    }

    private bool IsWithinRegion(Vector2 tokenPosition, Vector2 regionCenter, float radius)
    {
        float distance = Vector2.Distance(tokenPosition, regionCenter);
        return distance <= radius;
    }

    // Empty function for Region 1 effect
    private void OnDropInRegion1()
    {
        Debug.Log("Token dropped in RED Region");
        selectedRed = true;
        selectedYellow = false;
        selectedBlue = false;
        if (redSlider != null)
        {
            redSlider.GetComponent<ProgressBar>().targetValue += 0.333f;
        }
        StartCoroutine(FadeOutToken());
    }

    // Empty function for Region 2 effect
    private void OnDropInRegion2()
    {
        Debug.Log("Token dropped in YELLOW Region");
        selectedRed = false;
        selectedYellow = true;
        selectedBlue = false;
        if (yellowSlider != null)
        {
            yellowSlider.GetComponent<ProgressBar>().targetValue += 0.333f;
        }
        StartCoroutine(FadeOutToken());
    }

    // Empty function for Region 3 effect
    private void OnDropInRegion3()
    {
        Debug.Log("Token dropped in BLUE Region");
        selectedRed = false;
        selectedYellow = false;
        selectedBlue = true;
        if (blueSlider != null)
        {
            blueSlider.GetComponent<ProgressBar>().targetValue += 0.333f;
        }
        StartCoroutine(FadeOutToken());
    }

    private IEnumerator FadeOutToken()
    {
        if (image == null) yield break; // Ensure the Image component exists

        float fadeDuration = 0.5f; // Duration of the fade in seconds
        Color tempColor = image.color;
        float startAlpha = tempColor.a;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration; // Normalized time [0, 1]
            tempColor.a = Mathf.Lerp(startAlpha, 0, normalizedTime);
            image.color = tempColor;
            yield return null; // Wait until the next frame
        }

        // Ensure the alpha is exactly 0 at the end
        tempColor.a = 0;
        image.color = tempColor;

        gameObject.SetActive(false);
        if (nextObject != null)
        {
            nextObject.SetActive(true);
        }
    }
}