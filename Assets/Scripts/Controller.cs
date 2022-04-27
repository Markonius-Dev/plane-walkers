using System;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private GridLayout gridLayout;

    [SerializeField] private GameObject player;
    private Vector2 playerPosOffset;
    private Vector3Int playerCellPos;

    private bool playerShouldMove;
    private bool playerIsMoving;
    private Vector3Int playerIsMovingTo;
    private Vector3Int playerMovement;
    private List<Vector3Int> movementQueue;

    private float movementCompletion;

    private void Start()
    {
        playerPosOffset = new Vector2(0, (float)0.5);

        Vector3Int startingPosition = new Vector3Int(0, 0, 0);
        playerIsMovingTo = new Vector3Int(0, 0, 0);
        TeleportPlayerTo(startingPosition);

        movementQueue = new List<Vector3Int>();
        playerShouldMove = false;
        playerIsMoving = false;

        movementCompletion = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) HandlePrimaryTouch(Input.mousePosition);

        if (playerIsMoving)
        {
            movementCompletion += Time.deltaTime * 2;

            if (movementCompletion >= 1) movementCompletion = 1;
            ProgressPlayerMovement();
        }

        if (playerShouldMove && !playerIsMoving)
        {
            if (movementQueue.Count > 0)
            {
                playerMovement = movementQueue[0];
                playerIsMovingTo = new Vector3Int(playerCellPos.x + playerMovement.x, playerCellPos.y + playerMovement.y, playerCellPos.z);

                playerIsMoving = true;

                if (movementQueue.Count == 1) playerShouldMove = false;

                movementQueue.RemoveAt(0);
            }
            else
            {
                Debug.LogError("Player should move, but there's no queued movement.");
            }
        }
    }

    private void TeleportPlayerTo(Vector3Int destination)
    {
        Vector3 localDestination = gridLayout.CellToLocal(destination);
        Vector3 correctDestination = ReturnCorrectPlayerPosition(localDestination);

        player.transform.position = correctDestination;

        playerCellPos = destination;
    }

    private void TeleportPlayerTo(Vector3 destination)
    {
        Vector3 correctDestination = ReturnCorrectPlayerPosition(destination);

        player.transform.position = correctDestination;
    }

    private Vector3 ReturnCorrectPlayerPosition(Vector3 position)
    {
        return new Vector3(position.x + playerPosOffset.x, position.y + playerPosOffset.y, position.z);
    }

    private void HandlePrimaryTouch(Vector2 touchPos)
    {
        if (!playerShouldMove)
        {
            Vector3Int touchedCell = ReturnCellFromTouchPos(touchPos);
            HandleMovementPlaning(touchedCell);
        }
    }

    private Vector3Int ReturnCellFromTouchPos(Vector2 touchPos)
    {
        Vector2 worldTouchPos = Camera.main.ScreenToWorldPoint(touchPos);
        return gridLayout.WorldToCell(worldTouchPos);
    }

    private void HandleMovementPlaning(Vector3Int destination)
    {
        Vector3Int path = new Vector3Int(destination.x - playerCellPos.x, destination.y - playerCellPos.y, playerCellPos.z);

        GenerateMovementQueue(path);
        if (movementQueue.Count > 0) playerShouldMove = true;

    }

    private void GenerateMovementQueue(Vector3Int path)
    {
        int diagonalMoves = Math.Min(Math.Abs(path.x), Math.Abs(path.y));
        int direction = -1;

        if (diagonalMoves > 0)
        {
            if (path.x > 0)
            {
                path.x -= diagonalMoves;
                if (path.y > 0)
                {
                    path.y -= diagonalMoves;
                    direction = 1;
                }
                else // path.y < 0
                {
                    path.y += diagonalMoves;
                    direction = 2;
                }
            }
            else // path.x < 0
            {
                path.x += diagonalMoves;
                if (path.y < 0)
                {
                    path.y += diagonalMoves;
                    direction = 3;
                }
                else // path.y > 0
                {
                    path.y -= diagonalMoves;
                    direction = 4;
                }
            }
        }

        for (int i = 0; i < diagonalMoves; i++)
        {
            switch (direction)
            {
                case 1:
                    movementQueue.Add(new Vector3Int(1, 1, 0));
                    break;

                case 2:
                    movementQueue.Add(new Vector3Int(1, -1, 0));
                    break;

                case 3:
                    movementQueue.Add(new Vector3Int(-1, -1, 0));
                    break;

                case 4:
                    movementQueue.Add(new Vector3Int(-1, 1, 0));
                    break;

                default:
                    Debug.LogError("Unexpected direction");
                    break;
            }
        }

        if (path.x > 0)
        {
            for (int i = 0; i < path.x; i++)
            {
                movementQueue.Add(new Vector3Int(1, 0, 0));
            }
        }
        else if (path.x < 0)
        {
            for (int i = 0; i < Math.Abs(path.x); i++)
            {
                movementQueue.Add(new Vector3Int(-1, 0, 0));
            }
        }

        if (path.y > 0)
        {
            for (int i = 0; i < path.y; i++)
            {
                movementQueue.Add(new Vector3Int(0, 1, 0));
            }
        }
        else if (path.y < 0)
        {
            for (int i = 0; i < Math.Abs(path.y); i++)
            {
                movementQueue.Add(new Vector3Int(0, -1, 0));
            }
        }
    }

    private void ProgressPlayerMovement()
    {
        Vector3 localOrigin = gridLayout.CellToLocal(playerCellPos);
        Vector3 localDestination = gridLayout.CellToLocal(playerIsMovingTo);

        Vector3 progress = new Vector3((localDestination.x - localOrigin.x) * movementCompletion, (localDestination.y - localOrigin.y) * movementCompletion, localOrigin.z);
        Vector3 newPos = new Vector3(localOrigin.x + progress.x, localOrigin.y + progress.y, localOrigin.z);


        TeleportPlayerTo(newPos);

        if (movementCompletion == 1)
        {
            playerCellPos = playerIsMovingTo;
            playerIsMoving = false;
            movementCompletion = 0;

        }
    }
}
