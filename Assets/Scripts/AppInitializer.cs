using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppInitializer : MonoBehaviour
{
    private void Start()
    {
#if UNITY_IOS
        Application.targetFrameRate = 60;
#endif
    }
}
