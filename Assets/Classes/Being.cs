using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Being {

    // OK so, so far I've entered all info I can find for both Monsters and characters.
    // Some of this needs to be moved to Character sub-class, etc.
    // I have thought too hard about things like what keeps track of actions taken, etc.
    // It is apparent I need a species class that needs careful thought
    // I also need a powers class that can handle any type of action daily, standard, type of damage etc.
    // Also, there will be many types of items that can be carried on a person.  
    // So this probably calls for several sub-classes such as weapons, magical, potions, tools, other (or just generic items)

    // Basic Info
    public string name;
    public int level;
    public string education;
    public int XP;
    public int height;  // in inches
    public int weight;  // in pounds
    // Movement
    public int initiative;
    public int speed;  // for walking in squares per turn
    public int fly;  // Must be a racial or item ability - defaults to 0
    public bool hover;  // Must be a racial or item ability - defaults to false
    public int actionPoints;
    // Attributes
    public int strength;
    public int constitution;
    public int dexterity;
    public int intelligence;
    public int wisdom;
    public int charisma;
    // Defenses
    public int AC;
    public int fortitude;
    public int reflex;
    public int will;
    // Senses
    public string languages;
    public string senses;
    public int perception;  // wisdom
    public int insight;  // wisdom
    public int legilmency;  // intelligence - only available to magic species - (Must be studied not natural)
    public int occulmency;  // intelligence - only available to magic species
    // Stats
    public int maxHP;
    public int currentHP;
    // Attacks
        // For the moment I am disabling feats
    public Power basicAttack;
    public List<Power> atWillPowers;
    public List<Power> encounterPowers;
    public List<Power> utilityPowers;
        // For the moment I am disabling daily powers in favor of ritual powers
    // Resistances and Vulnerabilities
    public string vulnerable;
    public string resistance;
    public string immunity;

}
