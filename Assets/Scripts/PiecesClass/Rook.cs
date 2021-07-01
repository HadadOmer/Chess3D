using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{
    public Rook(bool isWhite,Vector2Int currentPos) : base(isWhite, currentPos)
    {

    }
    public override List<Vector2Int> AvailableMovement(GameObject[,] board)
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        //Right 
        for (int i = currentPos.x+1; i < board.GetLength(0); i++)
        {
            if (board[i, currentPos.y].transform.childCount == 0)
                moves.Add(new Vector2Int(i, currentPos.y));
            else
            {
                Piece piece = board[i, currentPos.y].transform.GetChild(0).GetComponent<Piece>();
                if(piece.isWhite!=this.isWhite)
                    moves.Add(new Vector2Int(i, currentPos.y));
                break;
            }
        }

        //Left
        for (int i =currentPos.x-1; i >=0; i--)
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
        for (int i = currentPos.y+1; i < board.GetLength(1); i++)
        {
            if (board[currentPos.x, i].transform.childCount == 0)
                moves.Add(new Vector2Int(currentPos.x, i));
            else
            {
                Piece piece = board[currentPos.x, i].transform.GetChild(0).GetComponent<Piece>();
                if(piece.isWhite!=this.isWhite)
                    moves.Add(new Vector2Int(currentPos.x, i));
                break;
            }
        }

        //Down 
        for (int i = currentPos.y - 1; i >=0; i--)
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

        //Checks Castling
        Piece king;
        GameObject kingPosition = isWhite ? board[4, 0] : board[4, 7];
        king = kingPosition.GetComponentInChildren<King>();

        if (king != null&& !king.moved && !this.moved)
        {
            Vector2Int moveClosestToKing = new Vector2Int(currentPos.x == 0 ? 3 : 5, currentPos.y);
            if (moves.Contains(moveClosestToKing))
                moves.Add(king.currentPos);
        }

      

        return moves;
    }

  
}
