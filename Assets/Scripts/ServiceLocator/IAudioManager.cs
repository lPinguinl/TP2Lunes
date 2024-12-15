using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAudioManager
{
    void PlaySound(string soundName);
    void StopSound(string soundName);
    void SetVolume(float volume);
}
