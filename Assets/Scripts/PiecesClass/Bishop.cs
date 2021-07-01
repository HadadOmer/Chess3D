using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
   public Bishop(bool isWhite, Vector2Int currentPos) : base(isWhite, currentPos)
   {

   }

    public override List<Vector2Int> AvailableMovement(GameObject[,] board)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        //Up Right
        for (int x = currentPos.x+1, y = currentPos.y+1; 
            x < board.GetLength(0) && y < board.GetLength(1);
            x++,y++)
        {
            if (board[x, y].transform.childCount == 0)
                moves.Add(new Vector2Int(x, y));
            else
            {
                Piece piece = board[x, y].GetComponentInChildren<Piece>();
                if (piece.isWhite != this.isWhite)
                    moves.Add(new Vector2Int(x, y));
                break;
            }
        }
        //Up Left
        for (int x = currentPos.x-1, y = currentPos.y+1;
            x >=0 && y < board.GetLength(1);
            x--, y++)
        {
            if (board[x, y].transform.childCount == 0)
                moves.Add(new Vector2Int(x, y));
            else
            {
                Piece piece = board[x, y].GetComponentInChildren<Piece>();
                if (piece.isWhite != this.isWhite)
                    moves.Add(new Vector2Int(x, y));
                break;
            }
        }

        //Down Right
        for (int x = currentPos.x+1, y = currentPos.y-1;
            x < board.GetLength(0) && y >=0;
            x++, y--)
        {
            if (board[x, y].transform.childCount == 0)
                moves.Add(new Vector2Int(x, y));
            else
            {
                Piece piece = board[x, y].GetComponentInChildren<Piece>();
                if (piece.isWhite != this.isWhite)
                    moves.Add(new Vector2Int(x, y));
                break;
            }
        }

        //Down Left
        for (int x = currentPos.x-1, y = currentPos.y-1;
            x >=0 && y >=0;
            x--, y--)
        {
            if (board[x, y].transform.childCount == 0)
                moves.Add(new Vector2Int(x, y));
            else
            {
                Piece piece = board[x, y].GetComponentInChildren<Piece>();
                if (piece.isWhite != this.isWhite)
                    moves.Add(new Vector2Int(x, y));
                break;
            }
        }

        return moves;
    }
}
