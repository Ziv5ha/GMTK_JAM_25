using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource[] musicLayers;

    void Start()
    {
        // Start all music layers simultaneously
        foreach (AudioSource layer in musicLayers)
        {
            layer.Play();
        }
    }

    public void SetLayerVolume(int layerIndex, float volume)
    {
        if (layerIndex >= 0 && layerIndex < musicLayers.Length)
        {
            musicLayers[layerIndex].volume = volume;
        }
    }

    public void FadeLayerVolume(int layerIndex, float targetVolume, float duration)
    {
        if (layerIndex >= 0 && layerIndex < musicLayers.Length)
        {
            StartCoroutine(FadeVolumeRoutine(musicLayers[layerIndex], targetVolume, duration));
        }
    }

    private IEnumerator FadeVolumeRoutine(AudioSource source, float targetVolume, float duration)
    {
        float startVolume = source.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
            yield return null;
        }
        source.volume = targetVolume;

    }


    public void ResetMusic()
    {
        for (int i = 0; i < musicLayers.Length; i++)
        {
            musicLayers[i].volume = (i == 0) ? 1f : 0f;
        }
    }
}