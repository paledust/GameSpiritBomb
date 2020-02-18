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
    public int healthCount;
    [HideInInspector]
    public int virusCount;
    [HideInInspector]
    public int step;
    [HideInInspector]
    public int level;
    private void Awake() {
        Runing.gameObject.SetActive (false);
        startButton.transform.parent.gameObject.SetActive (true);
    }
    private void Start() {
        startButton.onClick.AddListener (StartGame);
    }

    private void Update() {
        levelText.text = "level:" + level;
        healthCountText.text = "HealthCount:" + healthCount;
        virusCountText.text = "VirusCount:" + virusCount;
        stepText.text = "Step:" + step;
    }

    // 开始按钮
    void StartGame() {
        Runing.gameObject.SetActive (true);
        startButton.transform.parent.gameObject.SetActive (false);
    }

}
