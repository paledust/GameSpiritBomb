using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject GridPrefab;
    public GameObject VirusRedPrefab;
    public GameObject VirusGreyPrefab;
    public GameObject PlayerGreenPrefab;
    public int Level=1;
    public int TotalGrids{get{return (Level+2)*(Level*2);}}
    public int RowCount{get{return Level+2;}}
    public int virusCount{get; protected set;}
    public int Steps{get; protected set;}
    public int HealthCount{get; protected set;}
    public static GameManager instance;
    void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(instance != null){
            Destroy(gameObject);
        }
    }
    public void RefreshLevel(){

    }
    public void NextLevel(){
        Level ++;
        RefreshLevel();
    }
    public void GenerateGrid(){
        for(int i=0; i<TotalGrids; i++){

        }
    }
}
