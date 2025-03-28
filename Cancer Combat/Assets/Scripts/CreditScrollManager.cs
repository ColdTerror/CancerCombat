using UnityEngine;
using UnityEngine.UI;

public class CreditsScroller : MonoBehaviour
{
    public float scrollSpeed = 30f; // Adjust this for the desired scrolling speed
    private ScrollRect scrollRect;
    private RectTransform contentRect;
    private float contentHeight;
    private float viewportHeight;

    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        if (scrollRect != null && scrollRect.content != null)
        {
            contentRect = scrollRect.content;
            viewportHeight = scrollRect.viewport.rect.height;
            // Wait one frame for layout to be calculated accurately
            StartCoroutine(CalculateContentHeight());
        }
        else
        {
            Debug.LogError("ScrollRect or Content not found on this GameObject.");
            enabled = false;
        }
    }

    System.Collections.IEnumerator CalculateContentHeight()
    {
        yield return null;
        contentHeight = contentRect.rect.height;
        // Set the initial scroll position to the bottom
        scrollRect.normalizedPosition = Vector2.zero;
    }

    void Update()
    {
        if (contentHeight > viewportHeight)
        {
            // Scroll upwards
            scrollRect.normalizedPosition += Vector2.up * (scrollSpeed / contentHeight) * Time.deltaTime;

            // Loop back to the start if it goes past the top
            if (scrollRect.normalizedPosition.y > 1f)
            {
                scrollRect.normalizedPosition = Vector2.zero; // Or a value slightly above zero for a smoother loop
            }
        }
    }
}