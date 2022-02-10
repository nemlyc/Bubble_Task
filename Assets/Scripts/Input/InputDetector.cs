using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputDetector : EventTrigger
{
    /*
     * - ボタンクリック時、タイマースタート
     * - キャンセル時、タイマーをストップ
     *  - クリック判定閾値以下：クリック判定
     *  - クリック判定閾値以上：ホールドアニメーション再生
     *  - ホールド判定閾値以上：ホールド判定
     */

    readonly float ClickTime = 0.1f;
    readonly float HoldTime = 1f;

    float timer = 0;

    public bool isTimerStarted = false;

    IBubble bubble;

    public override void OnPointerDown(PointerEventData data)
    {
        if (!isTimerStarted)
        {
            timer = 0;
            isTimerStarted = true;
        }
    }
    public override void OnPointerUp(PointerEventData data)
    {
        SetUp();
    }

    public float CurrentHoldingNormalTime()
    {
        if (timer > ClickTime)
        {
            return timer / HoldTime;
        }
        return 0;
    }

    private void OnEnable()
    {
        bubble = GetComponent<IBubble>();

        SetUp();
    }

    private void Update()
    {
        if (isTimerStarted)
        {
            timer += Time.deltaTime;

            if (timer > HoldTime)
            {
                bubble.Crash();

                SetUp();
            }
        }
    }

    void SetUp()
    {
        isTimerStarted = false;
        timer = 0;
    }
}
