using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowGamePlayMenuPopup()
    {
        var popup = UIPopup.GetPopup("GamePlayMenuPopup");

        popup.Show(true);
    }
}
