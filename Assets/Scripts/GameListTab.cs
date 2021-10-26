using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameListTab : MonoBehaviour
{
    [SerializeField] int betAmount;

    private void Awake()
    {
        this.GetComponent<Toggle>().onValueChanged.AddListener(OnTabValueChange);
    }

    public void OnTabValueChange(bool selected)
    {
        if (selected)
            HomeController.Instance.RefreshTables(betAmount);
    }
}
