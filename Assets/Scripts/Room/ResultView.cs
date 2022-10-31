using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI WinORLose;
    [SerializeField] private TextMeshProUGUI MyResult;
    [SerializeField] private TextMeshProUGUI RivalResult; 

    private void Update()
    {
        if (!TurnManager.instance.GetShowingResults())
        {
            transform.localScale = Vector3.zero;
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }

    // ゲーム終了時に表示される結果表示ボタンで呼び出される
    public void SetResult()
    {
        
    }

}
