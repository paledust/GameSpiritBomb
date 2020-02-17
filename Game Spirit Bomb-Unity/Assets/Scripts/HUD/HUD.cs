using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    // 按钮
    public Button startButton;

    public Transform Runing;
    public Transform NotDisplay;

    // 显示 Text
    public Text levelText;
    public Text stepText;
    public Text virusCountText;
    public Text healthCountText;

    [HideInInspector]
    public int healthCount;
    [HideInInspector]
    public int virusCount;
    [HideInInspector]
    public int step;
    [HideInInspector]
    public int level;
    private void Awake() {
        Runing.parent = NotDisplay;
        startButton.transform.parent.parent = transform;
    }
    private void Start() {
        startButton.onClick.AddListener (StartGame);
    }

    // 开始按钮
    void StartGame() {
        Runing.parent = transform;
        startButton.transform.parent.parent = NotDisplay;
    }

}
