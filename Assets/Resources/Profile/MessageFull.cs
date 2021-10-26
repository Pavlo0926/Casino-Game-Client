using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageFull : MonoBehaviour
{
    private TMP_InputField inputField;
    // Start is called before the first frame update
    void Start()
    {
        inputField = GetComponentInChildren<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show(bool isEditing)
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
