using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum Firetype
    {
        FullAutomatic,
        SemiAutomatic
    }

    [SerializeField] protected Firetype _fireType;
    [SerializeField] protected int _maxAmmo;
    [SerializeField] protected int _currentAmmo;
    [SerializeField] protected int _maxClipAmmo;
    [SerializeField] protected int _currentClipAmmo;
    [SerializeField] protected float _fireRate;
    [SerializeField] protected float _reloadTime;

    [SerializeField] protected Transform _fireTransform;

    protected float _timeBetweenShots;
    protected float _timeOfLastShot;
    protected bool _isFiring;
    protected bool _canFire;
    protected bool _lastFireInput;
    protected bool _triggerPulled;
    protected bool _lastTriggerPulled;

    protected InputHandler _inputHandler;
    protected Camera _camera;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _camera = Camera.main;
        if (!_camera)
        {
            Logger.Error("Failed to find camera");
        }

        if (!_inputHandler)
        {
            _inputHandler = FindAnyObjectByType<InputHandler>();
            if(!_inputHandler)
                Logger.Error("Failed to find input handler");
        }

        _timeBetweenShots = 1 / _fireRate;
        Logger.Info($"_timeBetweenShots: {_timeBetweenShots}s");
        _timeOfLastShot = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        _lastFireInput = _inputHandler.attack;
        _lastTriggerPulled = _triggerPulled;
    }

    public void Fire(Vector3 cursorPosition)
    {
        _triggerPulled = true;
        float timeSinceLastShot = Time.time - _timeOfLastShot;
        _canFire = timeSinceLastShot >= _timeBetweenShots && _currentClipAmmo > 0;
        switch (_fireType)
        {
            case Firetype.SemiAutomatic:
                _canFire = _canFire && (!_lastTriggerPulled);
                if (_canFire && !_lastFireInput)
                {
                    _currentClipAmmo--;
                    _timeOfLastShot =  Time.time;
                    Logger.Info($"Fire! {_currentClipAmmo / _maxClipAmmo}");
                }

                break;
            case Firetype.FullAutomatic:
                if (_canFire)
                {
                    _currentClipAmmo--;
                    _timeOfLastShot =  Time.time;
                    Logger.Info($"Fire! {_currentClipAmmo / _maxClipAmmo}");
                }

                break;
        }
    }

    public void ReleaseTrigger()
    {
        Logger.Info($"Gun recieved release");
        _triggerPulled = false;
    }

}
