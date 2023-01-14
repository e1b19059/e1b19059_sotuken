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
        //phaseUIList[3].SetTeamColor(FirstColor);// ��Ƀv���O���~���O����������Ɏ��s
        //phaseUIList[4].SetTeamColor(SecondColor);
        phaseUIList[3].SetTeamColor(SecondColor);// ��Ƀv���O���~���O����������Ɏ��s
        phaseUIList[4].SetTeamColor(FirstColor);
    }

    public void SetHighLight(int _phase)
    {
        if(_phase == 1)
        {
            phaseUIList[4].OffHighLight();// �O�̃t�F�[�Y�̕���off�ɂ���
        }
        else
        {
            phaseUIList[_phase - 2].OffHighLight();// �O�̃t�F�[�Y�̕���off�ɂ���
            phaseUIList[_phase - 1].OnHighLight();// ���X�g��0�Ԗڂ��琔����̂œY������-1����
        }
        //phaseUIList[_phase - 1].OnHighLight();// ���X�g��0�Ԗڂ��琔����̂œY������-1����
    }

    public void Finished()
    {
        phaseUIList[4].OffHighLight();
    }

}
