using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;

public class CreateField : MonoBehaviourPunCallbacks
{
    /*
    *設定する値
    */
    public int max = 11;        //縦横のサイズ ※必ず奇数にすること
    public GameObject wall;    //壁用オブジェクト
    public GameObject wall_destroyable;    //壁用オブジェクト
    public GameObject floor_black;    //床用オブジェクト
    public GameObject floor_white;    //床用オブジェクト
    public GameObject teamA;   //チームA用オブジェクト
    public GameObject teamB;    //チームB用オブジェクト
    public GameObject coin;     //コイン用オブジェクト

    /*
    *内部パラメータ
    */
    private int[,] walls;      //マップの状態 0：壁 1：通路
    private int[] PosA;    //スタートの座標
    private int[] PosB;     //ゴールの座標

    public void Create()
    {
        //マップ状態初期化
        walls = new int[max, max];

        //スタート地点の取得
        PosA = GetStartPosition();

        //通路の生成
        //初回はゴール地点を設定する
        do
        {
            PosB = MakeDungeonMap(PosA);
        } while (PosA == PosB);// PosAとPosBが一致しないように繰り返す
        //通路生成を繰り返して袋小路を減らす
        int[] tmpStart = PosB;
        for (int i = 0; i < max * 5; i++)
        {
            MakeDungeonMap(tmpStart);
            tmpStart = GetStartPosition();
        }

        for (int i = 0; i < max; i++)
        {
            for (int j = 0; j < max; j++)
            {
                if ((PosA[0] != i || PosA[1] != j) && (PosB[0] != i || PosB[1] != j))
                {
                    // ランダムに壁を消す
                    if (Random.Range(0, 2) == 0) walls[i, j] = 1;
                    // ランダムに2種類目の壁を置く
                    if (Random.Range(0, 10) == 0) walls[i, j] = 2;
                }
            }
        }

        //マップの状態に応じて壁と通路を生成する
        BuildDungeon();

        //スタート地点とゴール地点にオブジェクトを配置する
        //初回で取得したスタート地点とゴール地点は必ずつながっているので破綻しない
        PhotonNetwork.InstantiateRoomObject(teamA.name, new Vector3(PosA[0], 0, PosA[1]), Quaternion.identity);
        PhotonNetwork.InstantiateRoomObject(teamB.name, new Vector3(PosB[0], 0, PosB[1]), Quaternion.identity);
    }

    /*
    *スタート地点の取得
    */
    int[] GetStartPosition()
    {
        //ランダムでx,yを設定
        int randx = Random.Range(0, max);
        int randy = Random.Range(0, max);

        //x、yが両方共偶数になるまで繰り返す
        while (randx % 2 != 0 || randy % 2 != 0)
        {
            randx = Mathf.RoundToInt(Random.Range(0, max));
            randy = Mathf.RoundToInt(Random.Range(0, max));
        }

        return new int[] { randx, randy };
    }

    /*
    *マップ生成
    */
    int[] MakeDungeonMap(int[] _PosA)
    {
        //スタート位置配列を複製
        int[] tmpStartPos = new int[2];
        _PosA.CopyTo(tmpStartPos, 0);
        //移動可能な座標のリストを取得
        Dictionary<int, int[]> movePos = GetPosition(tmpStartPos);

        //移動可能な座標がなくなるまで探索を繰り返す
        while (movePos != null)
        {
            //移動可能な座標からランダムで1つ取得し通路にする
            int[] tmpPos = movePos[Random.Range(0, movePos.Count)];
            walls[tmpPos[0], tmpPos[1]] = 1;

            //元の地点と通路にした座標の間を通路にする
            int xPos = tmpPos[0] + (tmpStartPos[0] - tmpPos[0]) / 2;
            int yPos = tmpPos[1] + (tmpStartPos[1] - tmpPos[1]) / 2;
            walls[xPos, yPos] = 1;

            //移動後の座標を一時変数に格納し、再度移動可能な座標を探索する
            tmpStartPos = tmpPos;
            movePos = GetPosition(tmpStartPos);
        }
        //探索終了時の座標を返す
        return tmpStartPos;
    }

    /*
    *移動可能な座標のリストを取得する
    */
    Dictionary<int, int[]> GetPosition(int[] _PosA)
    {
        //可読性のため座標を変数に格納
        int x = _PosA[0];
        int y = _PosA[1];

        //移動方向毎に2つ先のx,y座標を仮計算
        List<int[]> position = new List<int[]> {
            new int[] {x, y + 2},
            new int[] {x, y - 2},
            new int[] {x + 2, y},
            new int[] {x - 2, y}
        };

        //移動方向毎に移動先の座標が範囲内かつ壁であるかを判定する
        //真であれば、返却用リストに追加する
        Dictionary<int, int[]> positions = position.Where(p => !isOutOfRange(p[0], p[1]) && walls[p[0], p[1]] == 0)
                                                   .Select((p, i) => new { p, i })
                                                   .ToDictionary(p => p.i, p => p.p);
        //移動可能な場所が存在しない場合nullを返す
        return positions.Count() != 0 ? positions : null;
    }

    //与えられたx、y座標が範囲外の場合真を返す
    bool isOutOfRange(int x, int y)
    {
        return (x < 0 || y < 0 || x >= max || y >= max);
    }

    //パラメータに応じてオブジェクトを生成する
    void BuildDungeon()
    {
        //縦横1マスずつ大きくループを回し、壁とする
        for (int i = -1; i <= max; i++)
        {
            for (int j = -1; j <= max; j++)
            {
                //範囲外、または壁の場合に壁オブジェクトを生成する
                if (isOutOfRange(i, j)
                    || walls[i, j] == 0)
                {
                    PhotonNetwork.InstantiateRoomObject(wall.name, new Vector3(i, 0, j), Quaternion.identity);
                }
                else if (walls[i, j] == 2)
                {
                    PhotonNetwork.InstantiateRoomObject(wall_destroyable.name, new Vector3(i, 0, j), Quaternion.identity);
                }
            }
        }
    }

    public void CreateFloor()
    {
        for (int i = -1; i <= max; i++)
        {
            for (int j = -1; j <= max; j++)
            {
                //全ての場所に床オブジェクトを生成
                if (Mathf.Abs(i) % 2 == 0 && Mathf.Abs(j) % 2 == 1 || Mathf.Abs(i) % 2 == 1 && Mathf.Abs(j) % 2 == 0)
                {
                    GameObject floorObj = Instantiate(floor_black, new Vector3(i, -0.5f, j), Quaternion.Euler(90, 0, 0)) as GameObject;
                    floorObj.transform.parent = transform;
                }
                else
                {
                    GameObject floorObj = Instantiate(floor_white, new Vector3(i, -0.5f, j), Quaternion.Euler(90, 0, 0)) as GameObject;
                    floorObj.transform.parent = transform;
                }
            }
        }
    }

    [PunRPC]
    public void CreateCoin()
    {
        int randx;
        int randz;
        do
        {
            randx = Random.Range(0, max);
            randz = Random.Range(0, max);
        } while (Physics.OverlapSphere(new Vector3(randx, 0.3f, randz), 0).Length > 0);
        PhotonNetwork.InstantiateRoomObject(coin.name, new Vector3(randx, 0, randz), Quaternion.Euler(90, 0, 0));
    }
    
    [PunRPC]
    public void RPCPutObstacle(Vector3 targetPos)
    {
        if(Physics.OverlapSphere(targetPos, 0.3f).Length <= 0)
        {
            PhotonNetwork.InstantiateRoomObject(wall_destroyable.name, targetPos, Quaternion.identity);
        }
    }

}