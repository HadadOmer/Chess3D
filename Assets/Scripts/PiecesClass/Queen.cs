using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
   public Queen(bool isWhite, Vector2Int currentPos) : base(isWhite, currentPos)
    {

    }

    public override List<Vector2Int> AvailableMovement(GameObject[,] board)
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        //Right 
        for (int i = currentPos.x + 1; i < board.GetLength(0); i++)
        {
            if (board[i, currentPos.y].transform.childCount == 0)
                moves.Add(new Vector2Int(i, currentPos.y));
            else
            {
                Piece piece = board[i, currentPos.y].transform.GetChild(0).GetComponent<Piece>();
                if (piece.isWhite != this.isWhite)
                    moves.Add(new Vector2Int(i, currentPos.y));
                break;
            }
        }

        //Left
        for (int i = currentPos.x - 1; i >= 0; i--)
        {
            if (board[i, currentPos.y].transform.childCount == 0)
                moves.Add(new Vector2Int(i, currentPos.y));
            else
            {
                Piece piece = board[i, currentPos.y].transform.GetChild(0)?.GetComponent<Piece>();
                if (piece.isWhite != this.isWhite)
                    moves.Add(new Vector2Int(i, currentPos.y));
                break;
            }
        }
        //Up
        for (int i = currentPos.y + 1; i < board.GetLength(1); i++)
        {
            if (board[currentPos.x, i].transform.childCount == 0)
                moves.Add(new Vector2Int(currentPos.x, i));
            else
            {
                Piece piece = board[currentPos.x, i].transform.GetChild(0).GetComponent<Piece>();
                if (piece.isWhite != this.isWhite)
                    moves.Add(new Vector2Int(currentPos.x, i));
                break;
            }
        }

        //Down 
        for (int i = currentPos.y - 1; i >= 0; i--)
        {
            if (board[currentPos.x, i].transform.childCount == 0)
                moves.Add(new Vector2Int(currentPos.x, i));
            else
            {
                Piece piece = board[currentPos.x, i].transform.GetChild(0).GetComponent<Piece>();
                if (piece.isWhite != this.isWhite)
                    moves.Add(new Vector2Int(currentPos.x, i));
                break;
            }
        }


        //Up Right
        for (int x = currentPos.x + 1, y = currentPos.y + 1;
            x < board.GetLength(0) && y < board.GetLength(1);
            x++, y++)
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
        for (int x = currentPos.x - 1, y = currentPos.y + 1;
            x >= 0 && y < board.GetLength(1);
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
        for (int x = currentPos.x + 1, y = currentPos.y - 1;
            x < board.GetLength(0) && y >= 0;
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
        for (int x = currentPos.x - 1, y = currentPos.y - 1;
            x >= 0 && y >= 0;
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
