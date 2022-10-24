using ExitGames.Client.Photon;
using Photon.Realtime;

public static class PlayerPropertiesExtensions
{
    private const string TeamKey = "t";
    private const string OrderKey = "o";

    private static readonly Hashtable propsToSet = new Hashtable();

    // プレイヤーのブロックを取得する
    public static string GetTeam(this Player player)
    {
        return (player.CustomProperties[TeamKey] is string team) ? team : string.Empty;
    }

    public static int GetOrder(this Player player)
    {
        return (player.CustomProperties[OrderKey] is int order) ? order : 0;
    }

    // プレイヤーのブロックを設定する
    public static void SetTeam(this Player player, string team)
    {
        propsToSet[TeamKey] = team;
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