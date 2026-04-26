using UnityEngine;

public class SceneAudioVolume : MonoBehaviour
{
    public GameObject audioManager;
    public int audioClip;
    public int volume;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioManager = GameObject.Find("SceneAudioManager");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            audioManager.GetComponent<SceneAudioManager>().PlayBackgroundAudio(audioClip, volume);
        }
    }
}
