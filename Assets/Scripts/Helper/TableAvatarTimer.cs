using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.Soundy;
using UnityEngine;
using UnityEngine.UI;

public class TableAvatarTimer : MonoBehaviour
{
    public AudioSource FastTickingSound;
    public AudioSource SlowTickingSound;

    private float time = 0;
    private float totalTime = 0;
    private Image progress;

    // Start is called before the first frame update
    void Start()
    {
        progress = GetComponent<Image>();
        StopCountdown();
    }

    public void StartCountdown(int seconds)
    {
        totalTime = time = seconds;
        progress.enabled = true;
    }

    public void StopCountdown()
    {
        progress.enabled = false;
        FastTickingSound.Stop();
        SlowTickingSound.Stop();
    }

    void Update()
    {
        if(time  > 0)
        {
            time -= Time.deltaTime;
            this.progress.fillAmount = (time / totalTime);

            progress.color = Color.Lerp(Color.red, Color.green, time / totalTime);

            if (time <= 12 && time >= 3)
            {
                if (!SlowTickingSound.isPlaying)
                    SlowTickingSound.Play();

                SlowTickingSound.volume = Mathf.Lerp(1, .33f, time / 10f);
                //TickingSound.pitch = Mathf.Lerp(1.5f, 1, time / 10f);
            }
            else if (time <= 3)
            {
                SlowTickingSound.Stop();
                if (!FastTickingSound.isPlaying)
                    FastTickingSound.Play();
            }
        }
        else
        {
            FastTickingSound.Stop();
            SlowTickingSound.Stop();
        }
    }
}
