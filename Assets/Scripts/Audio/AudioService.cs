using UnityEngine;

public class AudioService : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Transform sourcesContainer;

    [SerializeField] private AudioClip warpSound;
    [SerializeField, Range(0, 1)] private float warpSoundStartTime;

    public void Warp()
    {
        CustomPlayOneShot(GetAudioSourceInstance(), warpSound, warpSoundStartTime);
    }

    private AudioSource GetAudioSourceInstance()
    {
        Transform container = (sourcesContainer != null) ? sourcesContainer : transform;
        return Instantiate(audioSource, container);
    }

    private void CustomPlayOneShot(AudioSource source, AudioClip clip, float startTime = 0)
    {
        source.clip = clip;
        source.time = clip.length * startTime;
        source.Play();

        Destroy(source.gameObject, clip.length - source.time * clip.length);
    }
}
