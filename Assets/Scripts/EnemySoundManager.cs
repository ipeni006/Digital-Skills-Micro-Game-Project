using UnityEngine;

public class EnemySoundManager : MonoBehaviour
{
    private AudioSource enemyAudio;
    public AudioClip[] creaks;
    public AudioClip[] thuds;
    private PlayerController playerController;

    public AudioClip[] swordSlash;
    public AudioClip[] swordHit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyAudio = GetComponent<AudioSource>();
        playerController = GetComponent<PlayerController>();
    }

    public void PlayCreak()
    {


            enemyAudio.pitch = Random.Range(0.7f, 1.3f);

            float volume = Random.Range(0.65f, 0.9f);
            int selection = Random.Range(0, creaks.Length);
            enemyAudio.PlayOneShot(creaks[selection], volume);

    }
    public void PlayThud()
    {

            enemyAudio.pitch = Random.Range(0.7f, 1.3f);

            float volume = Random.Range(0.65f, 0.9f);
            int selection = Random.Range(0, thuds.Length);
            enemyAudio.PlayOneShot(thuds[selection], volume);

    }
}
