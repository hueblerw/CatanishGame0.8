using System.Collections.Generic;
using UnityEngine;


public class Town {

    public int X;
    public int Z;
    public string name;
    public List<House> buildings;
    public Wildlife wildlife;
    public BuildingComponent[] componentTypes;
    // public Ground[] groundAlterations;


	public Town(string tileType)
    {
        name = "testName";
        X = 50;
        Z = 50;
        buildings = new List<House>();
        // Generate Wildlife
        wildlife = new Wildlife(X, Z, tileType);
        // Load in Building Components
        componentTypes = BuildingComponent.LoadBuildingComponentsFromFile(@"TSV\BuildingComponentInfo");
    }


    private BuildingComponent[] LoadComponents()
    {
        BuildingComponent[] components = new BuildingComponent[4];
        // Wall
        ResourceCard resources = new ResourceCard(2, 0, 0, 0, 0);
        components[0] = new BuildingComponent("wall", "border", new Vector2(1, 1), 10.0, 25, resources, 1000, 100, false, false, false, true);
        // Door
        resources = new ResourceCard(2, 0, 1, 0, 0);
        components[1] = new BuildingComponent("doorUp", "border", new Vector2(1, 1), 10.0, 25, resources, 1000, 70, true, false, false, true);
        // Floor
        resources = new ResourceCard(0, 0, 0, 0, 0);
        components[2] = new BuildingComponent("floor", "floor", new Vector2(1, 1), 0.0, 5, resources, 0, 50, false, false, false, false);
        return components;
    }


    public void AddHouse(House newHouse)
    {
        buildings.Add(newHouse);
    }


    public void SaveTownToFile(string gameName)
    {
        // Generate a new text file 
        TextAsset savefile = new TextAsset();
        // Give it a name unique to the town's name and location
            // savefile.name = gameName + "-" + name + " (" + X + ", " + Z + ")";
        // place the file in the correct location

        // Create the text save info
        string saveInfo = "Wildlife:\n";
        saveInfo += wildlife;
        saveInfo += "\nHouses:\n";
        // Save the houses
        
        // savefile.text = saveInfo;
        Debug.Log(saveInfo);
    }

}
