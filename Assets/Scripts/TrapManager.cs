using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class TrapManager : MonoBehaviour
{
    [SerializeField] TextMeshPro TrapLabel;
    [SerializeField] Material trapMaterial;

    void OnTriggerEnter(Collider other)
    {
        this.GetComponentInChildren<Renderer>().material = trapMaterial;
        GetComponentInChildren<TextMeshPro>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        switch (TrapLabel.text)
        {
            case "L":
                Debug.Log("ç∂âÒì]");
                other.gameObject.transform.Rotate(0, -90f, 0);
                break;
            case "R":
                Debug.Log("âEâÒì]");
                other.gameObject.transform.Rotate(0, 90f, 0);
                break;
            default:
                Debug.Log("å¯â Ç»Çµ");
                break;
        }
    }

}
