using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCursor : MonoBehaviour
{
    
    private static Image left;
    private static Image right;

    private static RectTransform rtransform;
    // Start is called before the first frame update
    void Start()
    {
        left = transform.Find("Left").GetComponent<Image>();
        right = transform.Find("Right").GetComponent<Image>();
        rtransform = this.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void SelectCursor(RectTransform t)
    {
        if(left == null || right == null)
        {
            Debug.LogWarning("UI cursor was not identified correctly");
            return;
        }
        Vector3[] corners = new Vector3[4];
        //t.GetWorldCorners(corners);
        //Vector3 position = corners[1] + corners[3]; //top-left corner + lower-right corner
        //rtransform.position = position; //new Vector3(t.position.x - t.rect.width/2, t.position.y - t.rect.height/2, rtransform.position.z);
        rtransform.SetParent(t, false); //worldPositionStays = false
        left.enabled = true;
        right.enabled = true;

    }

    public static void DeselectCursor()
    {
        if(left == null || right == null)
        {
            Debug.LogWarning("UI cursor was not identified correctly");
            return;
        }
        left.enabled = false;
        right.enabled = false;
    }
}
