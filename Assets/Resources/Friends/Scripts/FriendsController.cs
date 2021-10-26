using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using Facebook.Unity;
using GameSparks.Api.Requests;
using UnityEngine;
using System.Linq;

public class FriendsController : MonoBehaviour
{
    public UIView inviteFriendsView;
    public UIView friendsListView;

    public FriendsListController friendsListController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowFriendsListTab()
    {
        UIView.HideViewCategory("Friends");

        friendsListView.Show();
    }

    public void ShowInviteFriendsTab()
    {
        UIView.HideViewCategory("Friends");
        inviteFriendsView.Show();

        new ListGameFriendsRequest()
            .Send((response) => {
                var friends = response.Friends;

                Debug.Log(response.JSONString);
                Debug.Log(response.Friends.Count());

                friendsListController.LoadData(friends.ToArray());
            });

    }

    public void OnInviteFacebookFriendClick()
    {
        FB.AppRequest("Come play this great game!",
            null, null, null, null, null, null,
            delegate (IAppRequestResult result)
            {
                Debug.Log(result.RawResult);
            }
        );
    }
}
