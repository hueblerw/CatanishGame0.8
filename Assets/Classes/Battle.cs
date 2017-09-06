using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle {

    string type;
    public List<Character> sideA;
    public List<Being> sideB;
    public Wildlife battlefield;
    public Vector3[] unitLocations;

    public Battle(string type, List<Character> protagonist, Hex embattledTile)
    {
        if (type == "encounter")
        {
            // Generate a Battlefield
        }
        else if (type == "foundation")
        {
            // Assign the town founders
            sideA = protagonist;
            // Generate the enemies to be fought
            sideB = GenerateEnemies(embattledTile);
            // Generate a Battlefield at the location the town is to be founded.
            // Size of the battlefield depends upon the number of participants
            int dimension = 20 + 5 * (sideA.Count + sideB.Count - 2);
            battlefield = new Wildlife(dimension, dimension, embattledTile.type);
            // Generate Unit Locations

        }
    }


    private List<Being> GenerateEnemies(Hex embattledTile)
    {
        throw new NotImplementedException();
    }

}
