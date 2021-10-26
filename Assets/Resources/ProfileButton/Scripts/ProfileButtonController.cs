using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileButtonController : MonoBehaviour
{
    public Image profileImage;
    public TextMeshProUGUI nameText;

    public TextMeshProUGUI creditsText;

    public Texture2D LoadingProfileTexture;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Refresh()
    {
        Player.Instance.RefreshProfile((resp) =>
        {
            string url = "http://graph.facebook.com/" + Player.Instance.FacebookId + "/picture?width=256&height=256";
            Davinci.get().load(url).setLoadingPlaceholder(LoadingProfileTexture).into(profileImage).start();

            nameText.text = Player.Instance.UserName;
            creditsText.text = StringFormatUtils.CurrencyString(Player.Instance.Credits);
        });

    }
}
