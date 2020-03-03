using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Cell))]
public class HealthLevel : MonoBehaviour
{
    public int level {get; protected set;}
    public TextMesh text;
    Cell cell;
    public void UpdateLevel(){
        if(!cell) cell = GetComponent<Cell>();
        level = 1;
 
        if(GameManager.instance.IfPosGreen(cell.index + Vector2Int.right)){level ++;}
        if(GameManager.instance.IfPosGreen(cell.index + Vector2Int.up))   {level ++;}
        if(GameManager.instance.IfPosGreen(cell.index + Vector2Int.left)) {level ++;}
        if(GameManager.instance.IfPosGreen(cell.index + Vector2Int.down)) {level ++;}

        text.text = level.ToString();
    }

    public void setLevel(int inputLevel)
    {

        level = inputLevel;
        text.text = level.ToString();
    }
}
