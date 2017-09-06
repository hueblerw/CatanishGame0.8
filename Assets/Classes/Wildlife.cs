
public class Wildlife {

    public string[,] grid;

    public Wildlife(int X, int Z, string type)
    {
        grid = new string[X, Z];
        float randy;
        // Get the probabilities of generating a rock a hill or a tree
        float rockProb;
        float hillProb;
        float treeProb = GetProbabilities(type, out rockProb, out hillProb);
        
        // Create a grid assigning those probabilities
        // First check for rocks, then hills, then trees.
        // If none then leave it blank
        for(int x = 0; x < X; x++)
        {
            for (int z = 0; z < Z; z++)
            {
                randy = UnityEngine.Random.Range(0f, 100f);
                if(randy < rockProb)
                {
                    grid[x, z] = "R";
                }
                else
                {
                    randy = UnityEngine.Random.Range(0f, 100f);
                    if (randy < hillProb)
                    {
                        grid[x, z] = "H";
                    }
                    else
                    {
                        randy = UnityEngine.Random.Range(0f, 100f);
                        if (randy < treeProb)
                        {
                            grid[x, z] = "T";
                        }
                        else
                        {
                            grid[x, z] = "-";
                        }
                    }
                }            
            }
        }
    }


    // Get the probabilities of generating a rock a hill or a tree
    private float GetProbabilities(string type, out float rockProb, out float hillProb)
    {
        float tProb = 0f;
        rockProb = 0f;
        hillProb = 0f;

        switch (type)
        {
            case "forest":
                tProb = 5.0f;
                rockProb = .5f;
                hillProb = 1f;
                break;
            case "farmland":
                tProb = .5f;
                rockProb = .5f;
                hillProb = 1f;
                break;
            case "plains":
                tProb = .5f;
                rockProb = .5f;
                hillProb = 2f;
                break;
            case "quarry":
                tProb = 5.0f;
                rockProb = 1f;
                hillProb = 1f;
                break;
            case "mine":
                tProb = 5.0f;
                rockProb = 2.5f;
                hillProb = 5f;
                break;
        }

        return tProb;
    }


    public override string ToString()
    {
        // Create a tab delineated string for the wildlife array
        string gridString = "";
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int z = 0; z < grid.GetLength(1); z++)
            {
                gridString += grid[x, z] + "\t";
            }
            gridString += "\n";
        }

        return gridString;
    }

}
