using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class AudioSourceMuteButton : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Sprite _mutedView;
    [SerializeField] private Sprite _unmutedView;

    private Image _image;
    private Button _button;
    private float _audioSourceDefaultVolume;
    private bool _isMuted;

    private void Start()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnButtonClick);

        _audioSourceDefaultVolume = _audioSource.volume;
    }

    private void OnButtonClick()
    {
        _isMuted = !_isMuted;

        if (_isMuted)
        {
            _audioSource.volume = 0;
            _image.sprite = _mutedView;
        }
        else
        {
            _audioSource.volume = _audioSourceDefaultVolume;
            _image.sprite = _unmutedView;
        }
    }
}
