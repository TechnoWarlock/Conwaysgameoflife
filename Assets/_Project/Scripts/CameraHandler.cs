using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    private Camera _cam;
    private Vector2 _lastPosition;
    private Vector3 _resetCameraPosition;

    private const int MoveSpeed = 20;
    private const int MaxCameraSize = 100;
    private const int MinCameraSize = 5;

    private Vector3 _initialPosition;

    private Vector2 _mousePosition;
    
    public void Awake()
    {
        _cam = Camera.main;
        _initialPosition = _cam.transform.position;
    }

    private void OnEnable()
    {
        inputReader.OnPinch += Zoom;
        inputReader.OnMiddleButton += Move;
    }

    private void OnDisable()
    {
        inputReader.OnPinch -= Zoom;
        inputReader.OnMiddleButton -= Move;
    }

    private void Zoom(Vector2 value)
    {
        float currentCameraSize = _cam.orthographicSize - value.y;
        _cam.orthographicSize = Mathf.Clamp(currentCameraSize, MinCameraSize, MaxCameraSize);
    }

    private void Move()
    {
        if (_lastPosition != inputReader.PointerPosition)
        {
            Vector3 deltaCameraPosition = (_lastPosition - inputReader.PointerPosition).normalized;
            _cam.transform.position += deltaCameraPosition * (Time.deltaTime * MoveSpeed);
            _lastPosition = inputReader.PointerPosition;
        }
    }

    public void SavePosition()
    {
        _resetCameraPosition = _cam.transform.position;
    }

    public void ResetPosition()
    {
        _cam.transform.position = _resetCameraPosition;
    }

    public void ReturnInitialPosition()
    {
        _cam.transform.position = _initialPosition;
    }
}