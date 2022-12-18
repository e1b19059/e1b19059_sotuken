using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 0.1f));
    }

    void OnTriggerEnter(Collider other)
    {
        string team = other.gameObject.name.Substring(0, 1);
        PlayerPrefs.SetInt($"Score{team}", PlayerPrefs.GetInt($"Score{team}") + 1);
        Destroy(gameObject);
    }

}
