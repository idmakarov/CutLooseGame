using UnityEngine;

public class AnimationSoundController : MonoBehaviour
{
    private AudioSource _audioSourceCharacter;
    private void Start()
    {
        _audioSourceCharacter = GetComponent<AudioSource>();
    }
    private void JumpSound()
    {
        _audioSourceCharacter.Play();
    }
}