using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generateMaze : MonoBehaviour
{

    int[,] worldMap;
    public GameObject wall;
    Color[,] colorOfPixel;
    public Texture2D outlineImage;
    void Start()
    {
        //ReadFromArray();
        //ReadFromFile();
        ReadFromImage();
    }


    void ReadFromImage()
    {
        colorOfPixel = new Color[outlineImage.width, outlineImage.height];

        GameObject.Find("MiniMaze").transform.localScale = new Vector3(outlineImage.width * 10, 1, outlineImage.height * 10);
        for (int x=0; x < outlineImage.width; x++)
        {
            for (int y = 0; y < outlineImage.height; y++)
            {
                colorOfPixel[x, y] = outlineImage.GetPixel(x, y);
                if (colorOfPixel[x,y] != Color.white)
                {
                    GameObject t = (GameObject)(Instantiate(wall, new Vector3((outlineImage.width)-x*2 ,1, (outlineImage.height) -y*2 ), Quaternion.identity));
                }
            }

        }
    }


/*    void ReadFromFile()
    {
        TextAsset t1 = (TextAsset)Resources.Load("maze", typeof(TextAsset));

        string s = t1.text;
        s = s.Replace("\n", "");
        int i;
        for (i = 0; i < s.Length; i++)
        {
            if (s[i] == '1')
            {
                int column, row;
                column = i % 10;
                row = i / 10;
                GameObject t; 
                t = (GameObject)(Instantiate(wall, new Vector3(59.5f - column * 5, 203.5f, -199.5f - row * 3), Quaternion.identity));
                
            }
        }
    }*/

/*    void ReadFromArray()
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
            {1,1,1,1,1,1,1,1,1,1}
        };

        int i, j;
        for(i = 0; i < 10; i++)
        {
            for (j = 0; j < 10; j++)
            {
                GameObject t;
                if(worldMap [i,j] == 1)
                {
                    t = (GameObject)(Instantiate(wall, new Vector3(59.5f - i * 5, 203.5f, -199.5f - j * 5), Quaternion.identity));
                }
                }
            }
        }*/
}