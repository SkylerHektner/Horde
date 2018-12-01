using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
  
[RequireComponent (typeof (RectTransform), typeof (Collider2D))]
public class MenuButton : MonoBehaviour, ICanvasRaycastFilter, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField]
    private HInterface.HType heuristic;

    [SerializeField]
    private RadialMenu radialMenu;

    [SerializeField]
    private Sprite icon;

    private Collider2D myCollider;
    private RectTransform rectTransform;

    private Color buttonColor;
    
    void Awake () 
    {
        myCollider = GetComponent<Collider2D>();
        rectTransform = GetComponent<RectTransform>();

        buttonColor = GetComponent<Image>().color;
    }

    // So the button is only detected when the mouse hovers over the image.
    public bool IsRaycastLocationValid (Vector2 screenPos, Camera eventCamera)
    {
     	var worldPoint = Vector3.zero;
        var isInside = RectTransformUtility.ScreenPointToWorldPointInRectangle(
         	rectTransform,
         	screenPos,
         	eventCamera,
         	out worldPoint
        );

        if (isInside)
          isInside = myCollider.OverlapPoint(worldPoint);

        return isInside;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        radialMenu.SetCenterIcon(icon);
        radialMenu.SetCenterName(heuristic.ToString());
        radialMenu.SetCenterColor(buttonColor);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        radialMenu.SelectHeuristic();
    }
}
 