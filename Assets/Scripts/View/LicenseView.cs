using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LicenseView : MonoBehaviour
{
    [SerializeField]
    GameObject licenseView;

    private void Awake()
    {
        licenseView.SetActive(false);
    }

    public void OpenLicense()
    {
        licenseView.SetActive(true);
    }

    public void CloseLicense()
    {
        licenseView.SetActive(false);
    }
}
