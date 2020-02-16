using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//单位网格
public class Cell : MonoBehaviour
{
    public Vector2Int index{get; protected set;}
    public void SetIndex(Vector2Int _index){
        index = _index;
    }
}
