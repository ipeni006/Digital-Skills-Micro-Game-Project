using UnityEngine;
using UnityEngine.InputSystem;

public class DoorTeleport : MonoBehaviour
{
    public GameObject linkedDoor;
    public string playerTag = "Player";

    public bool playerInRange = false;
    public Transform playerTransform;
    private PlayerInput playerInput;
    private InputAction interactAction;

    private GameObject player;

    private PlayerController playerControllerScript;

    void Start()
    {
        player = GameObject.Find("Player");
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (playerInRange && Keyboard.current.eKey.wasPressedThisFrame)
        {
            TeleportPlayer();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = true;
            playerTransform = other.transform;

            // Grab the InputAction from the player's PlayerInput component
            playerInput = other.GetComponentInParent<PlayerInput>();

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = false;
            playerTransform = null;
            playerInput = null;
            interactAction = null;
        }
    }

    void TeleportPlayer()
    {
        CharacterController cc = playerTransform.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
            player.transform.position = linkedDoor.transform.position;
            cc.enabled = true;
        }
        playerInRange = false;
        Debug.Log("DOOR");
    }
}

