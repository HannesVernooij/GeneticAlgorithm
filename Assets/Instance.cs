using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class Instance : MonoBehaviour
{
    [SerializeField]
    private Text[] _texts;

    private float _crossOverRate;
    private float _mutationRate;
    private int _populationSize;
    private int _chromosomeLength;
    private int _geneLength;
    private int _maxGenerations;
    private int _targetNumber;


    void Start()
    {
        _targetNumber = 45;
        _crossOverRate = 0.7f;
        _mutationRate = 0.001f;
        _populationSize = 100;          // must be an even number
        _chromosomeLength = 300;
        _geneLength = 4;
        _maxGenerations = 400;

        string offspring1, offspring2;


        Chromosome[] chromosomes = GenerateInstances();
        Chromosome a = GetRandomChromosome(chromosomes);
        Chromosome b = GetRandomChromosome(chromosomes);
        Chromosome[] crossedChromosomes = Crossover(a, b);
        Mutate(crossedChromosomes);

    }

    private Chromosome[] GenerateInstances()
    {
        Chromosome[] chromosomes = new Chromosome[_populationSize];
        for (int i = 0; i < chromosomes.Length; i++)
        {
            chromosomes[i] = new Chromosome(_geneLength,_chromosomeLength, _targetNumber);
        }
        return chromosomes;
    }

    private Chromosome[] Crossover(Chromosome a, Chromosome b)
    {
        Debug.Log("A= " + a.Fitness + " B= " + b.Fitness);
        if(UnityEngine.Random.Range(0.0f,1.0f) < _crossOverRate)
        {
            //create crossover point
            int crossover = UnityEngine.Random.Range(0, _chromosomeLength);

            Gene[] genesA = a.Genes;
            Gene[] genesB = b.Genes;

            Gene[] genesC = new Gene[_chromosomeLength];
            Gene[] genesD = new Gene[_chromosomeLength];

            //cross the genes
            for (int i = 0; i < _chromosomeLength; i++)
            {
                if(i < crossover)
                {
                    genesC[i] = genesA[i];
                    genesD[i] = genesB[i];
                }
                else
                {
                    genesC[i] = genesB[i];
                    genesD[i] = genesA[i];
                }
            }
            Chromosome c = new Chromosome(genesC, _targetNumber);
            Chromosome d = new Chromosome(genesD, _targetNumber);
            Chromosome[] crossedChromosomes = new Chromosome[2] { c, d };
            return crossedChromosomes;
        }
        return new Chromosome[2] { a, b };
    }

    private void Mutate(Chromosome[] chromosomes)
    {
        for (int i = 0; i < chromosomes.Length; i++)
        {
            Chromosome c = chromosomes[i];
            for (int j = 0; j < c.Genes.Length; j++)
            {
                Gene g = c.Genes[j];
                for (int k = 0; k < g.Value.Length; k++)
                {
                    char b = g.Value[k];
                    if(UnityEngine.Random.Range(0.0f,1.0f) < _mutationRate)
                    {
                        char[] str = g.Value.ToCharArray();
                        str[k] = (b == '0' ? '1' : '0');
                        g.Value = str.ToString();
                    }
                }
            }
        }

    }

    private Chromosome GetRandomChromosome(Chromosome[] chromosomes)
    {
        float max = 0;

        // set total fitness value
        for (int i = 0; i < chromosomes.Length; i++)
        {
            max += chromosomes[i].Fitness;
        }

        float randomValue = UnityEngine.Random.Range(0.0f, max);
        float fitnessCount = 0;
        Debug.Log(randomValue);

        // find chromosome
        for (int i = 0; i < chromosomes.Length; i++)
        {
            Chromosome c = chromosomes[i];
            fitnessCount += c.Fitness;
            if (fitnessCount > randomValue)
            {
                return chromosomes[i];
            }
        }
        return null;
    }
}
