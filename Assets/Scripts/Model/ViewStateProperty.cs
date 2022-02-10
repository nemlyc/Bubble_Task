using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public enum ViewState
{
    Top,
    Editor,
    Dialog
}

[System.Serializable]
public class ViewStateProperty : ReactiveProperty<ViewState>
{
    public ViewStateProperty() { }
    public ViewStateProperty(ViewState initialView) : base(initialView) { }
}
