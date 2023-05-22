using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // �ǂ�������Ώۂ�Transform
    public float smoothing = 5f; // �J�����̃X���[�V���O�̒l
    public Vector2 offset = new Vector2(0f, 2f); // �J�����ƃv���C���[�̃I�t�Z�b�g�l
    

    /*
    public Transform player; // �v���C���[��Transform
    public Vector3 offset; // �J�����̃I�t�Z�b�g
    public float smoothing = 5f; // �J�����̃X���[�V���O�̒l */

    // Update is called once per frame
    /* void Start()
     {
         // �J�����ƃv���C���[�̏����̃I�t�Z�b�g�l���v�Z
         offset = transform.position - player.position;
     }

     void Update()
     {
         // �ǂ�������Ώۂ̍��W���擾
         Vector3 targetCamPos = player.position + offset;

         // �J�����̈ʒu���X���[�V���O���Ȃ���ύX
         transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
     } */
    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        // �ǂ�������Ώۂ̍��W�ɃI�t�Z�b�g�l�������ĖڕW�̃J�����ʒu���v�Z
        Vector3 targetCamPos = new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);

        // �J�����̈ʒu���X���[�V���O���Ȃ���ύX
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}

