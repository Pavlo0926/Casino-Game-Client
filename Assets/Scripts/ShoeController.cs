using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShoeController : MonoBehaviour
{
    public TextMeshProUGUI ShoeText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadFromState(GoldenFrogTable table)
    {
        ShoeText.text = $"Cards: {table.cardsLeftInShoe}";
    }
}
