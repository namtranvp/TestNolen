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
    [SerializeField] AudioClip shuffleSound;

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

    void OnEnable()
    {
        GameController.OnMatch += PlayMatchSound;
    }

    void OnDisable()
    {
        GameController.OnMatch -= PlayMatchSound;
    }

    private void PlayMatchSound(bool isMatch)
    {
        audioSource.PlayOneShot(isMatch ? clipMatch : clipNoMatch);
    }

    public void PlayBackgroundMusic()
    {
        audioSource.clip = clipBackground;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayWinSound()
    {
        audioSource.PlayOneShot(clipWin);
    }

    public void PlayLoseSound()
    {
        Debug.Log("Play Lose Sound");
        audioSource.PlayOneShot(clipLose);
    }
    public void PlayClickButtonSound()
    {
        audioSource.PlayOneShot(clickButton);
    }

    public void PlayShuffleSound()
    {
        audioSource.PlayOneShot(shuffleSound);
    }
}
