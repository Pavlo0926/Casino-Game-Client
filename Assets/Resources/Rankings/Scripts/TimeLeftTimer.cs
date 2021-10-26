using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimeLeftTimer : MonoBehaviour
{
    private float timeLeftSeconds = 0;
    private TextMeshProUGUI timeLeftText;

    // Start is called before the first frame update
    void Start()
    {
        timeLeftText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timeLeftSeconds  > 0)
        {
            timeLeftSeconds -= Time.deltaTime;

            var t = TimeSpan.FromSeconds(timeLeftSeconds);

            timeLeftText.text = $"Time Left: " + t.ToString("hh\\:mm\\:ss");
        }
    }

    public void SetText(string text)
    {
        timeLeftSeconds = 0;
        timeLeftText.text = text;
    }

    public void StartTimer(int timeLeft)
    {
        timeLeftSeconds = timeLeft;
    }
}
