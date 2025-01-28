using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Inputs/InputReader")]
public class InputReader : ScriptableObject, PlayerInput.IPlayerActions
{
    private PlayerInput _playerInput;

    public event UnityAction<Vector2> OnPointerPress;
    public event UnityAction OnMiddleButton;
    public event UnityAction<Vector2> OnPinch;

    public Vector2 PointerPosition => _pointerPosition;
    public bool IsPointerOnUI => _isPointerOnUI;

    private bool _isSecondaryTouchPress;
    private bool _isPrimaryPointerPress;
    private bool _isMiddleButtonPress;
    private bool _isPointerOnUI;

    private static readonly float FirstLastDistance = 0.0000001f;
    private float _lastDistance = FirstLastDistance;
    private float _currentDistance;

    private readonly Vector2 _pinchVector = new Vector2(0, 1);
    private Vector2 _zoomValue;
    private Vector2 _pointerPosition;
    private Vector2 _secondaryTouchPosition;
    private Vector2 _scrollZoom;
    private bool _isPrimaryPointerHold;


    private void OnEnable()
    {
        if (_playerInput == null)
        {
            _playerInput = new PlayerInput();
            _playerInput.Player.SetCallbacks(this);
        }

        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }


    void PlayerInput.IPlayerActions.OnMiddleButtonPress(InputAction.CallbackContext context)
    {
        PressButtonState(context, ref _isMiddleButtonPress);
    }


    private bool IsPointerOverUI(Vector2 screenPosition)
    {
        if (EventSystem.current == null) return true;
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        _isPointerOnUI = results.Count > 0;
        return _isPointerOnUI;
    }

    private void GetPosition(InputAction.CallbackContext context, ref Vector2 position)
    {
        if (context.control.device is Pointer pointer)
        {
            position = pointer.position.ReadValue();
        }
    }

    void PlayerInput.IPlayerActions.OnPrimaryPointerPosition(InputAction.CallbackContext context)
    {
        _pointerPosition = context.ReadValue<Vector2>();
        if(IsPointerOverUI(_pointerPosition)) return;
        CanPinch();
        if (_isPrimaryPointerHold || _isMiddleButtonPress)
        {
            OnMiddleButton?.Invoke();
        }
    }

    void PlayerInput.IPlayerActions.OnSecondaryTouchPosition(InputAction.CallbackContext context)
    {
        _secondaryTouchPosition = context.ReadValue<Vector2>();
        if (_isPointerOnUI) return;
        CanPinch();
    }


    void PlayerInput.IPlayerActions.OnPrimaryPointerPress(InputAction.CallbackContext context)
    {
        PressButtonState(context, ref _isPrimaryPointerPress);
        if (context.phase == InputActionPhase.Performed)
        {
            _isPrimaryPointerHold = true;
        }

        if (!_isPrimaryPointerPress && !_isPrimaryPointerHold)
        {
            Vector2 pointerPosition = new Vector2();
            GetPosition(context, ref pointerPosition);
            if (_isPointerOnUI) return;
            OnPointerPress?.Invoke(pointerPosition);
        }

        if (!_isPrimaryPointerPress)
        {
            _isPrimaryPointerHold = false;
        }
    }

    void PlayerInput.IPlayerActions.OnSecondaryTouchPress(InputAction.CallbackContext context)
    {
        PressButtonState(context, ref _isSecondaryTouchPress);
    }


    private void PressButtonState(InputAction.CallbackContext context, ref bool value)
    {
        if (context.phase == InputActionPhase.Started)
        {
            value = true;
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            value = false;
        }
    }

    void PlayerInput.IPlayerActions.OnScrollZoom(InputAction.CallbackContext context)
    {
        if(_isPointerOnUI) return;
        SendPinchValue(context.ReadValue<Vector2>());
    }

    private void CanPinch()
    {
        if (!(_isPrimaryPointerPress && _isSecondaryTouchPress))
        {
            return;
        }

        _currentDistance = (_pointerPosition - _secondaryTouchPosition).sqrMagnitude;

        if (!Mathf.Approximately(_lastDistance, FirstLastDistance))
        {
            CanSendPinchValue();
            return;
        }

        _lastDistance = _currentDistance;
    }

    private void CanSendPinchValue()
    {
        if (_currentDistance > _lastDistance)
        {
            _lastDistance = _currentDistance;
            SendPinchValue(_pinchVector);
            return;
        }

        _lastDistance = _currentDistance;
        SendPinchValue(_pinchVector * -1);
    }

    private void SendPinchValue(Vector2 value)
    {
        OnPinch?.Invoke(value);
    }
}