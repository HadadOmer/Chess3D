using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ReplayEngine : MonoBehaviour
{
    [Header("Objects")]
    public PieceData[,] pieces;
    GameObject[,] smallBoardTiles;
    public GameReplayUiHandler ui;

    [Header("Prefabs")]
    public GameObject tilePrefab;

    [Header("Materials")]
    public Material lightBrown;
    public Material darkBrown;

    
    AssetBundle icons;
    public List<string> moves;

    // Start is called before the first frame update
    void Start()
    {
        if (smallBoardTiles == null)
            CreateBoard();
        pieces = GenerateStartPiecesLayout();
        DisplayPieces(pieces);

        //Gets the game moves from database based on the game number from the user handler
        UserHandler userHandler = GameObject.Find("UserHandler").GetComponent<UserHandler>();
        moves = GameReplayDBHandler.GetGameMoves(userHandler.gameReplayNumber);

        StartCoroutine(SimulateGame(1));
    }
    //Start Functions
    #region
    public PieceData[,] GenerateStartPiecesLayout()
    {
        PieceData[,] pieces = new PieceData[8, 8];
        for (int i = 0; i < 8; i++)
        {
            //Initializes white pawns
            pieces[i, 1] = new PieceData("Pawn",true);
            //Initializes black pawns
            pieces[i, 6] = new PieceData("Pawn", false);

            //Initializes Rook
            if(i==0||i==7)
            {
                pieces[i, 0] = new PieceData("Rook", true);
                pieces[i, 7] = new PieceData("Rook", false);
            }
            //Initializes Knight
            else if (i == 1 || i == 6)
            {
                pieces[i, 0] = new PieceData("Knight", true);
                pieces[i, 7] = new PieceData("Knight", false);
            }
            //Initializes Bishop
            else if (i == 2 || i == 5)
            {
                pieces[i, 0] = new PieceData("Bishop", true);
                pieces[i, 7] = new PieceData("Bishop", false);
            }
            //Initializes Queen
            else if (i == 3)
            {
                pieces[i, 0] = new PieceData("Queen", true);
                pieces[i, 7] = new PieceData("Queen", false);
            }
            //Initializes King
            else
            {
                pieces[i, 0] = new PieceData("King", true);
                pieces[i, 7] = new PieceData("King", false);
            }
        }
        return pieces;
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

    public void DisplayPieces(PieceData[,] pieces)
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
                if (pieces[x, y] !=null)
                {
                    pieceName = pieces[x, y].type;
                    pieceName += pieces[x, y].isWhite ? "White" : "Black";
                    piece = icons.LoadAsset<GameObject>(pieceName);
                    piece = Instantiate(piece);

                    //Moves the icon to it's parent location
                    piece.transform.parent = smallBoardTiles[x, y].transform;
                    piece.transform.position = piece.transform.parent.position;

                    //Adjusts the piece icon to the board
                    piece.transform.localPosition = new Vector3(0, 1, 0);
                    piece.transform.localEulerAngles = new Vector3(90,0, 0);
                    piece.transform.localScale = new Vector3(0.2f, 0.2f, 0);                 
                }
            }
        }

        icons.Unload(false);
    }
    #endregion

    //Converts from letter number format to postion in the mat for example: "A1"=>{0,0}
    public Vector2Int ToMatLocation(string location)
    {
        //Format validation
        #region
        //Checks the length is valid
        if (location.Length != 2)
        {
            Debug.LogError("Location needs to be 2 chars long");
            return new Vector2Int(-1, -1);
        }

        location=location.ToUpper();
        char letter = location[0];
        int num = int.Parse(location[1].ToString());

        //Checks letter between A-H in the abc
        if ((int)letter<65|| (int)letter > 72)
        {
            Debug.LogError("The letter needs to be between A-H in the abc");
            return new Vector2Int(-1, -1);
        }
        //Checks number between 1-8
        if (num<1||num>8)
        {
            Debug.LogError("The number needs to be between 1-8");
            return new Vector2Int(-1, -1);
        }
        #endregion

        return new Vector2Int((int)(letter - 65), num - 1);
    }

    //Performs a move base on the string
    public void PerformMove(string move)
    {
        if(move.Length!=4)
        {
            Debug.LogError("Move needs to be 4 chars long");
            return;
        }
        Vector2Int start = ToMatLocation(move[0] + "" + move[1]),
            end = ToMatLocation(move[2] + "" + move[3]);

        pieces[end.x,end.y]=pieces[start.x, start.y];
        pieces[start.x, start.y] = null;
    }

    IEnumerator SimulateGame(float delay)
    {
        for (int i = 0; i < moves.Count; i++)
        {
            yield return new WaitForSeconds(delay);
            PerformMove(moves[i]);
            DisplayPieces(pieces);
        }
        ui.endText.text="Replay ended";
        yield return new WaitForSeconds(5);
    }
}
public class PieceData
{
    public string type;
    public bool isWhite;

    public PieceData(string type,bool isWhite)
    {
        this.type = type;
        this.isWhite = isWhite;
    }
}
