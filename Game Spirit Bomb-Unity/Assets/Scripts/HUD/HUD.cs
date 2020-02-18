using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    // 按钮 
    public Button startButton;
    // 界面
    public Transform Runing;
    public Transform Win;
    // 声音
    public GameObject audioOBJ;
    // 显示 Text
    public Text levelText;
    public Text stepText;
    public Text virusCountText;
    public Text healthCountText;

    // 显示参数
    public GameManager manager;

    private void Awake() {
        audioOBJ.GetComponent<AudioManager> ().PlayAudio(0);
        Runing.gameObject.SetActive (false);
        startButton.transform.parent.gameObject.SetActive (true);
    }
    private void Start() {
        startButton.onClick.AddListener (StartGame);
    }

    private void Update() {
        levelText.text = "level:" + manager.Level;
        healthCountText.text = "HealthCount:" + manager.HealthCount;
        virusCountText.text = "VirusCount:" + manager.virusCount;
        stepText.text = "Step:" + manager.Steps;
    }

    // 开始按钮
    void StartGame() {
        Runing.gameObject.SetActive (true);
        startButton.transform.parent.gameObject.SetActive (false);
        audioOBJ.GetComponent<AudioManager> ().PlayAudio (1);
    }

}
