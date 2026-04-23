using System.Data;
using UnityEngine;

public class DoorLocked : MonoBehaviour
{
    private KeyScript keyScript;

    public string playerTag = "Player";

    public bool playerTouches = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        keyScript = GameObject.Find("Key").GetComponent<KeyScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (keyScript.hasKey && playerTouches) 
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag))
        { 
            playerTouches = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerTouches = false;
        }
    }
}