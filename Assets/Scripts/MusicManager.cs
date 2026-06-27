using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioClip mainAudioClip;
    [SerializeField] AudioClip location1AudioClip;
    [SerializeField] AudioClip location2AudioClip;
    [SerializeField] AudioClip location3AudioClip;

    void Start()
    {
        GameObject[] musicObjects = GameObject.FindGameObjectsWithTag("MainMenuMusic");
        
        foreach (GameObject obj in musicObjects)
        {
            Destroy(obj);
        }

        musicAudioSource.clip = location1AudioClip;
        musicAudioSource.Play();
    }

    public void PlayAmbient(int ambientId)
    {
        if (ambientId == 1)
        {
            musicAudioSource.clip = location1AudioClip;
        }
        else if (ambientId == 2)
        {
            musicAudioSource.clip = location2AudioClip;
        }
        else if (ambientId == 3)
        {
            musicAudioSource.clip = location3AudioClip;
        }
        
        musicAudioSource.Play();
    }
}
