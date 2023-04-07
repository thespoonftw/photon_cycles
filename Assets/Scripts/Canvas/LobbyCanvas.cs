using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class LobbyCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lobbyList;

    public void UpdatePlayerList(string text)
    {
        lobbyList.text = text;
    }
}
