using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class Turtle : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _branchPrefab;

    private Transform[] _currentPosRot;
    private Stack<Vector3> _savedPos = new Stack<Vector3>();
    private Stack<Quaternion> _savedRot = new Stack<Quaternion>();

    private char[] _usingAlphabet;
    private int[] _xRotations;
    private int[] _yRotations;
    private int[] _zRotations;
    private int _startRotation;
    private bool[,] _matrixValues;
    private int _commandIterator;
    private string _commandString = "";
    private char[] _commands;
    private List<GameObject> _objects = new List<GameObject>();

    public void SetValues(char[] alphabetArray, int[] xRot, int[] yRot, int[] zRot, int startRot, bool[,] matrixValues, GameObject[] branchPrefabs)
    {
        _usingAlphabet = new char[6] { 'F', 'G', '+', '-', '[', ']' };//alphabetArray;
        _xRotations = xRot;
        _yRotations = yRot;
        _zRotations = zRot;
        //_matrixValues = new bool[43] { false,true,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,true,false,false,false,false,true,false,false,false,false,false,false,true,false,false,true,false,false,false,false,false };
        _branchPrefab = branchPrefabs;
        _currentPosRot = new Transform[1] { gameObject.transform };
        _currentPosRot[0].rotation = gameObject.transform.rotation *= Quaternion.Euler(startRot, 0, 0);
    }

    public void SetString(string commandString)
    {
        _commandString = commandString;
        _commandIterator = -1;
        _commands = commandString.ToCharArray();
        _currentPosRot = new Transform[1] { gameObject.transform };
        _currentPosRot[0].rotation = gameObject.transform.rotation *= Quaternion.Euler(0, 0, 0);
    }

    public void Update()
    {

        _commandIterator++;
        if (_commandString != "" && _commandIterator < _commands.Length)
        {
            for (int i = 0; i < _currentPosRot.Length; i++)
            {
                for (int j = 0; j < _commands.Length; j++)
                {
                    if (_commands[j] == 'F') MoveAndCreateBranch(i);
                    if (_commands[j] == 'G') Move(i);
                    if (_commands[j] == '[') SavePos(i);
                    if (_commands[j] == ']') LoadPos(i);
                    if (_commands[j] == '+') RotateX(i, 25);
                    if (_commands[j] == '-') RotateX(i, -25);
                }
            }
            _commandString = "";
        }
        else if (_commandString != "")
        {
            _commandString = "";
        }
    }
    private void MoveAndCreateBranch(int iteration)
    {
        GameObject temp = Instantiate(_branchPrefab[0], _currentPosRot[iteration].position, _currentPosRot[iteration].rotation) as GameObject;
        _currentPosRot[iteration].position += _currentPosRot[iteration].transform.up * 10f;
        _objects.Add(temp);
    }

    private void Move(int iteration)
    {
        _currentPosRot[iteration].position += _currentPosRot[iteration].transform.up * 10f;
    }

    private void RotateX(int iteration, int amount)
    {
        _currentPosRot[iteration].rotation = _currentPosRot[iteration].rotation * Quaternion.Euler(amount, 0, 0);
    }

    private void RotateY(int iteration, int amount)
    {
        _currentPosRot[iteration].rotation = _currentPosRot[iteration].rotation * Quaternion.Euler(0, amount, 0);
    }

    private void RotateZ(int iteration, int amount)
    {
        _currentPosRot[iteration].rotation = _currentPosRot[iteration].rotation * Quaternion.Euler(0, 0, amount);
    }

    private void SavePos(int iteration)
    {
        _savedPos.Push(_currentPosRot[iteration].position);
        _savedRot.Push(_currentPosRot[iteration].rotation);
    }

    private void LoadPos(int iteration)
    {
        _currentPosRot[iteration].position = _savedPos.Pop();
        _currentPosRot[iteration].rotation = _savedRot.Pop();
    }

    private void MoveAndCreateLeafBranch(int iteration)
    {
        GameObject temp = Instantiate(_branchPrefab[1], _currentPosRot[iteration].position, _currentPosRot[iteration].rotation) as GameObject;
        _currentPosRot[iteration].position += _currentPosRot[iteration].transform.up * 10f;
        _objects.Add(temp);
    }

    public void Destroy()
    {
        for (int i = 0; i < _objects.Count; i++)
        {
            Destroy(_objects[i]);
        }
    }
}
