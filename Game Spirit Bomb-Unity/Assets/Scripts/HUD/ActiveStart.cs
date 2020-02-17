using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveStart : MonoBehaviour
{
    // 动态标题
    public RectTransform titleImage;
    float activeCD = 0.7f; // 标题动态cd (需要大于0.3f)
    bool isBig = true;
    float timeFloat = 0f;

    // 漂浮物
    public Transform hoverOverParent;
    public Transform hoverImageParent;
    float timeCD;
    float curTime;


    private void Start() {
        timeCD = Random.Range (1f, 3f); // 1 到3 秒
        curTime = Time.time;

    }
    private void LateUpdate() {
        OnActiveTitle (); // 动态标题
        if (Time.time - curTime >= timeCD) {
            timeCD = Random.Range (1f, 3f);
            curTime = Time.time;
            GetHoverImage ();
        }
    }

    #region 标题大小动态改变
    void OnActiveTitle() {
        if (Time.time - timeFloat >= activeCD) {
            timeFloat = Time.time;
            if (isBig) {
                StartCoroutine ("GetSmaller");
            }
            else {
                StartCoroutine ("GetBigger");
            }
            isBig = !isBig;
        }
        else {
            if (isBig) {
                titleImage.localScale = Vector3.Lerp (titleImage.localScale, new Vector3 (0.9f, 0.9f), 0.1f);
            }
            else {

                titleImage.localScale = Vector3.Lerp (titleImage.localScale, new Vector3 (1f, 1f), 0.1f);
            }
        }
    }

    IEnumerator GetBigger() {
        titleImage.localScale = new Vector3 (1f, 1f);
        yield return activeCD - 0.3f;
    }

    IEnumerator GetSmaller() {
        titleImage.localScale = new Vector3 (0.9f, 0.9f);
        yield return activeCD - 0.3f;
    }
    #endregion

    // 向开始界面添加漂浮物
    void GetHoverImage() {
        if (hoverOverParent.childCount <= 0) return;
        Transform tHoverChild = hoverOverParent.GetChild (0);
        tHoverChild.parent = hoverImageParent;
    }
}
