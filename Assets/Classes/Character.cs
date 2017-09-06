using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Being {

    public Species race;  // contains information like size, natural bonuses, resistance and immunity, gear slots, natural speed
        // For the moment all are human or monster
    public PlayerClass pClass;  // [C]
    // Economy - [C]
    public string job;
    public double money;
    public string property;
    // Stats
    public bool secondWind;  // [C]
    public int deathSavesUsed; // [C]
    // Skills - [C]
    public int appraise;  // (was streetwise) - charisma
    public int acrobatics;  // dexterity
    public int athletics;  // strength
    public int arcana;  // intelligence - only available to magic species
    public int bluff;  // charisma
    public int endurance;  // constitution
    public int history;  // intelligence
    public int intimidate;  // charisma
    public int nature;  // wisdom -  good for farming and sheep
    public int persausion;  // (was diplomacy) - charisma
    public int religion;  // intelligence
    public int stealth;  // dexterity
    public int thievery;  // dexterity
    // Schooling Skills - skills that must be learned in school before they can be used - [C]
    public int mining; // (was dungeoneering) - wisdom - (must be studied not natural) - good for ore
    public int heal;  // wisdom - (must be studied not natural)
    public int architecture;  // (new) - intelligence - good for construction
    public int materials;  // (new) - wisdom - good for wood and bricks
    public int writing;  // (new) - intelligence - allows you to write books
    public int crafting;  // intelligence - allows you to craft and invent objects
    public int potions;  // (new) - intelligence - only available to magic users - allows you to craft potions and magic ingredients
    // Attacks
    public List<Power> rituals; // [C]
    // Weapons
        // Slots may depend on species
    public List<Items> inventory;  // [C]

}
