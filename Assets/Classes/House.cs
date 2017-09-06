using System.Collections.Generic;


public class House {

    public List<BuildingPart> stuff = new List<BuildingPart>();
    public string address;
    public string type;
    public int value;  // dollar value
    public ResourceCard resources;
    // public Person owner;


    public House(string type, string address, List<BuildingPart> houseParts)
    {
        this.type = type;
        this.address = address;
        stuff = houseParts;
        value = CalculateHouseValue(houseParts);
        resources = CalculateTotalResourceCost(houseParts);
    }


    private ResourceCard CalculateTotalResourceCost(List<BuildingPart> houseParts)
    {
        ResourceCard sum = new ResourceCard(0, 0, 0, 0, 0);

        for (int i = 0; i < houseParts.Count; i++)
        {
            sum.brick += houseParts[i].componentType.resources.brick;
            sum.ore += houseParts[i].componentType.resources.ore;
            sum.wheat += houseParts[i].componentType.resources.wheat;
            sum.wood += houseParts[i].componentType.resources.wood;
            sum.wool += houseParts[i].componentType.resources.wool;
        }

        return sum;
    }


    private int CalculateHouseValue(List<BuildingPart> houseParts)
    {
        int sum = 0;
        for (int i = 0; i < houseParts.Count; i++)
        {
            sum += houseParts[0].componentType.cost;
        }

        return sum;
    }

}
