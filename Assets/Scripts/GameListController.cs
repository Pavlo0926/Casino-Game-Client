using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameListController : MonoBehaviour
{
    [SerializeField]
    GameObject rowPrefab;

    [SerializeField] Transform rowContainer;
    [SerializeField] ToggleGroup tabs;
    List<GameObject> data;

    [SerializeField]GameObject list;

    private void Start()
    {
        data = new List<GameObject>();
        //LoadNewData(100);
    }

    public void LoadNewData(int betAmount)
    {
        ClearData();
        for (int i = 0; i < 10; i++)
            InstantiateRow();
    }

    public void InstantiateRow()
    {
        GameObject row = GameObject.Instantiate(rowPrefab);
        row.transform.SetParent( rowContainer);
        row.transform.localScale = Vector3.one;

        row.GetComponent<GameListRow>().Initialize(Random.Range(100, 10000), "Alex Micheal", null, null, Random.Range(0, 6));

        data.Add(row);
    }

    void ClearData()
    {
        foreach (GameObject obj in data)
        {
            GameObject.Destroy(obj);
            
        }

        data.Clear();
    }

    public void SetList(bool active)
    {
        list.SetActive(active);
    }
}
