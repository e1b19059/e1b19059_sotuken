using ExitGames.Client.Photon;
using Photon.Realtime;

public static class RoomPropertiesExtensions
{
    private const string ANumKey = "a";
    private const string BNumKey = "b";

    private static readonly Hashtable propsToSet = new Hashtable();

    public static int GetANum(this Room room)
    {
        return (room.CustomProperties[ANumKey] is int number) ? number : 0;
    }

    public static int GetBNum(this Room room)
    {
        return (room.CustomProperties[BNumKey] is int number) ? number : 0;
    }

    public static void SetANum(this Room room, int number)
    {
        propsToSet[ANumKey] = number;
        room.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }

    public static void SetBNum(this Room room, int number)
    {
        propsToSet[BNumKey] = number;
        room.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }

    public static void SetInit(this Room room, int Anum, int Bnum)
    {
        propsToSet[ANumKey] = Anum;
        propsToSet[BNumKey] = Bnum;
        room.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }
}