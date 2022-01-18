using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleScaler
{
    readonly Vector3 SmallScale = new Vector3(0.75f, 0.75f, 0.75f);
    readonly Vector3 MediumScale = new Vector3(1f, 1f, 1f);
    readonly Vector3 LargeScale = new Vector3(1.25f, 1.25f, 1.25f);

    public Vector3 GetScale(int priority)
    {
        switch (priority)
        {
            case 0:
                return SmallScale;
            case 1:
                return MediumScale;
            case 2:
                return LargeScale;
            default:
                return Vector3.one;
        }
    }

}