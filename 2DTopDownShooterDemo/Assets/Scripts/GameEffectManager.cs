using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEffectManager : MonoBehaviour
{
    private Camera camera;
    private Vector3 originalCameraPos;
    float shakeAmplitude;  // 相机抖动幅度
    float shakeDuration = 0.3f;   // 相机抖动持续时间

    public PlayerController player;

    void Start()
    {
        camera = Camera.main;
        originalCameraPos = camera.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator ShakeCamera()
    {
        // 循环一定时间
        float elapsed = 0;
        while (elapsed < shakeDuration)
        {
            // 相机抖动幅度在[0, 0.2f]之间，随玩家子弹伤害增加
            shakeAmplitude = Mathf.Clamp(0.004f * player.bulletDamage, 0f, 0.2f);
            // 生成随机偏移量
            float xOffset = Random.Range(-shakeAmplitude, shakeAmplitude);
            float yOffset = Random.Range(-shakeAmplitude, shakeAmplitude);

            // 移动相机位置
            camera.transform.localPosition = originalCameraPos + new Vector3(xOffset, yOffset, 0);

            // 等待下一帧
            yield return null;

            // 累加时间
            elapsed += Time.deltaTime;
        }
        // 恢复相机位置
        camera.transform.localPosition = originalCameraPos;
    }
}
