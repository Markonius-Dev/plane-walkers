using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private GameObject grid;
    private List<GameObject> entities;

    void Start()
    {
        GridLayout gridLayout = GetComponent<GridLayout>();
        Debug.Log($"Cell Gap : {gridLayout.cellGap.y}");

        entities = new List<GameObject>(0);

        GameObject character = Instantiate(Resources.Load<GameObject>("Prefabs/Character"));
        character.name = "Character";
        character.transform.parent = transform;
        entities.Add(character);

        
    }

    
    void Update()
    {
        
    }
}
