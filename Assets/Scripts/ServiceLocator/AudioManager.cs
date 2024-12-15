using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IAudioManager
{
    private Dictionary<string, AudioClip> audioClips;
    private AudioSource audioSource;
    private Coroutine soundCoroutine;

    public void Initialize(AudioSource source, Dictionary<string, AudioClip> clips)
    {
        audioSource = source;
        audioClips = clips;
    }

    public void PlaySound(string soundName)
    {
        if (audioClips.ContainsKey(soundName))
        {
            audioSource.PlayOneShot(audioClips[soundName]);
            if (soundCoroutine != null)
            {
                StopCoroutine(soundCoroutine);
            }
            soundCoroutine = StartCoroutine(StopSoundAfterDuration(audioClips[soundName].length));
        }
    }

    public void StopSound(string soundName)
    {
        audioSource.Stop();
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    private IEnumerator StopSoundAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        audioSource.Stop();
    }

    public void LoadAudioClips(Dictionary<string, AudioClip> clips)
    {
        audioClips = clips;
    }
}