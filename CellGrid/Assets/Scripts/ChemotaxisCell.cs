using System.Collections.Generic;
using UnityEngine;

public class ChemotaxisCell : MonoBehaviour
{
    public char cellType; // Example: F, G, H, I, J

    public List<float> chemoattractantAffinities = new List<float>(); // Affinities for chemicals T, U, V, W, X, Y

    
    public void SetChemoattractantAffinities() //assign chemical affinities for this cell
    {
        // Get the master affinities from the manager
        List<float> masterAffinities = ChemotaxisAffinityManager.GetChemoattractantAffinities(cellType);

        // Initialize chemoattractantAffinities as a new list
        chemoattractantAffinities = new List<float>();

        string nametype = this.transform.name + " " + cellType;

        // Copy values from the master list and apply randomness
        for (int i = 0; i < masterAffinities.Count; i++)
        {
            float mastervalue = masterAffinities[i];
            float newvalue = MakeRandFromNormal(mastervalue);
            //Debug.Log(nametype + " " + mastervalue + " " + newvalue);
            chemoattractantAffinities.Add(newvalue);
        }
    }

    // Method to get affinity to a specific chemoattractant
    public float GetAffinityToChemoattractant(int chemoattracindex)
    {
        if (chemoattracindex >= 0 && chemoattracindex < chemoattractantAffinities.Count)
        {
            return chemoattractantAffinities[chemoattracindex];
        }
        return 0f; // Default affinity value if index is out of range
    }

    float MakeRandFromNormal(float mean)
    {
        float result; //value to return
        float stdDev = mean*0.3f;  //make the standard deviation is about 30% of the mean
        //now for the black magic
        float u1 = Random.Range(0.0f, 1.0f);
        float u2 = Random.Range(0.0f, 1.0f);
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
        result = mean + (stdDev * randStdNormal);
        return result;
    }
}

