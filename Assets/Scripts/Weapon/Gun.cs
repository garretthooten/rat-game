using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Gun : MonoBehaviour
{
    public enum Firetype
    {
        FullAutomatic,
        SemiAutomatic
    }

    public Transform attachTransform;
    public float damage = 20f;

    [SerializeField] protected Firetype _fireType;
    [SerializeField] protected int _maxAmmo;
    [SerializeField] protected int _currentAmmo;
    [SerializeField] protected int _maxClipAmmo;
    [SerializeField] protected int _currentClipAmmo;
    [SerializeField] protected float _fireRate;
    [SerializeField] protected float _reloadTime;
    [SerializeField] protected float _bulletRadius = 0.05f;
    [SerializeField] protected float _bulletVisualizeTime = 0.2f;

    [SerializeField] protected Transform _fireTransform;
    [SerializeField] protected GameObject _bulletVisualizerPrefab;
    [SerializeField] protected LayerMask _shotLayerMask;
    [SerializeField] protected string layerCanTakeDamage = "Rat";

    [Header("Bullet Trail and Impact")]
    [SerializeField] protected ParticleSystem _muzzleFlash;
    [SerializeField] protected GameObject _bulletTrailPrefab;

    [Header("Audio")] 
    [SerializeField] [Range(0f, 1f)] protected float _sfxVolume = 0.7f;
    [SerializeField] protected AudioSource _audioSource;
    [SerializeField] protected AudioClip _fireSound;
    [SerializeField] protected AudioClip _emptyClipSound;
    
    protected float _timeBetweenShots;
    protected float _timeOfLastShot;
    protected bool _canFire;
    protected bool _isReloading;
    protected bool _triggerPulled;
    protected bool _lastTriggerPulled;

    protected Vector3 _cursorPosition;

    protected InputHandler _inputHandler;
    protected Camera _camera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _camera = Camera.main;
        if (!_camera)
        {
            MyLogger.Error("Failed to find camera");
        }

        if (!_audioSource)
        {
            _audioSource = GetComponent<AudioSource>();
        }

        if (!_inputHandler)
        {
            _inputHandler = FindAnyObjectByType<InputHandler>();
            if (!_inputHandler)
                MyLogger.Error("Failed to find input handler");
        }

        if (!_bulletVisualizerPrefab)
        {
            // _bulletVisualizer = GetComponent<LineRenderer>();
            // if(!_bulletVisualizer)
            //     MyLogger.Error("Failed to find bullet visualizer");
            MyLogger.Error("Failed to find bullet visualizer prefab");
        }

        _timeBetweenShots = 1 / _fireRate;
        MyLogger.Info($"_timeBetweenShots: {_timeBetweenShots}s");
        _timeOfLastShot = Time.time;
        _lastTriggerPulled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_triggerPulled)
        {
            Fire();
        }

        _lastTriggerPulled = _triggerPulled;
    }

    public void Fire()
    {
        float timeSinceLastShot = Time.time - _timeOfLastShot;
        _canFire = (timeSinceLastShot >= _timeBetweenShots && _currentClipAmmo > 0) && (!_isReloading);
        switch (_fireType)
        {
            case Firetype.SemiAutomatic:
                bool canFireSemiAutomatic = _canFire && (!_lastTriggerPulled);
                if (canFireSemiAutomatic)
                {
                    _currentClipAmmo--;
                    _timeOfLastShot = Time.time;
                    Vector3 newCursorPosition = new Vector3(_cursorPosition.x, 0.5f, _cursorPosition.z);
                    Debug.Log($"Cursor location: {_cursorPosition}, new cursor location: {newCursorPosition}");
                    FireHitscanShot(newCursorPosition);
                }
                else if (_currentClipAmmo <= 0 && !_lastTriggerPulled)
                {
                    _audioSource.PlayOneShot(_emptyClipSound, _sfxVolume);
                }

                break;
            case Firetype.FullAutomatic:
                if (_canFire)
                {
                    _currentClipAmmo--;
                    _timeOfLastShot = Time.time;
                    FireHitscanShot(_cursorPosition);
                    //MyLogger.Info($"Fire! {_currentClipAmmo} / {_maxClipAmmo}");
                }
                else if (_currentClipAmmo <= 0 && !_lastTriggerPulled)
                {
                    _audioSource.PlayOneShot(_emptyClipSound, _sfxVolume);
                }

                break;
        }
    }

    public void FireHitscanShot(Vector3 cursorPosition)
    {
        Vector3 shotDirection = cursorPosition - _fireTransform.position;
        //shotDirection = new Vector3(shotDirection.x, 0f, shotDirection.z);
        RaycastHit hit;
        if (Physics.SphereCast(_fireTransform.position, _bulletRadius, shotDirection, out hit))
        {
            //Debug.Log($"Hit {hit.transform.name}, {hit.transform.tag}");
            if (hit.transform.TryGetComponent(out Health health))
            {
                health.TakeDamage(damage, hit.point, hit.normal);
            }
                
            
            // GameObject bulletVisualizer = Instantiate(_bulletVisualizerPrefab);
            // LineRenderer lineRenderer = bulletVisualizer.GetComponent<LineRenderer>();
            //
            // lineRenderer.positionCount = 2;
            // lineRenderer.SetPosition(0, _fireTransform.position);
            // lineRenderer.SetPosition(1, cursorPosition);
            // lineRenderer.startWidth = _bulletRadius * 2;
            // lineRenderer.endWidth = _bulletRadius * 2;

            //StartCoroutine(StartBulletVisualizeTimer(bulletVisualizer));
        }

        if (_bulletTrailPrefab)
        {
            GameObject bulletTrail = Instantiate(_bulletTrailPrefab);
            bulletTrail.GetComponent<BulletTrail>().MoveTrail(_fireTransform.position, hit.point);
        }

        if (_muzzleFlash)
            _muzzleFlash.Play();
        _audioSource.PlayOneShot(_fireSound, _sfxVolume);
    }

    public void PullTrigger(Vector3 cursorPosition)
    {
        _triggerPulled = true;
        _cursorPosition = cursorPosition;
    }

    public void ReleaseTrigger()
    {
        _triggerPulled = false;
        _cursorPosition = Vector3.zero;
    }

    public void Reload()
    {
        if (_isReloading)
        {
            return;
        }

        _isReloading = true;
        StartCoroutine(StartReloadTimer());
        int ammoNeeded = _maxClipAmmo - _currentClipAmmo;
        int ammoToLoad = Mathf.Min(ammoNeeded, _currentAmmo);

        _currentClipAmmo += ammoToLoad;
        _currentAmmo -= ammoToLoad;
    }

    private IEnumerator StartReloadTimer()
    {
        yield return new WaitForSeconds(_reloadTime);
        _isReloading = false;
    }

    private IEnumerator StartBulletVisualizeTimer(GameObject bulletVisualizer)
    {
        yield return new WaitForSeconds(_bulletVisualizeTime);
        Destroy(bulletVisualizer);
    }

    public string MakeDebugString()
    {
        string result =
            $"fireType: {_fireType}\nammo: {_currentClipAmmo}/{_maxClipAmmo}/{_currentAmmo}\ntriggerPulled: {_triggerPulled}\nlastTriggerPulled: {_lastTriggerPulled}\nisReloading: {_isReloading}";
        return result;
    }
}