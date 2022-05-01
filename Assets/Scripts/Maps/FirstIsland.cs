using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstIsland : MonoBehaviour
{
    // Script managing First Island map.
    
    public class Entity
    {
        public GameObject gameObject;
        public Vector3Int position;
        public bool moving;
        public int direction;
        public float movementDelta;
        public List<int> movementQueue;

        public Entity()
        {
            gameObject = null;
            position = new Vector3Int(0, 0, 0);
            moving = false;
            direction = 1;
            movementDelta = 0;
            movementQueue = new List<int>();
        }
    }

    public struct Rectangle
    {
        public Vector3Int origin;
        public Vector2Int size;

        public Rectangle(Vector3Int origin, Vector2Int size)
        {
            this.origin = origin;
            this.size = size;
        }
    }

    private GridLayout gridLayout;

    [SerializeField] private GameObject cameraGameObject;
    private List<Entity> entities;
    private int framesPerMovement;

    private List<Rectangle> rectangles;

    private void Start()
    {
        entities = new List<Entity>();
        framesPerMovement = 30;

        rectangles = new List<Rectangle>();
        rectangles.Add(new Rectangle(new Vector3Int(-3, -4, 0), new Vector2Int(5, 7)));

        SpawnCharacter();

        void SpawnCharacter()
        {
            gridLayout = GetComponent<GridLayout>();

            Debug.Log($"Loading Character");
            Entity character = new Entity();

            character.gameObject = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSample"));

            character.gameObject.transform.parent = transform;
            character.gameObject.transform.name = "Character";

            cameraGameObject.transform.parent = character.gameObject.transform;

            character.movementQueue.Add(5);
            character.movementQueue.Add(5);
            character.movementQueue.Add(6);
            character.movementQueue.Add(2);

            if (entities.Count == 0) entities.Add(character);
            else entities[0] = character;

            Entity craftingStation = new Entity();

            craftingStation.gameObject = Instantiate(Resources.Load<GameObject>("Prefabs/CraftingStation"));

            craftingStation.gameObject.transform.parent = transform;
            craftingStation.gameObject.transform.name = "CraftingStation";

            entities.Add(craftingStation);
            TeleportEntity((entities.Count - 1), new Vector3Int(2, 1, 0));
        }
    }

    private void Update()
    {
        CheckEntitiesRequiringMovement();
    }

    private void CheckEntitiesRequiringMovement()
    {
        if (entities.Count > 0)
        {
            int index = 0;
            foreach (Entity entity in entities)
            {
                if (entity.moving) Debug.Log($"Character should move");
                if (entity.moving) MoveEntity(index);
                index++;
            }
        }
    }

    private void MoveEntity(int index)
    {
        Debug.Log($"Moving character");
        Entity entity = entities[index];
        Vector3 worldOrigin = gridLayout.CellToWorld(entity.position);

        Vector3Int direction = new Vector3Int(0, 0, 0);

        switch (entity.direction)
        {
            case 1:
                direction = new Vector3Int(1, 1, 0);
                break;

            case 2:
                direction = new Vector3Int(1, -1, 0);
                break;

            case 3: 
                direction = new Vector3Int(-1, -1, 0);
                break;

            case 4:
                direction = new Vector3Int(-1, 1, 0);
                break;

            case 5:
                direction = new Vector3Int(0, 1, 0);
                break;

            case 6:
                direction = new Vector3Int(1, 0, 0);
                break;

            case 7:
                direction = new Vector3Int(0, -1, 0);
                break;

            case 8:
                direction = new Vector3Int(-1, 0, 0);
                break;
        }

        Vector3Int cellDestination = new Vector3Int(entity.position.x + direction.x, entity.position.y + direction.y, 0);

        if (!ReturnCellExists(cellDestination) || ReturnCellIsOccupied(cellDestination))
        {
            Debug.LogError($"{entities[index].gameObject.name} tried to move at {cellDestination} but couldn't");
            entity.moving = false;
            entity.movementDelta = 0;
            entity.movementQueue.Clear();
        }

        Vector3 worldDestination = gridLayout.CellToWorld(cellDestination);

        Vector3 worldPositionDelta = new Vector3(worldDestination.x - worldOrigin.x, worldDestination.y - worldOrigin.y, 0);

        if (entities[index].direction == 2 || entities[index].direction == 4) entity.movementDelta += 0.5F;
        else entity.movementDelta += 1;
        float movementProgress;

        if (entity.movementDelta >= framesPerMovement) movementProgress = 1;

        else movementProgress = entity.movementDelta / framesPerMovement;
        Debug.Log($"movementProgress : {movementProgress}");

        if (movementProgress == 1)
        {
            entities[index].gameObject.transform.position = new Vector3(worldDestination.x, worldDestination.y, (worldDestination.y / 100));
            entities[index].movementDelta = 0;
            entities[index].position = cellDestination;

            if (entity.movementQueue.Count > 0)
            {
                entities[index].direction = entity.movementQueue[0];
                entities[index].movementQueue.RemoveAt(0);
            }
            else
            {
                entities[index].moving = false;
            }
        }
        else if (movementProgress < 1)
        {
            worldDestination = new Vector3((worldOrigin.x + (worldPositionDelta.x * movementProgress)), (worldOrigin.y + (worldPositionDelta.y * movementProgress)), ((worldOrigin.y + (worldPositionDelta.y * movementProgress)) / 100));
            entities[index].gameObject.transform.position = worldDestination;
            entities[index].movementDelta = entity.movementDelta;
        }
        else if (movementProgress > 1)
        {
            Debug.LogError($"There was an error while calculating movement progression for {entity.gameObject.transform.name}");
        }
    }

    private void TeleportEntity(int index, Vector3Int cell)
    {
        Entity entity = entities[index];

        Vector3 worldDestination = gridLayout.CellToWorld(cell);
        worldDestination.z = worldDestination.y / 100;

        entities[index].gameObject.transform.position = worldDestination;
        entities[index].position = cell;
    }

    private bool ReturnCellExists(Vector3Int cell)
    {
        if (rectangles.Count > 0)
        {
            foreach (Rectangle rectangle in rectangles)
            {
                if (cell.x >= rectangle.origin.x && cell.y >= rectangle.origin.y)
                {
                    if (cell.x <= (rectangle.origin.x + rectangle.size.x) && cell.y <= (rectangle.origin.y + rectangle.size.y)) return true;
                }
            }
        }

        return false;
    }

    private bool ReturnCellIsOccupied(Vector3Int cell)
    {
        if (entities.Count > 0)
        {
            foreach (Entity entity in entities)
            {
                if (entity.position.x == cell.x && entity.position.y == cell.y) return true;
            }
        }

        return false;
    }

    public void DebugOnClickButton()
    {
        entities[0].moving = true;
        entities[0].movementDelta = 0;
        entities[0].direction = 2;
    }
}
