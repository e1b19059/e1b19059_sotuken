using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PhaseUI : MonoBehaviour
{
    Animator animator;
    Image image;
    [SerializeField] private TextMeshProUGUI phaseLabel;
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
        phaseLabel.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    public void OffHighLight()
    {
        CurrentPhase = false;
        phaseLabel.color = new Color(0, 0, 0, 1.0f);
    }

    public void SetTeamColor(Color _color)
    {
        image.color = _color;
    }
}
