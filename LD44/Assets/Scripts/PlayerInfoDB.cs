using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Data", menuName = "PlayerInfoDB", order = 1)]
public class PlayerInfoDB : ScriptableObject
{
    [Header("NAMES")]
    public List<string> names;

    [Header("WEAPONS")]
    public List<GameObject> availableWeapons;
    public List<GameObject> unlockableWeapons;

    [Header("VEHICLES")]
    public List<GameObject> availableVehicles;
    public List<GameObject> unlockableVehicles;

    [Header("SKINS")]
    public List<GameObject> availableSkins;
    public List<GameObject> unlockableSkins;

    [Header("HAIRS")]
    public List<GameObject> availableHairs;
    public List<GameObject> unlockableHairs;

}