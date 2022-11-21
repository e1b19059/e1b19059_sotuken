using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class TrapManager : MonoBehaviour
{
    [SerializeField] private TextMeshPro TrapLabel;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Substring(0, 1) == PhotonNetwork.LocalPlayer.GetTeam())// 先頭の文字を切り出す
        {
            switch (TrapLabel.text)
            {
                case "L":
                    Debug.Log("左回転");
                    other.gameObject.transform.Rotate(0, -90f, 0);
                    break;
                case "R":
                    Debug.Log("右回転");
                    other.gameObject.transform.Rotate(0, 90f, 0);
                    break;
                default:
                    Debug.Log("効果なし");
                    break;
            }
        }
    }

}
