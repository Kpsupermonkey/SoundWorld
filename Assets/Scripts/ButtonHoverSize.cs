using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverSize : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform;
    public Vector2 normalSize = new Vector2(200f, 50f); // Default size
    public Vector2 hoverSize = new Vector2(220f, 60f); // Hovered size
    private bool isHovered = false;
    private float speed = 10f; // Speed of the size transition

    void Start()
    {
        // Get the RectTransform component
        rectTransform = GetComponent<RectTransform>(); 
        // Set default size
        rectTransform.sizeDelta = normalSize; 
    }

    // Detect when the mouse hovers over the button
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    // Detect when the mouse exits the button
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }

    void Update()
    {
        // Smoothly transition between normalSize and hoverSize
        rectTransform.sizeDelta = Vector2.Lerp(
            rectTransform.sizeDelta,
            isHovered ? hoverSize : normalSize,
            Time.deltaTime * speed
        );
    }
}
