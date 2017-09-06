using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TownBuilderController : MonoBehaviour {

    // Variables
    public string gameName;
    public Town currentTown;
    public TownView currentView;
    public Texture2D[] basicTiles;
    public Texture2D[] buildingComponents;
    public Material mapMaterial;
    public int currentFloor;
    public Collider mapCollider;

    private bool vertical;
    private string component;
    private List<BuildingPart> newBuilding;
    private Matrix4x4 matrix;
    private int count;
    private GameObject newBuildScreen;
    private GameObject buildScreenMenu;

    // Constants
    public const float GAME_UNITS_PER_TILE = 2f;
    public const float PIXEL_TO_UNIT_CONVERSION = (100f / 64f) * GAME_UNITS_PER_TILE;

	// Use this for initialization
	void Start () {
        // Create or (ultimately load) a new town model and view OR load the town from file
        currentTown = new Town("forest");
        currentView = new TownView(currentTown, basicTiles, buildingComponents);
        // Populate the selectors
        PopulateComponentSelector(currentTown.componentTypes);
        PopulateBuildingSelector();
        // Find the menu objects
        newBuildScreen = GameObject.Find("NewBuildingControls");
        buildScreenMenu = GameObject.Find("BuildScreenMenu");
        // Find the townobject the sprite map will be attached to
        GameObject townObject = GameObject.Find("Town");
        // Add a sprite renderer
        townObject.AddComponent<SpriteRenderer>();
        // Attache the views map texture to the sprite renderer
        townObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(currentView.mapTexture, new Rect(0, 0, currentView.mapTexture.width, currentView.mapTexture.height), new Vector2(0, 0));
        townObject.GetComponent<SpriteRenderer>().sortingOrder = -2;
        // Scale the sprite map to the appropriate size
        // Debug.Log(townObject.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);
        townObject.transform.localScale = new Vector3(PIXEL_TO_UNIT_CONVERSION, PIXEL_TO_UNIT_CONVERSION, 1);
        // Set the builder defaults
        vertical = true;
        component = "floor";
        currentFloor = 0;
        // Create building parts list
        StartNewBuilding();
    }


    // Update is called once per frame
    void Update () {
        // if left-click a new building component is added to the current house
        if (Input.GetMouseButtonDown(0) && Input.mousePosition.x > 200)
        {
            Vector3 newLocation = ConvertToSquareCoor(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            CreateNewBuildingPart(newLocation, component);
        }
        else if (Input.GetMouseButtonDown(1) && Input.mousePosition.x > 200)
        {
            // Delete any extant building object on this square
            Debug.Log("Undo last  step");
            UndoLastAddition();
        }
    }


    // Create a new blank building list
    public void StartNewBuilding()
    {
        ResetNewBuilding();
        GameObject temp = new GameObject("building a house");
        GoToNewBuildingScreen();
    }


    // Rotate the components
    public void RotateComponent()
    {
        if (vertical)
        {
            vertical = false;
        }
        else
        {
            vertical = true;
        }
        UpdateComponentImage();
    }


    // Button to press to change components
    public void ChangeComponent()
    {
        Dropdown dropdown = FindObjectOfType<Dropdown>();
        component = currentTown.componentTypes[dropdown.value].name.ToLower();

        UpdateComponentImage();
    }


    // Save new House
    public void SaveHouse()
    {
        string address = FindObjectOfType<InputField>().text;
        if (address != "")
        {
            // Save the House to the model.
            House house = new House("shack", address, newBuilding);
            currentTown.AddHouse(house);
            // Navigate Back to the Menu
            ReturnToBuildScreenMenu();
            PopulateBuildingSelector();
            // Reload the world map
            Debug.Log("Save House " + address);
            Debug.Log("Cost: " + house.value + " - " + house.resources);
            currentView.BuildMap(currentTown);
            GameObject.Find("Town").GetComponent<SpriteRenderer>().sprite = Sprite.Create(currentView.mapTexture, new Rect(0, 0, currentView.mapTexture.width, currentView.mapTexture.height), new Vector2(0, 0));
        }
        else
        {
            Debug.Log("Must give an address!");
        }
    
    }


    public void SaveTown()
    {
        currentTown.SaveTownToFile("testFile");
    }


    // Reset the display and the new building array
    public void ClearCurrentBuilding()
    {
        StartNewBuilding();
    }


    public void CancelBuilding()
    {
        ResetNewBuilding();
        ReturnToBuildScreenMenu();
    }

    
    public void ReturnToCatanMap()
    {
        // Returns to the main map
    }

    // ********************************************************************************************************************************
    // To fix need to load in the last number in the part list for the edited building AFTER the floor components have been loaded in
    // Floor components are a problem because they don't have the file names anymore but real component names and can't be matched correctly.
        // This is probably also going to be an issue in the reading from file implementation
    public void EditBuilding()
    {
        ResetNewBuilding();
        int  currentBuildingIndex = GameObject.Find("SelectedBuilding").GetComponent<Dropdown>().value;
        GameObject editedHouse = GameObject.Find(currentTown.buildings[currentBuildingIndex].address);
        newBuilding = currentTown.buildings[currentBuildingIndex].stuff;
        // Transfer the extant stuff to the 'building a house object' - present method will not include the floors
        editedHouse.name = "building a house";
        // Add the floors
        CreateFloorDisplayComponents();
        // Remove the house from the list
        currentTown.buildings.RemoveAt(currentBuildingIndex);
        GoToNewBuildingScreen();
    }


    private void ReturnToBuildScreenMenu()
    {
        newBuildScreen.SetActive(false);
        buildScreenMenu.SetActive(true);
    }


    private void GoToNewBuildingScreen()
    {
        newBuildScreen.SetActive(true);
        buildScreenMenu.SetActive(false);
    }

    // Update the component image you are trying to build
    private void UpdateComponentImage()
    {
        Image image = GetImageOfName("SelectedComponentImage");
        Sprite newSprite = GetSpriteFromString(component);
        // Make the image width proportional to the sprite's dimensions
        float widthRatio = (newSprite.texture.width / (float)newSprite.texture.height);
        image.rectTransform.sizeDelta = new Vector2(50 * widthRatio, 50);
        if (widthRatio <= 1f)
        {
            image.rectTransform.position = new Vector3(75, image.rectTransform.position.y, 0);
        }
        else
        {
            image.rectTransform.position = new Vector3(40, image.rectTransform.position.y, 0);
        }
        image.sprite = newSprite;
    }


    // Return the text of the vertical boolean
    private string VerticalName()
    {
        if (vertical)
        {
            return "Up";
        }
        else
        {
            return "Right";
        }
    }


    // Returns the image object in the scene with the given name
    private Image GetImageOfName(string name)
    {
        Image[] images = FindObjectsOfType<Image>();
        for (int i = 0; i < images.Length; i++)
        {
            if (images[i].name == name)
            {
                return images[i];
            }
        }

        return null;
    }


    // Add a building part to the new list
    private void CreateNewBuildingPart(Vector3 newLocation, string component)
    {
        string direction;
        if (vertical)
        {
            direction = "up";
        }
        else
        {
            direction = "right";
        }
        // Create the new building model
        component = GetAllFirstLowerCase(component);
        // Add the new building component to the new building list.
        newBuilding.Add(new BuildingPart(component, newLocation, direction, currentTown));
        // Create the temporary construction display
        DisplayNewComponent(newLocation, component);
    }


    // Add a new sprite temporarily for the house being constructed
    private void DisplayNewComponent(Vector3 newLocation, string component)
    {
        // Create the new game object while construction takes place
        // reate a new game object with the name part [number]
        GameObject nextPart = new GameObject("Part " + count);
        count++;
        // Attach the parent to the currently being built house component list
        nextPart.transform.parent = GameObject.Find("building a house").transform;
        // Create a new sprite renderer and attach it to the new part!
        SpriteRenderer sr = nextPart.AddComponent<SpriteRenderer>();
        sr.sprite = GetSpriteFromString(component);
        // if it is a floor component move it to the back
        if (currentView.GetComponentTypeFromString(component).type.Contains("floor"))
        {
            sr.sortingOrder = -1;
        }
        // Give it the appropriate coordinates (scale x and y by the appropriate amount)
        newLocation.Scale(new Vector3(GAME_UNITS_PER_TILE, GAME_UNITS_PER_TILE, 1));
        // For border components, and floor-offset components offset by half a tile else if it is vertical shift up, horizontal shift right
        if (currentView.GetComponentTypeFromString(component).type == "border" || currentView.GetComponentTypeFromString(component).type == "floor-offset")
        {
            if (vertical)
            {
                newLocation.y += GAME_UNITS_PER_TILE / 2f;
            }
            else
            {
                newLocation.x += GAME_UNITS_PER_TILE / 2f;
            }
        }
        else
        {
            if (vertical)
            {
                newLocation.x += Math.Max(currentView.GetComponentTypeFromString(component).area.y, 1);
                newLocation.y += Math.Max(currentView.GetComponentTypeFromString(component).area.x, 1);
            }
            else
            {
                newLocation.x += Math.Max(currentView.GetComponentTypeFromString(component).area.x, 1);
                newLocation.y += Math.Max(currentView.GetComponentTypeFromString(component).area.y, 1);
            }  
        }
        nextPart.transform.localPosition = newLocation;
        // Scale the new sprite to the world's scale
        nextPart.transform.localScale = new Vector3(PIXEL_TO_UNIT_CONVERSION, PIXEL_TO_UNIT_CONVERSION, 1);
    }


    // take only the uncapitalized start of a given string.
    private string GetAllFirstLowerCase(string component)
    {
        bool capitalFound = false;
        int i = 0;
        while (!capitalFound)
        {
            if (char.IsUpper(component[i]))
            {
                capitalFound = true;
                return component.Substring(0, i);
            }
            else if(i >= component.Length - 1)
            {
                capitalFound = true;
            }
            else
            {
                i++;
            }
        }

        return component;
    }


    // Convert from World Coordinates to Tile Coordinates
    private Vector3 ConvertToSquareCoor(Vector3 mousePosition)
    {
        Vector3 tileCoor = new Vector3();
        tileCoor.x = Mathf.Floor(mousePosition.x / 2.0f);
        tileCoor.y = Mathf.Floor(mousePosition.y / 2.0f);
        tileCoor.z = currentFloor * 10.0f;
        // Debug.Log(tileCoor);
        return tileCoor;
    }


    private Sprite GetSpriteFromString(string component)
    {
        // Default is delete symbol
        Sprite newSprite = Sprite.Create(buildingComponents[4], new Rect(0, 0, 16, 16), new Vector2(.5f, .5f));
        int count = 0;
        int num = 0;
        bool done = false;
        while (!done)
        {
            // if it is the right one
            if (component == currentTown.componentTypes[count].name.ToLower())
            {
                // if rotatable choose from two available sprites, else choose the one sprite
                if (currentTown.componentTypes[count].rotatable)
                {
                    if (vertical)
                    {
                        newSprite = Sprite.Create(buildingComponents[num], new Rect(0, 0, buildingComponents[num].width, buildingComponents[num].height), new Vector2(.5f, .5f));
                    }
                    else
                    {
                        newSprite = Sprite.Create(buildingComponents[num + 1], new Rect(0, 0, buildingComponents[num].height, buildingComponents[num].width), new Vector2(.5f, .5f));
                    }
                }
                else
                {
                    newSprite = Sprite.Create(buildingComponents[num], new Rect(0, 0, buildingComponents[num].width, buildingComponents[num].height), new Vector2(.5f, .5f));
                }
                // exit the loop
                done = true;
            }
            // if not correct, count++ and num + 2 if rotatable else num++
            else
            {
                // Debug.Log(currentTown.componentTypes[count]);
                if (currentTown.componentTypes[count].rotatable)
                {
                    num += 2;
                }
                else
                {
                    num++;
                }
                // if you reach the end of the array stop
                count++;
                if (count >= currentTown.componentTypes.Length)
                {
                    done = true;
                }
            }
        }

        // Debug.Log(count + " - " + num);
        return newSprite;
    }


    private void PopulateComponentSelector(BuildingComponent[] componentTypes)
    {
        Dropdown selector = GameObject.Find("SelectedComponent").GetComponent<Dropdown>();
        selector.options.Capacity = componentTypes.Length;
        for (int i = 0; i < componentTypes.Length; i++)
        {
            selector.options.Add(new Dropdown.OptionData(componentTypes[i].name));
        }

        selector.value = 2;
    }


    private void UndoLastAddition()
    {
        Destroy(GameObject.Find("building a house").transform.GetChild(count - 1).gameObject);
        newBuilding.RemoveAt(count - 1);
        count--;
    }

    private void CreateFloorDisplayComponents()
    {
        Debug.Log(newBuilding.Count);
        for (int i = 0; i < newBuilding.Count; i++)
        {
            Debug.Log(i + " - " + newBuilding[i].componentType.type);
            if (newBuilding[i].componentType.type.Contains("floor"))
            {
                Debug.Log(i + " ~ " + newBuilding[i].componentType.type);
                DisplayNewComponent(newBuilding[i].location, newBuilding[i].componentType.name);
            }
        }
    }


    private void PopulateBuildingSelector()
    {
        Dropdown selector = GameObject.Find("SelectedBuilding").GetComponent<Dropdown>();
        List<House> buildings = currentTown.buildings;
        selector.options.Capacity = buildings.Count;
        selector.options = new List<Dropdown.OptionData>();
        for (int i = 0; i < buildings.Count; i++)
        {
            selector.options.Add(new Dropdown.OptionData(buildings[i].address));
        }
    }


    private void ResetNewBuilding()
    {
        Destroy(GameObject.Find("building a house"));
        count = 0;
        newBuilding = new List<BuildingPart>();
    }

}
