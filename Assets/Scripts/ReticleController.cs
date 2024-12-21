//using System.Diagnostics;
using UnityEngine;

public class ReticleController : MonoBehaviour
{
    public RectTransform canvasRect; // Reference to the Canvas RectTransform
    private RectTransform rectTransform; // RectTransform of the reticle

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        if (canvasRect == null)
        {
            Debug.LogError("Canvas RectTransform is not assigned. Please assign it in the inspector.");
        }
    }

    void Update()
    {
        if (canvasRect == null) return;

        // Convert mouse position to UI space
        Vector2 mousePosition = Input.mousePosition;
        Vector2 uiPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect, mousePosition, null, out uiPosition
        );

        // Update the position of the reticle
        rectTransform.localPosition = uiPosition;
    }

    public Vector2 GetReticlePosition()
    {
        // Returns the current world position of the reticle
        return rectTransform.position;
    }
}
