using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BubbleMovement : MonoBehaviour
{
    /*
     * - �Ԃ���������̕����Ƌt�x�N�g���ɓ����B
     * - �Ԃ��肪�Ȃ��Ȃ�����A�͂�0�Ɏ����B
     * - ��ʊO�ɍs������A�߂�
     */

    Vector3 moveVector = Vector3.zero;
    IDisposable timer;

    float radius;

    readonly float MinimumSpeed = 0.001f;
    readonly float BoundX = Screen.width;
    readonly float BoundY = Screen.height;
    readonly float HeaderHeight = 350f;

    private void Awake()
    {
        radius = GetComponent<CircleCollider2D>().radius;
    }

    public void InitializePosition()
    {
        transform.position = new Vector2(
            UnityEngine.Random.Range(0f + radius, Screen.width - radius), 
            UnityEngine.Random.Range(0f + radius, Screen.height - radius - HeaderHeight)
            );
        if (transform.position.x + radius > BoundX || transform.position.x - radius < 0 ||
            transform.position.y + radius > BoundY || transform.position.y - radius < 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, 1);
        }
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
        //�t�x�N�g����ݒ�
        moveVector = -1 * (collision.transform.position - transform.position).normalized;

        var mag = collision.collider.bounds.size.magnitude;

        moveVector *= mag;

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
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
