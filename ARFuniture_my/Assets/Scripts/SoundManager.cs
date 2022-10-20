//SoundManager
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioMixer mixer;//����� �ͼ� BGM�� ȿ���� ����
    public AudioSource bgSound;//�� ���� ������Ʈ -> soundobject �ڽ��� audiosource
    public AudioClip[] bglist;
    public static SoundManager instance;

    private void Awake()    //�̱���
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            SceneManager.sceneLoaded += OnSceneLoaded;  //������ �ٸ� ����� �߰�
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        for (int i = 0; i < bglist.Length; i++)
        {
            if (arg0.name == bglist[i].name)
            {
                BgSoundPlay(bglist[i]);
            }
        }
    }

    public void BGSoundVolume(float val) //�ٸ� �����̴����� �����ϴ� �Լ�
    {
        mixer.SetFloat("BGSoundVolume", Mathf.Log10(val) * 20);
    }

    public void SFXSoundVolume(float val) //�ٸ� �����̴����� �����ϴ� �Լ�
    {
        mixer.SetFloat("SFXVolume", Mathf.Log10(val) * 20);
    }

    public void SFXPlay(string sfxName, AudioClip clip)
    {
        GameObject go = new GameObject(sfxName + "Sound");//������ ���� �׿�����Ʈ ����(�̸�)
        AudioSource audiosource = go.AddComponent<AudioSource>();
        audiosource.clip = clip;
        audiosource.Play();
        audiosource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0]; //�׷켳��
                                                                                //����� �ͼ����� �Ķ���͸� ����� ����� �ִ�!
        Destroy(go, clip.length); //Ŭ�����̸�ŭ ����ϰ� ����
    }

    public void BgSoundPlay(AudioClip clip)
    {
        bgSound.outputAudioMixerGroup = mixer.FindMatchingGroups("BGSound")[0];
        bgSound.clip = clip;
        bgSound.loop = true; //�ݺ����
        bgSound.volume = 0.1f;
        bgSound.Play();
    }
}
