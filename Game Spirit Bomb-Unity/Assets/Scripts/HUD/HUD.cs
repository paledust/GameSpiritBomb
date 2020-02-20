using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    // 按钮 
    public Button startButton;          // 开始按钮
    public Button runingBackButton;     // 运行中的返回按钮
    public Button gameoverWinButton;    // 游戏结束界面按钮
    public Button gameoverLoseButton;   // 游戏结束界面按钮
    public Button staffButton;          // 制作组名单按钮
    public GameObject staffOBJ;         // 制作组名单界面
    public Image gameoverImage;         // 游戏结束界面图片
    public ActivrGoDown goDownScript;   // 游戏结束界面动态图片控制代码

    public Button OFFhowToPlayButton;  // 打开 玩法介绍按钮
    public Button ONhowToPlayButton;   // 关闭 玩法介绍按钮
    public GameObject howToPlayOBJ;   // 关闭 玩法介绍按钮

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
        //Screen.SetResolution (607, 1080, false); // PC打包默认大小
        OnBackButton ();

    }
    private void Start() {
        startButton.onClick.AddListener (StartGame);
        runingBackButton.onClick.AddListener (OnBackButton);
        staffButton.onClick.AddListener (DisplayStaff);
        gameoverWinButton.onClick.AddListener (NextLevel);
        gameoverLoseButton.onClick.AddListener (RefreshLevel);
        OFFhowToPlayButton.onClick.AddListener (DispalyHowToPlay);
        ONhowToPlayButton.onClick.AddListener (DispalyHowToPlay);
    }

    private void Update() {
        levelText.text = "level:" + GameManager.instance.Level;
        healthCountText.text = "HealthCount:" + GameManager.instance.HealthCount;
        virusCountText.text = "VirusCount:" + GameManager.instance.virusCount;
        stepText.text = "Step:" + GameManager.instance.Steps;

        if (Time.time - staffCD > staffTime && isSetStaff == true) {
            staffOBJ.SetActive (false);
            isSetStaff = false;
        }

    }

    void DispalyHowToPlay() {
        Debug.Log ($"{howToPlayOBJ.activeSelf}");
        howToPlayOBJ.SetActive (!howToPlayOBJ.activeSelf); // 默认关闭
    }

    // 返回按钮
    void OnBackButton() {
        Running.gameObject.SetActive (false);
        startButton.transform.parent.gameObject.SetActive (true);
        gameoverLoseButton.transform.parent.gameObject.SetActive (false);
        audioOBJ.GetComponent<AudioManager> ().PlayAudio (0);
    }
    // 开始按钮
    void StartGame() {
        startButton.transform.parent.gameObject.SetActive (false);
        Running.gameObject.SetActive (true);
        audioOBJ.GetComponent<AudioManager> ().PlayAudio (1);
    }

    public void DisplayWinScenes() {
        gameoverImage.transform.parent.gameObject.SetActive (true);
        gameoverImage.sprite = WinImg;
        goDownScript.isTime = true;
        goDownScript.ChangeImage (goDownScript.WinImage);
        audioOBJ.GetComponent<AudioManager> ().PlaySound (6); // 音效 success
        StartCoroutine ("IWinDisplay");
    }
    public void DisplayLoseScenes() {
        gameoverImage.transform.parent.gameObject.SetActive (true);
        gameoverImage.sprite = LoseImg;
        goDownScript.isTime = true;
        goDownScript.ChangeImage (goDownScript.LoseImage);
        audioOBJ.GetComponent<AudioManager> ().PlaySound (7); // 音效 fail
        StartCoroutine ("ILoseDisplay");
    }

    IEnumerator ILoseDisplay() {
        yield return 0.1f;
        gameoverLoseButton.gameObject.SetActive (true);
    }
    IEnumerator IWinDisplay() {
        yield return 0.1f;
        gameoverWinButton.gameObject.SetActive (true);
    }

    // 进行下一关
    void NextLevel() {
        gameoverImage.transform.parent.gameObject.SetActive (false);
        gameoverWinButton.gameObject.SetActive (false);
        gameoverLoseButton.gameObject.SetActive (false);
        GameManager.instance.NextLevel ();
        goDownScript.ResetActivr ();
    }

    void RefreshLevel() {
        gameoverWinButton.gameObject.SetActive (false);
        gameoverLoseButton.gameObject.SetActive (false);
        gameoverImage.transform.parent.gameObject.SetActive (false);
        GameManager.instance.RefreshLevel ();
        goDownScript.ResetActivr ();
    }

    void DisplayStaff() {
        staffTime = Time.time;
        isSetStaff = true;
        staffOBJ.SetActive (true);
    }
}
