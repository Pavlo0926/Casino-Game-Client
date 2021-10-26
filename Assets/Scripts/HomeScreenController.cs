using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using Facebook.Unity;
using UnityEngine;
using UnityEngine.UI;

public class HomeScreenController : MonoBehaviour
{

    [SerializeField] Toggle multiPlayerToggle, singlePlayerToggle, rankingsToggle, friendsToggle;
    [SerializeField] GameObject multiPlayerPanel, singlePlayerPanel, rankingsPanel, friendsPanel;

    public RankingsController RankingsController;
    public FriendsController FriendsController;
    void Start()
    {
        multiPlayerToggle.onValueChanged.AddListener(MultiPlayerToggleValueChanged);
        singlePlayerToggle.onValueChanged.AddListener(SinglePlayerToggleValueChanged);
        rankingsToggle.onValueChanged.AddListener(RankingsToggleValueChanged);
        friendsToggle.onValueChanged.AddListener(FriendsToggleValueChanged);
    }

    private void FriendsToggleValueChanged(bool val)
    {
        if (val)
        {
            UIView.HideViewCategory("Main");
            friendsPanel.GetComponent<UIView>().Show();
            // friendsPanel.SetActive(true);
            // rankingsPanel.SetActive(false);
            // multiPlayerPanel.SetActive(false);
            // singlePlayerPanel.SetActive(false);

            //FriendsController.ShowFriendsListTab();
        }
        
    }

    private void RankingsToggleValueChanged(bool val)
    {
        if (val)
        {
            UIView.HideViewCategory("Main");
            rankingsPanel.GetComponent<UIView>().Show();
            // friendsPanel.SetActive(false);
            // rankingsPanel.SetActive(true);
            // multiPlayerPanel.SetActive(false);
            // singlePlayerPanel.SetActive(false);

            //RankingsController.LoadDailyLeaderboards();
        }
    }

    private void SinglePlayerToggleValueChanged(bool val)
    {
        if (val)
        {
            UIView.HideViewCategory("Main");

            var popup = UIPopup.GetPopup("SinglePlayer");
            popup.Show();
        }

    }

    private void MultiPlayerToggleValueChanged(bool val)
    {
        if (val)
        {
            UIView.HideViewCategory("Main");
            multiPlayerPanel.GetComponent<UIView>().Show();

            // friendsPanel.SetActive(false);
            // rankingsPanel.SetActive(false);
            // multiPlayerPanel.SetActive(true);
            // singlePlayerPanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
