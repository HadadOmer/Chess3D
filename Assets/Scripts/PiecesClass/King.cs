using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
   public King(bool isWhite, Vector2Int currentPos) : base(isWhite, currentPos)
    {

    }

    public override List<Vector2Int> AvailableMovement(GameObject[,] board)
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        if (currentPos.y < 7)  // if the king can move up
        {


            if (board[currentPos.x, currentPos.y + 1].transform.childCount == 0)  // just up movement
                moves.Add(new Vector2Int(currentPos.x, currentPos.y + 1));
            else
            {
                if (board[currentPos.x, currentPos.y + 1].transform.GetComponentInChildren<Piece>().isWhite != this.isWhite)
                    moves.Add(new Vector2Int(currentPos.x, currentPos.y + 1));
            }




            if (currentPos.x < 7)  // if the king can move up and right
            {
                if (board[currentPos.x + 1, currentPos.y + 1].transform.childCount == 0)
                    moves.Add(new Vector2Int(currentPos.x + 1, currentPos.y + 1));
                else
                {
                    if (board[currentPos.x + 1, currentPos.y + 1].transform.GetComponentInChildren<Piece>().isWhite != this.isWhite)
                        moves.Add(new Vector2Int(currentPos.x + 1, currentPos.y + 1));
                }
            }




            if (currentPos.x > 0)  // if the king can move up and left
            {
                if (board[currentPos.x - 1, currentPos.y + 1].transform.childCount == 0)
                    moves.Add(new Vector2Int(currentPos.x - 1, currentPos.y + 1));
                else
                {
                    if (board[currentPos.x - 1, currentPos.y + 1].transform.GetComponentInChildren<Piece>().isWhite != this.isWhite)
                        moves.Add(new Vector2Int(currentPos.x - 1, currentPos.y + 1));
                }
            }
        }


        if (currentPos.y > 0)  // if the king can move down
        {



            if (board[currentPos.x, currentPos.y - 1].transform.childCount == 0)  //just down movement
                moves.Add(new Vector2Int(currentPos.x, currentPos.y - 1));
            else
            {
                if (board[currentPos.x, currentPos.y - 1].transform.GetComponentInChildren<Piece>().isWhite != this.isWhite)
                    moves.Add(new Vector2Int(currentPos.x, currentPos.y - 1));
            }


            if (currentPos.x < 7)    // if the king can move down and right
            {
                if (board[currentPos.x + 1, currentPos.y - 1].transform.childCount == 0)
                    moves.Add(new Vector2Int(currentPos.x + 1, currentPos.y - 1));

                else
                {
                    if (board[currentPos.x + 1, currentPos.y - 1].transform.GetComponentInChildren<Piece>().isWhite != this.isWhite)
                        moves.Add(new Vector2Int(currentPos.x + 1, currentPos.y - 1));
                }
            }


            if (currentPos.x > 0)   // if the king can move down and left
            {
                if (board[currentPos.x - 1, currentPos.y - 1].transform.childCount == 0)
                    moves.Add(new Vector2Int(currentPos.x - 1, currentPos.y - 1));

                else
                {
                    if (board[currentPos.x - 1, currentPos.y - 1].transform.GetComponentInChildren<Piece>().isWhite != this.isWhite)
                        moves.Add(new Vector2Int(currentPos.x - 1, currentPos.y - 1));
                }
            }
        }


        if (currentPos.x < 7)   //if the king can move right
        {
            if (board[currentPos.x + 1, currentPos.y].transform.childCount != 0)
            {
                if (board[currentPos.x + 1, currentPos.y].transform.GetComponentInChildren<Piece>().isWhite != this.isWhite)
                    moves.Add(new Vector2Int(currentPos.x + 1, currentPos.y));
            }
            else
                moves.Add(new Vector2Int(currentPos.x + 1, currentPos.y));                       
        }


        if(currentPos.x > 0)   //if the king can move left
        {
            if (board[currentPos.x - 1, currentPos.y].transform.childCount != 0)
            {
                if (board[currentPos.x - 1, currentPos.y].transform.GetComponentInChildren<Piece>().isWhite != this.isWhite)
                    moves.Add(new Vector2Int(currentPos.x - 1, currentPos.y));
            }
            else
                moves.Add(new Vector2Int(currentPos.x - 1, currentPos.y));
        }


        return moves;
    }
}
