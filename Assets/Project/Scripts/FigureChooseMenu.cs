using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FigureChooseMenu : MonoBehaviour
{
    [Header("Menu Elements")]
    [SerializeField] private Image _backgroundPanel;
    [SerializeField] private Transform _figuresCardsChoiceContainer;
    [SerializeField] private Transform _figuresCardsAnimationContainer;
    [SerializeField] private FigureCard _figureCardPrefab;
    [Header("Figures Cards Positions")]
    [SerializeField] private Vector3 _attackingSideStartPosition;
    [SerializeField] private Vector3 _attackingSideEndPosition;
    [SerializeField] private Vector3 _defendingSideStartPosition;
    [SerializeField] private Vector3 _defendingSideEndPosition;
    [Header("Animation Settings")]
    [SerializeField] private float _animationFiguresCardsSpeed;
    [SerializeField] private float _pauseBeforeFiguresCardsStartMoving;
    [SerializeField] private float _pauseAfterFiguresCardsEndMoving;

    private List<FigureCard> _createdFiguresCards = new List<FigureCard>();
    private Coroutine _displayChosenFiguresInJob;

    public bool IsFiguresAnimationRunning { get; private set; }

    public event UnityAction<FiguresCore.FigureInfo> FigureChosen;

    public void CreateFiguresCards()
    {
        DestroyAllCreatedCards();
        _backgroundPanel.enabled = true;
        List<FiguresCore.FigureInfo> allFigures = FiguresCore.GetInstance().GetAllFigures();
        FigureCard newFigureCard;

        foreach (var figure in allFigures)
        {
            newFigureCard = Instantiate(_figureCardPrefab, _figuresCardsChoiceContainer);
            newFigureCard.Init(figure);
            newFigureCard.SetNewButtonEnabledValue(true);
            newFigureCard.Click += OnFigureCardClick;

            _createdFiguresCards.Add(newFigureCard);
        }
    }

    public void StartDisplayingChosenFigures(FiguresCore.FigureInfo attackingSideFigure, FiguresCore.FigureInfo defendingSideFigure)
    {
        if (_displayChosenFiguresInJob != null)
            StopCoroutine(_displayChosenFiguresInJob);

        _displayChosenFiguresInJob = StartCoroutine(DisplayChosenFigures(attackingSideFigure, defendingSideFigure));
    }

    private void DestroyAllCreatedCards()
    {
        foreach (var figureCard in _createdFiguresCards)
        {
            figureCard.Click -= OnFigureCardClick;
            Destroy(figureCard.gameObject);
        }

        _createdFiguresCards = new List<FigureCard>();
    }

    private IEnumerator DisplayChosenFigures(FiguresCore.FigureInfo attackingSideFigure, FiguresCore.FigureInfo defendingSideFigure)
    {
        _backgroundPanel.enabled = false;
        IsFiguresAnimationRunning = true;
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        FigureCard attackingSideFigureCard = Instantiate(_figureCardPrefab, _figuresCardsAnimationContainer);
        attackingSideFigureCard.transform.localPosition = _attackingSideStartPosition;
        FigureCard defendingSideFigureCard = Instantiate(_figureCardPrefab, _figuresCardsAnimationContainer);
        defendingSideFigureCard.transform.localPosition = _defendingSideStartPosition;
        defendingSideFigureCard.transform.rotation = Quaternion.Euler(0, 0, defendingSideFigureCard.transform.localEulerAngles.z + 180);
        
        attackingSideFigureCard.Init(attackingSideFigure);
        attackingSideFigureCard.SetNewButtonEnabledValue(false);
        defendingSideFigureCard.Init(defendingSideFigure);
        defendingSideFigureCard.SetNewButtonEnabledValue(false);

        _createdFiguresCards.Add(attackingSideFigureCard);
        _createdFiguresCards.Add(defendingSideFigureCard);

        yield return new WaitForSeconds(_pauseBeforeFiguresCardsStartMoving);

        while (attackingSideFigureCard.transform.localPosition != _attackingSideEndPosition || defendingSideFigureCard.transform.localPosition != _defendingSideEndPosition)
        {
            attackingSideFigureCard.transform.localPosition = Vector2.MoveTowards(attackingSideFigureCard.transform.localPosition, _attackingSideEndPosition, _animationFiguresCardsSpeed * Time.deltaTime);
            defendingSideFigureCard.transform.localPosition = Vector2.MoveTowards(defendingSideFigureCard.transform.localPosition, _defendingSideEndPosition, _animationFiguresCardsSpeed * Time.deltaTime);
            yield return waitForEndOfFrame;
        }

        yield return new WaitForSeconds(_pauseAfterFiguresCardsEndMoving);
        IsFiguresAnimationRunning = false;
    }

    private void OnFigureCardClick(FiguresCore.FigureInfo figureInfo)
    {
        FigureChosen?.Invoke(figureInfo);
        DestroyAllCreatedCards();
    }
}
