using System;
using UnityEngine;


public class BuildingPart {

    public Vector3 location;
    public string orientation;
    public BuildingComponent componentType;
    public string componentName;


    public BuildingPart(string componentType, Vector3 location, string orientation, Town town)
    {
        componentName = componentType;
        this.componentType = SelectComponent(componentType, town);
        this.location = location;
        this.orientation = orientation;
    }


    // Select the appropriate Building Component from town's possible component array
    private BuildingComponent SelectComponent(string componentName, Town town)
    {
        // Debug.Log(componentName);
        int i = 0;
        while(i < town.componentTypes.Length && componentName != town.componentTypes[i].name.ToLower())
        {
            i++;
        }

        if (i < town.componentTypes.Length)
        {
            return town.componentTypes[i];
        }
        else
        {
            throw new NotSupportedException();
        }
    }

}
