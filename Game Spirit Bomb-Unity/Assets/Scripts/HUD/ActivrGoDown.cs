using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivrGoDown : MonoBehaviour
{
    public Sprite WinImage;
    public Sprite LoseImage;
    public HUD hud;
    RectTransform rect;
    public bool isActivr = false;
    float speed = 2f;
    float curSpeed;
    // Update is called once per frame
    private void Awake() {
        rect = GetComponent<RectTransform> ();
    }
    void Update()
    {
        if (isActivr) {

        }
    }

    void GoDown() {
        
    }
}
