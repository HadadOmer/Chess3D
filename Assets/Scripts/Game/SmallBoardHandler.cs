using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SmallBoardHandler : MonoBehaviour
{
    [Header("Objects")]
    public Transform smallBoardUIPanel;
    public Transform smallBoardNumbers;
    public Transform smallBoardLetters;
    public BoardManager gameEngine;

    [Header("Prefabs")]
    public GameObject tilePrefab;
    public RawImage smallBoardUI;
    public GameObject smallBoardNumbersBlack;
    public GameObject smallBoardLettersBlack;

    [Header("Materials")]
    public Material lightBrown;
    public Material darkBrown;

    
    GameObject[,] smallBoardTiles;
    AssetBundle icons;
    // Start is called before the first frame update
    void Start()
    {
        
        if(smallBoardTiles == null)
            CreateBoard();
        gameEngine = GameObject.Find("Board").GetComponent<BoardManager>();
        if (!gameEngine.isPlayerWhite)
            AdjustSmallBoardToBlack();
    }
    void AdjustSmallBoardToBlack()
    {
        //Adjusts the numbers on the small board
        Vector3 position = smallBoardNumbers.position;
        Quaternion rotation= smallBoardNumbers.rotation;
        Destroy(smallBoardNumbers.gameObject);
        smallBoardNumbers=Instantiate(smallBoardNumbersBlack, position, rotation, smallBoardUIPanel).transform;
        smallBoardNumbers.Rotate(new Vector3(0, 0, 180));

        //Adjust the letters on the small board
        position = smallBoardLetters.position;
        rotation = smallBoardLetters.rotation;
        Destroy(smallBoardLetters.gameObject);
        smallBoardLetters = Instantiate(smallBoardLettersBlack, position, rotation, smallBoardUIPanel).transform;
        smallBoardLetters.Rotate(new Vector3(0, 0, 180));
    }
    //Creates a basic chess board
    void CreateBoard()
    {
        smallBoardTiles = new GameObject[8, 8];
        Vector3 position;
        for (int x = 0; x < smallBoardTiles.GetLength(0); x++)
        {
            for (int y = 0; y < smallBoardTiles.GetLength(1); y++)
            {
                position = new Vector3(-8 + x * 2, 0, -8 + y * 2);
                smallBoardTiles[x, y] = Instantiate<GameObject>(tilePrefab, transform);
                smallBoardTiles[x, y].transform.localPosition = position;

                smallBoardTiles[x, y].GetComponent<MeshRenderer>().material = (x + y) % 2 == 0 ? lightBrown : darkBrown;
                smallBoardTiles[x, y].name = (char)(x + 65) + "," + (y + 1);

                smallBoardTiles[x, y].GetComponent<Tile>().matLocation = new Vector2Int(x, y);
                smallBoardTiles[x, y].GetComponent<Tile>().originalColor = (x + y) % 2 == 0 ? lightBrown : darkBrown;
            }
        }
    }

    //Resets the board to blank board
    private void ResetBoard()
    {
        if (smallBoardTiles == null)
            CreateBoard();
        for (int x = 0; x < smallBoardTiles.GetLength(0); x++)
        {
            for (int y = 0; y < smallBoardTiles.GetLength(1); y++)
            {
                if (smallBoardTiles[x, y].transform.childCount != 0)
                    Destroy(smallBoardTiles[x, y].transform.GetChild(0).gameObject);
            }
        }
    }

    public void DisplayPieces(GameObject[,] boardTiles,bool isWhite)
    {
        ResetBoard();

        if (icons == null)
        {
            icons = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "AssetsBundle/pieceicons"));
        }

        string pieceName;
        GameObject piece;

       
        for (int x = 0; x < smallBoardTiles.GetLength(0); x++)
        {
            for (int y = 0; y < smallBoardTiles.GetLength(1); y++)
            {
                if(boardTiles[x,y].transform.childCount!=0)
                {
                    pieceName = boardTiles[x, y].transform.GetChild(0).name;
                    pieceName += boardTiles[x, y].GetComponentInChildren<Piece>().isWhite ? "White" : "Black";
                    piece = icons.LoadAsset<GameObject>(pieceName);
                    piece=Instantiate(piece);

                    //Moves the icon to it's parent location
                    piece.transform.parent = smallBoardTiles[x, y].transform;
                    piece.transform.position = piece.transform.parent.position;

                    //Adjusts the piece icon to the board
                    piece.transform.localPosition = new Vector3(0, 1, 0);
                    piece.transform.localEulerAngles = new Vector3(90,isWhite? 0:180, 0);
                    piece.transform.localScale = new Vector3(0.2f, 0.2f, 0);

                    //Adjusts the board to the player view
                    smallBoardUI.transform.eulerAngles=new Vector3(0, 0, isWhite ? 0 : 180);
                }
            }
        }

        icons.Unload(false);
    }
}
