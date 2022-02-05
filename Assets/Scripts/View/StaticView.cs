using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticView : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_Text headerText;

    readonly Dictionary<ViewState, string> headerDictionary = new Dictionary<ViewState, string>() {
        {ViewState.Top, "�^�X�N���X�g"},
        {ViewState.Editor, "�ҏW" },
        //{ViewState.Dialog, "�ҏW" },
    };

    private void Awake()
    {
        headerText.text = headerDictionary[ViewState.Top];
    }

    public void UpdateHeader(ViewState state)
    {
        if (headerDictionary.TryGetValue(state, out var value))
        {
            headerText.text = value;
        }
    }
}
