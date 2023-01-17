using ExitGames.Client.Photon;
using Photon.Realtime;
using MyConstant;

public static class PlayerPropertiesExtensions
{
    private const string OrderKey = "o";

    private static readonly Hashtable propsToSet = new Hashtable();

    public static string GetTeam(this Player player)
    {
        return (player.CustomProperties[GrovalConst.TeamKey] is string team) ? team : string.Empty;
    }

    public static int GetOrder(this Player player)
    {
        return (player.CustomProperties[OrderKey] is int order) ? order : 0;
    }

    public static void SetTeam(this Player player, string team)
    {
        propsToSet[GrovalConst.TeamKey] = team;
        player.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }

    public static void SetOrder(this Player player, int order)
    {
        propsToSet[OrderKey] = order;
        player.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }
}