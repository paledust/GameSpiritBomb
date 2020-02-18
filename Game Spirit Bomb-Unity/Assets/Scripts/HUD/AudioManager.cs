using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //将要播放的音乐组
    public AudioClip[] audioGroup;
    //AudioSource组件
    private AudioSource audioSource;

    // 切换背景音乐播放
    public void PlayAudio(int index) {
        audioSource = this.GetComponent<AudioSource> ();
        audioSource.clip = audioGroup[index];
        audioSource.Play ();
    }
}
