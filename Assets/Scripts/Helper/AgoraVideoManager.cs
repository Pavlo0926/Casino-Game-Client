using agora_gaming_rtc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AgoraVideoManager : MonoBehaviour
{
    public const string AppId = "f2c04b25fd03420fb3a825c1798fa911";
    // instance of agora engine
    private IRtcEngine mRtcEngine;

    private bool isVideoStreamingEnabled = false;

    ~AgoraVideoManager()
    {
        leave();
    }

    public void loadEngine()
    {
        // start sdk
        if (mRtcEngine != null)
        {
            Debug.Log("Engine exists. Please unload it first!");
            return;
        }

        // init engine
        mRtcEngine = IRtcEngine.GetEngine(AppId);
        mRtcEngine.OnError += OnError;

        // enable log
        mRtcEngine.SetLogFilter(LOG_FILTER.DEBUG | LOG_FILTER.INFO | LOG_FILTER.WARNING | LOG_FILTER.ERROR | LOG_FILTER.CRITICAL);
    }

    private void OnError(int error, string msg)
    {
        Debug.LogError(error);
        Debug.LogError(msg);
    }

    public void unloadEngine()
    {
        // delete
        if (mRtcEngine != null)
        {
            IRtcEngine.Destroy();  // Place this call in ApplicationQuit
            mRtcEngine = null;
        }
    }

    public void join(string channel, uint videoStreamUid)
    {
        Debug.Log("calling join (channel = " + channel + ")");

        if (mRtcEngine == null)
            return;

        Application.RequestUserAuthorization(UserAuthorization.WebCam);
        Application.RequestUserAuthorization(UserAuthorization.Microphone);

        // set callbacks (optional)
        mRtcEngine.OnJoinChannelSuccess = onJoinChannelSuccess;
        mRtcEngine.OnUserJoined = onUserJoined;
        mRtcEngine.OnUserOffline = onUserOffline;
        mRtcEngine.OnUserEnableVideo = onUserEnableVideo;

        //EnableVideo(true);
        // enable video
        mRtcEngine.EnableVideo();
        // allow camera output callback
        mRtcEngine.EnableVideoObserver();

        // join channel
        mRtcEngine.JoinChannel(channel, null, videoStreamUid);

        Debug.Log("initializeEngine done");
    }

    private void onUserEnableVideo(uint uid, bool enabled)
    {
        Debug.Log("onUserEnableVideo: uid = " + uid + " enabled = " + enabled);
    }

    public void leave()
    {
        Debug.Log("calling leave");

        if (mRtcEngine == null)
            return;

        // leave channel
        mRtcEngine.LeaveChannel();
        // deregister video frame observers in native-c code
        mRtcEngine.DisableVideoObserver();
    }



    private void onUserOffline(uint uid, USER_OFFLINE_REASON reason)
    {
        Debug.Log("onUserOffline: uid = " + uid);
        GoldenFrogTableController.Instance.DetachVideoStreamToAvatarByVideoStreamUid(uid);

    }

    // When a remote user joined, this delegate will be called. Typically
    // create a GameObject to render video on it
    private void onUserJoined(uint uid, int elapsed)
    {
        Debug.Log("onUserJoined: uid = " + uid + " elapsed = " + elapsed);
        GoldenFrogTableController.Instance.AttachVideoStreamToAvatarByVideoStreamUid(uid);


        //// this is called in main thread

        //// find a game object to render video stream from 'uid'
        //GameObject go = GameObject.Find(uid.ToString());
        //if (!ReferenceEquals(go, null))
        //{
        //    return; // reuse
        //}

        //// create a GameObject and assign to this new user
        //VideoSurface videoSurface = makeImageSurface(uid.ToString());
        //if (!ReferenceEquals(videoSurface, null))
        //{
        //    // configure videoSurface
        //    videoSurface.SetForUser(uid);
        //    videoSurface.SetEnable(true);
        //    videoSurface.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);
        //    videoSurface.SetGameFps(30);
        //}
    }

    private void onJoinChannelSuccess(string channelName, uint uid, int elapsed)
    {
        Debug.Log("JoinChannelSuccessHandler: uid = " + uid + " channelName: " + channelName);
        //GameObject textVersionGameObject = GameObject.Find("VersionText");
        //textVersionGameObject.GetComponent<Text>().text = "SDK Version : " + getSdkVersion();
    }
}