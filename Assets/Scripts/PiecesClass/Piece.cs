using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece:MonoBehaviour
{
    public bool isWhite;
    public Vector2Int currentPos; // tile's position in the matrix
    public bool moved;//Did the piece move already

    public Piece(bool isWhite,Vector2Int currentPos)
    {
        this.isWhite = isWhite;
        this.currentPos = currentPos;
        this.moved = false;
    }
    public void ChangePosition(Vector2Int position)
    {
        moved = true;
        currentPos = position;
    }
    public virtual List<Vector2Int> AvailableMovement(GameObject[,] board)
    {
        return new List<Vector2Int>();
    }
}



