using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generateMaze : MonoBehaviour
{

    int[,] worldMap;
    public GameObject wall;
    void Start()
    {
        worldMap = new int[,]
        {
            {1,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,1}
        };

        int i, j;
        for(i = 0; i < 10; i++)
        {
            for (j = 0; j < 10; j++) ;
            {
                GameObject t;
                if(worldMap [i,j] == 1)
                {
                    t = (GameObject)(Instantiate(wall, new Vector3(50 - i * 10, 1.5f, 50 - j * 10), Quaternion.identity));
                }
                }
            }
        }
    }