using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public int points = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        points = PlayerPrefs.GetInt("points");
        //scene2();

    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10,10,100,20), "Score : " + points);
    }

    public void setPoints(int points) { PlayerPrefs.SetInt("points", points); print("Points: " + points); }


/*    private void scene2()
    {
        if (points >= 3 && SceneManager.GetActiveScene() != SceneManager.GetSceneByName("lvl-2")) SceneManager.LoadScene("lvl-2");

    }*/
}
