using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Chromosome
{
    private Gene[] _genes;
    private int _fitness;
    private string _formula;

    public string ByteValue { get { return GetChromosome(); } }
    public float Fitness { get { return _fitness; } }
    public Gene[] Genes { get { return _genes; } }
    public string Formula { get { return _formula; } }

    public Chromosome(int geneLength, int chromosomeLength)
    {
        _genes = new Gene[chromosomeLength];
        for (int i = 0; i < _genes.Length; i++)
        {
            _genes[i] = new Gene(geneLength);
        }
        Translate();
    }

    public Chromosome(Gene[] genes)
    {
        _genes = genes;
        Translate();
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

    private void Translate()
    {
        string result = "";

        for (int i = 0; i < _genes.Length; i++)
        {
            string gene = _genes[i].Value;

            switch (gene)
            {


                //[[[[[X]+FX]X]+]+F+F-FFX]F
                //[[-FX[X] - FX]+F-FX]F
                //[+F - FX] F
                //[[[[X]-FX]+F-FX]+F-FX]F
                //[[-FX] F]
                //[+FX]-F-F[+F - F] F
                //[[[X]-FX]+F-FX]F
                //[+FX]-F-F[+F + F] F
                //[[-F+FX]-F-F]F

                // score = -Mathf.Abs([ count - ]) * 100 -> only give a chance to correct bracket formulas
                // score += F between correct brackets * 5
                //___________________________________maybe_________________________________________
                // score += Mathf.Abs((amount of +) - (amount of -) between F)


                //case "0000":
                //result += "0";
                //break;
                case "0001":
                    result += "F";  //branch
                    break;
                //case "0010":
                //result += "G";  // move
                //break;
                case "0011":
                    result += "+";  // rotateLeft
                    break;
                case "0100":
                    result += "-";  // rotateRight
                    break;
                case "0101":
                    result += "[";  // save pos
                    break;
                case "0110":
                    result += "]";  // load pos
                    break;
                    //case "0111":
                    //    result += "*";
                    //    break;
                    //case "1000":
                    //    result += "/";
                    //    break;
                    //case "1001":
                    //    result += "9";
                    //    break;
                    //default:
                    //    lastGeneIsNumber = false;
                    //    break;
            }

        }

        result = ImproveFormula(result);


        _formula = result;
        _fitness = CalculateFitness();
        Debug.Log("Fitness = " + _fitness.ToString());
    }

    private string ImproveFormula(string s)
    {
        //Fix save operators
        int passedSaveAmount = 0;
        for (int i = 0; i < s.Length; i++)
        {
            char c = s[i];
            if (c == '[') passedSaveAmount++;
            else if (c == ']' && passedSaveAmount > 0) passedSaveAmount--;
            else if (c == ']') s = s.Remove(i);
        }
        while (passedSaveAmount > 0)
        {
            s += "]";
            passedSaveAmount--;
        }

        // remove unnecesary rotations
        s = RemoveCharsIfNextToEachother('+', '-', s);
        s = RemoveCharsIfNextToEachother('-', '+', s);

        // remove empty brackets
        s = RemoveCharsIfNextToEachother('[', ']', s);

        // remove useless Brackets
        int depth = 0;
        for (int i = 0; i < s.Length; i++)
        {
            char c = s[i];
            if (c == '[')
            {
                depth = 0;
                for (int j = i; j < s.Length; j++)
                {
                    c = s[j];
                    if (c == '[') depth++;
                    else if (c == ']' && depth > 0) depth--;
                    else if (c == ']' && depth == 0)
                    {
                        RemoveUselessBrackets(s, i, j);
                        break;
                    }
                }
            }
        }


        return s;
    }

    private string RemoveUselessBrackets(string s, int savePos, int loadPos)
    {
        //check for each part if it is usefull
        if (!UsefulCheck(s, savePos, loadPos))
        {
            s = s.Remove(savePos, 1);
            s = s.Remove(loadPos - 1, 1);
        }
        return s;
    }

    private bool UsefulCheck(string s, int start, int end)
    {
        for (int i = start; i < end; i++)
        {
            char c = s[i];
            if (c == 'F')
            {
                return true;
            }
        }
        return false;
    }

    private string RemoveCharsIfNextToEachother(char cb, char ce, string s)
    {
        for (int i = 0; i < s.Length - 1; i++)
        {
            if (s[i] == cb && s[i + 1] == ce)
            {
                s = s.Remove(i, 2);
                i--;
            }
        }
        return s;
    }

    private int CalculateFitness()
    {
        string s = _formula;
        int fitness = 0;

        fitness += CountCharInString('[', s);
        fitness += CountCharInString('F', s) * 2;
        fitness -= CountCharInString('-', s);
        fitness -= CountCharInString('+', s);
        return fitness;
    }

    private int CountCharInString(char c, string s)
    {
        int count = 0;
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == c) count++;
        }
        return count;
    }
}
