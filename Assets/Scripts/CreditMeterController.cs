using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Doozy.Engine.UI;

public class CreditMeterController : MonoBehaviour
{
    public UIView deltaCreditsView;
    public UIView creditsView;

    private TextMeshProUGUI deltaText;
    private TextMeshProUGUI creditText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        deltaText = deltaCreditsView.GetComponentInChildren<TextMeshProUGUI>();
        creditText = creditsView.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowTotalCredits(uint credits)
    {
        creditText.text = StringFormatUtils.CurrencyString(credits);

        ShowTotalCreditsView();
    }

    public void ShowDeltaCreditsView(int credits)
    {
        deltaText.text = StringFormatUtils.PayoutString(credits);

        ShowDeltaCreditsView();
    }

    private void ShowTotalCreditsView()
    {
        creditsView.Show();
        deltaCreditsView.Hide();
    }

    private void ShowDeltaCreditsView()
    {
        deltaCreditsView.Show();
        creditsView.Hide();
    }

    
}
