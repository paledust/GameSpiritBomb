using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleHoverImage : MonoBehaviour
{
    float timeX = 0f;
    float timeCD = 0.01f;
    float curPI = 5f; // 漂浮物横坐标
    float curY;
    public RectTransform rect;
    void Awake()
    {
        float y = Random.Range (-rect.rect.height / 2, rect.rect.height / 2);
        transform.localPosition = new Vector3 (transform.localPosition.x, y);
        curY = y;
        timeCD = Random.Range (0.009f, 0.015f);
        curPI = Random.Range (5f, 7f);
    }
    private void LateUpdate() {
        if (Time.time - timeX > timeCD) {
            timeX = Time.time;
            float Y = Mathf.Cos ((transform.localPosition.x + 2f) / (1080 / curPI)) * rect.rect.height / 3;
            transform.localPosition = new Vector3 (transform.localPosition.x + 2f, Y + curY);
        }
        if (Mathf.Abs (transform.localPosition.x) > rect.rect.width + 300f) {
            float y = Random.Range (-rect.rect.height / 2 - 100f, rect.rect.height / 2 - 100f);
            transform.localPosition = new Vector3 (0, (float)y, 0);
            curY = y;
            timeCD = Random.Range (0.009f, 0.015f);
        }
    }

}
