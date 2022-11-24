using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseUI : MonoBehaviour
{
    Animator animator;
    Image image;
    bool currentPhase;
    bool CurrentPhase
    { // �v���p�e�B
        get { return currentPhase; }
        set
        { // �l���قȂ�Z�b�g���̂�animator.SetBool���ĂԂ悤�ɂ���
            if (value != currentPhase)
            {
                currentPhase = value;
                animator.SetBool("CurrentPhase", currentPhase);
            }
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        image = GetComponent<Image>();
    }

    public void OnHighLight()
    {
        CurrentPhase = true;
    }

    public void OffHighLight()
    {
        CurrentPhase = false;
    }

    public void SetTeamColor(Color _color)
    {
        image.color = _color;
    }
}
