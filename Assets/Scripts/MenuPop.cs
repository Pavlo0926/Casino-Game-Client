using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;

public class MenuPop : MonoBehaviour
{
    UIPopup popup;


    void Start()
    {
        popup = GetComponent<UIPopup>();
    }

    public void OnSettingsClicked()
    {
        var settingsPopup = UIPopup.GetPopup("SettingsPopup");
        settingsPopup.GetComponent<SettingsPopup>().Initialize("Settings",true);
        settingsPopup.Show();
        
    }

    public void OnFanpageClicked()
    {
        Application.OpenURL("www.facebook.com");
        popup.Hide();
    }

    public void OnHelpClicked()
    {
        Debug.Log("<color =red> Help Clicked");
        popup.Hide();
    }

    public void OnShareClicked()
    {
        Debug.Log("<color =red> Share Clicked");
        popup.Hide();
    }

    public void OnChipStoreClicked()
    {
        var popup = ChipStoreController.CreateShopDialog();
        popup.Show();
    }

    public void OnSupportClicked()
    {
        CreateSupportEmail();
    }

    public void OnCloseClicked()
    {
        popup.Hide();
    }

    public void OnGameHistoryClicked()
    {
        var popup = GameHistoryController.CreateGameHistoryDialog();

        popup.Show();
    }

    private void CreateSupportEmail()
    {
        string email = "customerservice@playfulint.com";
        string subject = MyEscapeURL("Inquiry");
        string body = MyEscapeURL($"Golden Frog Baccarat v{Application.version}\r\n");
        body += MyEscapeURL($"{SystemInfo.deviceName} {SystemInfo.operatingSystem}\r\n");
        body += MyEscapeURL($"ID: {Player.Instance.UserId}\r\n");
        body += MyEscapeURL($"Username: {Player.Instance.UserName}\r\n");
        body += MyEscapeURL($"Balance: {Player.Instance.Credits}\r\n");

        body += MyEscapeURL($"\r\nPlease write down your comments or questions:\r\n");

        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    string MyEscapeURL(string URL)
    {
        return WWW.EscapeURL(URL).Replace("+", "%20");
    }
}
