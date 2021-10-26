using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : MonoBehaviour
{
    [SerializeField] Transform contentHolder;
    [SerializeField] GameObject SettingsRow;
    [SerializeField] GameObject closeButton;
    
    [SerializeField] Text headingText;
    [SerializeField] Sprite[] listIcons;
    [SerializeField] string[] listValues;

    [SerializeField] ScrollRect scrollRect;
    

    public void Initialize(string heading, bool hasCloseButton = false)
    {

        headingText.text = heading;
        closeButton.SetActive(hasCloseButton);

        int i = 0;

        //For Sound and Viberation
        GameObject soundViberations = Instantiate(SettingsRow);
        soundViberations.transform.SetParent(contentHolder);

        soundViberations.GetComponent<SettingsRow>().Initialize(listValues[i],listIcons[i],true,false,false,false,()=> {
            var settingsPopup = UIPopup.GetPopup("SettingsPopup");
            settingsPopup.GetComponent<SettingsPopup>().InitializeForSounds(listValues[0], true);
            settingsPopup.Show();
        });

        soundViberations.transform.localScale = Vector3.one;

        
        //For Graphics
        i++;
        GameObject graphics = Instantiate(SettingsRow);
        graphics.transform.SetParent(contentHolder);

        graphics.GetComponent<SettingsRow>().Initialize(listValues[i], listIcons[i], true, false, false, false, () => {
            var settingsPopup = UIPopup.GetPopup("SettingsPopup");
            settingsPopup.GetComponent<SettingsPopup>().InitializeForGraphics(listValues[1], true);
            settingsPopup.Show();
        });

        graphics.transform.localScale = Vector3.one;

        //For Lobby Type
        i++;
        GameObject lobbyType = Instantiate(SettingsRow);
        lobbyType.transform.SetParent(contentHolder);
        lobbyType.GetComponent<SettingsRow>().Initialize(listValues[i], listIcons[i], true, false, false, false, () => {
            var popup = UIPopup.GetPopup("List");
            popup.GetComponent<ListManager>().Initialize(GlobalKeys.LOBBY_TYPE, GlobalKeys.lobbyType, PlayerPrefs.GetInt(GlobalKeys.LOBBY_TYPE, 0) + 1, (val) => {
                lobbyType.GetComponent<SettingsRow>().SetText(GlobalKeys.lobbyType[val - 1]);
                PlayerPrefs.SetInt(GlobalKeys.LOBBY_TYPE, val - 1);
                PlayerPrefs.Save();

            });
            popup.Show();
        });
        lobbyType.GetComponent<SettingsRow>().SetText(GlobalKeys.lobbyType[PlayerPrefs.GetInt(GlobalKeys.LOBBY_TYPE, 0)]);
        lobbyType.transform.localScale = Vector3.one;

        //For Score Board
        i++;
        GameObject scoreBoard = Instantiate(SettingsRow);
        scoreBoard.transform.SetParent(contentHolder);

        scoreBoard.GetComponent<SettingsRow>().Initialize(listValues[i], listIcons[i], true, false, false, false, () => {
            var settingsPopup = UIPopup.GetPopup("SettingsPopup");
            settingsPopup.GetComponent<SettingsPopup>().InitializeForScoreboard(listValues[3], true);
            settingsPopup.Show();
        });

        scoreBoard.transform.localScale = Vector3.one;

        //For Squeeze
        i++;
        GameObject squeeze = Instantiate(SettingsRow);
        squeeze.transform.SetParent(contentHolder);

        squeeze.GetComponent<SettingsRow>().Initialize(listValues[i], listIcons[i], true, false, false, false, () => {
            var settingsPopup = UIPopup.GetPopup("SettingsPopup");
            settingsPopup.GetComponent<SettingsPopup>().InitializeForSqueezeType(listValues[4], true);
            settingsPopup.Show();
        });

        squeeze.transform.localScale = Vector3.one;

        //For Language
        i++;
        GameObject language = Instantiate(SettingsRow);
        language.transform.SetParent(contentHolder);
        language.GetComponent<SettingsRow>().Initialize(listValues[i], listIcons[i], true, false, false, false, () => {
            var popup = UIPopup.GetPopup("List");
            popup.GetComponent<ListManager>().Initialize(GlobalKeys.LANGUAGE, GlobalKeys.language, PlayerPrefs.GetInt(GlobalKeys.LANGUAGE, 0) + 1, (val) => {
                language.GetComponent<SettingsRow>().SetText(GlobalKeys.language[val - 1]);
                PlayerPrefs.SetInt(GlobalKeys.LANGUAGE, val - 1);
                PlayerPrefs.Save();

            });
            popup.Show();
        });
        language.GetComponent<SettingsRow>().SetText(GlobalKeys.language[PlayerPrefs.GetInt(GlobalKeys.LANGUAGE, 0)]);
        language.transform.localScale = Vector3.one;

        //For Country
        i++;
        GameObject country = Instantiate(SettingsRow);
        country.transform.SetParent(contentHolder);
        country.GetComponent<SettingsRow>().Initialize(listValues[i], listIcons[i], true, false, false, false, () => {
            var popup = UIPopup.GetPopup("List");
            popup.GetComponent<ListManager>().Initialize(GlobalKeys.COUNTRY, GlobalKeys.country, PlayerPrefs.GetInt(GlobalKeys.COUNTRY, 0) + 1, (val) => {
                country.GetComponent<SettingsRow>().SetText(GlobalKeys.country[val - 1]);
                PlayerPrefs.SetInt(GlobalKeys.COUNTRY, val - 1);
                PlayerPrefs.Save();

            });
            popup.Show();
        });
        country.GetComponent<SettingsRow>().SetText(GlobalKeys.country[PlayerPrefs.GetInt(GlobalKeys.COUNTRY, 0)]);
        country.transform.localScale = Vector3.one;

        //For Chat
        i++;
        GameObject chat = Instantiate(SettingsRow);
        chat.transform.SetParent(contentHolder);

        chat.GetComponent<SettingsRow>().Initialize(listValues[i], listIcons[i], true, false, false, false, () => {
            var popup = UIPopup.GetPopup("Alertbox");
            popup.GetComponent<Alertbox>().ShowAlertBox("Need To Be Implemented", "Are you leaving?", () =>
            {
                popup.Hide();
            },
            () =>
            {
                popup.Hide();
            });
            popup.Show();
        });

        chat.transform.localScale = Vector3.one;

        //For Review Us
        i++;
        GameObject reviewus = Instantiate(SettingsRow);
        reviewus.transform.SetParent(contentHolder);

        reviewus.GetComponent<SettingsRow>().Initialize(listValues[i], listIcons[i], true, false, false, false, () => {
            var popup = UIPopup.GetPopup("Alertbox");
            popup.GetComponent<Alertbox>().ShowAlertBox("This will open in your browser.", "Are you leaving?",()=>
            {
                Application.OpenURL(GlobalKeys.RateUsURL);
            },
            () =>
            {
                popup.Hide();
            });
            popup.Show();
        });

        reviewus.transform.localScale = Vector3.one;

        //For GameVersion
        i++;
        GameObject gameVersion = Instantiate(SettingsRow);
        gameVersion.transform.SetParent(contentHolder);

        gameVersion.GetComponent<SettingsRow>().Initialize(listValues[i], listIcons[i], true, false, false, false, () => {
            var popup = UIPopup.GetPopup("Alertbox");
            popup.GetComponent<Alertbox>().ShowAlertBox("Your Game Version is Upto Date", "Checking Game Update", () =>
            {
                popup.Hide();
            },
            () =>
            {
                popup.Hide();
            });
            popup.Show();
        });

        gameVersion.transform.localScale = Vector3.one;

        //For PrivacyPolicy
        i++;
        GameObject privacyPolicy = Instantiate(SettingsRow);
        privacyPolicy.transform.SetParent(contentHolder);

        privacyPolicy.GetComponent<SettingsRow>().Initialize(listValues[i], listIcons[i], true, false, false, false, () => {
            var popup = UIPopup.GetPopup("Alertbox");
            popup.GetComponent<Alertbox>().ShowAlertBox("This will open in your browser.", "Are you leaving?", () =>
            {
                Application.OpenURL(GlobalKeys.PolicyURL);
            },
            () =>
            {
                popup.Hide();
            });
            popup.Show();
        });

        privacyPolicy.transform.localScale = Vector3.one;

        //For TOS
        i++;
        GameObject tos = Instantiate(SettingsRow);
        tos.transform.SetParent(contentHolder);

        tos.GetComponent<SettingsRow>().Initialize(listValues[i], listIcons[i], true, false, false, false, () => {
            var popup = UIPopup.GetPopup("Alertbox");
            popup.GetComponent<Alertbox>().ShowAlertBox("This will open in your browser.", "Are you leaving?", () =>
            {
                Application.OpenURL(GlobalKeys.TOSURL);
            },
            ()=>
            {
                popup.Hide();
            });
            popup.Show();
        });

        tos.transform.localScale = Vector3.one;


        Invoke("SetInertia",1f);
    }

    public void SetInertia()
    {
        scrollRect.inertia = true;
    }

    
    public void OnClose()
    {
        GetComponent<UIPopup>().Hide();
        print("Hidre");
    }

    public void InitializeForSounds(string heading , bool hasCloseButton = false)
    {
        headingText.text = heading;
        closeButton.SetActive(hasCloseButton);


        GameObject voice = Instantiate(SettingsRow);
        voice.transform.SetParent(contentHolder);

        voice.GetComponent<SettingsRow>().Initialize(GlobalKeys.VOICE, null, false, true, false, false);

        voice.transform.localScale = Vector3.one;

        GameObject effects = Instantiate(SettingsRow);
        effects.transform.SetParent(contentHolder);

        effects.GetComponent<SettingsRow>().Initialize(GlobalKeys.EFFECTS, null, false, true, false, false);

        effects.transform.localScale = Vector3.one;

        GameObject music = Instantiate(SettingsRow);
        music.transform.SetParent(contentHolder);

        music.GetComponent<SettingsRow>().Initialize(GlobalKeys.MUSIC, null, false, true, false, false);

        music.transform.localScale = Vector3.one;


        GameObject vibration = Instantiate(SettingsRow);
        vibration.transform.SetParent(contentHolder);

        vibration.GetComponent<SettingsRow>().Initialize(GlobalKeys.VIBRATION,null,false,false,true,false);

        vibration.transform.localScale = Vector3.one;

        Invoke("SetInertia", 1f);
    }

    public void InitializeForGraphics(string heading, bool hasCloseButton = false)
    {
        headingText.text = heading;
        closeButton.SetActive(hasCloseButton);

        GameObject bgColor = Instantiate(SettingsRow);
        bgColor.transform.SetParent(contentHolder);
        bgColor.GetComponent<SettingsRow>().SetText(GlobalKeys.bgcolors[PlayerPrefs.GetInt(GlobalKeys.BACKGROUND_COLOR, 0)]);
        bgColor.GetComponent<SettingsRow>().Initialize(GlobalKeys.BACKGROUND_COLOR, null, true, false, false, true,()=> {
            var popup = UIPopup.GetPopup("List");
            popup.GetComponent<ListManager>().Initialize(GlobalKeys.BACKGROUND_COLOR, GlobalKeys.bgcolors,PlayerPrefs.GetInt(GlobalKeys.BACKGROUND_COLOR,0)+1,(val)=> {
                bgColor.GetComponent<SettingsRow>().SetText(GlobalKeys.bgcolors[val-1]);
                PlayerPrefs.SetInt(GlobalKeys.BACKGROUND_COLOR, val-1);
                PlayerPrefs.Save();
                
            });
            popup.Show();
        });

        bgColor.transform.localScale = Vector3.one;

        GameObject showShadow = Instantiate(SettingsRow);
        showShadow.transform.SetParent(contentHolder);

        showShadow.GetComponent<SettingsRow>().Initialize(GlobalKeys.SHOW_SHADOW, null, false, false, true, false);

        showShadow.transform.localScale = Vector3.one;

        Invoke("SetInertia", 1f);
    }


    public void InitializeForScoreboard(string heading, bool hasCloseButton = false)
    {
        headingText.text = heading;
        closeButton.SetActive(hasCloseButton);

        GameObject sbType = Instantiate(SettingsRow);
        sbType.transform.SetParent(contentHolder);
        sbType.GetComponent<SettingsRow>().SetText(GlobalKeys.scoreBoardType[PlayerPrefs.GetInt(GlobalKeys.SCOREBOARD_TYPE, 0)]);
        sbType.GetComponent<SettingsRow>().Initialize(GlobalKeys.SCOREBOARD_TYPE, null, true, false, false, true, () => {
            var popup = UIPopup.GetPopup("List");
            popup.GetComponent<ListManager>().Initialize(GlobalKeys.SCOREBOARD_TYPE, GlobalKeys.scoreBoardType, PlayerPrefs.GetInt(GlobalKeys.SCOREBOARD_TYPE, 0) + 1, (val) => {
                sbType.GetComponent<SettingsRow>().SetText(GlobalKeys.scoreBoardType[val - 1]);
                PlayerPrefs.SetInt(GlobalKeys.SCOREBOARD_TYPE, val - 1);
                PlayerPrefs.Save();

            });
            popup.Show();
        });

        sbType.transform.localScale = Vector3.one;



        GameObject automaticDisplay = Instantiate(SettingsRow);
        automaticDisplay.transform.SetParent(contentHolder);

        automaticDisplay.GetComponent<SettingsRow>().Initialize(GlobalKeys.AUTOMATIC_DISPLAY, null, false, false, true, false);

        automaticDisplay.transform.localScale = Vector3.one;


        GameObject showTie = Instantiate(SettingsRow);
        showTie.transform.SetParent(contentHolder);

        showTie.GetComponent<SettingsRow>().Initialize(GlobalKeys.SHOW_TIE, null, false, false, true, false);

        showTie.transform.localScale = Vector3.one;


        GameObject showPair = Instantiate(SettingsRow);
        showPair.transform.SetParent(contentHolder);

        showPair.GetComponent<SettingsRow>().Initialize(GlobalKeys.SHOW_PAIR, null, false, false, true, false);

        showPair.transform.localScale = Vector3.one;


        GameObject showNatural = Instantiate(SettingsRow);
        showNatural.transform.SetParent(contentHolder);

        showNatural.GetComponent<SettingsRow>().Initialize(GlobalKeys.SHOW_NATURAL, null, false, false, true, false);

        showNatural.transform.localScale = Vector3.one;


        GameObject showWin = Instantiate(SettingsRow);
        showWin.transform.SetParent(contentHolder);

        showWin.GetComponent<SettingsRow>().Initialize(GlobalKeys.SHOW_WIN, null, false, false, true, false);

        showWin.transform.localScale = Vector3.one;


        GameObject showLoose = Instantiate(SettingsRow);
        showLoose.transform.SetParent(contentHolder);

        showLoose.GetComponent<SettingsRow>().Initialize(GlobalKeys.SHOW_LOOSE, null, false, false, true, false);

        showLoose.transform.localScale = Vector3.one;


        GameObject showWinningNumber = Instantiate(SettingsRow);
        showWinningNumber.transform.SetParent(contentHolder);

        showWinningNumber.GetComponent<SettingsRow>().Initialize(GlobalKeys.SHOW_WINNING_NUMBER, null, false, false, true, false);

        showWinningNumber.transform.localScale = Vector3.one;

        Invoke("SetInertia", 1f);
    }

    public void InitializeForSqueezeType(string heading, bool hasCloseButton = false)
    {

        headingText.text = heading;
        closeButton.SetActive(hasCloseButton);

        GameObject sqType = Instantiate(SettingsRow);
        sqType.transform.SetParent(contentHolder);
        sqType.GetComponent<SettingsRow>().SetText(GlobalKeys.squeezeType[PlayerPrefs.GetInt(GlobalKeys.SQUEEZE_TYPE, 0)]);
        sqType.GetComponent<SettingsRow>().Initialize(GlobalKeys.SQUEEZE_TYPE, null, true, false, false, true, () => {
            var popup = UIPopup.GetPopup("List");
            popup.GetComponent<ListManager>().Initialize(GlobalKeys.SQUEEZE_TYPE, GlobalKeys.squeezeType, PlayerPrefs.GetInt(GlobalKeys.SQUEEZE_TYPE, 0) + 1, (val) => {
                sqType.GetComponent<SettingsRow>().SetText(GlobalKeys.squeezeType[val - 1]);
                PlayerPrefs.SetInt(GlobalKeys.SQUEEZE_TYPE, val - 1);
                PlayerPrefs.Save();

            });
            popup.Show();
        });

        sqType.transform.localScale = Vector3.one;


        GameObject sqGuide = Instantiate(SettingsRow);
        sqGuide.transform.SetParent(contentHolder);

        sqGuide.GetComponent<SettingsRow>().Initialize(GlobalKeys.SQUEEZE_GUIDE, null, false, false, true, false);

        sqGuide.transform.localScale = Vector3.one;


        GameObject sqEffect = Instantiate(SettingsRow);
        sqEffect.transform.SetParent(contentHolder);

        sqEffect.GetComponent<SettingsRow>().Initialize(GlobalKeys.EFFECTS, null, false, false, true, false);

        sqEffect.transform.localScale = Vector3.one;


        GameObject autoZoomIn = Instantiate(SettingsRow);
        autoZoomIn.transform.SetParent(contentHolder);
        autoZoomIn.GetComponent<SettingsRow>().SetText(GlobalKeys.autoZoomIn[PlayerPrefs.GetInt(GlobalKeys.AUTO_ZOOM_IN, 0)]);
        autoZoomIn.GetComponent<SettingsRow>().Initialize(GlobalKeys.AUTO_ZOOM_IN, null, true, false, false, true, () => {
            var popup = UIPopup.GetPopup("List");
            popup.GetComponent<ListManager>().Initialize(GlobalKeys.AUTO_ZOOM_IN, GlobalKeys.autoZoomIn, PlayerPrefs.GetInt(GlobalKeys.AUTO_ZOOM_IN, 0) + 1, (val) => {
                sqType.GetComponent<SettingsRow>().SetText(GlobalKeys.autoZoomIn[val - 1]);
                PlayerPrefs.SetInt(GlobalKeys.AUTO_ZOOM_IN, val - 1);
                PlayerPrefs.Save();

            });
            popup.Show();
        });
        autoZoomIn.transform.localScale = Vector3.one;



        GameObject howToSqueeze = Instantiate(SettingsRow);
        howToSqueeze.transform.SetParent(contentHolder);

        howToSqueeze.GetComponent<SettingsRow>().Initialize(GlobalKeys.HOW_TO_SQUEEZE, null, true, false, false, false, () => {
            var popup = UIPopup.GetPopup("Alertbox");
            popup.GetComponent<Alertbox>().ShowAlertBox("This will open in your browser.", "Are you leaving?", () =>
            {
                Application.OpenURL(GlobalKeys.TOSURL);
            },
            () =>
            {
                popup.Hide();
            });
            popup.Show();
        });

        howToSqueeze.transform.localScale = Vector3.one;

        Invoke("SetInertia", 1f);
    }


}
