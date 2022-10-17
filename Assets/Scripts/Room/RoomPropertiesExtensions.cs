using ExitGames.Client.Photon;
using Photon.Realtime;

public static class RoomPropertiesExtensions
{
    private const string BlockKey = "b";

    private static readonly Hashtable propsToSet = new Hashtable();

    // プレイヤーのブロックを取得する
    public static string GetBlock(this Room room)
    {
        return (room.CustomProperties[BlockKey] is string block) ? block : string.Empty;
    }

    // プレイヤーのブロックを設定する
    public static void SetBlock(this Room room, string block)
    {
        propsToSet[BlockKey] = block;
        room.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }

}