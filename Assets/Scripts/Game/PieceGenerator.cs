
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

public class PieceGenerator : MonoBehaviour
{
    public AssetBundle team1;
    public AssetBundle team2;

    private void Start()
    {
        
    }
    public void CreateStartPawns(BoardManager script)
    {
        Debug.Log("Check");
        team1 = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "AssetsBundle/zombies"));
        team2 = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "AssetsBundle/cops"));
             
        GameObject temp;
        Vector3 position;
        string[] names = new string[5] { "Rook", "Knight", "Bishop", "Queen", "King" };
        string name;
        for (int i = 0; i < script.boardTiles.GetLength(0); i++)
        {
            //Loads the pawns
            //Team1
            position = script.boardTiles[i, 1].transform.position;
            temp = Instantiate(LoadPiece("Pawn", 1),position,Quaternion.identity) ;
            temp.transform.parent = script.boardTiles[i, 1].transform;
            temp.name = "Pawn";

            temp.GetComponent<Piece>().isWhite = true;
            temp.GetComponent<Piece>().currentPos = new Vector2Int(i, 1);

            temp.AddComponent<NavMeshAgent>();

            //Team2
            position = script.boardTiles[i, 6].transform.position;
            temp =Instantiate(LoadPiece("Pawn", 2), position, Quaternion.Euler(0, 180, 0));
            temp.transform.parent = script.boardTiles[i, 6].transform;
            temp.name = "Pawn";

            temp.GetComponent<Piece>().isWhite = false;
            temp.GetComponent<Piece>().currentPos = new Vector2Int(i, 6);

            temp.AddComponent<NavMeshAgent>();

            //Loads the relevant piece
            if (i == 0 || i == 7)
                name = "Rook";
            else if (i == 1 || i == 6)
                name = "Knight";
            else if (i == 2 || i == 5)
                name = "Bishop";
            else if (i == 3)
                name = "Queen";
            else
                name = "King";
            //Team1
            position = script.boardTiles[i, 0].transform.position;
            temp = Instantiate(LoadPiece(name, 1), position, Quaternion.identity);
            temp.transform.parent = script.boardTiles[i, 0].transform;
            temp.name = name;

            temp.GetComponent<Piece>().isWhite = true;
            temp.GetComponent<Piece>().currentPos = new Vector2Int(i, 0);

            temp.AddComponent<NavMeshAgent>();

            //Team2
            position = script.boardTiles[i, 7].transform.position;
            temp = Instantiate(LoadPiece(name, 2), position, Quaternion.Euler(0,180,0));
            temp.transform.parent = script.boardTiles[i, 7].transform;
            temp.name = name;

            temp.GetComponent<Piece>().isWhite = false;
            temp.GetComponent<Piece>().currentPos = new Vector2Int(i, 7);

            temp.AddComponent<NavMeshAgent>();
        }


        team1.Unload(false);
        team2.Unload(false);

        script.clientLoaded = true;
        script.ReadyToStart();
        Debug.Log("Loaded");
    }
    GameObject LoadPiece(string name,int team)
    {
        return team == 1 ? team1.LoadAsset<GameObject>(name) :
                           team2.LoadAsset<GameObject>(name);
    }
}
