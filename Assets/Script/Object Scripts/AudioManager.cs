using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("AudioSource")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] private AudioSource loopSource;

    [Header("Audio Clip")]

    public AudioClip background;
    public AudioClip[] deathSounds;
    public AudioClip HitMontser;
    public AudioClip pipAttark;
    public AudioClip Hitpip;
    public AudioClip Poko;

    public AudioClip PipWalk;
    public AudioClip PipDash;

    [SerializeField] private AudioSource skillSource;
    [SerializeField] private AudioClip skillSound;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        
    }

    public void PokoBark()
    {
        SFXSource.PlayOneShot(Poko);
    }

    public void StartWalk()
    {
        if (loopSource.isPlaying)
            return;

        loopSource.clip = PipWalk;
        loopSource.loop = true;
        loopSource.Play();
    }

    public void StopWalk()
    {
        if (!loopSource.isPlaying)
            return;

        loopSource.Stop();
    }

    public void Dash()
    {
        SFXSource.PlayOneShot(PipDash);
    }

    public void PlayDeath()
    {
        if (deathSounds.Length == 0)
            return;

        AudioClip sound = deathSounds[Random.Range(0, deathSounds.Length)];

        SFXSource.PlayOneShot(sound);
    }

    public void HitMontsers()
    {
        SFXSource.PlayOneShot(HitMontser);
    }

    public void PipAttark()
    {
        SFXSource.PlayOneShot(pipAttark);
    }

    public void PipHit()
    {
        SFXSource.PlayOneShot(Hitpip);
    }
    public void StartSkillSound()
    {
        if (skillSource.isPlaying)
            return;

        skillSource.clip = skillSound;
        skillSource.loop = true;
        skillSource.Play();
    }

    public void StopSkillSound()
    {
        skillSource.Stop();
    }





}
