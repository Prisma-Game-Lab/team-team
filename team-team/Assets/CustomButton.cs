using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomButton : EventTrigger
{
    public override void OnSelect (BaseEventData eventData)
    {
        base.OnSelect (eventData);
        // Call methods when the UI element is selected
        MenuCursor.SelectCursor(this.gameObject.GetComponent<RectTransform>());
    }
 
    public override void OnDeselect (BaseEventData eventData)
    {
        base.OnDeselect (eventData);
        // Call methods when the UI element is deselected
        MenuCursor.DeselectCursor();
    }
}
