using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankTypeManager : MonoBehaviour
{
    public GameObject TotalRank;
    public GameObject WeekRank;
    public GameObject MonthRank;
    public Button TotalRankBtn;
    public Button WeekRankBtn;
    public Button MonthRankBtn;
    public GameObject[] RankLists;
    public Button[] Btns;
    // Start is called before the first frame update
    void Start()
    {
        RankLists = new GameObject[] { TotalRank, WeekRank, MonthRank };
        Btns = new Button[] { TotalRankBtn, WeekRankBtn, MonthRankBtn };
    }

    public void setTypeActive(int index)
    {
        for (int i = 0; i < RankLists.Length; i++)
        {
            if (i == index)
            {
                RankLists[i].SetActive(true);
                Btns[i].interactable = false;
            }
            else
            {
                RankLists[i].SetActive(false);
                Btns[i].interactable = true;
            }
        }
    }

    public void total()
    {
        setTypeActive(0);
    }

    public void week()
    {
        setTypeActive(1);
    }

    public void month()
    {
        setTypeActive(2);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
