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
        //_audioSource = GetComponent<AudioSource>();
        //_meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<SphereCollider>();

        // enable components when pulled from pool or spawned
        //_audioSource.enabled = true;
        _collider.enabled = true;
        //_meshRenderer.enabled = true;
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
                //Destroy(gameObject);
            }
        }
    }

    private IEnumerator PickupObjectWithAudio(Collider other)
    {
        other.GetComponent<PlayerHealth>()?.Heal(amount);
        //_meshRenderer.enabled = false;
        _collider.enabled = false;
        //_audioSource.clip = _pickupSoundEffect;
        AudioSource.PlayClipAtPoint(_pickupSoundEffect, transform.position, SettingsManager.instance.sfxVolume);
        //yield return new WaitUntil(() => _audioSource.time >= _pickupSoundEffect.length);
        gameObject.SetActive(false);
        yield return null; // not optimal but just temporary
        // later rework pickups to use PlayClipAtPoint instead of a coroutine....
    }
}
