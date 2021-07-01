using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    public Knight(bool isWhite, Vector2Int currentPos) : base(isWhite, currentPos)
    {

    }

    public override List<Vector2Int> AvailableMovement(GameObject[,] board)
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        if (currentPos.x < 7)    //  up/down and right movement
        {

            if (currentPos.y < 6) //up check
            {
                if (board[currentPos.x + 1, currentPos.y + 2].transform.childCount != 0)
                {
                    if (board[currentPos.x + 1, currentPos.y + 2].transform.GetComponentInChildren<Piece>().isWhite != this.isWhite)
                        moves.Add(new Vector2Int(currentPos.x + 1, currentPos.y + 2));
                }
                else
                    moves.Add(new Vector2Int(currentPos.x + 1, currentPos.y + 2));
            }


            if(currentPos.y > 1)  //down check
            {
                if (board[currentPos.x + 1, currentPos.y - 2].transform.childCount != 0)
                {
                    if (board[currentPos.x + 1, currentPos.y - 2].transform.GetComponentInChildren<Piece>().isWhite != this.isWhite)
                        moves.Add(new Vector2Int(currentPos.x + 1, currentPos.y - 2));
                }
                else
                    moves.Add(new Vector2Int(currentPos.x + 1, currentPos.y - 2));

            }
        }


        if (currentPos.x > 0)    //up/down left movement
        {

            if (currentPos.y < 6)  //up check
            {
                if (board[currentPos.x - 1, currentPos.y + 2].transform.childCount != 0)
                {
                    if (board[currentPos.x - 1, currentPos.y + 2].transform.GetComponentInChildren<Piece>().isWhite != this.isWhite)
                        moves.Add(new Vector2Int(currentPos.x - 1, currentPos.y + 2));
                }
                else
                    moves.Add(new Vector2Int(currentPos.x - 1, currentPos.y + 2));
            }


            if(currentPos.y > 1)  //down check
            {
                if (board[currentPos.x - 1, currentPos.y - 2].transform.childCount != 0)
                {
                    if (board[currentPos.x - 1, currentPos.y - 2].transform.GetComponentInChildren<Piece>().isWhite != this.isWhite)
                        moves.Add(new Vector2Int(currentPos.x - 1, currentPos.y - 2));
                }
                else
                    moves.Add(new Vector2Int(currentPos.x - 1, currentPos.y - 2));
            }
        }


        if (currentPos.x < 6)     //right and up/down movement
        {

            if (currentPos.y < 7) //up check
            {
                if (board[currentPos.x + 2, currentPos.y + 1].transform.childCount != 0)
                {
                    if (board[currentPos.x + 2, currentPos.y + 1].transform.GetComponentInChildren<Piece>().isWhite != this.isWhite)
                        moves.Add(new Vector2Int(currentPos.x + 2, currentPos.y + 1));
                }
                else
                    moves.Add(new Vector2Int(currentPos.x + 2, currentPos.y + 1));
            }

            if (currentPos.y > 0) //down check
            {
                if (board[currentPos.x + 2, currentPos.y - 1].transform.childCount != 0)
                {
                    if (board[currentPos.x + 2, currentPos.y - 1].transform.GetComponentInChildren<Piece>().isWhite != this.isWhite)
                        moves.Add(new Vector2Int(currentPos.x + 2, currentPos.y - 1));
                }
                else
                    moves.Add(new Vector2Int(currentPos.x + 2, currentPos.y - 1));
            }
        }


        if (currentPos.x > 1)    //left and up/down movement
        {

            if (currentPos.y < 7) //up check
            {
                if (board[currentPos.x - 2, currentPos.y + 1].transform.childCount != 0)
                {
                    if (board[currentPos.x - 2, currentPos.y + 1].transform.GetComponentInChildren<Piece>().isWhite != this.isWhite)
                        moves.Add(new Vector2Int(currentPos.x - 2, currentPos.y + 1));
                }
                else
                    moves.Add(new Vector2Int(currentPos.x - 2, currentPos.y + 1));
            }

            if (currentPos.y > 0) //down check
            {
                if (board[currentPos.x - 2, currentPos.y - 1].transform.childCount != 0)
                {
                    if (board[currentPos.x - 2, currentPos.y - 1].transform.GetComponentInChildren<Piece>().isWhite != this.isWhite)
                        moves.Add(new Vector2Int(currentPos.x - 2, currentPos.y - 1));
                }
                else
                    moves.Add(new Vector2Int(currentPos.x - 2, currentPos.y - 1));
            }
        }








        return moves;
    }
}
