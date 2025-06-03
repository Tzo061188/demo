using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicManager : SingletonBase_MonoAuto<MusicManager>
{
    private AudioSource audioSource = null;

    private float BGMusicVolume = 0.5f;
    private float SoundEffectVolume = 0.5f;

    private List<AudioSource> SoundEffects = new List<AudioSource>();
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    private void Update()
    {
        //不断的进行回收
        for (int i = SoundEffects.Count - 1; i >= 0; i--) {
            if (SoundEffects[i].isPlaying == false) {

                SoundEffects[i].clip = null;
                PoolManager.Instance.PutObj(SoundEffects[i].gameObject);
                SoundEffects.RemoveAt(i);
            }
        
        }
    }

    #region 播放背景音乐相关
    /// <summary>
    ///  播放背景音乐
    /// </summary>
    /// <param name="clip">声音片段</param>
    public void PlayMusic(AudioClip clip) {
        if (audioSource == null) { 

            GameObject gameObject = new GameObject("BG_Music");
            GameObject.DontDestroyOnLoad(gameObject);
            audioSource =  gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = clip;
        audioSource.volume = BGMusicVolume;
        audioSource.loop = true;
        audioSource.Play();
    }
    public void StopMusic() {
        if (audioSource == null)
            return;
        audioSource.Stop();
    }
    public void PauseMusic() {
         if (audioSource == null)
            return;
         audioSource.Pause();
    }
    public void SetVolume(float value) {
        if (audioSource == null)
            return;
        BGMusicVolume = value;
        audioSource.volume = value;
    }
    #endregion



    #region 播放音效相关
    public void PlaySound(AudioClip clip, UnityAction<AudioSource> callback = null) {
        
        AudioSource audioSource =  PoolManager.Instance.TakeObj("Prefabs/", "SoundObj").GetComponent<AudioSource>();
        //执行回调函数
        callback?.Invoke(audioSource);

        audioSource.clip = clip;
        audioSource.volume = SoundEffectVolume;
        audioSource.Play();

        SoundEffects.Add(audioSource);
     
    }
    public void StopSound(AudioSource source) {

        if (SoundEffects.Contains(source)) { 

            source.Stop();
            source.clip = null;
            SoundEffects.Remove(source);
            PoolManager.Instance.PutObj(source.gameObject);
        }
    }
    public void SetSoundEffectVolume(float value) {
        SoundEffectVolume = value;
        foreach (AudioSource source in SoundEffects) {
            source.volume = SoundEffectVolume;
        }
    }
    #endregion

    /// <summary>
    /// 跳转场景进行清除所有播放中的
    /// </summary>
    public void Clear() {
        for (int i = SoundEffects.Count - 1; i >= 0; i--)
        {            
            SoundEffects[i].clip = null;
            PoolManager.Instance.PutObj(SoundEffects[i].gameObject);
            SoundEffects.RemoveAt(i);
        }
        SoundEffects.Clear();
    }


}
