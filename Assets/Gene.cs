using UnityEngine;
using System.Collections;

public class Gene
{
    private string _value;
    public string Value { get { return _value; } set { _value = value; } }

    public Gene(int geneLength)
    {
        _value = "";
        for (int i = 0; i < geneLength; i++)
        {
            _value += (Random.Range(0.0f, 1.0f) > 0.5 ? '1' : '0');
        }
    }
}
