using UnityEngine;

public class SFXManager
{
    public void PlaySFX(AudioSource _source, AudioClip _clip, float _volume)
    {
        _source.clip = _clip;
        _source.playOnAwake = false;
        _source.volume = _volume;
        _source.Play();
    }
}
