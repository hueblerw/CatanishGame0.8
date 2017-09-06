using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HexListenerController : MonoBehaviour {

    private float SQRT_OF_3 = Mathf.Sqrt(3);

    private EventSystem events;
    private CatanController boardController;
    private Text displayInfo;
    public bool activated;

	// Use this for initialization
	void Start () {
        events = FindObjectOfType<EventSystem>();
        boardController = GameObject.Find("CatanBoard").GetComponent<CatanController>();
        displayInfo = GameObject.Find("HexInfo").GetComponent<Text>();
        activated = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (activated && Input.GetMouseButtonDown(0))
        {
            Vector2 newLocation = ConvertToHexCoor(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            GameObject.Find("SelectedTileValue").GetComponent<Text>().text = CoordinatesToText(newLocation);
            boardController.CreatePopUp();
            activated = false;
        }

        Vector2 mouseCoor = ConvertToHexCoor(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        Debug.Log(mouseCoor);
        string info = boardController.worldMap[(int) mouseCoor.x, (int) mouseCoor.y].GetHexInfo();
        displayInfo.text = info;
    }


    // Converts the mouse location to a hex grid coordinate
    private Vector2 ConvertToHexCoor(Vector3 worldCoor)
    {
        Debug.Log(worldCoor);
        float v = (1f / 3f) * worldCoor.y;
        float u = -(1f / 6f) * worldCoor.y + (1f / 2f * SQRT_OF_3) * worldCoor.x;

        int Ru = (int) Mathf.Round(u);
        int Rv = (int) Mathf.Round(v);



        Vector2 hexCoor = new Vector2(Ru, Rv);
        return hexCoor;
    }


    // Converts a vector2 to a string
    private string CoordinatesToText(Vector2 coors)
    {
        return "(" + coors.x + ", " + coors.y + ")";
    }

}
