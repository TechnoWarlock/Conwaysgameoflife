using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Menu : MonoBehaviour
{
    [SerializeField] private UnityEvent<bool> onChangeMode;
    [SerializeField] private UnityEvent<bool> onPlay;
    [SerializeField] private UnityEvent onStep;
    [SerializeField] private UnityEvent onReset;
    [SerializeField] private UnityEvent onClear;
    [SerializeField] private UnityEvent<float> onSpeedChange;
    // public static event Func<bool, bool> OnPlay;

    private VisualElement _root;
    private Button _modeButton;
    private Button _playButton;
    private Button _stepButton;
    private Button _resetButton;
    private Button _clearButton;
    private Slider _speedSlider;

    private bool _isHexMode;
    private bool _isPlay;

    private const string DefaultModeText = "HexMode";
    private const string AlternateModeText = "RectMode";

    private const string DefaultPlayText = "Play";
    private const string AlternatePlayText = "Pause";
    
    private void Awake()
    {
        GetVisualElements();

        RegisterHoverCallbacks(_modeButton,_playButton,_stepButton,_resetButton,_clearButton,_speedSlider);

        RegisterClickCallbacks();
    }

    private void GetVisualElements()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _modeButton = _root.Q<Button>("Mode");
        _playButton = _root.Q<Button>("Play");
        _stepButton = _root.Q<Button>("Step");
        _resetButton = _root.Q<Button>("Reset");
        _clearButton = _root.Q<Button>("Clear");
        _speedSlider = _root.Q<Slider>("Speed");
    }

    private void RegisterClickCallbacks()
    {
        _modeButton.RegisterCallback<ClickEvent>(args =>
        {
            ToggleButtonState(_modeButton, ref _isHexMode, DefaultModeText, AlternateModeText);
            SetDefaultPlayButtonState();
            CallEvent(onChangeMode, _isHexMode);
        });

        _playButton.RegisterCallback<ClickEvent>(args =>
        {
            CallEvent(onPlay,!_isPlay);
            TogglePlayState(_playButton, ref _isPlay, DefaultPlayText, AlternatePlayText);
        });
        _stepButton.RegisterCallback<ClickEvent>(args =>
        {
            SetDefaultPlayButtonState();
            CallEvent(onStep);
        });
        _resetButton.RegisterCallback<ClickEvent>(args =>
        {
            SetDefaultPlayButtonState();
            CallEvent(onReset);
        });
        _clearButton.RegisterCallback<ClickEvent>(args =>
        {
            SetDefaultPlayButtonState();
            CallEvent(onClear);
        });
        _speedSlider.RegisterValueChangedCallback(args => CallEvent<float>(onSpeedChange, args.newValue));
    }

    private void RegisterHoverCallbacks(params VisualElement[] elements)
    {
        foreach (VisualElement item in elements)
        {
            item.RegisterCallback<MouseEnterEvent>(evt => { SoundTrigger.PreviewCellClip(); });
        }
    }

    private void SetDefaultPlayButtonState()
    {
        _isPlay = true;
        ToggleButtonState(_playButton, ref _isPlay, DefaultPlayText, AlternatePlayText);
    }

    private void ToggleButtonState(Button button, ref bool value, string defaultText, string alternateText)
    {
        
        value = !value;
        button.text = !value ? defaultText : alternateText;
    }

    private void TogglePlayState(Button button, ref bool value, string defaultText, string alternateText)
    {

        if (GlobalData.BoardHasCells)
        {
            value = !value;
            button.text = !value ? defaultText : alternateText;
        }
    }

        
    private void CallEvent<T>(UnityEvent<T> eventCallback, T value)
    {
        eventCallback?.Invoke(value);
        DoSound();
    }

    private void CallEvent(UnityEvent eventCallback)
    {
        eventCallback?.Invoke();
        DoSound();
    }

    private void DoSound()
    {
        SoundTrigger.PutCellClip();
    }
}