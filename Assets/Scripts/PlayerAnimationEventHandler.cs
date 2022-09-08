using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventHandler : MonoBehaviour
{
    public AudioClip[] footStepClips;

    private AudioSource animationAudioSource;

    // Start is called before the first frame update
    void Awake()
    {
        animationAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Footstep()
    {
        AudioClip clip = GetRandomClip(footStepClips);
        animationAudioSource.pitch = Random.Range(0.92f, 1.08f);
        animationAudioSource.PlayOneShot(clip);
    }

    AudioClip GetRandomClip(AudioClip[] clips)
    {
        return clips[UnityEngine.Random.Range(0, clips.Length)];
    }
}
