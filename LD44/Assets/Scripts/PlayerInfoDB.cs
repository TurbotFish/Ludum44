using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Data", menuName = "PlayerInfoDB", order = 1)]
public class PlayerInfoDB : ScriptableObject
{
    public List<string> names;
}