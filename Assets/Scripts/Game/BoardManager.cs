using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Realtime;

public class BoardManager : MonoBehaviour
{
    [Header("Objects")]
    PhotonView PV;
    PieceGenerator pieceGenerator;
    NetworkManager network;
    UserHandler user;
    DBGameMoves dbGameMoves;

    public Text threatText;
    public Text gameOverText;
    public Button voiceBtn;

    public TimeSpan playerTime;
    public Image playerTimerImage;
    public Text playerTimerText;

    public TimeSpan opponentTime;
    public Image opponentTimerImage;
    public Text opponentTimerText;

    public Camera mainCamera;
    public SmallBoardHandler smallBoard;
    public GameObject board;
    public GameObject[,] boardTiles;

    [Header("Prefabs")]
    public GameObject tilePrefab;

    [Header("Eaten Pieces Locations")]
    public Transform whiteEaten;
    public Transform blackEaten;

    [Header("Materials")]
    public Material white;
    public Material black;
    public Material green;
    public Material yellow;

    
    [Header("Tile Selection")]
    private RaycastHit hit;
    Vector2Int selectedTile;
  
    [Header("Game loaded state")]
    public bool clientLoaded;
    public bool otherClientLoaded;

    [Header("Client Values")]

    public bool isPlayerWhite;
    private bool turn;
    private Piece playersKing;

    // Start is called before the first frame update
    void Awake()
    {
        LoadGame();
        StartCoroutine(AfterAssetLoad());
       
    }
  
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && turn&&
            clientLoaded&&otherClientLoaded)
        {
            PickTile();
        }
        playerTimerText.text = playerTime.Minutes.ToString() + ":" + playerTime.Seconds.ToString();
        opponentTimerText.text = opponentTime.Minutes.ToString() + ":" + opponentTime.Seconds.ToString();
    }



   
    //Start Functions
    #region
    //Creates the tiles of the chess board
    public void LoadGame()
    {
        playerTime = new TimeSpan(0, 3, 30);
        opponentTime = new TimeSpan(0, 3, 30);

        playerTimerImage.color = Color.white;
        opponentTimerImage.color = Color.black;

        playerTimerText.color = Color.black;
        opponentTimerText.color = Color.white;

        isPlayerWhite = PhotonNetwork.IsMasterClient;
        network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

        PV = GetComponent<PhotonView>();

        user = GameObject.Find("UserHandler").GetComponent<UserHandler>();

        CreateChessBoard();
        pieceGenerator = transform.GetComponent<PieceGenerator>();
        pieceGenerator.CreateStartPawns(this);
    }
    public IEnumerator AfterAssetLoad()
    {
        while (!clientLoaded || !otherClientLoaded)
            yield return null;

        dbGameMoves = GameObject.Find("DBGameMoves").GetComponent<DBGameMoves>();
        selectedTile = new Vector2Int(-1, -1);
      
        playersKing = FindKing(isPlayerWhite);
        turn = isPlayerWhite;

        //The voice control is currently supported on android only
        if (Application.platform == RuntimePlatform.Android)
            voiceBtn.gameObject.SetActive(turn);
        else
            voiceBtn.gameObject.SetActive(false);

        //Updates the small board layout
        smallBoard.DisplayPieces(boardTiles, isPlayerWhite);
        AdjustCamera();
        StartCoroutine(TakeDownClock());
    }
    void CreateChessBoard()
    {
        Vector3 position;
        boardTiles = new GameObject[8, 8];
        for (int i = 0; i < boardTiles.GetLength(0); i++)
        {
            for (int j = 0; j < boardTiles.GetLength(1); j++)
            {
                position = new Vector3(-8 + i * 2,0, -8 + j * 2);
                boardTiles[i, j] = Instantiate<GameObject>(tilePrefab, position,Quaternion.identity, board.transform);
                boardTiles[i, j].GetComponent<MeshRenderer>().material = (i+j)%2==0?white:black;
                boardTiles[i, j].name = (char)(i+65) + "" + (j+1);

                boardTiles[i, j].GetComponent<Tile>().matLocation = new Vector2Int(i, j);
                boardTiles[i, j].GetComponent<Tile>().originalColor = (i + j) % 2 == 0 ? white : black;
            }
        }
    }
    private Piece FindKing(bool isPlayerWhite)
    {
        if (isPlayerWhite)
            return boardTiles[4, 0].transform.GetChild(0).GetComponent<Piece>();
        else
            return boardTiles[4, 7].transform.GetChild(0).GetComponent<Piece>();
    }
    public IEnumerator TakeDownClock()
    {
        while (turn)
        {
            yield return new WaitForSeconds(1);
            playerTime = playerTime.Subtract(new TimeSpan(0, 0, 1));
            PV.RPC("UpdateClock", RpcTarget.Others, playerTime.Seconds, playerTime.Minutes);
        }
    }
    [PunRPC]
    public void UpdateClock(int seconds, int minutes)
    {
        opponentTime = new TimeSpan(0, minutes, seconds);
    }
    void AdjustCamera()
    {
        if (!isPlayerWhite)
        {
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, -mainCamera.transform.position.z);
            mainCamera.transform.Rotate(Vector3.up, 180f, Space.World);
        }
    }

    public void ReadyToStart()
    {
        PV.RPC("SendReadyToStart", RpcTarget.Others);
    }
    [PunRPC]
    public void SendReadyToStart()
    {
        otherClientLoaded = true;
    }
    #endregion


    //Tiles Functions
    #region 
    //Paints the avalivable moves in green
    void HighlightMoves(List<Vector2Int> movesPositions)
    {
        int x, y;
        for (int i = 0; i < movesPositions.Count; i++)
        {
            x = movesPositions[i].x;
            y = movesPositions[i].y;
            boardTiles[x, y].GetComponent<MeshRenderer>().material = green;
        }
    }

    //Paints the tiles in their original color
    void UnHighlightMoves(List<Vector2Int> movesPositions)
    {
        int x, y;
        for (int i = 0; i < movesPositions.Count; i++)
        {
            x = movesPositions[i].x;
            y = movesPositions[i].y;
            Material originalColor=boardTiles[x, y].GetComponent<Tile>().originalColor;
            boardTiles[x, y].GetComponent<MeshRenderer>().material = originalColor;
        }
    }

    void HighlightTile(RaycastHit hit)
    {
       
            //Higlights the selected tile in yellow
            hit.collider.GetComponent<Renderer>().material = yellow;
            selectedTile = hit.collider.GetComponent<Tile>().matLocation;

            //Highlights the selected moves if there is a piece in the tile
            Piece piece = hit.collider.GetComponentInChildren<Piece>();
            if (piece != null && piece.isWhite == isPlayerWhite)
                HighlightMoves(piece.AvailableMovement(boardTiles));
    }
    void HighlightTile(Transform tile)
    {

        //Higlights the selected tile in yellow
        tile.GetComponent<Renderer>().material = yellow;
        selectedTile = tile.GetComponent<Tile>().matLocation;

        //Highlights the selected moves if there is a piece in the tile
        Piece piece = tile.GetComponentInChildren<Piece>();
        if (piece != null && piece.isWhite == isPlayerWhite)
            HighlightMoves(piece.AvailableMovement(boardTiles));
    }
    void CancelTileHighlight()
    {
        Material original = boardTiles[selectedTile.x, selectedTile.y].GetComponent<Tile>().originalColor;
        boardTiles[selectedTile.x, selectedTile.y].GetComponent<Renderer>().material = original;

        //Cansels the selected moves highlight if there is a piece in the tile
        Piece piece = boardTiles[selectedTile.x, selectedTile.y].GetComponentInChildren<Piece>();
        if (piece != null)
            UnHighlightMoves(piece.AvailableMovement(boardTiles));
    }
    public void PickTile()
    {
        Ray ray;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //if a green tile is selected moves the piece
        if (Physics.Raycast(ray, out hit) && hit.collider.tag == "Tile"
            && hit.collider.GetComponent<Renderer>().material.name == green.name + " (Instance)")
        {           
            MovePiece(hit.collider.GetComponent<Tile>().matLocation);
            CancelTileHighlight();
            return;
        }
             
        //Highlights the selected tile in yellow
        if (Physics.Raycast(ray, out hit) && hit.collider?.tag == "Tile"&&
            hit.collider.GetComponent<Renderer>().material != yellow)
        {
            //Cancels the highlight of the selected tile
            if (selectedTile != new Vector2Int(-1, -1))
                CancelTileHighlight();                  
            HighlightTile(hit);
        }                   
      /*  else
            selectedTile = new Vector2Int(-1, -1);*/
    }
    public void VoicePickTile(string name)
    {
        Transform tile = GetTile(name).transform;
        //if a green tile is selected moves the piece
        if (tile.GetComponent<Renderer>().material.name == green.name + " (Instance)")
        {
            MovePiece(tile.GetComponent<Tile>().matLocation);
            CancelTileHighlight();
            return;
        }

        //Highlights the selected tile in yellow
        if (tile.GetComponent<Renderer>().material != yellow)
        {
            //Cancels the highlight of the selected tile
            if (selectedTile != new Vector2Int(-1, -1))
                CancelTileHighlight();
            HighlightTile(tile);
        }
    }
    public GameObject GetTile(string name)
    {
        int x = (int)(name[0] - 65),
        y = int.Parse(name[1].ToString()) - 1;
        return boardTiles[x, y];
    }
    #endregion


    //Piece Functions
    #region 
    public void MovePiece(Vector2Int newTile)
    {
        //Cansels the selected moves highlight if the piece moves
        Piece piece = boardTiles[selectedTile.x, selectedTile.y].GetComponentInChildren<Piece>();
        UnHighlightMoves(piece.AvailableMovement(boardTiles));

        //Moves the piece to the desired tile
        PV.RPC("ServerMovePiece", RpcTarget.All,boardTiles[newTile.x, newTile.y].transform.name, boardTiles[selectedTile.x, selectedTile.y].transform.name,false);
    }
    [PunRPC]
    void ServerMovePiece(string newTileString, string currentTileString,bool isKingCastleMove)
    {
        Transform newTile;
        newTile = GameObject.Find(newTileString).transform;
        Transform currentTile;
        currentTile = GameObject.Find(currentTileString).transform;

        Transform piece = currentTile.transform.GetChild(0);

        //Moves the eaten piece aside or castles
        if (newTile.transform.childCount != 0)
        {
            Transform otherPiece = newTile.GetChild(0);
            Rook rook = piece.GetComponent<Rook>();

            //Castles if the piece in the tile to move is a king
            if (rook!=null && otherPiece.GetComponent<King>() != null)
            {
                newTile=Castle(rook.currentPos, otherPiece);
            }
            else
            {                
                EatPiece(piece,otherPiece);
            }
        }

        //Moves the piece and changes its parent to the new tile       
        piece.GetComponent<NavMeshAgent>().SetDestination(newTile.transform.position);
        piece.parent = newTile;
        piece.GetComponent<Piece>().ChangePosition( newTile.GetComponent<Tile>().matLocation);



        //Moves the turn only if its a regular move and not a castle
        if (!isKingCastleMove)
            StartCoroutine(SwitchTurn(piece));

       

        //Updates the small board layout
        smallBoard.DisplayPieces(boardTiles, isPlayerWhite);

        //Checks for threats on king
        if (turn == isPlayerWhite)
            if (!CheckKingSafe(playersKing.currentPos, GetEnemyPieces(isPlayerWhite)))
                threatText.text = "Check";
            else
                threatText.text = "";

        //Adds the move on the database
        if (PhotonNetwork.IsMasterClient)
            AddMoveToDatabase(currentTile.name + newTile.name);
    }
    //Adds a move in the database *this function supposed to run only on master client*
    public void AddMoveToDatabase(string move)
    {
        dbGameMoves.AddMove(move);
    }
    [PunRPC]
    void EatPiece(Transform eatingPiece,Transform eatenPiece)
    {
        //The game ends when the king is eaten
        if (eatenPiece.name == "King")
            StartCoroutine(EndGame(eatenPiece));

        //Triggers the attack animation
        StartCoroutine(TriggerFightAnimation(eatingPiece, eatenPiece));
              
    }
    IEnumerator MovePieceAside(Transform piece)
    {
        yield return new WaitForSeconds(1f);
        //Destroys the nav mesh agent to prevent it from moving after death
        Destroy(piece.GetComponent<NavMeshAgent>());

        //Puts the piece in it's team location
        bool isWhite = piece.GetComponent<Piece>().isWhite;
        piece.parent = isWhite ? whiteEaten : blackEaten;
        
        Vector3 location = new Vector3(0, 0, isWhite ? whiteEaten.childCount : -blackEaten.childCount);
        piece.position = piece.parent.position + location;
    }

    //Moves the king to its position in the castle and return the rook tile's transform afther the castle
    [PunRPC]
    Transform Castle(Vector2Int rookLocation,Transform king)
    {
        //Moves the king to its position in the castle
        Vector2Int kingNewPosition;
        int kingY = king.GetComponent<King>().currentPos.y;
        kingNewPosition = new Vector2Int(rookLocation.x == 0 ? 2 : 6, kingY);
        ServerMovePiece(boardTiles[kingNewPosition.x, kingNewPosition.y].name, king.parent.name,true);

        //Defines the new rook location after the castle
        Vector2Int rookNewLocation = new Vector2Int(rookLocation.x == 0 ? 3 : 5, kingY);
        return boardTiles[rookNewLocation.x, rookNewLocation.y].transform;
    }

    IEnumerator TriggerFightAnimation(Transform attacker,Transform defender)
    {
        yield return new WaitForSeconds(0.2f);
      
        yield return new WaitUntil(() => !attacker.GetComponent<Animator>().GetBool("isWalking"));
        
        //Triggers the animations
        attacker.GetComponent<Animator>().SetTrigger("Attack");
        defender.GetComponent<Animator>().SetTrigger("Death");

        //Plays the death sound
        if (defender.GetComponent<AudioSource>()!=null)
            defender.GetComponent<AudioSource>().Play();


        //Moves the defender to the eaten pieces location
        StartCoroutine(MovePieceAside(defender));
    }

    //Switches turn only after the piece finished moving
    IEnumerator SwitchTurn(Transform piece)
    {
        yield return new WaitForSeconds(0.2f);

        yield return new WaitUntil(() => !piece.GetComponent<Animator>().GetBool("isWalking"));

        turn = !turn;

        //The voice control is currently supported on android only
        if (Application.platform== RuntimePlatform.Android)
            voiceBtn.gameObject.SetActive(turn);
        else
            voiceBtn.gameObject.SetActive(false);

        StartCoroutine(TakeDownClock());
        
    }

    
    #endregion

    //End Functions
    #region 
    private IEnumerator EndGame(Transform king)
    {
        bool winnerWhite = !king.GetComponent<Piece>().isWhite;

        //Displays the winner in the text alert
        gameOverText.text = winnerWhite ? "White Wins!" : "Black Wins!";
        
        //Updates the player's stats if he is logined
        if(user.isLogined)
        {
            user.AddXP(winnerWhite == isPlayerWhite ? 20 : 10);
            if (winnerWhite == isPlayerWhite)
                user.AddWins(1);
            else
                user.AddLoses(1);
        }
        //Inserts the doc with the game data to the database
        if (PhotonNetwork.IsMasterClient)
            dbGameMoves.InsertGameRecord();

        yield return new WaitForSeconds(5);

        PhotonNetwork.LeaveRoom(true);
    }

    public void LeaveGame()
    {
        PV.RPC("ServerLeaveGame", RpcTarget.Others, user.username);
        
        PhotonNetwork.LeaveRoom(true);
    }
    [PunRPC]
    public void ServerLeaveGame(string playerLeavingName)
    {
        gameOverText.text = playerLeavingName + " Left";
        StartCoroutine(network.ForceExit(2));
    }
    #endregion


    //returns all enemy pieces
    private List<Piece> GetEnemyPieces(bool isPlayerWhite)
    {
        List<Piece> pieces = new List<Piece>();
        Piece tempPiece;
        for (int x = 0; x < boardTiles.GetLength(0); x++)
        {
            for (int y = 0; y < boardTiles.GetLength(1); y++)
            {
                tempPiece = boardTiles[x, y].GetComponentInChildren<Piece>();
                if (tempPiece != null && tempPiece.isWhite != isPlayerWhite)
                    pieces.Add(tempPiece);
            }
        }
        return pieces;
    }


    //checks if the king is threatened
    private bool CheckKingSafe(Vector2Int kingsPosition, List<Piece> enemyPieces)
    {
        for (int i = 0; i < enemyPieces.Count; i++)
        {
            if (enemyPieces[i].AvailableMovement(boardTiles).Contains(kingsPosition))
            {
                return false;
            }
        }
        return true;
    }

    
}


public class testCode
{
   /* private bool MateCheck(List<Piece> enemyPieces)
   {
       List<Vector2Int> availableMoves = playersKing.AvailableMovement(boardTiles);
       int counter = 0;
       for (int i = 0; i < availableMoves.Count; i++)
       {
           if (!CheckKingSafe(availableMoves[i], enemyPieces))
               counter++;
       }
       if (counter == availableMoves.Count)
           print("mate");
       return counter == availableMoves.Count;
   }
    private IEnumerator FindKingDelay()
  {
      yield return new WaitForSeconds(1);
      playersKing = FindKing(isPlayerWhite);
  }
    private bool CheckKingSafe(Vector2Int kingsPosition , List<Piece> enemyPieces)
     {        
         for (int i = 0; i < enemyPieces.Count; i++)
         {
             if (enemyPieces[i].AvailableMovement(boardTiles).Contains(kingsPosition))
             {
                return false;
             }
         }
         return true;
     }
    private List<Piece> GetEnemyPieces(bool isPlayerWhite)
    {
        List<Piece> pieces = new List<Piece>();
        Piece tempPiece;
        for (int x = 0; x < boardTiles.GetLength(0); x++)
        {
            for (int y = 0; y < boardTiles.GetLength(1); y++)
            {
                tempPiece = boardTiles[x, y].GetComponentInChildren<Piece>();
                if (tempPiece != null && tempPiece.isWhite != isPlayerWhite)
                    pieces.Add(tempPiece);
            }
        }
        return pieces;
    }
   */
}