
using UnityEngine;
using UnityEngine.EventSystems;

public class UIElementDragger : EventTrigger
{

    bool dragging;
    Vector2 offset;

    public void Update()
    {
        if (dragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - offset;
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;
        offset = eventData.position - new Vector2(transform.position.x, transform.position.y);
    }

}
