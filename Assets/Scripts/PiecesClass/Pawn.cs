using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public Pawn(bool isWhite, Vector2Int currentPos):base(isWhite,currentPos)
    {

    }
    public override List<Vector2Int> AvailableMovement(GameObject[,] board)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        if (isWhite)
        {
            if (currentPos.y == 7)   //if the piece is at the edge
                return moves;

            if (currentPos.y == 1)    //if its the pawns first move he can either move 2 moves forward or 1 move forward
            {
                if (board[currentPos.x, 2].transform.childCount == 0)
                {
                    moves.Add(new Vector2Int(currentPos.x, 2));
                    if (board[currentPos.x, 3].transform.childCount == 0)
                        moves.Add(new Vector2Int(currentPos.x, 3));
                }
            }


            else if (board[currentPos.x, currentPos.y + 1].transform.childCount == 0)            //any other move for the pawn is available if there is no piece in front of him           
                moves.Add(new Vector2Int(currentPos.x, currentPos.y + 1));

            if (currentPos.x != 7)
                if (board[currentPos.x + 1, currentPos.y + 1].transform.childCount != 0)        // if there are pieces in diagnol to the pawn he can eat them
                    if (board[currentPos.x + 1, currentPos.y + 1].transform.GetComponentInChildren<Piece>().isWhite != this.isWhite)
                        moves.Add(new Vector2Int(currentPos.x + 1, currentPos.y + 1));
            if (currentPos.x != 0)
                if (board[currentPos.x - 1, currentPos.y + 1].transform.childCount != 0)
                    if (board[currentPos.x - 1, currentPos.y + 1].transform.GetComponentInChildren<Piece>().isWhite != this.isWhite)
                        moves.Add(new Vector2Int(currentPos.x - 1, currentPos.y + 1));
        }
        else    //same actions but on black pieces
        {
            if (currentPos.y == 0)
                return moves;

            if (currentPos.y == 6)
            {
                if (board[currentPos.x, 5].transform.childCount == 0)
                {
                    moves.Add(new Vector2Int(currentPos.x, 5));
                    if (board[currentPos.x, 4].transform.childCount == 0)
                        moves.Add(new Vector2Int(currentPos.x, 4));
                }
            }

            else if (board[currentPos.x, currentPos.y - 1].transform.childCount == 0)
                moves.Add(new Vector2Int(currentPos.x, currentPos.y - 1));

            if (currentPos.x != 7)
                if (board[currentPos.x + 1, currentPos.y - 1].transform.childCount != 0)
                    if (board[currentPos.x + 1, currentPos.y - 1].transform.GetComponentInChildren<Piece>().isWhite != this.isWhite)
                        moves.Add(new Vector2Int(currentPos.x + 1, currentPos.y - 1));

            if (currentPos.x != 0)
                if (board[currentPos.x - 1, currentPos.y - 1].transform.childCount != 0)
                    if (board[currentPos.x - 1, currentPos.y - 1].transform.GetComponentInChildren<Piece>().isWhite != this.isWhite)
                        moves.Add(new Vector2Int(currentPos.x - 1, currentPos.y - 1));
        }

        return moves;
    }

}
