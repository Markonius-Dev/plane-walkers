using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    void Start()
    {
        GridLayout gridLayout = GetComponentInParent<GridLayout>();
        Debug.Log($"Cell Gap (from character) : {gridLayout.cellGap.y}");
    }

    void Update()
    {
        
    }
}
