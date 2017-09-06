using System;
using UnityEngine;


public class TownView {

    // Variables
    private Town town;
    private Texture2D[] basicTiles;
    private Texture2D[] buildComponents;
    public Texture2D mapTexture;
    private int count;

    // Constants
    private const int PIXELS_PER_TILE = 64;

    // Constructor
	public TownView(Town currentTown, Texture2D[] incomingTextures, Texture2D[] incomingBuildings)
    {
        basicTiles = incomingTextures;
        buildComponents = incomingBuildings;
        BuildMap(currentTown);
        count = 0;
    }


    // Build Map
    public void BuildMap(Town town)
    {
        this.town = town;
        DestroyOldHouses();
        CreateTownHouses();
        mapTexture = CreateTownSprites();
    }


    // Create Basic Town View
    private Texture2D CreateTownSprites()
    {
        // Create a blank texture of size 64 x town dimensions
        Texture2D map = new Texture2D(PIXELS_PER_TILE * town.X, PIXELS_PER_TILE * town.Z);
        // Use the wildlife grid to place trees, rocks, hills, building interiors and roads base texture.
        for (int x = 0; x < town.X; x++)
        {
            for (int z = 0; z < town.Z; z++)
            {
                Color[] tile = ChooseTilesPixels(town.wildlife.grid[x, z]);
                if (town.wildlife.grid[x, z] == "RC")
                {
                    map.SetPixels(x * PIXELS_PER_TILE, z * PIXELS_PER_TILE, 2 * PIXELS_PER_TILE, 2 * PIXELS_PER_TILE, tile);
                }
                else if (town.wildlife.grid[x, z] == "RU")
                {
                    map.SetPixels(x * PIXELS_PER_TILE, z * PIXELS_PER_TILE, 2 * PIXELS_PER_TILE, PIXELS_PER_TILE, tile);
                }
                else if (town.wildlife.grid[x, z] == "RR")
                {
                    map.SetPixels(x * PIXELS_PER_TILE, z * PIXELS_PER_TILE, PIXELS_PER_TILE, 2 * PIXELS_PER_TILE, tile);
                }
                else if(town.wildlife.grid[x, z] == "")
                {
                    // Do nothing
                }
                else
                {
                    map.SetPixels(x * PIXELS_PER_TILE, z * PIXELS_PER_TILE, PIXELS_PER_TILE, PIXELS_PER_TILE, tile);
                }
            }
        }
        map.Apply();

        return map;
    }


    private Color[] ChooseTilesPixels(string wildlifeType)
    {
        Color[] colors = null;
        // 0 - blank tile (means it is indoors), 1 - blank grass, 2 - hill, 3 - rock, 4 - treebase, 5 - dirt road, 6 - road up, 7 - road right, 8 - road crossing
        switch (wildlifeType)
        {
            case "B":
                colors = basicTiles[0].GetPixels();
                break;
            case "-":
                colors = basicTiles[1].GetPixels();
                break;
            case "H":
                colors = basicTiles[2].GetPixels();
                break;
            case "R":
                colors = basicTiles[3].GetPixels();
                break;
            case "T":
                colors = basicTiles[4].GetPixels();
                break;
            case "G":
                colors = basicTiles[5].GetPixels();
                break;
            case "RU":
                colors = basicTiles[6].GetPixels();
                break;
            case "RR":
                colors = basicTiles[7].GetPixels();
                break;
            case "RC":
                colors = basicTiles[8].GetPixels();
                break;
        }
        
        return colors;
    }


    private void CreateTownHouses()
    {
        // Create a blank game object that stores a single house
        for (int h = 0; h < town.buildings.Count; h++)
        {
            GameObject house = new GameObject(town.buildings[h].address);
            // Under it create a bunch of game objects that are each individual component.
            for (int c = 0; c < town.buildings[h].stuff.Count; c++)
            {
                if (!town.buildings[h].stuff[c].componentType.type.Contains("floor"))
                {
                    DisplayNewComponent(house, town.buildings[h].stuff[c].location, town.buildings[h].stuff[c].componentName, town.buildings[h].stuff[c].orientation);
                }
                else
                {
                    UpdateTownsFloorMap(town.buildings[h].stuff[c].componentName, town.buildings[h].stuff[c].orientation, town.buildings[h].stuff[c].location);
                    // Note: Don't remove the extra stuff maybe Roads?  Because I still want the floors to be part of the cost of the building.
                }
            }
        }
    }


    private void DisplayNewComponent(GameObject house, Vector3 local, string component, string vertical)
    {
        // Create the new game object while construction takes place
        GameObject nextPart = new GameObject("Part " + count);
        count++;
        nextPart.transform.parent = house.transform;
        SpriteRenderer sr = nextPart.AddComponent<SpriteRenderer>();
        sr.sprite = GetSpriteFromString(component, vertical);
        // Give it the appropriate coordinates
        local.Scale(new Vector3(2, 2, 1));
        if (GetComponentTypeFromString(component).type == "border" || GetComponentTypeFromString(component).type == "floor-offset")
        {
            if (vertical == "up")
            {
                local.y += TownBuilderController.GAME_UNITS_PER_TILE / 2f;
            }
            else
            {
                local.x += TownBuilderController.GAME_UNITS_PER_TILE / 2f;
            }
        }
        else
        {
            if (vertical == "up")
            {
                local.x += Math.Max(GetComponentTypeFromString(component).area.y, 1);
                local.y += Math.Max(GetComponentTypeFromString(component).area.x, 1);
            }
            else
            {
                local.x += Math.Max(GetComponentTypeFromString(component).area.x, 1);
                local.y += Math.Max(GetComponentTypeFromString(component).area.y, 1);
            }
        }
        nextPart.transform.localPosition = local;
        nextPart.transform.localScale = new Vector3(TownBuilderController.PIXEL_TO_UNIT_CONVERSION, TownBuilderController.PIXEL_TO_UNIT_CONVERSION, 1);
    }


    private Sprite GetSpriteFromString(string component, string vertical)
    {
        // Default is delete symbol
        Sprite newSprite = Sprite.Create(buildComponents[4], new Rect(0, 0, 16, 16), new Vector2(.5f, .5f));
        int count = 0;
        int num = 0;
        bool done = false;
        while (!done)
        {
            // if it is the right one
            if (component == town.componentTypes[count].name.ToLower())
            {
                // if rotatable choose from two available sprites, else choose the one sprite
                if (town.componentTypes[count].rotatable)
                {
                    if (vertical == "up")
                    {
                        newSprite = Sprite.Create(buildComponents[num], new Rect(0, 0, buildComponents[num].width, buildComponents[num].height), new Vector2(.5f, .5f));
                    }
                    else
                    {
                        newSprite = Sprite.Create(buildComponents[num + 1], new Rect(0, 0, buildComponents[num].height, buildComponents[num].width), new Vector2(.5f, .5f));
                    }
                }
                else
                {
                    newSprite = Sprite.Create(buildComponents[num], new Rect(0, 0, buildComponents[num].width, buildComponents[num].height), new Vector2(.5f, .5f));
                }
                // exit the loop
                done = true;
            }
            // if not correct, count++ and num + 2 if rotatable else num++
            else
            {
                // Debug.Log(currentTown.componentTypes[count]);
                if (town.componentTypes[count].rotatable)
                {
                    num += 2;
                }
                else
                {
                    num++;
                }
                // if you reach the end of the array stop
                count++;
                if (count >= town.componentTypes.Length)
                {
                    done = true;
                }
            }
        }

        return newSprite;
    }


    private void DestroyOldHouses()
    {
        for (int h = 0; h < town.buildings.Count; h++)
        {
            GameObject.Destroy(GameObject.Find(town.buildings[h].address));
        }
    }


    // Adds the appopriate floor tiles to the wildlife grid
    private void UpdateTownsFloorMap(string component, string direction, Vector3 newLocation)
    {
        switch (component)
        {
            case "floor":
                town.wildlife.grid[(int)newLocation.x, (int)newLocation.y] = "B";
                break;
            case "road":
                if (direction == "up")
                {
                    town.wildlife.grid[(int)newLocation.x, (int)newLocation.y] = "";
                    town.wildlife.grid[(int)newLocation.x - 1, (int)newLocation.y] = "RU";
                }
                else
                {
                    town.wildlife.grid[(int)newLocation.x, (int)newLocation.y] = "";
                    town.wildlife.grid[(int)newLocation.x, (int)newLocation.y - 1] = "RR";
                }
                break;
            case "road crossing":
                town.wildlife.grid[(int)newLocation.x, (int)newLocation.y] = "RC";
                town.wildlife.grid[(int)newLocation.x + 1, (int)newLocation.y + 1] = "";
                town.wildlife.grid[(int)newLocation.x + 1, (int)newLocation.y] = "";
                town.wildlife.grid[(int)newLocation.x, (int)newLocation.y + 1] = "";
                break;
            case "dirt road":
                town.wildlife.grid[(int)newLocation.x, (int)newLocation.y] = "G";
                break;
        }
    }


    public BuildingComponent GetComponentTypeFromString(string component)
    {
        int i = 0;
        while (i < town.componentTypes.Length && component != town.componentTypes[i].name.ToLower())
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

