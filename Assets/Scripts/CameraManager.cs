using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera playerCamera;

    public float camDistance = -10f;

    public float camSpeed;

    private Vector3 camPosOffset;
    private Vector3 camPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         camPosOffset = new Vector3(0, 1, camDistance);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        camPos = playerCamera.transform.position;
        playerCamera.transform.position = Vector3.Lerp(camPos, transform.position + camPosOffset, Time.deltaTime * camSpeed);
        


    }
}
