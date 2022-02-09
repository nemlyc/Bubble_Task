using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputDetector : EventTrigger
{
    /*
     * - �{�^���N���b�N���A�^�C�}�[�X�^�[�g
     * - �L�����Z�����A�^�C�}�[���X�g�b�v
     *  - �N���b�N����臒l�ȉ��F�N���b�N����
     *  - �N���b�N����臒l�ȏ�F�z�[���h�A�j���[�V�����Đ�
     *  - �z�[���h����臒l�ȏ�F�z�[���h����
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
