using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject GridPrefab;       //网格 白
    public GameObject VirusRedPrefab;   //敌方主将 红
    public GameObject VirusGreyPrefab;  //敌方人员 灰
    public GameObject PlayerGreenPrefab;//我方主将 绿
    [Header("Level")]
    public int Level=1;//关卡数
    public int TotalGrids{get{return (Level+2)*(Level+2);}}//总网格数
    public int RowCount{get{return Level+2;}}//每一排网格数
    public int virusCount{get; protected set;}//病毒个数
    public int Steps{get; protected set;}//步骤数
    public float perUnitScale{get; protected set;}//每一格的单位大小
    public int HealthCount{get; protected set;}
    public GameObject VirusRed{get; protected set;}
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    //加载下一关
    public void NextLevel(){
        Level ++;
        RefreshLevel();
    }
    //创建格子
    public void GenerateGrid(){
        //创建一个网格根节点
        GameObject GridRoot = new GameObject("GridRoot");
        GridRoot.transform.position = Vector2.zero;

        //根据摄像机的scale，以及网格总数，获取单元网格的大小
        Camera cam = Camera.main;
        perUnitScale = (cam.orthographicSize*4f/3f)/RowCount;

        //设置网格根节点的起始点
        Vector2 InitOffset = new Vector2(-perUnitScale*(RowCount-1)/2f,-perUnitScale*(RowCount-1)/2f);
        GridRoot.transform.position = InitOffset;

        //生成网格
        Vector2 pos = Vector2.zero;
        for(int i=0; i<TotalGrids; i++){
            GameObject grid = Instantiate(GridPrefab, Vector3.zero, Quaternion.identity, GridRoot.transform);
            pos.y = i/RowCount*perUnitScale;
            pos.x = i%RowCount*perUnitScale;
            grid.transform.localPosition = pos;
            grid.transform.localScale *= perUnitScale;
        }

        //放置红色病毒
        Vector2Int index_red = new Vector2Int(Random.Range(0, RowCount), Random.Range(0, RowCount));
        VirusRed = Instantiate(VirusRedPrefab);
        VirusRed.transform.parent = GridRoot.transform;
        VirusRed.transform.localScale *= perUnitScale;
        VirusRed.transform.localPosition = new Vector2(index_red.x*perUnitScale, index_red.y*perUnitScale);
    }
}
