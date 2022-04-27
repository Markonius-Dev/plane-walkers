using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTable : MonoBehaviour
{

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(0, 1, 0);

        GridLayout gridLayout = transform.parent.GetComponentInParent<GridLayout>();
        Vector3 localPosition = gridLayout.CellToLocal(new Vector3Int(5, 0, 0));

        Vector3 correctPosition = new Vector3(localPosition.x + offset.x, localPosition.y + offset.y, localPosition.z);
        transform.position = correctPosition;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
