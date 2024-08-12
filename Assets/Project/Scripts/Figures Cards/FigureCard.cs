using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class FigureCard : MonoBehaviour
{
    [SerializeField] private Image _image;

    private Button _button;

    public FiguresCore.FigureInfo FigureInfo { get; private set; }

    public event UnityAction<FiguresCore.FigureInfo> Click;

    public void Init(FiguresCore.FigureInfo figureInfo)
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnButtonClick);

        FigureInfo = figureInfo;
        _image.sprite = FigureInfo.Icon;
        _image.SetNativeSize();
    }

    public void SetNewButtonEnabledValue(bool newValue) => _button.enabled = newValue;

    private void OnButtonClick() => Click?.Invoke(FigureInfo);
}
