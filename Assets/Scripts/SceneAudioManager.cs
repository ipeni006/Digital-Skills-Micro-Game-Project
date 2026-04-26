using UnityEngine;

public class SceneAudioManager : MonoBehaviour
{
    public AudioSource cameraAudio;
    public AudioClip[] backgroundSounds;
    public int startingAudio = 0;
    private int currentIndex;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayBackgroundAudio(startingAudio, 1);
        cameraAudio.loop = true;
        cameraAudio.Play();
    }

    public void PlayBackgroundAudio(int audioIndex, float volume)
    {
        if (audioIndex != currentIndex)
        {
            cameraAudio.Stop();
            cameraAudio.clip = backgroundSounds[audioIndex];
            cameraAudio.volume = volume;
            cameraAudio.Play();
            Debug.Log("Switched Audio");
            currentIndex = audioIndex;
        }
    }
    // Update is called once per frame

}
