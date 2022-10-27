using ExitGames.Client.Photon;
using Photon.Realtime;
using MyConstant;

public static class RoomPropertiesExtensions
{
    private static readonly Hashtable propsToSet = new Hashtable();

    public static int GetANum(this Room room)
    {
        return (room.CustomProperties[GrovalConst.ANumKey] is int number) ? number : 0;
    }

    public static int GetBNum(this Room room)
    {
        return (room.CustomProperties[GrovalConst.BNumKey] is int number) ? number : 0;
    }

    public static string GetFirst(this Room room)
    {
        return (room.CustomProperties[GrovalConst.FirstKey] is string first) ? first : string.Empty;
    }

    public static void SetANum(this Room room, int number)
    {
        propsToSet[GrovalConst.ANumKey] = number;
        room.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }

    public static void SetBNum(this Room room, int number)
    {
        propsToSet[GrovalConst.BNumKey] = number;
        room.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }

    public static void SetFirst(this Room room, string first)
    {
        propsToSet[GrovalConst.FirstKey] = first;
        room.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }
}