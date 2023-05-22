using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // 追いかける対象のTransform
    public float smoothing = 5f; // カメラのスムーシングの値
    public Vector2 offset = new Vector2(0f, 2f); // カメラとプレイヤーのオフセット値
    

    /*
    public Transform player; // プレイヤーのTransform
    public Vector3 offset; // カメラのオフセット
    public float smoothing = 5f; // カメラのスムーシングの値 */

    // Update is called once per frame
    /* void Start()
     {
         // カメラとプレイヤーの初期のオフセット値を計算
         offset = transform.position - player.position;
     }

     void Update()
     {
         // 追いかける対象の座標を取得
         Vector3 targetCamPos = player.position + offset;

         // カメラの位置をスムーシングしながら変更
         transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
     } */
    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        // 追いかける対象の座標にオフセット値を加えて目標のカメラ位置を計算
        Vector3 targetCamPos = new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);

        // カメラの位置をスムーシングしながら変更
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}

