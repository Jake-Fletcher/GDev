using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class Col : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "FPSController")
        {
            other.GetComponent<PlayerScript>().setPoints(PlayerPrefs.GetInt("points")+1);
            StartCoroutine(pickUpHandler(gameObject));
        }
        
    }

    IEnumerator pickUpHandler(GameObject item)
    {
        item.transform.GetChild(0).gameObject.SetActive(false);
        item.GetComponent<MeshRenderer>().enabled = false;
        item.GetComponent<SphereCollider>().enabled = false;
        item.GetComponent<AudioSource>().PlayOneShot(item.GetComponent<AudioSource>().clip);
        yield return new WaitForSecondsRealtime(2f);
        Destroy(item);
    }

}

