using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;     // 사운드 이름

    public AudioClip clip;      // 사운드 파일
    public AudioSource source;     // 사운드 플레이어

    public bool loop;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
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
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    public Sound[] sounds;      // 배경음
    public Sound[] effectSounds;        // 효과음
    public string nowSound;
    public bool isBackgroundSound;
    public bool isEffectSound;

    public float musicVolume;
    public float effectVolume;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = this;    // 싱글톤 사용
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitAudioManager()
    {
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
        musicVolume = DataSave.instance.dbData.nowMusicVolume;
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject soundObj = new GameObject(sounds[i].name);
            sounds[i].SetSource(soundObj.AddComponent<AudioSource>());
            sounds[i].SetVolume(musicVolume);
            soundObj.transform.SetParent(this.transform);
        }
    }

    public void InitEffectSound()
    {
        effectVolume = DataSave.instance.dbData.nowEffectVolume;
        for (int i = 0; i < effectSounds.Length; i++)
        {
            GameObject soundObj = new GameObject(effectSounds[i].name);
            effectSounds[i].SetSource(soundObj.AddComponent<AudioSource>());
            effectSounds[i].SetVolume(effectVolume);
            soundObj.transform.SetParent(this.transform);
        }
    }

    public void Play(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                //sounds[i].SetVolume(musicVolume);
                sounds[i].Play();
                nowSound = sounds[i].name;
                return;
            }
        }
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if (_name == effectSounds[i].name)
            {
                effectSounds[i].SetVolume(effectVolume);
                effectSounds[i].Play();
                return;
            }
        }
    }

    public void Stop(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].Stop();
                return;
            }
        }
    }

    public void SetPause(string _name, bool pause)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
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
            if (_name == sounds[i].name)
            {
                sounds[i].SetLoop(loop);
                return;
            }
        }
    }
    public void SetMusicVolume(float _Volume)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].SetVolume(_Volume);
        }
        musicVolume = _Volume;
    }

    public void SetEffectVolume(float _Volume)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            effectSounds[i].SetVolume(_Volume);
        }
        effectVolume = _Volume;
    }

    public void SetOneVolume(string _name, float _Volume)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
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