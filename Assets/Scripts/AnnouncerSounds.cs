using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.Soundy;
using UnityEngine;

public class AnnouncerSounds : MonoBehaviour
{
    public void PlayGameFinishedSounds(GoldenFrogEvaluation evaluation, int playerPoints, int bankerPoints, Action callback)
    {
        var sequencePlaylist = new List<string>();


        PlaySoundWithCallback($"Banker-has-{bankerPoints}", () =>
        {
            PlaySoundWithCallback($"Player-has-{playerPoints}", () => 
            {
                switch (evaluation.outcome)
                {
                    case "banker":
                    PlaySoundWithCallback("Banker-Wins", callback);
                    break;
                    case "player":
                    PlaySoundWithCallback("Player-Wins", callback);
                    break;
                    case "tie":
                    PlaySoundWithCallback("Its-a-tie", callback);
                    break;
                }
            });
        });
    }

    public void PlaySoundWithCallback(string announcerClipName, Action callback)
    {
        var clip = SoundyManager.Play("Announcer", announcerClipName);
        StartCoroutine(DelayedCallback(clip.AudioSource.clip.length, callback));
    }

    private IEnumerator DelayedCallback(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }
}
