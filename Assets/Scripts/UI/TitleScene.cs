using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleScene : MonoBehaviour
{
    public TMP_InputField inputField;

    void Start()
    {
        inputField.GetComponent<TMP_InputField>().onEndEdit.AddListener(IfPushEnter);
    }

    void IfPushEnter(string _)
    {
        if (Input.GetKey(KeyCode.Return))
        {
            MoveScene(inputField.text.ToString());
        }
    }

    public void OnClick()
    {
        MoveScene(inputField.text.ToString());
    }

    public void MoveScene(string _playerName)
    {
        if(_playerName != "")
        {
            PlayerPrefs.SetString("PlayerName", _playerName);
            SceneManager.LoadScene("GameScene");
        }
    }
}
