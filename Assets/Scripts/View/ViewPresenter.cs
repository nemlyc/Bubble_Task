using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;


public class ViewPresenter : MonoBehaviour
{
    [SerializeField]
    GameObject topView, editView, dialogView;

    [SerializeField]
    ViewStateProperty viewStatus = new ViewStateProperty();
    public IReadOnlyReactiveProperty<ViewState> ViewModel { get { return viewStatus; } }

    private void Start()
    {
        this.viewStatus
            .Subscribe(
                
            );
    }

    public void UpdateView(ViewState view)
    {
        viewStatus.Value = view;
    }
}
