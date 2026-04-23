using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public bool hasKey = false;
    public string playerTag = "Player";
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            hasKey = true;
            transform.position = new Vector3(180, 1.38f, 175);
        }
    }
}
