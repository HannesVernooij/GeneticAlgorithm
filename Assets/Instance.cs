using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

using System.Diagnostics;

public class Instance : MonoBehaviour
{

    private float _crossOverRate;
    private float _mutationRate;
    private int _populationSize;
    private int _chromosomeLength;
    private int _geneLength;
    private int _targetNumber;
    [SerializeField]
    private GameObject _builderPrefab;
    private Turtle[] _turtles;
    private Turtle _existingTurtle;
    [SerializeField]
    private InputField _formulaTxt;
    [SerializeField]
    private Text _text, _fitnessTexts;
    private int _currentGen;
    private float _currentFitness;
    Chromosome[] _chromosomes;


    private void Start()
    {
        _targetNumber = 45;
        _crossOverRate = 0.7f;
        _mutationRate = 0.001f;
        _populationSize = 200;          // must be an even number
        _chromosomeLength = 1000;
        _geneLength = 4;
        _currentGen = 0;
        _chromosomes = GenerateInstances();

        Evolve();
    }

    private void Evolve()
    {
        for (int i = 0; i < 1; i++)
        {
            Chromosome a = GetRandomChromosome(_chromosomes);
            Chromosome b = GetRandomChromosome(_chromosomes);
            Chromosome[] crossedChromosomes = Crossover(a, b);
            Mutate(crossedChromosomes);
            List<Chromosome> ll = _chromosomes.ToList().OrderByDescending(x => x.Fitness).ToList();
            ll.Add(crossedChromosomes[0]);
            ll.Add(crossedChromosomes[1]);
            ll.Remove(ll[ll.Count - 1]);
            ll.Remove(ll[ll.Count - 1]);
            for (int j = 0; j < _populationSize/10; j++)
            {
                ll[ll.Count - (i+1)] = new Chromosome(_geneLength, _chromosomeLength);
            }
            ll = ll.OrderByDescending(x => x.Fitness).ToList();
            _chromosomes = ll.ToArray();

            Chromosome best = ll[0];
            BuildLSystem(best);
            _currentFitness = best.Fitness;
            _currentGen++;

            _text.text = "Current Generation = " + _currentGen + " Current Fitness = " + _currentFitness;
            _fitnessTexts.text = "";
            for (int j = 0; j < ll.Count; j+=4)
            {
                _fitnessTexts.text += ll[j].Fitness + "     " + ll[j+1].Fitness + "     " + ll[j+2].Fitness + "     " + ll[j + 3].Fitness +  "\n";
            }
        }
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(.4f);
        Evolve();
    }

    private Chromosome[] GenerateInstances()
    {
        Chromosome[] chromosomes = new Chromosome[_populationSize];
        for (int i = 0; i < chromosomes.Length; i++)
        {
            chromosomes[i] = new Chromosome(_geneLength, _chromosomeLength);
        }
        return chromosomes;
    }

    private Chromosome[] Crossover(Chromosome a, Chromosome b)
    {
        if (UnityEngine.Random.Range(0.0f, 1.0f) < _crossOverRate)
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
                if (i < crossover)
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
            Chromosome c = new Chromosome(genesC);
            Chromosome d = new Chromosome(genesD);
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
                    if (UnityEngine.Random.Range(0.0f, 1.0f) < _mutationRate)
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

        float randomValue = Random.Range(0.0f, max * 0.4f);
        float fitnessCount = 0;
        //Debug.Log(randomValue);

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

    private void BuildLSystem(Chromosome c)
    {
        if (_existingTurtle != null)
        {
            _existingTurtle.Destroy();
            Destroy(_existingTurtle.gameObject);
        }

        GameObject temp = Instantiate(_builderPrefab);
        _existingTurtle = temp.GetComponent<Generate>().SetChromosome(c);
        _formulaTxt.text = "F -> " + c.Formula;
    }
}
