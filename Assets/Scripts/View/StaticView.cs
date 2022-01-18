using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticView : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_Text headerText;

    readonly Dictionary<ViewState, string> headerDictionary = new Dictionary<ViewState, string>() {
        {ViewState.Top, "タスクリスト"},
        {ViewState.Editor, "編集" },
        //{ViewState.Dialog, "編集" },
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
