using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
  
    // Start is called before the first frame update
    void Start()
    {
        GetTile("A3");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GetTile(string name)
    {
        int x = (int)(name[0] - 65),
        y = int.Parse(name[1].ToString()) - 1;
        Debug.Log(name[0]);
        Debug.Log(name[1]);
        Debug.Log(x +"\n"+ y);
        //return boardTiles[x, y];
    }
}
