using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
           
    }

    // âπÇèoÇ∑ÇΩÇﬂÇÃä÷êî
    public void PlayAudio(AudioClip audioClip, float volume)
    {
        if (audioSource != null && audioSource.enabled)
        {
            audioSource.volume = volume;
            audioSource.PlayOneShot(audioClip);
        }
        else
        {
            Debug.LogWarning("AudioSource is not available or enabled.");
        }
    }
}
