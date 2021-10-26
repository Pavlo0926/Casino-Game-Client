using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AspectRatioSetter : MonoBehaviour
{
    [SerializeField]
    AspectRatio aspectRatio;

    private void Awake()
    {
        //SetAspectRatio();
        transform.GetComponent<Doozy.Engine.UI.UIView>().OnVisibilityChanged.AddListener(SetAspectRatio);
    }

    public void SetAspectRatio(float s)
    {
        if(s==1)
            transform.parent.GetComponent<CanvasScaler>().matchWidthOrHeight = (float)aspectRatio;
    }

    
}
