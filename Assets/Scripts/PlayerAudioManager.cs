using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    private AudioSource playerAudio;
    public AudioClip[] footsteps;
    private PlayerController playerController;

    public AudioClip[] swordSlash;
    public AudioClip[] swordHit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        playerController = GetComponent<PlayerController>();
    }

    public void PlayFootstep()
    {
        if(playerController.isGrounded)
        {
            playerAudio.pitch = Random.Range(0.7f, 1.3f);

            float footstepVolume = Random.Range(0.65f, 0.9f);
            int selectedFoostep = Random.Range(0, footsteps.Length);
            playerAudio.PlayOneShot(footsteps[selectedFoostep], footstepVolume);

        }

    }
    public void PlaySwordSlash()
    {
        playerAudio.pitch = Random.Range(0.7f, 1.3f);
        float slashVolume = Random.Range(0.65f, 0.9f);
        int selectedSlash = Random.Range(0, swordSlash.Length);
        playerAudio.PlayOneShot(swordSlash[selectedSlash], slashVolume);
        Debug.Log("Player sword slash");
    }
    public void PlaySwordHit()
    {
        playerAudio.pitch = Random.Range(0.7f, 1.3f);
        float hitVolume = Random.Range(0.5f, 0.6f);
        int selectedHit = Random.Range(0, swordHit.Length);
        playerAudio.PlayOneShot(swordHit[selectedHit], hitVolume);
        Debug.Log("Player sword slash");
    }

    // Update is called once per frame

}
