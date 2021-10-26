using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class SinglePlayerPopupController : MonoBehaviour
{

    string[] GameSpeed = new string[] { "Slow", "Normal", "Fast (without voice)" };
    string[] Commissions = new string[] { "5% commission on all Banker wins", "50% commissions on all Banker 6 wins only" };
    string[] TableLimit = new string[] { "Min 1K ~ Max 1M", "Min 10K ~ Max 10M", "Min 100K ~ Max 100M", "Min 1M ~ Max 1B", "Min 10M ~ Max 10B" };
    string[] CardsOpen = new string[] { "Squeeze", "Slide", "Automatic" };

    [SerializeField] Text GameSpeedText, CommissionText, TableLimitText, CardsOpenText;

    public void OnGameSpeedClick()
    {
        var popup = UIPopup.GetPopup("List");
        popup.GetComponent<ListManager>().Initialize("Game Speed",GameSpeed, PlayerPrefs.GetInt("GameSpeed", 1), (index)=> {
            PlayerPrefs.SetInt("GameSpeed", index);
            UpdateUI();
        });
        popup.Show();
    }

    public void OnCommissionClick()
    {
        var popup = UIPopup.GetPopup("List");
        popup.GetComponent<ListManager>().Initialize("Game Commissions", Commissions, PlayerPrefs.GetInt("Commission", 1), (index) => {
            PlayerPrefs.SetInt("Commission", index);
            UpdateUI();
        });
        popup.Show();
    }

    public void OnTableLimitClick()
    {
        var popup = UIPopup.GetPopup("List");
        popup.GetComponent<ListManager>().Initialize("Table Limit", TableLimit, PlayerPrefs.GetInt("TableLimit", 1), (index) => {
            PlayerPrefs.SetInt("TableLimit", index);
            UpdateUI();
        });
        popup.Show();
    }

    public void OnCardsOpenClick()
    {
        var popup = UIPopup.GetPopup("List");
        popup.GetComponent<ListManager>().Initialize("Cards Open", CardsOpen, PlayerPrefs.GetInt("CardsOpen", 1), (index) => {
            PlayerPrefs.SetInt("CardsOpen", index);
            UpdateUI();
        });
        popup.Show();
    }

    public void Awake()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        CardsOpenText.text = CardsOpen[PlayerPrefs.GetInt("CardsOpen", 1)-1];
        TableLimitText.text = TableLimit[PlayerPrefs.GetInt("TableLimit", 1)-1];
        CommissionText.text = Commissions[PlayerPrefs.GetInt("Commission", 1)-1];
        GameSpeedText.text = GameSpeed[PlayerPrefs.GetInt("GameSpeed", 1)-1];
    }
}
