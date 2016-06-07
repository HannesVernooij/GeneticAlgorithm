using System;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Diagnostics;

[RequireComponent(typeof(Turtle))]
public class Generate : MonoBehaviour
{
    private int _generation = 0;
    private StringBuilder _stringBuilder;
    private string[] _words;
    private string _axiom = "F";
    private int _iterationSpeed = 1000;
    private int _targetGeneration = 3;
    private int _lli1, _lli2;
    private Stopwatch _stopWatch;

    private Chromosome _chromosome;

    private void Awake()
    {
        _stopWatch = new Stopwatch();
    }
    public Turtle SetChromosome(Chromosome c)
    {
        _stringBuilder = new StringBuilder();
        _stringBuilder.Append(_axiom);
        _chromosome = c;
        return Activate();
    }

    private Turtle Activate()
    {
        _stopWatch.Start();
        while (_generation < _targetGeneration)
        {
            SplittingOldString();
            CalculateString();
            FillNewString();
            _generation++;
        }
        return BuildGeneration();
    }

    private void SplittingOldString()
    {
        _words = new string[_stringBuilder.Length];
        for (int i = 0; i < _iterationSpeed; i++)
        {
            if (_lli1 < _stringBuilder.Length)
            {
                _words[_lli1] = _stringBuilder[_lli1].ToString();
                _lli1++;
                if (_lli1 >= _words.Length)
                {
                    _lli1 = 0;
                    return;
                }
            }
        }
        if (_lli1 < _words.Length)
        {
            SplittingOldString();
        }
    }

    void CalculateString()
    {
        for (int i = 0; i < _iterationSpeed; i++)
        {
            if (_lli1 < _words.Length)
            {
                if (_words[_lli1] == "F")
                {
                    _words[_lli1] = _chromosome.Formula;
                }
                _lli1++;
            }
            else
            {
                _lli1 = 0;
                return;
            }
        }
        if (_lli1 < _words.Length)
        {
            CalculateString();
        }
    }

    private void FillNewString()
    {
        for (int i = 0; i < _iterationSpeed; i++)
        {
            if (_lli1 < _words.Length)
            {
                _stringBuilder.Append(_words[i]);
                _lli1++;
            }
            else
            {
                _lli1 = 0;
                return;
            }
        }
        if (_lli1 < _words.Length)
        {
            FillNewString();
        }
    }

    public Turtle BuildGeneration()
    {
        UnityEngine.Debug.Log("Generate string: " + _stopWatch.ElapsedMilliseconds);
        _stopWatch.Stop();
        Turtle turtle = GetComponent<Turtle>();
        turtle.enabled = true;
        turtle.SetString(_stringBuilder.ToString());
        return turtle;
    }

}
