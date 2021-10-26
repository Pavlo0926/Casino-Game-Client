using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TableInformationController : MonoBehaviour
{
    public TextMeshProUGUI tableNameText;
    public TextMeshProUGUI minBetText;
    public TextMeshProUGUI maxBetText;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void LoadTableInformation(GoldenFrogTableInformation tableInformation)
    {
        tableNameText.text = tableInformation.tableName;
        minBetText.text = "Min Bet: " + NumberUtils.ToFriendlyQuantityString(tableInformation.minimumBet);
        maxBetText.text = "Max Bet: " + NumberUtils.ToFriendlyQuantityString(tableInformation.maximumBet);
    }
}
