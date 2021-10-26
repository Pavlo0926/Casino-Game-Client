using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAvatarControls : MonoBehaviour
{
    public AgoraVideoManager videoManager;
    public GameObject VideoChat;

    public Sprite VideoChatEnabledSprite;
    public Sprite VideoChatDisabledSprite;

    private bool videoChatEnabled = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnVideoChatToggledClick()
    {
        videoChatEnabled = !videoChatEnabled;

        if (videoChatEnabled)
        {
            OnVideoChatStartClick();
        }
        else
        {
            OnVideoChatStopClick();
        }
    }

    public void OnVideoChatStartClick()
    {

        GoldenFrogTableController.Instance.EnableVideoChattingAvatars(); ;
        VideoChat.GetComponent<Image>().sprite = VideoChatEnabledSprite;
    }

    public void OnVideoChatStopClick()
    {
        VideoChat.GetComponent<Image>().sprite = VideoChatDisabledSprite;
        GoldenFrogTableController.Instance.DisableVideoChattingAvatars();
    }
}
