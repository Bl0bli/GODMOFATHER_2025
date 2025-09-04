using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] AudioSource soundFX;

    [SerializeField] AudioMixer audioMixer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundFX(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        //spawn in GameObject
        AudioSource audioSource = Instantiate(soundFX, spawnTransform.position, Quaternion.identity);

        //assign AudioClip
        audioSource.clip = audioClip;

        //assign Volume
        audioSource.volume = volume;

        //play sound
        audioSource.Play();

        //get the lenght of the sound FX clip
        float clipLengh = audioSource.clip.length;

        //detroy the clip after playing
        Destroy(audioSource.gameObject, clipLengh);
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("master", Mathf.Log10(volume) * 20f);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("music", Mathf.Log10(volume) * 20f);
    }

    public void SetFxVolume(float volume)
    {
        audioMixer.SetFloat("FX", Mathf.Log10(volume) * 20f);
    }
}

