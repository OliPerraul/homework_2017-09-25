using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicallyChangingContent : MonoBehaviour
{
    //TODO decrease offset based on size or position (offset is way too big)


    [SerializeField]
    private float offset = 100;

    public void AdjustRectWidth()
    {
        RectTransform rectTrans = GetComponent<RectTransform>();

        int numChild = rectTrans.childCount;

        rectTrans.sizeDelta = new Vector2(numChild*offset, rectTrans.sizeDelta.y);
    }
}
