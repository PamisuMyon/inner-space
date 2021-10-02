using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// 视差图层
public class Parallax : MonoBehaviour
{
    public Camera cam;
    public Vector2 moveRate;    // 移动比率
    public Vector2 repeat;  // x y 方向重复间隔
    public float bgNum; // 当前背景已重复数量

    private Vector2 startPosition;
    private Vector2 camStartPosition;

    private void Start() 
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
        startPosition = transform.position;
        camStartPosition = cam.transform.position;
    }

    void Update()
    {
        float x = startPosition.x + (cam.transform.position.x - camStartPosition.x) * moveRate.x;
        float y = startPosition.y + (cam.transform.position.y - camStartPosition.y) * moveRate.y;
        transform.position = new Vector2(x, y);

        Vector2 camSizeHalf = new Vector2(cam.orthographicSize * cam.aspect, cam.orthographicSize);
        // 背景无限重复 横向
        if (repeat.x > 0)
        {
            float camRight = cam.transform.position.x + camSizeHalf.x;
            float camLeft = cam.transform.position.x - camSizeHalf.x;

            // Debug.Log("Size: " + camSizeHalf + " Left: " + camLeft + " Right: " + camRight);
            if (camRight - transform.position.x > repeat.x * bgNum / 2)
            {
                // 相机右侧超出背景，将背景向右移动
                startPosition = new Vector2(startPosition.x + repeat.x, startPosition.y);
            }
            else if (transform.position.x - camLeft > repeat.x * bgNum / 2)
            {
                startPosition = new Vector2(startPosition.x - repeat.x, startPosition.y);
            }
        }

    }
}
