using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatanController : MonoBehaviour {

    // Constants
    private const int WORLD_DIMENSION = 10;

    // Variables
    public Sprite[] tileSprites;
    public Sprite[] numberSprites;
    public Hex[,] worldMap;
    public List<Town> towns;
    public List<Character> characters;
    public List<Faction> factions;
    public int year;
    public int day;


	// Use this for initialization
	void Start () {
        // Set the main camera position
        SetCameraPosition();
        // Create the game board
        GameObject board = GameObject.Find("CatanBoard");
        worldMap = GenerateMap(board);
        // Create the town, faction, and character lists
        towns = new List<Town>();
        characters = new List<Character>();
        factions = new List<Faction>();
        // Set the time
        year = 1;
        day = 1;
    }


    // Set the main camera position to the center of the parallelgram world
    private void SetCameraPosition()
    {
        GameObject mainCamera = GameObject.Find("Main Camera");
        Vector3 initPos = new Vector3(WORLD_DIMENSION * Hex.RADIUS * (float)Math.Sqrt(3) / 2f, .75f * WORLD_DIMENSION * Hex.RADIUS / 2f, 0);
        mainCamera.transform.position = initPos;
        mainCamera.GetComponent<CameraController>().SetPostion(initPos);
    }


    // Generate the initial world
    private Hex[,] GenerateMap(GameObject board)
    {
        Hex[,] hexGrid = new Hex[WORLD_DIMENSION, WORLD_DIMENSION];
        for (int x = 0; x < WORLD_DIMENSION; x++)
        {
            for (int y = 0; y < WORLD_DIMENSION; y++)
            {
                // Create the hex tile model
                int tileNum = UnityEngine.Random.Range(1, 7) + UnityEngine.Random.Range(1, 7);
                int index = 11;
                string type = RandomTileType(out index);
                hexGrid[x, y] = new Hex(x, y, type, tileNum);
                // Create a new Tile Object for display
                GameObject nextHex = new GameObject(x + ", " + y);
                nextHex.transform.parent = board.transform;
                SpriteRenderer sr = nextHex.AddComponent<SpriteRenderer>();
                if (tileNum == 7)
                {
                    // Make an ocean
                    sr.sprite = tileSprites[11];
                }
                else
                {
                    sr.sprite = tileSprites[index];
                    // Create the number attached to the tile if not a desert.
                    if (index != 10)
                    {
                        GameObject nextNum = new GameObject("number");
                        nextNum.transform.parent = nextHex.transform;
                        SpriteRenderer sr2 = nextNum.AddComponent<SpriteRenderer>();
                        if (tileNum < 7)
                        {
                            sr2.sprite = numberSprites[tileNum - 2];
                        }
                        else
                        {
                            sr2.sprite = numberSprites[tileNum - 3];
                        }
                    }
                }
                sr.sortingOrder = -1;
                // Use the grid position from the model.
                nextHex.transform.position = hexGrid[x, y].Position();
            }
        }

        return hexGrid;
    }


    private string RandomTileType(out int index)
    {
        string type = "desert";
        index = UnityEngine.Random.Range(0, 11);
        switch ((int) (index / 6.0))
        {
            case 0:
                type = "brick";
                break;
            case 1:
                type = "farmland";
                break;
            case 2:
                type = "forest";
                break;
            case 3:
                type = "ore";
                break;
            case 4:
                type = "sheep";
                break;
        }

        return type;
    }


    // BUTTON METHODS!!!

    // Begins the encounter for creating a new town
    public void StartNewTownEncounter()
    {
        // Ask for the square the encounter will take place upon
        GameObject.Find("SelectInstructions").GetComponent<Text>().enabled = true;
        // Add a listener for a mouse click to tell use the coordinates
        GameObject.Find("HexInfo").GetComponent<HexListenerController>().activated = true;
    }


    // Create pop-up menu that returns the faction creating the new town
    public void CreatePopUp()
    {
        // Create Pop-up panel
        Dropdown dropdown = GameObject.Find("Faction Dropdown").GetComponent<Dropdown>();
        for (int i = 0; i < factions.Count; i++)
        {
            dropdown.options.Add(new Dropdown.OptionData(factions[i].name));
        }
        GameObject.Find("FactionChooseCanvas").GetComponent<Canvas>().enabled = true;
    }


    public void NewFactionTownEncounter()
    {
        // Once a faction is chosen create or select new migrating characters
        string input = GameObject.Find("FactionText").GetComponent<Text>().text;
        Faction active = new Faction(input);
        factions.Add(active);
        // Spawn new characters from 4-8
        int sizeOfParty = UnityEngine.Random.Range(4, 9);
        List<Character> newTownParty = new List<Character>();
        for (int n = 0; n < sizeOfParty; n++)
        {
            newTownParty.Add(new Character());
            characters.Add(newTownParty[n]);
        }
        // Create a battle encounter
        Vector2 coor = TextToCoordinates(GameObject.Find("SelectedTileValue").GetComponent<Text>().text);
        Hex location = worldMap[(int) coor.x, (int) coor.y];
        Battle foundationEncounter = new Battle("foundation", newTownParty, location);
        // Fight that encounter

        // Create the town

        // Let us know the time to build

        // Create the building town icon

    }


    // Converts a (x, y) to a vector2
    private Vector2 TextToCoordinates(string text)
    {
        string modifiedText = text.Replace("(", "");
        modifiedText = text.Replace(")", "");
        string[] nums = modifiedText.Split(',');
        Vector2 coors = new Vector2(Convert.ToInt32(nums[0]), Convert.ToInt32(nums[1]));
        return coors;
    }

}
