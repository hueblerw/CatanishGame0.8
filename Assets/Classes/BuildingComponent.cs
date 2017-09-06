using System;
using System.Collections.Generic;
using UnityEngine;


public class BuildingComponent {

    public string name;
    public string type;
    public Vector2 area;
    public double height;
    public int cost;
    public ResourceCard resources;
    public double weight;
    public int HP;
    public bool lockable;
    public bool sleepable;
    public bool fire;
    public bool rotatable;


    public BuildingComponent(string name, string furniture, Vector2 area, double height, int cost, ResourceCard resources, double weight, int HP, bool lockable, bool sleepable, bool fire, bool rotate)
    {
        this.name = name;
        this.type = furniture;
        this.area = area;
        this.height = height;
        this.cost = cost;
        this.resources = resources;
        this.weight = weight;
        this.HP = HP;
        this.lockable = lockable;
        this.sleepable = sleepable;
        this.fire = fire;
        rotatable = rotate;
    }


    public BuildingComponent(string[] columns)
    {
        name = columns[0];
        type = columns[1];
        string[] dim = columns[2].Split('x');
        area = new Vector2((float) Convert.ToDouble(dim[0]), (float)Convert.ToDouble(dim[1]));
        height = Convert.ToDouble(columns[3].Split('f')[0]);
        columns[4] = columns[4].Remove(0, 1);
        cost = Convert.ToInt32(columns[4]);
        resources = ResourceCard.StringToObject(columns[5]);
        weight = Convert.ToInt32(columns[6]);
        HP = Convert.ToInt32(columns[7]);
        lockable = YesNoToBool(columns[8].Trim());
        sleepable = YesNoToBool(columns[9].Trim());
        fire = YesNoToBool(columns[10].Trim());
        rotatable = YesNoToBool(columns[11].Trim());
    }


    private bool YesNoToBool(string input)
    {
        if (input == "yes")
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public static BuildingComponent[] LoadBuildingComponentsFromFile(string filePath)
    {
        // Load the file and break it up by line
        TextAsset TSVFile = Resources.Load(filePath) as TextAsset;
        string[] components = TSVFile.text.Split('\n');
        List<BuildingComponent> buildingComp = new List<BuildingComponent>();
        // Don't start with the first line which is the header
        // Break the line into components and use the values to create a new Building component object in the building component array
        for (int c = 1; c < components.Length; c++)
        {
            string[] columns = components[c].Split('\t');
            // if this is the end of the file stop else keep adding components
            if (columns[0] != "***** Don't erase this!  Only put comments below this line *****")
            {
                buildingComp.Add(new BuildingComponent(columns));
            }
            else
            {
                break;
            }   
        }

        return buildingComp.ToArray();
    }


    public override string ToString()
    {
        string build = name + ": \n";
        build += type + "\t";
        build += area.x + "x" + area.y + "\t";
        build += height + "\t";
        build += cost + "\t";
        build += resources + "\t";
        build += weight + "\t";
        build += HP + "\t";
        build += lockable + "\t";
        build += sleepable + "\t";
        build += fire + "\t";
        build += rotatable;

        return build;
    }

}
