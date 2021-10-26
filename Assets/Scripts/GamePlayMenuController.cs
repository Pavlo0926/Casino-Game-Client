using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;

public class GamePlayMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowGameHistory()
     {
        var popup = GameHistoryController.CreateGameHistoryDialog();

        popup.Show();
     }

    public void ShowChipStore()
    {
        var popup = ChipStoreController.CreateShopDialog();

        popup.Show();
    }

     public void Lobby()
     {
        Alertbox.ShowMessage("Would you like to leave this table?", "Leave?", () =>
        {
           UIManager.Instance.UnloadGamePlayScene(() =>
           {
               UIManager.Instance.ShowHomeScreen();
           });
        }, () => {});
     }
}
