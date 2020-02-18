using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    // 按钮 
    public Button startButton;
    public Button runingBackButton; // 运行中的返回按钮
    public Button gameoverBackButton;   // 游戏结束界面的返回按钮
    public Button gameoverNextLevelButton;   // 游戏结束界面的返回按钮
    public Text gameoverText;
    // 界面
    public Transform Runing;

    // 声音
    public GameObject audioOBJ;
    // 显示 Text
    public Text levelText;
    public Text stepText;
    public Text virusCountText;
    public Text healthCountText;

    private void Awake() {
        OnBackButton ();
    }
    private void Start() {
        startButton.onClick.AddListener (StartGame);
        runingBackButton.onClick.AddListener (OnBackButton);
        gameoverBackButton.onClick.AddListener (OnBackButton);
        gameoverNextLevelButton.onClick.AddListener (NextLevel);
    }

    private void Update() {
        levelText.text = "level:" + GameManager.instance.Level;
        healthCountText.text = "HealthCount:" + GameManager.instance.HealthCount;
        virusCountText.text = "VirusCount:" + GameManager.instance.virusCount;
        stepText.text = "Step:" + GameManager.instance.Steps;
    }
    // 返回按钮
    void OnBackButton() {
        Runing.gameObject.SetActive (false);
        startButton.transform.parent.gameObject.SetActive (true);
        gameoverText.transform.parent.gameObject.SetActive (false);
        audioOBJ.GetComponent<AudioManager> ().PlayAudio (0);
    }
    // 开始按钮
    void StartGame() {
        startButton.transform.parent.gameObject.SetActive (false);
        Runing.gameObject.SetActive (true);
        audioOBJ.GetComponent<AudioManager> ().PlayAudio (1);
    }

    public void DisplayWinScenes() {
        gameoverText.transform.parent.gameObject.SetActive (true);
        gameoverText.text = "游戏胜利";
        gameoverNextLevelButton.interactable = true;
        audioOBJ.GetComponent<AudioManager> ().PlaySound (6); // 音效 success
    }

    public void DisplayLoseScenes() {
        gameoverText.transform.parent.gameObject.SetActive (true);
        gameoverText.text = "游戏失败";
        gameoverNextLevelButton.interactable = false;
        audioOBJ.GetComponent<AudioManager> ().PlaySound (7); // 音效 fail
    }

    void NextLevel() {
        GameManager.instance.NextLevel ();
    }

}
