using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Collecting : MonoBehaviour
{
    // Start is called before the first frame update
    int score;
    void Start()
    {
        Debug.Log("Hello world");
        //GameObject.Find("userMessage").GetComponent<Text>().text = "Score:"+ score;
        displayScore();
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log("hello World from Update");
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        GameObject other = hit.gameObject;
        Debug.Log("Just collided with " + other.name);
        //if (other.tag == "pick_me") Destroy(other);
        if (other.CompareTag("pick_me"))
        {
            Destroy(other);
            score++;
            //GameObject.Find("userMessage").GetComponent<Text>().text = "Score:" + score;
            displayScore();
            Debug.Log("Score:" + score);
            if (score >= 2) SceneManager.LoadScene("level2");
        }
    }
    void displayScore()
    {
        GameObject.Find("userMessage").GetComponent<Text>().text = "Score:" + score;
    }
}