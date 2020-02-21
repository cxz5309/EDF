using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TmpSound
{
    public string name;     // 사운드 이름

    public AudioClip clip;      // 사운드 파일
    private AudioSource source;     // 사운드 플레이어

    public float Volume;
    public bool loop;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
        source.volume = Volume;
    }
    public void Play()
    {
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }

    public void SetVolume(float _volume)
    {
        source.volume = _volume;
    }

    public void SetPause(bool _pauseOnOff)
    {
        if (_pauseOnOff) source.Pause();
        else source.UnPause();
    }

    public void SetLoop(bool _loopOnOff)
    {
        source.loop = _loopOnOff;
    }

    public void SetMute(bool _muteOnOff)
    {
        source.mute = _muteOnOff;
    }

    public void SetPitch(float _pitch)
    {
        source.pitch = _pitch;
    }
}

public class TmpAudioManager : MonoBehaviour
{
    static public TmpAudioManager instance;

    [SerializeField]
    public TmpSound[] sounds;      // 배경음
    public TmpSound[] effectSounds;        // 효과음
    public string nowSound;
    public bool isBackgroundSound;
    public bool isEffectSound;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;    // 싱글톤 사용
        }
        else
        {
            Destroy(gameObject);
        }
        InitSound();
        InitEffectSound();
    }

    private void Start()
    {
        isBackgroundSound = true;
        isEffectSound = true;
    }

    public void InitSound()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject soundObj = new GameObject(sounds[i].name);
            sounds[i].SetSource(soundObj.AddComponent<AudioSource>());
            sounds[i].SetVolume(0.5f);
            soundObj.transform.SetParent(this.transform);
        }
    }

    public void InitEffectSound()
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            GameObject soundObj = new GameObject(effectSounds[i].name);
            effectSounds[i].SetSource(soundObj.AddComponent<AudioSource>());
            soundObj.transform.SetParent(this.transform);
        }
    }

    public void Play(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Play();
                nowSound = sounds[i].name;
                return;
            }
        }
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if (effectSounds[i].name == _name)
            {
                effectSounds[i].Play();
                return;
            }
        }
    }

    public void SetPitch(float _pitch)
    {
        for(int k = 0; k < sounds.Length; k++)
        {
            if(sounds[k].name == nowSound)
            {
                sounds[k].SetPitch(_pitch);
            }
        }
    }
    
    public void Stop(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Stop();
                return;
            }
        }
    }

    public void SetPause(bool pause)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == nowSound)
            {
                sounds[i].SetPause(pause);
                return;
            }
        }
    }

    public void SetLoop(string _name, bool loop)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].SetLoop(loop);
                return;
            }
        }
    }

    public void SetVolume(string _name, float _Volume)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].SetVolume(_Volume);
                return;
            }
        }
    }

    public void SetBackGroundMute()
    {
        if (isBackgroundSound)      // 소리 끄기
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                sounds[i].SetMute(false);
            }
            isBackgroundSound = false;
        }
        else if (!isBackgroundSound)        // 소리 켜기
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                sounds[i].SetMute(true);
            }
            isBackgroundSound = true;
        }
    }

    public void SetEffectMute()
    {
        if (isEffectSound)
        {
            for (int i = 0; i < effectSounds.Length; i++)
            {
                effectSounds[i].SetMute(false);
            }
            isEffectSound = false;
        }
        else if (!isEffectSound)
        {
            for (int i = 0; i < effectSounds.Length; i++)
            {
                effectSounds[i].SetMute(true);
            }
            isEffectSound = true;
        }
    }
}