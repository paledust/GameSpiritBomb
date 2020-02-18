using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    // 按钮
    public Button startButton;
    public Transform Runing;
    // 显示 Text
    public Text levelText;
    public Text stepText;
    public Text virusCountText;
    public Text healthCountText;

    [HideInInspector]
    public int healthCount = 0;
    [HideInInspector]
    public int virusCount = 0;
    [HideInInspector]
    public int step = 0;
    [HideInInspector]
    public int level = 0;
    private void Awake() {

        Runing.gameObject.SetActive (false);
        startButton.transform.parent.gameObject.SetActive (true);
    }
    private void Start() {
        startButton.onClick.AddListener (StartGame);
    }

    private void Update() {
        levelText.text = "level:" + level;
        healthCountText.text = "healthCount:" + healthCount;
        virusCountText.text = "VirusCount:" + healthCount;
        stepText.text = "Step:" + step;
    }
    // 开始按钮
    void StartGame() {
        Runing.gameObject.SetActive (true);
        startButton.transform.parent.gameObject.SetActive (false);
    }

}
