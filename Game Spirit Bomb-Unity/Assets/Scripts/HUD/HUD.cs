using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    // 按钮 
    public Button startButton;
    public Button runingBackButton; // 运行中的返回按钮
    public Button gameoverButton;   // 游戏结束界面按钮
    public Button staffButton;         // 制作组名单按钮
    public GameObject staffOBJ;           // 制作组名单界面
    public Image gameoverImage;     // 游戏结束界面图片

    float staffCD = 5f;
    float staffTime = 0f;
    bool isSetStaff = false;
    // 界面
    public Transform Running;

    // 声音
    public GameObject audioOBJ;
    // 显示 Text
    public Text levelText;
    public Text stepText;
    public Text virusCountText;
    public Text healthCountText;

    public Sprite WinImg;
    public Sprite LoseImg;

    private void Awake() {
        OnBackButton ();
        
    }
    private void Start() {
        startButton.onClick.AddListener (StartGame);
        runingBackButton.onClick.AddListener (OnBackButton);
        staffButton.onClick.AddListener (DisplayStaff);
    }

    private void Update() {
        levelText.text = "level:" + GameManager.instance.Level;
        healthCountText.text = "HealthCount:" + GameManager.instance.HealthCount;
        virusCountText.text = "VirusCount:" + GameManager.instance.virusCount;
        stepText.text = "Step:" + GameManager.instance.Steps;

        if (Time.time - staffCD > staffTime && isSetStaff == true) {
            staffOBJ.SetActive (false);
            
        }
    }
    // 返回按钮
    void OnBackButton() {
        Running.gameObject.SetActive (false);
        startButton.transform.parent.gameObject.SetActive (true);
        gameoverButton.transform.parent.gameObject.SetActive (false);
        audioOBJ.GetComponent<AudioManager> ().PlayAudio (0);
    }
    // 开始按钮
    void StartGame() {
        startButton.transform.parent.gameObject.SetActive (false);
        Running.gameObject.SetActive (true);
        audioOBJ.GetComponent<AudioManager> ().PlayAudio (1);
    }

    public void DisplayWinScenes() {
        gameoverButton.transform.parent.gameObject.SetActive (true);
        gameoverImage.sprite = WinImg;
        gameoverButton.onClick.AddListener (NextLevel);
        audioOBJ.GetComponent<AudioManager> ().PlaySound (6); // 音效 success
    }

    public void DisplayLoseScenes() {
        gameoverButton.transform.parent.gameObject.SetActive (true);
        gameoverImage.sprite = LoseImg;
        gameoverButton.onClick.AddListener (RefreshLevel);
        audioOBJ.GetComponent<AudioManager> ().PlaySound (7); // 音效 fail
    }

    // 进行下一关
    void NextLevel() {
        gameoverButton.transform.parent.gameObject.SetActive (false);
        GameManager.instance.NextLevel ();
        gameoverButton.onClick.RemoveAllListeners();
    }

    void RefreshLevel() {
        gameoverButton.transform.parent.gameObject.SetActive (false);
        GameManager.instance.RefreshLevel ();
        gameoverButton.onClick.RemoveAllListeners ();
    }

    void DisplayStaff() {
        staffTime = Time.time;
        isSetStaff = true;
        staffOBJ.SetActive (true);
    }
}
