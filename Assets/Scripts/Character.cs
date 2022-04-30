using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) Debug.Log($"The Character was hit wih On Mouse Down");
    }
}
