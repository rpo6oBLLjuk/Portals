using UnityEngine;

public class AudioService : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Transform sourcesContainer;

    [SerializeField] private AudioClip warpSound;
    [SerializeField] private float warpSoundStartTime;

    public void Warp()
    {
        CustomPlayOneShot(GetAudioSourceInstance(), warpSound, warpSoundStartTime);
    }

    private AudioSource GetAudioSourceInstance()
    {
        Transform container = (sourcesContainer != null) ? sourcesContainer : null;
        return Instantiate(audioSource, container);
    }

    private void CustomPlayOneShot(AudioSource source, AudioClip clip, float startTime = 0)
    {
        source.clip = clip;
        source.time = clip.length * startTime;
        source.Play();

        Destroy(source, source.time);
    }
}
