using System;
using UnityEngine;

public class ResourceCard {

    public double wood;
    public double brick;
    public double ore;
    public double wool;
    public double wheat;


    public ResourceCard(double wood, double brick, double ore, double wool, double wheat)
    {
        this.wood = wood;
        this.brick = brick;
        this.ore = ore;
        this.wool = wool;
        this.wheat = wheat;
    }


    public ResourceCard(double[] resourcesArray)
    {
        wood = resourcesArray[0];
        brick = resourcesArray[1];
        ore = resourcesArray[2];
        wool = resourcesArray[3];
        wheat = resourcesArray[4];
    }


    public override string ToString()
    {
        string resourceString = "";
        if (wood != 0)
        {
            resourceString += wood + " wood ";
        }
        if (brick != 0)
        {
            resourceString += brick + " brick ";
        }
        if (ore != 0)
        {
            resourceString += ore + " ore ";
        }
        if (wool != 0)
        {
            resourceString += wool + " wool ";
        }
        if (wheat != 0)
        {
            resourceString += wheat + " wheat";
        }
        if (resourceString == "")
        {
            resourceString = "none";
        }

        return resourceString;
    }


    public static ResourceCard StringToObject(string CSResources)
    {
        ResourceCard resources = new ResourceCard(0, 0, 0, 0, 0);
        if (CSResources != "-" && CSResources != "" && CSResources != "none")
        {
            if (CSResources.Contains(","))
            {
                string[] data = CSResources.Split(',');
                for (int i = 0; i < data.Length; i++)
                {
                    string[] numAndName = data[i].Split(' ');
                    resources.AddResourceByName(numAndName[0], numAndName[1]);
                }
            }
            else
            {
                string[] numAndName = CSResources.Split(' ');
                resources.AddResourceByName(numAndName[0], numAndName[1]);
            }
        }

        return resources;
    }


    private void AddResourceByName(string num, string name)
    {
        switch (name)
        {
            case "wood":
                wood += Convert.ToDouble(num);
                break;
            case "brick":
                brick += Convert.ToDouble(num);
                break;
            case "ore":
                ore += Convert.ToDouble(num);
                break;
            case "wool":
                wool += Convert.ToDouble(num);
                break;
            case "wheat":
                wheat += Convert.ToDouble(num);
                break;
        }
    }


}
