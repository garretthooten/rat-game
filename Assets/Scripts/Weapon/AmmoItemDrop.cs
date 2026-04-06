using System.Collections;
using UnityEngine;

public class AmmoItemDrop : MonoBehaviour
{

    public Gun.Firetype firetype;
    public int amount = 30;

    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private AudioClip _pickupSoundEffect;

    private AudioSource _audioSource;
    private MeshRenderer _meshRenderer;
    private SphereCollider _collider;

    private void OnEnable()
    {
        _audioSource = GetComponent<AudioSource>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<SphereCollider>();

        // enable components when pulled from pool or spawned
        _audioSource.enabled = true;
        _meshRenderer.enabled = true;
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
                other.GetComponent<PlayerCombat>()?.AddWeaponAmmo(firetype, amount);
                gameObject.SetActive(false);
                //Destroy(gameObject);
            }
        }
    }

    private IEnumerator PickupObjectWithAudio(Collider other)
    {
        other.GetComponent<PlayerCombat>()?.AddWeaponAmmo(firetype, amount);
        _meshRenderer.enabled = false;
        _collider.enabled = false;
        _audioSource.clip = _pickupSoundEffect;
        _audioSource.PlayOneShot(_pickupSoundEffect, SettingsManager.instance.sfxVolume);
        yield return new WaitUntil(() => _audioSource.time >= _pickupSoundEffect.length);
        gameObject.SetActive(false);
    }

}
