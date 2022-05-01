using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstIsland : MonoBehaviour
{
    // Script managing First Island map.

    private List<GameObject> entities;

    private void Start()
    {
        entities = new List<GameObject>();

        SpawnCharacter();

        void SpawnCharacter()
        {
            Debug.Log($"Loading Character");
            GameObject character = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSample"));

            character.transform.parent = transform;
            character.transform.name = "Character";

            if (entities.Count == 0) entities.Add(character);
            else entities[0] = character;
        }
    }
}
