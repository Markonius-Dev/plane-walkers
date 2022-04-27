using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private GridLayout gridLayout;

    [SerializeField] private GameObject player;
    private Vector2 playerPosOffset;
    private Vector3Int playerCellPos;

    private void Start()
    {
        playerPosOffset = new Vector2(0, (float)0.5);

        Vector3Int startingPosition = new Vector3Int(0, 0, 0);
        TeleportPlayerTo(startingPosition);
    }

    private void TeleportPlayerTo(Vector3Int destination)
    {
        Vector3 localDestination = gridLayout.CellToLocal(destination);
        Vector3 correctDestination = ReturnCorrectPlayerPosition(localDestination);

        player.transform.position = correctDestination;

        playerCellPos = destination;
    }

    private Vector3 ReturnCorrectPlayerPosition(Vector3 position)
    {
        return new Vector3(position.x + playerPosOffset.x, position.y + playerPosOffset.y, position.z);
    }
}
