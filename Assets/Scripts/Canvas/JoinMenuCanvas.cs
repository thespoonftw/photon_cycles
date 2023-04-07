using TMPro;
using UnityEngine;

public class JoinMenuCanvas : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameInput;

    public string GetUsername() => usernameInput.text;
}
