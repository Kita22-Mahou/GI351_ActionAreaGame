using UnityEngine;

public class MenuBGM : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    public AudioClip bgm;

    private void Start()
    {
        musicSource.PlayOneShot(bgm);
    }
}
