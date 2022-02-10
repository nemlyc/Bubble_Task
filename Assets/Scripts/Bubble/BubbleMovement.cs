using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BubbleMovement : MonoBehaviour
{
    /*
     * - ぶつかった相手の方向と逆ベクトルに動く。
     * - ぶつかりがなくなったら、力は0に収束。
     * - 画面外に行ったら、戻る
     */

    Vector3 moveVector = Vector3.zero;
    IDisposable timer;

    float radius;

    readonly float MinimumSpeed = 0.001f;
    readonly float BoundX = Screen.width;
    readonly float BoundY = Screen.height;
    readonly float HeaderHeight = 350f;
    readonly string BoundTag = "Bound";

    private void Awake()
    {
        radius = GetComponent<CircleCollider2D>().radius;
    }

    public void InitializePosition()
    {
        transform.position = new Vector2(
            UnityEngine.Random.Range(0f + radius * 2, Screen.width - radius * 2),
            UnityEngine.Random.Range(0f + radius * 2, Screen.height - (radius * 2) - HeaderHeight)
            );
    }

    private void Update()
    {
        transform.position += moveVector * MinimumSpeed;

        if (timer != null && moveVector.magnitude < MinimumSpeed)
        {
            timer.Dispose();
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //相手と反発するベクトルを設定
        moveVector = -1 * (collision.transform.position - transform.position).normalized;

        //壁に対しては反転?
        if (collision.collider.gameObject.CompareTag(BoundTag))
        {
            moveVector *= -1;
        }

        var mag = collision.collider.bounds.size.magnitude;

        moveVector *= mag;

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("脱出");
        timer = Observable.Interval(TimeSpan.FromMilliseconds(100))
             .Subscribe(time =>
             {
                 moveVector *= 0.7f;
                 if (moveVector.magnitude <= MinimumSpeed)
                 {
                     moveVector = Vector3.zero;
                 }
             });
    }
}
