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
        //����Ɣ�������x�N�g����ݒ�
        moveVector = -1 * (collision.transform.position - transform.position).normalized;

        //�ǂɑ΂��Ă͔��]?
        if (collision.collider.gameObject.CompareTag(BoundTag))
        {
            moveVector *= -1;
        }

        var mag = collision.collider.bounds.size.magnitude;

        moveVector *= mag;

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("�E�o");
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
