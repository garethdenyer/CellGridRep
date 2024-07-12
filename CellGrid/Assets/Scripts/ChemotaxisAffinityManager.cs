using System.Collections.Generic;
using UnityEngine;


public static class ChemotaxisAffinityManager
{
    //values in affinity table are for Diluent, fMLP, PMA, LPS, C5a, LTB4

    private static Dictionary<char, List<float>> affinityTable = new Dictionary<char, List<float>>
    {
        {'F', new List<float> {0.01f, 0.1f, 0.9f, 0.9f, 0.9f, 0.8f}},  //F is a normal control - doesn't migrate to fMLP but does to others
        {'G', new List<float> {0.01f, 0.01f, 0.01f, 0.01f, 0.01f, 0.01f}}, //G is a migration deficient cell (maximal inhibition) - don't migrate
        {'H', new List<float> {0.01f, 0.5f, 0.4f, 0.4f, 0.35f, 0.4f}}, //H is a partial inhibition
        {'I', new List<float> {0.01f, 0.25f, 0.3f, 0.2f, 0.25f, 0.15f}}, //I is moderate inhibition
        {'J', new List<float> {0.01f, 0.1f, 0.1f, 0.2f, 0.2f, 0.1f}} //J is complete inhibition
    };

    public static List<float> GetChemoattractantAffinities(char cellType)
    {
        if (affinityTable.ContainsKey(cellType))
        {
            return affinityTable[cellType];
        }
        else
        {
            Debug.LogWarning($"No affinity data found for cell type: {cellType}");
            return new List<float>(); // Return an empty list or handle error as per your needs
        }
    }



}
