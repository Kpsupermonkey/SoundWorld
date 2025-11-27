using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems; // Required for UI events

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // Reference to Animator
    public Animator animator; 

    void Start()
    {
        // Get Animator Component
        animator = GetComponent<Animator>(); 
    }

    // When the mouse hovers over the button
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Play hover animation
        animator.SetTrigger("Hover"); 
    }

    // When the mouse exits the button
    public void OnPointerExit(PointerEventData eventData)
    {
        // Reset to idle
        animator.SetTrigger("Exit"); 
    }

    // When the button is clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        // Optional click animation
        animator.SetTrigger("Pressed"); 
    }
}
