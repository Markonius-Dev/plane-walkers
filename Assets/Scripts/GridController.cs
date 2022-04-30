using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridController : MonoBehaviour
{
    private GridLayout gridLayout;
    [SerializeField] private GameObject grid;
    
    private List<Entity> entities;
    private List<Rectangle> rectangles;
    private List<Vector3Int> cells;
    private List<Vector3Int> occupiedCells;

    public class Rectangle
    {
        public Vector3Int origin;
        public Vector2Int size;

        public Rectangle(Vector3Int origin, Vector2Int size)
        {
            this.origin = origin;
            this.size = size;
        }
    }

    public class Entity
    {
        public GameObject gameObject;
        public Vector3Int position;
        public int size;

        public Entity(GameObject gameObject, Vector3Int position, int size = 1)
        {
            this.gameObject = gameObject;
            this.position = position;
            this.size = size;
        }
    }

    void Start()
    {
        gridLayout = GetComponent<GridLayout>();
        Debug.Log($"Cell Gap : {gridLayout.cellGap.y}");

        entities = new List<Entity>();

        Entity character = new Entity(Instantiate(Resources.Load<GameObject>("Prefabs/Character")), new Vector3Int(0, 0, 0));

        character.gameObject.name = "Character";
        character.gameObject.transform.parent = transform;
        entities.Add(character);

        rectangles = new List<Rectangle>();
        cells = new List<Vector3Int>();

        rectangles.Add(new Rectangle(new Vector3Int(-2, -2, 0), new Vector2Int(3, 3)));

    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Mouse0)) DebugLogCell(Input.mousePosition);

    }

    void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) Debug.Log($"The Grid was hit wih On Mouse Down");
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
        if (rectangles.Count > 0)
        {
            foreach (Rectangle rectangle in rectangles )
            {
                if (cell.x >= rectangle.origin.x && cell.y >= rectangle.origin.y)
                {
                    if (cell.x <= rectangle.origin.x + rectangle.size.x && cell.y <= rectangle.origin.y + rectangle.size.y) return true;
                }
            }
        }

        if (cells.Count > 0)
        {
            foreach (Vector3Int cellToCheck in cells)
            {
                if (cell.x == cellToCheck.x && cell.y == cellToCheck.y) return true;
            }
        }

        return false;
    }
}
