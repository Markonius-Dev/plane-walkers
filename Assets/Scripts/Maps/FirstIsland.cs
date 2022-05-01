using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstIsland : MonoBehaviour
{
    // Script managing First Island map.

    private List<Entity> entities;

    private void Start()
    {
        entities = new List<Entity>();

        SpawnCharacter();

        void SpawnCharacter()
        {
            Debug.Log($"Loading Character");
            Entity character = new Entity();

            character.gameObject = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSample"));

            character.gameObject.transform.parent = transform;
            character.gameObject.transform.name = "Character";



            if (entities.Count == 0) entities.Add(character);
            else entities[0] = character;
        }
    }

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
            direction = 7;
            movementDelta = 0;
            movementQueue = new List<int>();
        }
    }
}
