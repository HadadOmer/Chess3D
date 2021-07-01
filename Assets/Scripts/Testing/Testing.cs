using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class Testing : MonoBehaviour
{
    string id = "4169977";
    bool testMode = true;

    private void Start()
    {
        Advertisement.Initialize(id, testMode);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Advertisement.IsReady())
            {
                print("ready");
                Advertisement.Show();
            }
        }
    }
}
