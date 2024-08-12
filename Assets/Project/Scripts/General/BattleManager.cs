using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FiguresCore;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private FigureChooseMenu _figureChooseMenu;
    [SerializeField] private EndOfGameMenu _endOfGameMenu;

    private static BattleManager _instance;
    private Coroutine _executeBattleModeInJob;
    private FigureInfo _playerFigure;
    private bool _isPlayerChosenFigure = false;

    private void Awake()
    {
        if (_instance != null)
            Debug.LogWarning($"Ѕыло найдено более одного экземпл€ра класса {_instance.GetType()}. “ребуетс€ проверка.");

        _instance = this;
    }

    private void Start()
    {
        _figureChooseMenu.FigureChosen += OnFigureChosen;
        StartBattle();
    }

    public static BattleManager GetInstance() => _instance;

    public void StartBattle()
    {
        if (_executeBattleModeInJob != null)
            StopCoroutine(_executeBattleModeInJob);

        _executeBattleModeInJob = StartCoroutine(ExecuteBattleMode());
    }

    public int CalculateBattleResult(FigureInfo _attackingSide, FigureInfo _defendingSide)
    {
        int battleResult = 0; // -1 for attacking side defeat; 0 for draw; 1 for attacking side victory;

        if (_attackingSide.Weaknesses.Contains(_defendingSide.Figure))
            battleResult = -1;
        else if (_defendingSide.Weaknesses.Contains(_attackingSide.Figure))
            battleResult = 1;

        return battleResult;
    }

    private IEnumerator ExecuteBattleMode()
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        List<FigureInfo> allFigures = FiguresCore.GetInstance().GetAllFigures();
        FigureInfo enemyFigure = allFigures[Random.Range(0, allFigures.Count)];
        int battleResult;

        _figureChooseMenu.CreateFiguresCards();

        while (_isPlayerChosenFigure == false)
            yield return waitForEndOfFrame;

        _isPlayerChosenFigure = false;
        _figureChooseMenu.StartDisplayingChosenFigures(_playerFigure, enemyFigure);
        battleResult = CalculateBattleResult(_playerFigure, enemyFigure);

        while (_figureChooseMenu.IsFiguresAnimationRunning)
            yield return waitForEndOfFrame;

        _endOfGameMenu.DisplayEndOfGameScreen(battleResult);
    }

    private void OnFigureChosen(FigureInfo figureInfo)
    {
        _playerFigure = figureInfo;
        _isPlayerChosenFigure = true;
    }
}
