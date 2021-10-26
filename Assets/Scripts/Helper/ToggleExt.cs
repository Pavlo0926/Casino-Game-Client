using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ToggleExt : MonoBehaviour
{
    [SerializeField] GameObject Handler;
    private void Awake()
    {
        GetComponent<Toggle>().onValueChanged.AddListener((val)=>
        {
            if(val)
                Handler.transform.DOLocalMoveX(30f, 0.5f);
            else
                Handler.transform.DOLocalMoveX(-30f, 0.5f);
        });
    }
}
