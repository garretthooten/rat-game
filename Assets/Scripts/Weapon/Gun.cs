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
    [SerializeField] protected int _maxClipAmmo;
    [SerializeField] protected int _currentAmmo;
    [SerializeField] protected float _fireRate;

    [SerializeField] protected Transform _fireTransform;

    protected float _timeBetweenShots;
    protected float _timeOfLastShot;
    protected bool _isFiring;
    protected bool _canFire;
    protected bool _lastFireInput;

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
    }

    public void Fire(Vector3 cursorPosition)
    {
        
        float timeSinceLastShot = Time.time - _timeOfLastShot;
        _canFire = timeSinceLastShot >= _timeBetweenShots && _currentAmmo > 0;
        Logger.Info($"Entering fire switch with canFire: {_canFire} timeSinceLastShot: {timeSinceLastShot} currentAmmo: {_currentAmmo}");
        switch (_fireType)
        {
            case Firetype.SemiAutomatic:
                if (_canFire && !_lastFireInput)
                {
                    _currentAmmo--;
                    Logger.Info($"Fire! {_currentAmmo / _maxClipAmmo}");
                }

                break;
            case Firetype.FullAutomatic:
                if (_canFire)
                {
                    _currentAmmo--;
                    Logger.Info($"Fire! {_currentAmmo / _maxClipAmmo}");
                }

                break;
        }
    }

}
