using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
  
[RequireComponent (typeof (RectTransform), typeof (Collider2D))]
public class MenuButton : MonoBehaviour, ICanvasRaycastFilter, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField]
    private HInterface.HType heuristic;

    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private CapsuleSelector capsuleSelector;

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
        RadialMenuUI.instance.SetCenterIcon(icon);
        RadialMenuUI.instance.SetCenterName(heuristic.ToString());
        RadialMenuUI.instance.SetCenterColor(buttonColor);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //radialMenu.SelectHeuristic();

        Capsule capsule = capsuleSelector.SelectedCapsule;
        capsule.AddHeuristicToCapsule(icon, buttonColor, heuristic);
    }
}
 