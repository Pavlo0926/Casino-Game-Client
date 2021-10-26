using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ListManager : MonoBehaviour
{
    [SerializeField] Text headingText;
    [SerializeField] GameObject listRow;
    [SerializeField] Transform parent;
    ToggleGroup toggleGroup;
    UnityAction<int> OnSelect;

    private void Awake()
    {
        toggleGroup = GetComponent<ToggleGroup>();
    }
    // Start is called before the first frame update
    public void Initialize(string heading,string [] values, int selectedIndex, UnityAction<int> OnSelect)
    {
        headingText.text = heading;
        for(int i =0;i<values.Length;i++)
        {
            GameObject row = Instantiate(listRow);
            row.transform.SetParent(parent);

            bool isOn = false;

            if(i+1 == selectedIndex)
            {
                isOn = true;
            }
            row.GetComponent<ListRow>().index = i + 1;
            row.GetComponent<ListRow>().Initialize(values[i], isOn, toggleGroup);

            row.transform.localScale = Vector3.one;
        }

        if (OnSelect != null)
            this.OnSelect = OnSelect;
    }

    public void SetIndex(int val)
    {
        OnSelect?.Invoke(val);
    }
}
