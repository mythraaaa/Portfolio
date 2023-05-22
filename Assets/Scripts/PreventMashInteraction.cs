using UnityEngine;
using UnityEngine.InputSystem;

public class PreventMashInteraction : IInputInteraction
{
    // �ŏ��̓��͊Ԋu[s]�i�����ꂽ��A���͂��󂯕t���Ȃ�����[s]�j
    public float minInputDuration;

    // ���͔����臒l(0�Ńf�t�H���g�l)
    public float pressPoint;

    // �ݒ�l���f�t�H���g�l�̒l���i�[����t�B�[���h
    private float pressPointOrDefault => pressPoint > 0 ? pressPoint : InputSystem.settings.defaultButtonPressPoint;
    private float releasePointOrDefault => pressPointOrDefault * InputSystem.settings.buttonReleaseThreshold;

    // ���߂�Performed��ԂɑJ�ڂ�������
    private double _lastPerformedTime;

    /// <summary>
    /// ������
    /// </summary>
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoadMethod]
#else
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
    public static void Initialize()
    {
        // �����Interaction��o�^����K�v������
        InputSystem.RegisterInteraction<PreventMashInteraction>();
    }

    public void Process(ref InputInteractionContext context)
    {
        if (context.isWaiting)
        {
            // Waiting���

            // ���͂��O�ȊO���ǂ���
            if (context.ControlIsActuated())
            {
                // �O�ȊO�Ȃ�Started��ԂɑJ��
                context.Started();
            }
        }

        if (context.isStarted)
        {
            // Started���

            // ���͂�Press�ȏ�
            //     ����
            // �O���Performed��ԑJ�ڂ���uminInputDuration�v�ȏ�o�� �������ǂ���
            if (context.ControlIsActuated(pressPointOrDefault) && context.time >= _lastPerformedTime + minInputDuration)
            {
                // Performed��ԂɑJ��
                context.PerformedAndStayPerformed();

                // Performed��ԑJ�ڎ��̎�����ێ�
                _lastPerformedTime = context.time;
            }
            // ���͂��O���ǂ���
            else if (!context.ControlIsActuated())
            {
                // �O�Ȃ�Canceled��ԂɑJ��
                context.Canceled();
            }
        }

        if (context.phase == InputActionPhase.Performed)
        {
            // Performed���

            // ���͂�Release�ȉ����ǂ���
            if (!context.ControlIsActuated(releasePointOrDefault))
            {
                // Canceled��ԂɑJ��
                context.Canceled();
            }
        }
    }

    public void Reset()
    {
    }
}