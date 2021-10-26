using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;
using System;
using Doozy.Engine.UI;
using Doozy.Engine.SceneManagement;
using UnityEngine.SceneManagement;
using Colyseus;

[Serializable]
public class IAPHelper
{
    public bool hasPromo;
    public Text promoText, priceText;
    public GameObject promoBanner;
    public string IAPBundle;
}

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenuScreen;
    [SerializeField] GameObject roomScreen;
    [SerializeField] GameObject chipsStoreScreen;
    [SerializeField] GameObject settingsScreen;
    [SerializeField] GameObject profileScreen;
    [SerializeField] GameObject loginWindow;

    [SerializeField] InputField passwordInputField;
    [SerializeField] InputField usernameInputField;

    [SerializeField] Text messageText;

    [SerializeField] GameObject[] Chips;

    static UIManager instance;

    GoldenFrogLobbyClient lobbyClient;
    public int currentIndex = 1;

    public static UIManager Instance
    {
        get {
            return instance;
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            //wager = new GoldenFrogWager();

            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            DestroyImmediate(this.gameObject);
        }
    }

    void Start()
    {
        lobbyClient = GetComponent<GoldenFrogLobbyClient>();
    }

    #region clickenvent

    public void OnFacebookClickEvent()
    {
        //mainMenuScreen.SetActive(false);
        //roomScreen.SetActive(true);
        NetworkManager.Instance.FacebookConnect();
    }

    public void OnEmailClickEvent()
    {
        loginWindow.SetActive(true);
    }

    public void OnGuestClickEvent()
    {
        mainMenuScreen.SetActive(false);
        roomScreen.SetActive(true);
    }

    public void OnSettingsClickEvent()
    {
        var popup = UIPopup.GetPopup("MenuPopup");
        popup.Show();
    }

    public void OnChipsStoreClickEvent()
    {
        var popup = ChipStoreController.CreateShopDialog();
        popup.Show();
    }

    public void OnProfileStoreClickEvent()
    {
        var popup = UIPopup.GetPopup("ProfilePopup");
        var profileScreen = popup.GetComponent<ProfileScreen>();
        profileScreen.LoadPlayerProfile(Player.Instance.UserId);
        popup.Show();
    }

    public void OnPlayNowClickEvent()
    {
        //var popup = UIPopup.GetPopup("Notification");
        //popup.GetComponent<Notification>().Initialize("Notification", "I am Notification Body!", null);
        //popup.Show();

        ShowGameScreen(null);
    }

    public void OnLoginClicked()
    {
        if(usernameInputField.text.Equals(string.Empty))
        {
            NetworkManager.Instance.UserAuthentication("mickel@gmail.com", "123456");
        }
        else
        {
            NetworkManager.Instance.UserAuthentication(usernameInputField.text, passwordInputField.text);
        }
    }

    #endregion

    public void Print(string text)
    {
        messageText.text += text+"\n";
    }
    
    public void ShowHomeScreen()
    {
        UIView.HideViewCategory("Screens");
        loginWindow.SetActive(false);

        GetComponent<HomeController>().loadingView.Show();
        
        GetComponent<HomeController>().LoadProfileButton();
        roomScreen.GetComponent<UIView>().Show();
    }

    public async void OnHomeScreenShown()
    {
        try
        {
            await lobbyClient.JoinLobbyRoom();
        }
        catch (Exception)
        {
            Alertbox.ShowMessage("Could not join lobby.", "Lobby Down");
            // Show login screen?
        }
    }


    private void OnGamePlaySceneUnloaded(Scene scene)
    {
        SceneManager.sceneUnloaded -= OnGamePlaySceneUnloaded;
        onGamePlaySceneUnloaded?.Invoke();
    }

    private Action onGamePlaySceneUnloaded;

    public async void UnloadGamePlayScene(Action onUnloaded)
    {
        onGamePlaySceneUnloaded = onUnloaded;
        SceneManager.sceneUnloaded += OnGamePlaySceneUnloaded;

        await GoldenFrogTableController.Instance.GetComponent<GoldenFrogRoomClient>().Disconnect();
        GoldenFrogTableController.DestroyInstance();

        SceneManager.UnloadSceneAsync("GamePlayScene");
    }

    private string roomId;

    private async void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
            if (scene.name == "GamePlayScene")
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
                var tableController = GoldenFrogTableController.Instance;

#if !UNITY_EDITOR
                // Enable the agora internal data structures.
                var agoraVideoManager = tableController.GetComponent<AgoraVideoManager>();
                agoraVideoManager.loadEngine();
#endif

                //roomScreen.GetComponent<UIView>().Hide();
                try
                {
                    var gameClient = tableController.GetComponent<GoldenFrogRoomClient>();
                    gameClient.CreateConnection();
                    await gameClient.ConnectToServer(roomId);
                }
                catch (MatchMakeException e)
                {
                    // Room locked
                    if (e.Code == 4212)
                    {
                        Alertbox.ShowMessage("The table is already full.", "Already Full");
                    }
                    else
                    {
                        Alertbox.ShowMessage("Could not join the table.", "Failed To Join");
                    }

                    UnloadGamePlayScene(() =>
                    {
                        ShowHomeScreen();
                    });

                }
            }
    }

    public async void ShowGameScreen(string roomId)
    {
        if (roomId != null && !PlayerHasEnoughCreditsToJoinRoom())
        {
            var popup = ChipStoreController.CreateShopDialog();
            popup.Show();

            return;
        }
        this.roomId = roomId;
        
        await this.lobbyClient.LeaveLobbyRoom();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadSceneAsync("GamePlayScene", LoadSceneMode.Additive);

        //roomScreen.GetComponent<UIView>().Hide();
        //mainMenuScreen.GetComponent<UIView>().Hide();
        //roomScreen.GetComponent<UIView>().Hide();
        //gameplayScreen.GetComponent<UIView>().Show();
    }

    public void ShowPopup()
    {
        Alertbox.ShowMessage("Welcome to Golden Frog Baccarat Game!", "Welcome", ()=> {
            Debug.Log("On Success Called");
        });
    }

    private bool PlayerHasEnoughCreditsToJoinRoom()
    {
        var playerCredits = Player.Instance.Credits;

        return playerCredits >= lobbyClient.minimumBetFilter;
    }

    public void AddChips(int amount, string sessionID, WagerType wager)
    {
        // int index = 0;
        // if (amount == 500)
        //     index = 1;
        // else if (amount == 1000)
        //     index = 2;
        // else if (amount == 5000)
        //     index = 3;
        // else if (amount == 10000)
        //     index = 4;


        // int seatIndex = 0;

        // for(int i=0;i<sessionIds.Length;i++)
        // {
        //     if(sessionIds[i].Equals(sessionID))
        //     {
        //         seatIndex = i;
        //         break;
        //     }
        // }

        // GameObject obj = Instantiate(Chips[index]);
        // obj.transform.SetParent(gameplayScreen.transform);
        // obj.transform.position = Vector3.zero;
        // obj.GetComponent<RectTransform>().anchoredPosition = GameObject.Find("Seat"+seatIndex).GetComponent<RectTransform>().anchoredPosition;
        // obj.GetComponent<RectTransform>().localScale = Vector3.one;

        // obj.name = amount.ToString();

        // if(wager == WagerType.Banker)
        //     obj.GetComponent<ChipHandler>().SetForOtherPlayer(GameObject.Find("Seat" + seatIndex).transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition);
        // else
        //     obj.GetComponent<ChipHandler>().SetForOtherPlayer(GameObject.Find("Seat" + seatIndex).transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition);
    }
}
