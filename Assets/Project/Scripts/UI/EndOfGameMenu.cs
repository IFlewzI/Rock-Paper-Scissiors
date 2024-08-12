using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class EndOfGameMenu : MonoBehaviour
{
    [SerializeField] private Transform _victoryScreen;
    [SerializeField] private Transform _drawScreen;
    [SerializeField] private Transform _defeatScreen;
    [SerializeField] private Button _restartButton;

    private const string FadeAnimationTriggerName = "Fade";
    private const string UnfadeAnimationTriggerName = "Unfade";
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _restartButton.onClick.AddListener(OnRestartButtonClick);
    }

    public void DisplayEndOfGameScreen(int battleResult)
    {
        MakeAllScreensInactive();

        switch (battleResult)
        {
            case -1:
                _defeatScreen.gameObject.SetActive(true);
                break;
            case 0:
                _drawScreen.gameObject.SetActive(true);
                break;
            case 1:
                _victoryScreen.gameObject.SetActive(true);
                break;
        }

        Unfade();
    }

    private void Fade() => _animator.SetTrigger(FadeAnimationTriggerName);

    private void Unfade() => _animator.SetTrigger(UnfadeAnimationTriggerName);

    private void MakeAllScreensInactive()
    {
        _defeatScreen.gameObject.SetActive(false);
        _drawScreen.gameObject.SetActive(false);
        _victoryScreen.gameObject.SetActive(false);
    }

    private void OnRestartButtonClick()
    {
        BattleManager.GetInstance().StartBattle();
        Fade();
    }
}
