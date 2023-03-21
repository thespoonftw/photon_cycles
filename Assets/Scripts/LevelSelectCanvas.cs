using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelSelectCanvas : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> texts;

    public void ChangeSelection(int selectionIndex)
    {
        for (int i = 0; i < texts.Count; i++)
        {
            texts[i].color = selectionIndex == i ? Color.cyan : Color.white;
        }
    }
}
