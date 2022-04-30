using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    // Contains all the data necessary for the game to work.
    public static Data instance;

    public string currentScene;

    public Data()
    {
        instance = this;

        currentScene = "FirstIsland";
    }
}
