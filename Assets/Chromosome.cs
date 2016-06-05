using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Chromosome
{
    private Gene[] _genes;
    private float _fitness;
    StringToCalculation _calculator;
    private string _calculationString;

    public string ByteValue { get { return GetChromosome(); } }
    public Gene[] Genes { get { return _genes; } set { _genes = value; } }
    public float Fitness { get { return _fitness; } }
    public string CalculationString { get { return _calculationString; } }

    public Chromosome(int geneLength, int chromosomeLength, int targetNumber)
    {
        _calculator = new StringToCalculation();
        _genes = new Gene[chromosomeLength];
        for (int i = 0; i < _genes.Length; i++)
        {
            _genes[i] = new Gene(geneLength);
        }
        Translate(targetNumber);
    }

    public Chromosome(Gene[] genes, int targetNumber)
    {
        _genes = genes;
        Translate(targetNumber);
    }

    private string GetChromosome()
    {
        string chromosome = "";
        for (int i = 0; i < _genes.Length; i++)
        {
            chromosome += _genes[i].Value;
        }
        return chromosome;
    }

    private string[] GetGenes()
    {
        string[] genes = new string[_genes.Length];

        for (int i = 0; i < _genes.Length; i++)
        {
            genes[i] = _genes[i].Value;
        }
        return genes;
    }

    private void Translate(int targetNumber)
    {
        string result = "";
        bool lastGeneIsNumber = false;

        for (int i = 0; i < Genes.Length; i++)
        {
            string gene = Genes[i].Value;

            if (!lastGeneIsNumber)
            {
                lastGeneIsNumber = true;
                switch (gene)
                {
                    //case "0000":
                    //result += "0";
                    //break;
                    case "0001":
                        result += "1";
                        break;
                    case "0010":
                        result += "2";
                        break;
                    case "0011":
                        result += "3";
                        break;
                    case "0100":
                        result += "4";
                        break;
                    case "0101":
                        result += "5";
                        break;
                    case "0110":
                        result += "6";
                        break;
                    case "0111":
                        result += "7";
                        break;
                    case "1000":
                        result += "8";
                        break;
                    case "1001":
                        result += "9";
                        break;
                    default:
                        lastGeneIsNumber = false;
                        break;
                }
            }
            else
            {
                lastGeneIsNumber = false;
                switch (gene)
                {
                    case "1010":
                        result += "+";
                        break;
                    case "1011":
                        result += "-";
                        break;
                    case "1100":
                        result += "*";
                        break;
                    case "1101":
                        result += "/";
                        break;
                    default:
                        lastGeneIsNumber = true;
                        break;
                }
            }
        }
        int last = result.Length - 1;
        char lastC = result[last];
        if (lastGeneIsNumber == false)
        {
            result = result.Remove(last, 1);
        }
        _calculationString = result;
        float answer = _calculator.GetAnswer(result);
        _fitness = Mathf.Abs((targetNumber - answer));
    }
}
