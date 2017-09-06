using UnityEngine;


public class Hex {

    public const float RADIUS = 2f;
    private readonly float ROOT3OVER2;

    public int Q;  // column
    public int R;  // row
    public int S;  // the other one
    public string type;
    public int number;
    public int wildness;
    public Vector2 lastPatrol;

    // Q + R + S = 0 always

    // Constructor
    public Hex(int q, int r, string type, int num)
    {
        if (num == 7)
        {
            this.type = "ocean";
        }
        else
        {
            this.type = type;
            if (type != "desert")
            {
                number = num;
            }      
        }
        
        ROOT3OVER2 = Mathf.Sqrt(3f) / 2f;
        Q = q;
        R = r;
        S = -(q + r);
        wildness = 10;
        lastPatrol = new Vector2(1, 1);
    }


    // Returns the world space position of this hex.
    public Vector3 Position()
    {
        float height = RADIUS * 2f;
        float width = ROOT3OVER2 * height;

        float vert = 0.75f * height;
        float horz = width;

        return new Vector3(horz * (Q + R / 2f), vert * R, 0);
    }


    public string GetHexInfo()
    {
        string info = "Type: " + type;
        info += "\n" + "Value: " + number;
        info += "\n" + "Wildness: " + wildness;
        info += "\n" + "Last Patrol: year " + lastPatrol.x + " day " + lastPatrol.y;

        return info;
    }

}
