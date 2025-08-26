using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private AudioSource audioSource;

    [SerializeField] AudioClip clipBackground;
    [SerializeField] AudioClip clipMatch;
    [SerializeField] AudioClip clipNoMatch;
    [SerializeField] AudioClip clipWin;
    [SerializeField] AudioClip clipLose;
    [SerializeField] AudioClip clickButton;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayBackgroundMusic()
    {
        audioSource.clip = clipBackground;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayMatchSound()
    {
        audioSource.PlayOneShot(clipMatch);
    }

    public void PlayNoMatchSound()
    {
        audioSource.PlayOneShot(clipNoMatch);
    }

    public void PlayWinSound()
    {
        audioSource.PlayOneShot(clipWin);
    }

    public void PlayLoseSound()
    {
        audioSource.PlayOneShot(clipLose);
    }
    public void PlayClickButtonSound()
    {
        audioSource.PlayOneShot(clickButton);
    }
}
