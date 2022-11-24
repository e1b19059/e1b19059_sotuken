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
    { // プロパティ
        get { return currentPhase; }
        set
        { // 値が異なるセット時のみanimator.SetBoolを呼ぶようにする
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
