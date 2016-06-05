using UnityEngine;
using System.Collections.Generic;

public class StringToCalculation
{
    List<float> _calculationNumbers;
    List<char> _operators;

    public float GetAnswer(string calculation)
    {
        GetCalculation(calculation);
        return Calculate();
    }

    private void GetCalculation(string calculation)
    {
        _calculationNumbers = new List<float>();
        _operators = new List<char>();

        for (int i = 0; i < calculation.Length; i++)
        {
            char c = calculation[i];
            if (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9')
            {
                _calculationNumbers.Add((int)c - 48);
            }
            else if (c == '+' || c == '-' || c == '*' || c == '/')
            {
                _operators.Add(c);
            }
        }
    }


    private float Calculate()
    {
        List<float> newNumberList = new List<float>();
        List<char> newOperatorList = new List<char>();

        if (_calculationNumbers.Count == 1) return _calculationNumbers[0];
        for (int i = 0; i < _operators.Count; i++)
        {
            char o;
            char no = 'x';
            o = _operators[i];
            if (i + 1 < _operators.Count) no = _operators[i + 1];

            if (o == '*' || o == '/')
            {
                if (i == 0)
                {
                    newNumberList.Add(ExecuteCalculation(new Calculation(_calculationNumbers[i], o, _calculationNumbers[i + 1])));
                    Debug.Log(newNumberList[newNumberList.Count - 1]);
                }
                else
                {
                    float number = ExecuteCalculation(new Calculation(newNumberList[newNumberList.Count - 1], o, _calculationNumbers[i + 1]));
                    newNumberList[newNumberList.Count - 1] = number;
                    Debug.Log(newNumberList[newNumberList.Count - 1]);
                }

                if (i + 1 < _operators.Count)
                {
                    if (no == '+' || no == '-')
                    {
                        newOperatorList.Add(no);
                        Debug.Log(newOperatorList[newOperatorList.Count - 1]);
                    }
                }
            }
            else
            {
                newNumberList.Add(_calculationNumbers[i]);
                newOperatorList.Add(o);
            }
        }
        if(newOperatorList.Count > 0) newNumberList.Add(_calculationNumbers[_calculationNumbers.Count - 1]);
        float answer = 0;

        if (newNumberList.Count == 1) return newNumberList[0];
        for (int i = 0; i < newOperatorList.Count; i++)
        {
            answer += ExecuteCalculation(new Calculation(newNumberList[i], newOperatorList[i], newNumberList[i + 1]));
        }
        return answer;
    }

    private float ExecuteCalculation(Calculation c)
    {
        switch (c.O)
        {
            case '+':
                return (float)c.X + (float)c.Y;
            case '-':
                return (float)c.X - (float)c.Y;
            case '*':
                return (float)c.X * (float)c.Y;
            case '/':
                return (float)c.X / (float)c.Y;
            default: throw new System.Exception("Operator is not valid");
        }
    }
}