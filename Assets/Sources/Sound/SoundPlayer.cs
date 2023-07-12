using UnityEngine;

namespace Sources.Sound
{
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _source;

        [SerializeField] private AudioClip _click;

        [SerializeField] private AudioClip _lose;

        public void PlayClick()
        {
            _source.PlayOneShot(_click);
        }

        public void PlayLose()
        {
            _source.PlayOneShot(_lose);
        }

        public void StartPlayMove()
        {
            _source.Play();
        }

        public void StopPlayMove()
        {
            _source.Stop();
        }
    }
}