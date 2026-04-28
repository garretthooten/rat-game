using UnityEngine;
using System.Collections;
using static Gun;

public class HealthPickupItem : MonoBehaviour
{
    public int amount = 10;

    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private AudioClip _pickupSoundEffect;

    private AudioSource _audioSource;
    private MeshRenderer _meshRenderer;
    private SphereCollider _collider;

    private void OnEnable()
    {
        _collider = GetComponent<SphereCollider>();
        _collider.enabled = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_playerTag))
        {
            if (_audioSource != null && _pickupSoundEffect != null)
            {
                StartCoroutine(PickupObjectWithAudio(other));
            }
            else
            {
                other.GetComponent<PlayerHealth>()?.Heal(amount);
                gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator PickupObjectWithAudio(Collider other)
    {
        other.GetComponent<PlayerHealth>()?.Heal(amount);
        _collider.enabled = false;
        AudioSource.PlayClipAtPoint(_pickupSoundEffect, transform.position, SettingsManager.instance.sfxVolume);
        gameObject.SetActive(false);
        yield return null; // not optimal but just temporary
    }
}
