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

    public static int GetScoreA(this Room room)
    {
        return (room.CustomProperties[GrovalConst.ScoreAKey] is int score) ? score : 0;
    }

    public static int GetScoreB(this Room room)
    {
        return (room.CustomProperties[GrovalConst.ScoreBKey] is int score) ? score : 0;
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

    public static void SetScoreA(this Room room, int score)
    {
        propsToSet[GrovalConst.ScoreAKey] = score;
        room.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }

    public static void SetScoreB(this Room room, int score)
    {
        propsToSet[GrovalConst.ScoreBKey] = score;
        room.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }

    public static void SetInit(this Room room, int Anum, int Bnum)
    {
        propsToSet[GrovalConst.ANumKey] = Anum;
        propsToSet[GrovalConst.BNumKey] = Bnum;
        propsToSet[GrovalConst.ScoreAKey] = 0;
        propsToSet[GrovalConst.ScoreBKey] = 0;
        room.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }
}