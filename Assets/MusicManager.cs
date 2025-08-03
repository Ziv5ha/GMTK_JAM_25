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
}