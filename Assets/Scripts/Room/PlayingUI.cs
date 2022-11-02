using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingUI : MonoBehaviour
{
    [SerializeField] private PhotonLogin photonLogin;

    private void Update()
    {
        if (!photonLogin.GetPlayingFlag())
        {
            transform.localScale = Vector3.zero;
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }

}
