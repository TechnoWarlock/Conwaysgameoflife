using System;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip putCellClip;
    [SerializeField] private AudioClip removeCellClip;
    [SerializeField] private AudioClip previewCellClip;

    private static AudioSource _audioSource;

    private static event Func<AudioClip> OnPutCell; 
    private static event Func<AudioClip> OnPreviewCell; 
    private static event Func<AudioClip> OnRemoveCell; 
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        OnPutCell = () => putCellClip;
        OnPreviewCell = () => previewCellClip;
        OnRemoveCell = () => removeCellClip;
    }
    
    public static void PutCellClip()
    {
        _audioSource.PlayOneShot(OnPutCell?.Invoke());       
        
    }
    public static void RemoveCellClip()
    {
        _audioSource.PlayOneShot(OnRemoveCell?.Invoke());       
    }
    public static void PreviewCellClip()
    {
        _audioSource.PlayOneShot(OnPreviewCell?.Invoke());       
    }
    
    
}