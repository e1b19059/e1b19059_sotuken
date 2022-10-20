using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Photon.Pun;
using System.Linq;
using TMPro;

public class TeamSelect : MonoBehaviour
{
    [SerializeField] private ToggleGroup toggleGroup;
    [SerializeField] private Toggle readyToggle;
    [SerializeField] private Button startButton;

    private void Start()
    {
        readyToggle.interactable = false;
        startButton.interactable = false;
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.transform.localScale = Vector3.one;
        }
        else
        {
            startButton.transform.localScale = Vector3.zero;
        }   
    }

    public void Ready()
    {
        
        Toggle[] toggles = toggleGroup.GetComponentsInChildren<Toggle>();
        if (readyToggle.isOn == true)
        {
            Debug.Log("èÄîıäÆóπ");
            foreach (Toggle toggle in toggles)
            {
                if (toggle != readyToggle)
                {
                    if(toggle.isOn == true)
                    {
                        PhotonNetwork.LocalPlayer.SetTeam(toggle.GetComponentsInChildren<Text>()
                            .First(t => t.name == "label").text.Substring(0, 1));
                    }
                    toggle.interactable = false;
                }
            }
            startButton.interactable = true;
        }
        else
        {
            PhotonNetwork.LocalPlayer.SetTeam(null);
            foreach (Toggle toggle in toggles)
            {
                toggle.interactable = true;
                
            }
            startButton.interactable = false;
        }
    }

    public void Cancel()
    {
        if (readyToggle.interactable)
        {
            Debug.Log("ÉLÉÉÉìÉZÉã");
            PhotonNetwork.LocalPlayer.SetTeam(null);
            toggleGroup.ActiveToggles().First().isOn = false;
            readyToggle.isOn = false;
            readyToggle.interactable = false;
        } 
    }

    public void Selected()
    {
        readyToggle.interactable = !readyToggle.interactable;
    }

}
