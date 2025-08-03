using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource sfxSource;

    public AudioClip buttonClickClip;
    public AudioClip[] hitClips;

    public void PlayButtonClick()
    {
        sfxSource.PlayOneShot(buttonClickClip);
    }

    public void PlayRandomHit()
    {
        int index = Random.Range(0, hitClips.Length);
        sfxSource.PlayOneShot(hitClips[index]);
    }
}