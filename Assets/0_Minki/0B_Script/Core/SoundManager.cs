using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField] private AudioMixer _mixer;

    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _sfxSource;

    [SerializeField] private AudioClip[] _clips;

    private Dictionary<string, AudioClip> _clipDictionary = new Dictionary<string, AudioClip>();

    protected override void Awake() {
        base.Awake();
        
        for(int i = 0; i < _clips.Length; ++i) {
            _clipDictionary.Add(_clips[i].name, _clips[i]);
        }
    }

    public void SetVolumeMaster(float volume) {
        if(volume == -40f) _mixer.SetFloat("Master", -80f);
        else _mixer.SetFloat("Master", volume);
    }

    public void SetVolumeBgm(float volume) {
        if(volume == -40f) _mixer.SetFloat("BGM", -80f);
        else _mixer.SetFloat("BGM", volume);
    }

    public void SetVolumeSFX(float volume) {
        if(volume == -40f) _mixer.SetFloat("SFX", -80f);
        else _mixer.SetFloat("SFX", volume);
    }

    public void PlaySFX(string name) {
        if(_clipDictionary.TryGetValue(name, out AudioClip clip)) {
            _sfxSource.PlayOneShot(clip);
        }
        else {
            Debug.LogError("아니 없음 - SFX");
        }
    }

    public void PlaySFX(string name, float time) {
        if(_clipDictionary.TryGetValue(name, out AudioClip clip)) {
            GameObject obj = new GameObject();
            obj.transform.parent = transform;

            AudioSource source = obj.AddComponent<AudioSource>();
            source.clip = clip;
            source.Play();
            
            Destroy(obj, time);
        }
        else {
            Debug.LogError("아니 없음 - SFX");
        }
    }

    public void PlayBGM(string name) {
        if(_clipDictionary.TryGetValue(name, out AudioClip clip)) {
            _bgmSource.Stop();
            _bgmSource.clip = clip;
            _bgmSource.Play();
        }
        else {
            Debug.LogError("아니 없음 - BGM");
        }
    }
}
