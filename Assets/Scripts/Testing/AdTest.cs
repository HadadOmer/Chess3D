using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdTest : MonoBehaviour
{
    string gameId = "4180155";
    bool testMode = true;
    void Start()
    {
        Advertisement.Initialize(gameId, testMode);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Advertisement.IsReady())
                Advertisement.Show();
        }
    }
}
