using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour
{
    [Header ("Prefabs")]
    public GameObject GridPrefab;       //网格 白
    public GameObject VirusRedPrefab;   //敌方主将 红
    public GameObject VirusGreyPrefab;  //敌方人员 灰
    public GameObject PlayerGreenPrefab;//我方主将 绿
    [Header ("Grid")]
    public float margin;
    public float cellMoveTime = .2f;
    [Header ("Level")]
    public int Level = 1;//关卡数
    public int Timer = 10;//倒计时
    public int TotalGrids { get { return (Level*2) * (Level*3); } }//总网格数
    public int RowCount { get { return Level*2; } }//每一排网格数
    public int ColumnCount{ get { return Level*3; }}//每一纵队网格数
    public int GreenCount { get; protected set; }//总体绿色格子的数目
    public int HealthCount { get{return GreenCount;}}
    public int virusCount { get; protected set; }//病毒个数
    public int Steps { get; protected set; }//步骤数
    public float perUnitScale { get; protected set; }//每一格的单位大小
    public Camera cam { get; protected set; }//获取主摄像机
    public Cell VirusRed { get; protected set; }
    public SpawnGreyVirus VirusSpawner { get; protected set; }
    public Cell[] GreenCells { get; protected set; }
    public List<Cell> GreyCells { get; protected set; }
    public GameObject GridRoot { get; protected set; }
    public AudioManager audioManager {get; protected set; }
    public bool WaitForInput{get; protected set;}//等待输入，真为等待输入，否为不接受输入
    public HUD hud{get; protected set; }
    public static GameManager instance;
    protected Cell ClickedGreyCell;
    protected bool FirstStep = true;
    protected Coroutine _TimerCoroutine = null;
    protected Save _save;
    //设置GameManager为Singleton，且不会因为加载关卡被重置
    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad (gameObject);
        }
        else if (instance != null) {
            Destroy (gameObject);
        }

        LoadByBin();
    }

    //获取主摄像机
    public void GetMainCamera() {
        cam = Camera.main;
    }

    //获取AudioManager
    public void GetAudioManager(AudioManager _audioManager){
        audioManager = _audioManager;
    }

    //获取HUD
    public void GetHUD(HUD _hud){
        hud = _hud;
    }

    //卸载所有关卡资源
    public void UnloadLevel() {
        closeTimer();
        GreenCount = 0;
        FirstStep  = true;
        GreyCells.Clear();
        Destroy(GridRoot.gameObject);
    }

    //重新加载关卡
    public void RefreshLevel() {
        WaitForInput = true;
        UnloadLevel();
        GenerateGrid();
       
    }

    //加载下一关
    public void NextLevel() {
        Level++;
        RefreshLevel ();
    }

    //加载上一关
    public void PreviousLevel() {
        Level--;
        if (Level < 1)
            Level = 1;
        RefreshLevel ();
    }

    //创建格子
    public void GenerateGrid() {
        SaveByBin();
        //创建一个网格根节点
        GridRoot = new GameObject ("GridRoot");
        GridRoot.transform.position = Vector2.zero;

        //根据摄像机的scale，以及网格总数，获取单元网格的大小
        Camera cam = Camera.main;
        perUnitScale = (cam.orthographicSize * 2 * cam.aspect-margin) / RowCount;

        //设置网格根节点的起始点
        Vector2 InitOffset = new Vector2 (-perUnitScale * (RowCount - 1) / 2f, -perUnitScale * (ColumnCount - 1) / 2f);
        GridRoot.transform.position = InitOffset;

        //生成网格与所有绿色格子，并关掉所有的绿色格子
        GreenCells = new Cell[TotalGrids];
        Vector2 pos = Vector2.zero;
        for (int i = 0; i < TotalGrids; i++) {
            pos.y = i / RowCount * perUnitScale;
            pos.x = i % RowCount * perUnitScale;

            GameObject grid = Instantiate (GridPrefab);
            grid.transform.parent = GridRoot.transform;
            grid.transform.localPosition = pos;
            grid.transform.localPosition += Vector3.forward;
            grid.transform.localScale *= perUnitScale;
            grid.GetComponent<Cell> ().SetIndex (i % RowCount, i / RowCount);

            GreenCells[i] = Instantiate (PlayerGreenPrefab).GetComponent<Cell> ();
            GreenCells[i].transform.parent = GridRoot.transform;
            GreenCells[i].transform.localPosition = pos;
            GreenCells[i].transform.localScale *= perUnitScale;
            GreenCells[i].SetIndex (i % RowCount, i / RowCount);
            GreenCells[i].SetId(i);
            GreenCells[i].DisableCell ();
        }

        //放置红色格子
        Vector2Int index_red = new Vector2Int (Random.Range (0, RowCount), Random.Range (0, RowCount));
        VirusRed = Instantiate (VirusRedPrefab).GetComponent<Cell> ();
        VirusRed.transform.parent = GridRoot.transform;
        VirusRed.transform.localPosition = new Vector2 (index_red.x * perUnitScale, index_red.y * perUnitScale);
        VirusRed.transform.localScale *= perUnitScale;
        VirusRed.SetIndex (index_red);

        VirusSpawner = VirusRed.GetComponent<SpawnGreyVirus> ();

        GreyCells = new List<Cell> ();
        virusCount = 1;
        Steps = 0;
        WaitForInput = true;


        closeTimer();

        _TimerCoroutine = StartCoroutine(Coroutine_TimerDown(10));
    }

    public void closeTimer()
    {
        if (_TimerCoroutine != null) StopCoroutine(_TimerCoroutine);
    }
    //放置灰色格子
    public void PlaceGreyCell(Vector2Int pos) {
        GameObject grid = Instantiate (VirusGreyPrefab);
        grid.transform.parent = GridRoot.transform;
        grid.transform.localPosition = perUnitScale * (Vector2)pos;
        grid.transform.localScale *= perUnitScale;
        grid.GetComponent<Cell> ().SetIndex (pos.x, pos.y);
        GreyCells.Add (grid.GetComponent<Cell> ());
        virusCount ++;
    }


    //倒计时
    IEnumerator Coroutine_TimerDown(int timer) {
        Timer = timer;
        while (Timer > 0)
        {
            yield return new WaitForSeconds(1);
            Timer--;
            if(Timer <= 3 && Timer > 0) audioManager.PlaySound(9);

        }

        audioManager.PlaySound(10);
        TimerOut();

    }


    //用coroutine处理格子的交互
    //Coroutine开始
    IEnumerator Coroutine_WhiteCellInteraction(Vector2Int pos){
        WaitForInput = false;
        if (FirstStep)
            FirstStep = false;
        Steps++;
        GreenCount++;
        audioManager.PlaySound(2);
        getGreenCell (pos).EnableCell ();
        UpdateWholeGrid ();
        //停顿让移动完全进行
        yield return new WaitForSeconds(cellMoveTime+.1f);

        //如果游戏未结束，重新开启输入检测
        if(!CheckEndState()) WaitForInput = true;

        yield return null;
    }
    IEnumerator Coroutine_GreyCellInteraction(Cell cell){
        WaitForInput = false;
        if (FirstStep)
            FirstStep = false;
        Steps++;
        ReduceInfectionLevel(cell);
        ClickedGreyCell = cell;
        UpdateWholeGrid ();
        ClickedGreyCell = null;
        //停顿让移动完全进行
        yield return new WaitForSeconds(cellMoveTime);

        //如果游戏未结束，重新开启输入检测
        if(!CheckEndState()) WaitForInput = true;

        yield return null;
    }
    IEnumerator Coroutine_RedCellInteraction(Vector2Int pos){
        WaitForInput = false;
        Steps++;
        UpdateGreen ();
        UpdateVirusGrey ();
        UpdateGreen ();
        //停顿让移动完全进行
        yield return new WaitForSeconds(cellMoveTime);

        //如果游戏未结束，重新开启输入检测
        if(!CheckEndState()) WaitForInput = true;

        yield return null;
    }
    IEnumerator Coroutine_MovePieces(Vector2Int pos, Cell cell, float time = 1){
        Vector3 initPos = cell.transform.localPosition;
        Vector3 targetPos = perUnitScale*(Vector2)pos;
        for(float timer = 0; timer < 1; timer += Time.deltaTime/time){
            cell.transform.localPosition = Vector3.Lerp(initPos, targetPos, timer);
            yield return null;
        }
        cell.transform.localPosition = perUnitScale*(Vector2)pos;
        yield return null;
    }
//Coroutine结束

    //点击空白格子，放置一个绿色格子
    public void InteractWithWhiteCell(Vector2Int pos) {
        StartCoroutine(Coroutine_WhiteCellInteraction(pos));
    }
    //点击灰色格子，并削弱它
    public void InteractWithGreyCell(Cell cell){
        StartCoroutine(Coroutine_GreyCellInteraction(cell));
    }
    //与红色格子交互，并限制它的移动
    public void InteractWithRedCell(Vector2Int pos){
        StartCoroutine(Coroutine_RedCellInteraction(pos));
    }
    //移动格子
    public void MovePieces(Vector2Int pos, Cell cell, float time = 1){
        StartCoroutine(Coroutine_MovePieces(pos, cell, time));
    }

    //检查胜败条件
    protected bool CheckEndState(){
      
        if(CheckFillState()){
            if(GreenCount > virusCount){
                hud.DisplayWinScenes();
            }
            else{
                hud.DisplayLoseScenes();
            }
            return true;
        }
        else{
            if(CheckLoseState()){
                WaitForInput = false;
                hud.DisplayLoseScenes();
                return true;
            }
            else if(CheckWinState()){
                WaitForInput = false;
                hud.DisplayWinScenes();
                return true;
            }
        }

        return false;
    }

    //遭遇绿色格子
    protected bool ResolveGreenCell(Cell greenCell, Cell greyCell){
        int infectionLevel = greyCell.GetComponent<InfectionLevel>().level;
        int healthLevel    = greenCell.GetComponent<HealthLevel>().level;

        if(infectionLevel >= healthLevel){
            return true;
        }
        else{ //补尝倒计时间
            if(Timer>0)Timer++;
            return false;
        }
    }

    //消灭灰色格子
    public void RemoveGreyCell(Cell cell){
        virusCount --;
        audioManager.PlaySound(3);
        GreyCells.Remove(cell);
        Destroy(cell.gameObject);
    }

    //消灭绿色格子
    protected void RemoveGreenCell(Vector2Int pos) {
        GreenCount--;
        audioManager.PlaySound(4);
        getGreenCell (pos).DisableCell ();
    }

    //削弱灰色格子的感染值
    protected void ReduceInfectionLevel(Cell cell){
        cell.GetComponent<InfectionLevel>().ReduceLevel();
    }

    protected void TimerOut() //倒计时结束
    {
        if (Timer <= 0)
        {
            if (WaitForInput == true)
            {
                WaitForInput = false;
            }

            hud.DisplayLoseScenes();
        }
    }

    //更新整个网格
    protected void UpdateWholeGrid() {
        UpdateGreen2 ();
        UpdateVirusGrey ();
        UpdateVirusRed ();
        UpdateGreen2 ();
        // UpdateGreen2();
        // Debug.Log ($"3 更新整个网格");
        TimerOut();
    }

    //更新所有绿色格子
    protected void UpdateGreen() {
      
        for (int i=0; i<GreenCells.Length; i++){
            if(GreenCells[i].IF_Activate){
                GreenCells[i].GetComponent<HealthLevel>().UpdateLevel();
            }
        }
    }



    #region 判断绿块数量

    //递归算法
    protected void addBlock(ref ArrayList block, Cell checkCell)
    {
        if (block.IndexOf(checkCell.id) != -1) return;

        int blockid = checkInBlock(checkCell.id);

        if (blockid != -1) return;

        block.Add(checkCell.id);


        if (IfPosGreen(checkCell.index + Vector2Int.up) == true)
        {

            addBlock(ref block, getGreenCell(checkCell.index + Vector2Int.up));
        }


        if (IfPosGreen(checkCell.index + Vector2Int.right) == true)
        {

            addBlock(ref block, getGreenCell(checkCell.index + Vector2Int.right));
        }

        if (IfPosGreen(checkCell.index + Vector2Int.down) == true)
        {

           addBlock(ref block, getGreenCell(checkCell.index + Vector2Int.down));
        }

        if (IfPosGreen(checkCell.index + Vector2Int.left) == true)
        {

             addBlock(ref block, getGreenCell(checkCell.index + Vector2Int.left));
        }


      

    }



    protected List<ArrayList> greenBlock = new List<ArrayList>();


    //判断是否在块记录中 返回块id
    protected int checkInBlock(int cellId) 
    {
        var res = (from q2 in greenBlock where q2.Contains(cellId) == true select q2).ToList();//
        if(res.Count > 0){
            return (int)res[0][0];
        }
        else
        {
            return -1;
        }
       
    }

    //判断连成片个数
    protected int checkNBISGree (int count)
    {


        //IfPosGreen();
        return 0;


    }


    //更新所有绿色格子 检查块连成块
    protected void UpdateGreen2()
    {
        Debug.Log("UpdateGreen2 begin--->");
        //刷新绿色组块表
        greenBlock.Clear();
        for (int i = 0; i < GreenCells.Length; i++)
        {
            ArrayList block = new ArrayList();
            block.Add(greenBlock.Count);
            if (GreenCells[i].IF_Activate)
            {

                addBlock(ref block, GreenCells[i]);
                if(block.Count>1) greenBlock.Add(block);
            }


        }


       //判断是否在某块中
        for (int i = 0; i < GreenCells.Length; i++)
        {
            if (GreenCells[i].IF_Activate)
            {
                var res = (from q2 in greenBlock where q2.Contains(i) == true select q2).ToList();//
                //Debug.Log("green id:"+i);
                if(res.Count >0)
                    GreenCells[i].GetComponent<HealthLevel>().setLevel(res[0].Count-1);
                else
                    GreenCells[i].GetComponent<HealthLevel>().setLevel(1);
            }
        }

    
        //先更新绿块列表 

        //在更新单个格子生命值


    }

    #endregion

  

    //更新红色格子的位置
    protected void UpdateVirusRed() {
        VirusRed.PrepareStep ();

        if (IfPosGreen (VirusRed.nextIndex) || IfPosGrey (VirusRed.nextIndex)) {
            VirusRed.Reset ();
        }
        else {
            VirusSpawner.SpawnGreyCellOnCurrentPos ();
            VirusRed.Step ();
        }
    }

    //更新灰色格子
    protected void UpdateVirusGrey() {
        for (int i=GreyCells.Count-1; i>=0; i--) {

            //跳过被点击的灰色格子
            if (GreyCells[i] == ClickedGreyCell) continue;

            //灰色格子感染值+1
            GreyCells[i].GetComponent<InfectionLevel>().UpgradeLevel();

            //更新灰色格子的位置
            GreyCells[i].PrepareStep ();

            if (IfPosGrey (GreyCells[i].nextIndex) || IfPosRed (GreyCells[i].nextIndex)) {
                GreyCells[i].Reset ();
            }
            else if (IfPosGreen (GreyCells[i].nextIndex)) {
                if(ResolveGreenCell(getGreenCell(GreyCells[i].nextIndex), GreyCells[i])){
                    RemoveGreenCell(GreyCells[i].nextIndex);
                    PlaceGreyCell(GreyCells[i].index);
                    GreyCells[i].Step ();
                    continue;
                }
                else{
                    RemoveGreyCell(GreyCells[i]);
                    continue;
                }
            }
            else {
                GreyCells[i].Step ();
            }

         
        }
    }

    //检查给定位置是否有网格
    public bool CheckGrid(Vector2Int pos) {
        if (pos.x < 0 || pos.x >= RowCount || pos.y < 0 || pos.y >= ColumnCount)
            return false;
        return true;
    }

    //检查当前网格是否为绿色
    public bool IfPosGreen(Vector2Int pos) {
        if(!CheckGrid(pos)) return false;
        return getGreenCell (pos).IF_Activate;
    }

    //检查当前网格是否为红色
    public bool IfPosRed(Vector2Int pos) {
        if(!CheckGrid(pos)) return false;
        return VirusRed.index == pos;
    }

    //检查当前网格是否为灰色
    public bool IfPosGrey(Vector2Int pos) {
        if(!CheckGrid(pos)) return false;
        for(int i=0; i<GreyCells.Count; i++){
            if (GreyCells[i].index == pos)
                return true;
        }

        return false;
    }

    //获取给定位置的绿色网格
    protected Cell getGreenCell(Vector2Int pos) {
        int index = pos.y * RowCount + pos.x;
        return GreenCells[index].GetComponent<Cell> ();
    }

    //检查网格是否占满
    protected bool CheckFillState(){
        for(int i=0; i<TotalGrids; i++){
            Vector2Int pos = new Vector2Int(i%RowCount, i/RowCount);
            if(!IfPosGrey(pos)){
                if(!IfPosGreen(pos)){
                    if(!IfPosRed(pos)){
                        return false;
                    }
                }
            }
        }

        return true;
    }

    //检查胜利条件
    protected bool CheckWinState() {
        //先检查角落，只对格子内的区域做检查
        List<Vector2Int> possibleSteps = new List<Vector2Int> ();
        Vector2Int right, up, left, down;
        right = new Vector2Int (1, 0);
        up = new Vector2Int (0, 1);
        left = new Vector2Int (-1, 0);
        down = new Vector2Int (0, -1);

        if (CheckGrid (VirusRed.index + right))
            possibleSteps.Add (right);
        if (CheckGrid (VirusRed.index + up))
            possibleSteps.Add (up);
        if (CheckGrid (VirusRed.index + left))
            possibleSteps.Add (left);
        if (CheckGrid (VirusRed.index + down))
            possibleSteps.Add (down);

        //检查周围的区域是否都有绿色格子，如果都有为真，否则为假。
        for (int i = 0; i < possibleSteps.Count; i++) {
            if (!IfPosGreen (VirusRed.index + possibleSteps[i]))
                return false;
        }

        return true;
    }

    //检查失败条件
    protected bool CheckLoseState() {
        return GreenCount == 0 && !FirstStep;
    }





    //创建Save对象并存储当前游戏状态信息
    private Save CreateSaveGO()
    {
        //新建Save对象
        Save save = new Save();
        //遍历所有的target
        //如果其中有处于激活状态的怪物，就把该target的位置信息和激活状态的怪物的类型添加到List中

        save.level = Level;
        //返回该Save对象
        return save;
    }

    //通过读档信息重置我们的游戏状态（分数、激活状态的怪物）
    private void SetGame(Save save)
    {
        Level = save.level;
    }


    //二进制方法：存档和读档
    private void SaveByBin()
    {
        //序列化过程（将Save对象转换为字节流）
        //创建Save对象并保存当前游戏状态
        Save save = CreateSaveGO();
        //创建一个二进制格式化程序
        BinaryFormatter bf = new BinaryFormatter();
        //创建一个文件流
        FileStream fileStream = File.Create(Application.persistentDataPath + "/game.bin");
        //用二进制格式化程序的序列化方法来序列化Save对象,参数：创建的文件流和需要序列化的对象
        bf.Serialize(fileStream, save);
        //关闭流
        fileStream.Close();

        //如果文件存在，则显示保存成功
        if (File.Exists(Application.persistentDataPath + "/game.bin"))
        {
            //UIManager._instance.ShowMessage("保存成功");
        }
    }

    private void LoadByBin()
    {
        if (File.Exists(Application.persistentDataPath + "/game.bin"))
        {
            //反序列化过程
            //创建一个二进制格式化程序
            BinaryFormatter bf = new BinaryFormatter();
            //打开一个文件流
            FileStream fileStream = File.Open(Application.persistentDataPath + "/game.bin", FileMode.Open);
            //调用格式化程序的反序列化方法，将文件流转换为一个Save对象
            Save save = (Save)bf.Deserialize(fileStream);
            //关闭文件流
            fileStream.Close();

            SetGame(save);
          

        }
        else
        {
           
        }


    }
}
