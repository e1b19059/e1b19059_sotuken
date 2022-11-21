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
        if (other.gameObject.name.Substring(0, 1) == PhotonNetwork.LocalPlayer.GetTeam())// æ“ª‚Ì•¶š‚ğØ‚èo‚·
        {
            switch (TrapLabel.text)
            {
                case "L":
                    Debug.Log("¶‰ñ“]");
                    other.gameObject.transform.Rotate(0, -90f, 0);
                    break;
                case "R":
                    Debug.Log("‰E‰ñ“]");
                    other.gameObject.transform.Rotate(0, 90f, 0);
                    break;
                default:
                    Debug.Log("Œø‰Ê‚È‚µ");
                    break;
            }
        }
    }

}
