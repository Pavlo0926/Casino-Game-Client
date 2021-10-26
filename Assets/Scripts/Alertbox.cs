using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Alertbox : MonoBehaviour
{
    [SerializeField] Text headingText, messageText;
    UnityAction OnSuccess, OnFail;

    [SerializeField] GameObject SuccessButton, FailButton;

    public void ShowAlertBox(string message, string heading = null, UnityAction OnSuccess=null, UnityAction OnFail=null, string successButtonText = "Yes", string failButtonText = "Cancel")
    {
        if(string.IsNullOrEmpty(heading))
        {
            headingText.gameObject.SetActive(false);
        }
        else
        {
            headingText.gameObject.SetActive(true);
            headingText.text = heading.ToUpper();
        }

        messageText.text = message;

        if(OnSuccess != null)
        {
            this.OnSuccess = OnSuccess;
            SuccessButton.GetComponentInChildren<Text>().text = successButtonText;
        }
        else
        {
            SuccessButton.SetActive(false);
        }

        if (OnFail != null)
        {
            this.OnFail = OnFail;
            FailButton.GetComponentInChildren<Text>().text = failButtonText;

        }
        else
        {
            FailButton.SetActive(false);
        }
    }

    public static UIPopup ShowMessage(string message, string heading = null, UnityAction OnSuccess = null, UnityAction OnFail = null, string successButtonText = "Yes", string failButtonText = "Cancel")
    {
        var popup = UIPopup.GetPopup("Alertbox");
        var controller = popup.GetComponent<Alertbox>();

        controller.ShowAlertBox(message, heading, OnSuccess, OnFail, successButtonText, failButtonText);

        popup.Show();

        return popup;
    }

    public void OnSuccessClick()
    {
        OnSuccess?.Invoke();
        this.GetComponent<UIPopup>().Hide();
            
    }

    public void OnFailClick()
    {
        OnFail?.Invoke();
        this.GetComponent<UIPopup>().Hide();
    }
}
