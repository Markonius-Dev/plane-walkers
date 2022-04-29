using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    private GridLayout gridLayout;
    [SerializeField] private GameObject grid;
    private List<GameObject> entities;

    private List<Vector3Int> cells;

    void Start()
    {
        gridLayout = GetComponent<GridLayout>();
        Debug.Log($"Cell Gap : {gridLayout.cellGap.y}");

        entities = new List<GameObject>(0);

        GameObject character = Instantiate(Resources.Load<GameObject>("Prefabs/Character"));
        character.name = "Character";
        character.transform.parent = transform;
        entities.Add(character);

        cells = new List<Vector3Int>();

        cells.Add(new Vector3Int(0, 0, 0));
        cells.Add(new Vector3Int(1, 0, 0));
        cells.Add(new Vector3Int(-1, 0, 0));
        cells.Add(new Vector3Int(-2, 0, 0));
        cells.Add(new Vector3Int(0, 1, 0));
        cells.Add(new Vector3Int(1, 1, 0));
        cells.Add(new Vector3Int(-1, 1, 0));
        cells.Add(new Vector3Int(-2, 1, 0));
        cells.Add(new Vector3Int(0, -1, 0));
        cells.Add(new Vector3Int(1, -1, 0));
        cells.Add(new Vector3Int(-1, -1, 0));
        cells.Add(new Vector3Int(-2, -1, 0));
        cells.Add(new Vector3Int(0, -2, 0));
        cells.Add(new Vector3Int(1, -2, 0));
        cells.Add(new Vector3Int(-1, -2, 0));
        cells.Add(new Vector3Int(-2, -2, 0));

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) DebugLogCell(Input.mousePosition);
    }

    private void DebugLogCell(Vector2 touchPos)
    {
        Vector2 worldTouchPos = Camera.main.ScreenToWorldPoint(touchPos);
        Vector3Int cell = gridLayout.WorldToCell(worldTouchPos);
        Debug.Log($"{cell}");
        if (ReturnCellExists(cell)) Debug.Log($"The cell is in the board");
        else Debug.Log($"The cell isn't in the board");
    }

    private bool ReturnCellExists(Vector3Int cell)
    {
        return cells.Exists(el => (el.x == cell.x && el.y == cell.y));
    }
}
