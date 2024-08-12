using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiguresCore : MonoBehaviour
{
    [SerializeField] private List<FigureInfo> _allFigures;

    private static FiguresCore _instance;

    public enum Types
    {
        Rock,
        Paper,
        Scissors,
    }

    private void Awake()
    {
        if (_instance != null)
            Debug.LogWarning($"Ѕыло найдено более одного экземпл€ра класса {_instance.GetType()}. “ребуетс€ проверка.");

        _instance = this;
    }

    public static FiguresCore GetInstance() => _instance;

    public List<FigureInfo> GetAllFigures()
    {
        List<FigureInfo> listForReturning = new List<FigureInfo>();

        foreach (var figure in _allFigures)
            listForReturning.Add(figure);

        return listForReturning;
    }

    [System.Serializable]
    public struct FigureInfo
    {
        public Types Figure;
        public Sprite Icon;
        public List<Types> Weaknesses;
    }
}
