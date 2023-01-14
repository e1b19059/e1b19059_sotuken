using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseUIManager : MonoBehaviour
{
    private List<PhaseUI> phaseUIList = new List<PhaseUI>();

    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<PhaseUI>(out var phaseUI))
            {
                phaseUIList.Add(phaseUI);
            }
        }
    }

    public void Init(string team, bool isFirst)
    {
        Color FirstColor;
        Color SecondColor;
        if((isFirst && team == "A") || (!isFirst && team == "B")){
            FirstColor = new Color(0, 0, 1.0f, 1.0f);
            SecondColor = new Color(1.0f, 0, 0, 1.0f);
        }
        else
        {
            FirstColor = new Color(1.0f, 0, 0, 1.0f);
            SecondColor = new Color(0, 0, 1.0f, 1.0f);
        }
        phaseUIList[1].SetTeamColor(FirstColor);
        phaseUIList[2].SetTeamColor(SecondColor);
        //phaseUIList[3].SetTeamColor(FirstColor);// 先にプログラミングした方が先に実行
        //phaseUIList[4].SetTeamColor(SecondColor);
        phaseUIList[3].SetTeamColor(SecondColor);// 後にプログラミングした方が先に実行
        phaseUIList[4].SetTeamColor(FirstColor);
    }

    public void SetHighLight(int _phase)
    {
        if(_phase == 1)
        {
            phaseUIList[4].OffHighLight();// 前のフェーズの分をoffにする
        }
        else
        {
            phaseUIList[_phase - 2].OffHighLight();// 前のフェーズの分をoffにする
            phaseUIList[_phase - 1].OnHighLight();// リストは0番目から数えるので添え字は-1する
        }
        //phaseUIList[_phase - 1].OnHighLight();// リストは0番目から数えるので添え字は-1する
    }

    public void Finished()
    {
        phaseUIList[4].OffHighLight();
    }

}
