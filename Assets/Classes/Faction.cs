using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faction {

    // Basic Info
    public string name;
    // public string nationality;
    public List<Town> cities;
    // Treasury
    ResourceCard resources;
    public double money;
    public List<Items> inventory;
    // Politics
    public Character leader;
    // Laws
    public List<string> laws;


    // Constructor
    public Faction(string name)
    {
        this.name = name;
        cities = new List<Town>();
        resources = new ResourceCard(0, 0, 0, 0, 0);
        money = 0.00;
        inventory = new List<Items>();
        laws = new List<string>();
    }

}
