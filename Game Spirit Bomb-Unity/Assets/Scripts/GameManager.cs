using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject GridPrefab;
    public GameObject VirusRedPrefab;
    public GameObject VirusGreyPrefab;
    public GameObject PlayerGreenPrefab;
    [Header("Level")]
    public int Level=1;
    public int TotalGrids{get{return (Level+2)*(Level*2);}}
    public int RowCount{get{return Level+2;}}
    public int virusCount{get; protected set;}
    public int Steps{get; protected set;}
    public int HealthCount{get; protected set;}
    public static GameManager instance;
    //设置GameManager为Singleton，且不会因为加载关卡被重置
    void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(instance != null){
            Destroy(gameObject);
        }
    }
    //重新加载关卡
    public void RefreshLevel(){

    }
    //加载下一关
    public void NextLevel(){
        Level ++;
        RefreshLevel();
    }
    //创建格子
    public void GenerateGrid(){
        float width = GridPrefab.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        float height;
        Vector2 pos;

        for(int i=0; i<TotalGrids; i++){

            // GameObject grid = Instantiate()
        }
    }
}
