using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameListRow : MonoBehaviour
{
    [SerializeField]
    Text tableID, nameText;

    [SerializeField]
    Image profilePic, flagImage;

    [SerializeField] Toggle[] players;

    public void Initialize(int id, string name, string prifilePic, string flag, int playersCount)
    {
        tableID.text = id.ToString();
        nameText.text = name;

        //profilePic

        //flagImage.sprite = (Sprite)Resources.Load("/flags/" + flag);

        for(int i=0;i<5;i++)
        {
            if (i < playersCount)
                players[i].isOn = true;
        }
    }
}
