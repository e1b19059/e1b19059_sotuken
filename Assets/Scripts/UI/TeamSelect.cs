using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Photon.Pun;
using System.Linq;
using TMPro;

public class TeamSelect : MonoBehaviour
{
    [SerializeField] ScoreBoard scoreBoard;
    [SerializeField] ToggleGroup toggleGroup;
    [SerializeField] Toggle readyToggle;
    [SerializeField] Button startButton;

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
                    if (toggle.isOn == true)
                    {
                        string team = toggle.GetComponentsInChildren<TextMeshProUGUI>()
                            .First(t => t.name == "label").text.Substring(0, 1);
                        PhotonNetwork.LocalPlayer.SetTeam(team);
                        scoreBoard.SetTeamLabel(team);
                    }
                    toggle.interactable = false;
                }
            }
        }
        else
        {
            PhotonNetwork.LocalPlayer.SetTeam(null);
            foreach (Toggle toggle in toggles)
            {
                toggle.interactable = true;
            }
        }
    }

    public void Cancel()
    {
        if (readyToggle.interactable)
        {
            Debug.Log("ÉLÉÉÉìÉZÉã");
            toggleGroup.ActiveToggles().First().isOn = false;
            readyToggle.isOn = false;
            readyToggle.interactable = false;
        }
    }

    public void Selected()
    {
        readyToggle.interactable = !readyToggle.interactable;
    }

    public void CheckAllReady()
    {
        int Anum = 0, Bnum = 0;
        if (PhotonNetwork.CurrentRoom.PlayerCount < 2) { return; }
        foreach (var player in PhotonNetwork.PlayerList)
        {
            switch (player.GetTeam())
            {
                case "A":
                    Anum++;
                    break;
                case "B":
                    Bnum++;
                    break;
                default:
                    startButton.interactable = false;
                    return;
            }
        }
        if (Anum != 0 && Bnum != 0)
        {
            startButton.interactable = true;
        }
        else
        {
            startButton.interactable = false;
        }
    }

}
