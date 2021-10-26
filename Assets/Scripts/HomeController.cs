using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Doozy.Engine.UI;
using UnityEngine;

public class HomeController : MonoBehaviour
{
    public TableListController TableListController;
    public ProfileButtonController ProfileButtonController;

    public UIView loadingView;

    private GoldenFrogLobbyClient lobbyClient;

    private static HomeController instance;
    public static HomeController Instance
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
            instance = this;

            lobbyClient = GetComponent<GoldenFrogLobbyClient>();
        }
        else
        {
            DestroyImmediate(this.gameObject);
        }
    }

    public void LoadProfileButton()
    {
        ProfileButtonController.Refresh();
    }

    // Refreshes the table data based on the filter
    public Task RefreshTables(int minimumBet)
    {
        loadingView.Show();
        return lobbyClient.FilterTables(minimumBet);
    }


    public void LoadTables(RoomListingData[] data)
    {
        TableListController.LoadData(data);
        loadingView.Hide();
    }

    public void UpdateTable(string roomId, RoomListingData data)
    {
        TableListController.UpdateListItem(roomId, data);
    }
}
