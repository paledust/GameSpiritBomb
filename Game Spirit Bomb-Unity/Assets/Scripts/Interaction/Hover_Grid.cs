using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover_Grid : MonoBehaviour
{
    public Color HighLightColor = Color.white;
    protected Color originalColor;
    protected float highLightFactor = 0;
    protected float targetFactor = 1;
    void Start(){
        originalColor = GetComponent<SpriteRenderer>().color;
    }
    void OnMouseEnter(){
        
    }
    void OnMouseExit(){
        // targetFactor
    }
}
