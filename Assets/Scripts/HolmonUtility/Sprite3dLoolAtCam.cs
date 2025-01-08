using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite3dLoolAtCam : MonoBehaviour
{
    Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        TurningImage();
    }

    // オブジェクトの向きをメインカメラの方向に向ける
    private void TurningImage()
    {
        transform.LookAt(mainCam.transform.position);
    }
}