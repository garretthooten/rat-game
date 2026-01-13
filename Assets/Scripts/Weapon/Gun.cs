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

    //temp remove this
    [Header("Temp Debugging")]
    private GameObject _cursorVisualizer;
    private GameObject _newCursorVisualizer;
    [SerializeField] private Material _visualizerMaterial;
    [SerializeField] private float _cursorOffsetMultiplier = 1.0f;

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
        Vector3 cursorDirectionToCamera = new Vector3(0f, 1f, -1f).normalized;
        Vector3 newCursorPosition = _cursorPosition + (cursorDirectionToCamera * _cursorOffsetMultiplier);
        switch (_fireType)
        {
            case Firetype.SemiAutomatic:
                bool canFireSemiAutomatic = _canFire && (!_lastTriggerPulled);
                if (canFireSemiAutomatic)
                {
                    _currentClipAmmo--;
                    _timeOfLastShot = Time.time;                 
                    //Vector3 cursorDirectionToCamera = new Vector3(0f, 1f, -1f).normalized;
                    //Vector3 newCursorPosition = _cursorPosition + (cursorDirectionToCamera * _cursorOffsetMultiplier);

                    //_cursorVisualizer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    //_cursorVisualizer.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    //_cursorVisualizer.transform.position = _cursorPosition;

                    //_newCursorVisualizer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    //_newCursorVisualizer.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    //_newCursorVisualizer.transform.position = newCursorPosition;
                    //_newCursorVisualizer.GetComponent<MeshRenderer>().material = _visualizerMaterial; 

                    //Debug.Log($"Cursor location: {_cursorPosition}, new cursor location: {newCursorPosition}");
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
                    FireHitscanShot(newCursorPosition);
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

        RaycastHit hit;
        if(Physics.CapsuleCast(new Vector3(_fireTransform.position.x, 100f, _fireTransform.position.z), new Vector3(_fireTransform.position.x, -100f, _fireTransform.position.z), _bulletRadius, shotDirection.normalized, out hit, 100f))
        {
            //Debug.Log($"Hit {hit.transform.name}, {hit.transform.tag}");
            if (hit.transform.TryGetComponent(out Health health))
            {
                health.TakeDamage(damage, hit.point, hit.normal);
            }
        }

        if (_bulletTrailPrefab)
        {
            GameObject bulletTrail = Instantiate(_bulletTrailPrefab);
            Vector3 trailEndpoint;
            if (hit.transform)
            {
                trailEndpoint = hit.point;
            }
            else 
            {
                trailEndpoint = _fireTransform.position + (shotDirection.normalized * 100f);
            }
            bulletTrail.GetComponent<BulletTrail>().MoveTrail(_fireTransform.position, trailEndpoint);
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

        if(_cursorVisualizer != null)
        {
            Destroy(_cursorVisualizer.gameObject);
        }
        if(_newCursorVisualizer != null)
        {
            Destroy(_newCursorVisualizer.gameObject);
        }
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