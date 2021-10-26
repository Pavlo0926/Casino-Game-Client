using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MessageExpander : MonoBehaviour
{
    public MessageFull FullMessage;

    void Start()
    {

    }

    public void ToggleExpanded()
    {
        Debug.Log("Toggling");
        FullMessage.Show(false);
    }


    void Update()
    {
        
    }
}
