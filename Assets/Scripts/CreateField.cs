using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using TMPro;

public class CreateField : MonoBehaviourPunCallbacks
{
    /*
    *�ݒ肷��l
    */
    public int max = 9;        //�c���̃T�C�Y ���K����ɂ��邱��
    public GameObject wall;    //�Ǘp�I�u�W�F�N�g
    public GameObject wall_destroyable;    //�Ǘp�I�u�W�F�N�g
    public GameObject floor_black;    //���p�I�u�W�F�N�g
    public GameObject floor_white;    //���p�I�u�W�F�N�g
    public GameObject teamA;   //�`�[��A�p�I�u�W�F�N�g
    public GameObject teamB;    //�`�[��B�p�I�u�W�F�N�g
    public GameObject coin;     //�R�C���p�I�u�W�F�N�g
    public GameObject trap;     //�g���b�v�p�I�u�W�F�N�g
    public GameObject bomb;     //���e�p�I�u�W�F�N�g
    public GameObject trapContainer;
    public Material transparentMaterial;
    /*
    *�����p�����[�^
    */
    private int[,] walls;      //�}�b�v�̏�� 0�F�� 1�F�ʘH
    private int[] PosA;    //�X�^�[�g�̍��W
    private int[] PosB;     //�S�[���̍��W

    public void CreateWallAndCharacter()
    {
        //�}�b�v��ԏ�����
        walls = new int[max, max];

        //�X�^�[�g�n�_�̎擾
        PosA = GetStartPosition();

        //�ʘH�̐���
        //����̓S�[���n�_��ݒ肷��
        PosB = MakeDungeonMap(PosA);
        //�ʘH�������J��Ԃ��đ܏��H�����炷
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
                // �����_���ɕǂ�����
                if (Random.Range(0, 2) == 0) walls[i, j] = 1;
                // �����_����2��ޖڂ̕ǂ�u��
                if (Random.Range(0, 10) == 0) walls[i, j] = 2;
            }
        }

        int randx;
        int randy;
        while (PosA[0] == PosB[0] && PosA[1] == PosB[1])
        {
            randx = Random.Range(0, max);
            randy = Random.Range(0, max);
            PosB[0] = randx;
            PosB[1] = randy;
        }

        // A��B�̍��W�ɕǂ����Ԃ�Ȃ��悤�ɒʘH�ɂ��Ă���
        walls[PosA[0], PosA[1]] = 1;
        walls[PosB[0], PosB[1]] = 1;
        BuildDungeon();

        photonView.RPC(nameof(CreateCharacter), RpcTarget.AllBuffered, PosA[0], PosA[1], PosB[0], PosB[1]);
    }

    [PunRPC]
    public void CreateCharacter(int x1, int z1, int x2, int z2)
    {
        Instantiate(teamA, new Vector3(x1, -0.5f, z1), Quaternion.identity);
        Instantiate(teamB, new Vector3(x2, -0.5f, z2), Quaternion.identity);
    }

    /*
    *�X�^�[�g�n�_�̎擾
    */
    int[] GetStartPosition()
    {
        //�����_����x,y��ݒ�
        int randx = Random.Range(0, max);
        int randy = Random.Range(0, max);

        //x�Ay�������������ɂȂ�܂ŌJ��Ԃ�
        while (randx % 2 != 0 || randy % 2 != 0)
        {
            randx = Mathf.RoundToInt(Random.Range(0, max));
            randy = Mathf.RoundToInt(Random.Range(0, max));
        }

        return new int[] { randx, randy };
    }

    /*
    *�}�b�v����
    */
    int[] MakeDungeonMap(int[] _PosA)
    {
        //�X�^�[�g�ʒu�z��𕡐�
        int[] tmpStartPos = new int[2];
        _PosA.CopyTo(tmpStartPos, 0);
        //�ړ��\�ȍ��W�̃��X�g���擾
        Dictionary<int, int[]> movePos = GetPosition(tmpStartPos);

        //�ړ��\�ȍ��W���Ȃ��Ȃ�܂ŒT�����J��Ԃ�
        while (movePos != null)
        {
            //�ړ��\�ȍ��W���烉���_����1�擾���ʘH�ɂ���
            int[] tmpPos = movePos[Random.Range(0, movePos.Count)];
            walls[tmpPos[0], tmpPos[1]] = 1;

            //���̒n�_�ƒʘH�ɂ������W�̊Ԃ�ʘH�ɂ���
            int xPos = tmpPos[0] + (tmpStartPos[0] - tmpPos[0]) / 2;
            int yPos = tmpPos[1] + (tmpStartPos[1] - tmpPos[1]) / 2;
            walls[xPos, yPos] = 1;

            //�ړ���̍��W���ꎞ�ϐ��Ɋi�[���A�ēx�ړ��\�ȍ��W��T������
            tmpStartPos = tmpPos;
            movePos = GetPosition(tmpStartPos);
        }
        //�T���I�����̍��W��Ԃ�
        return tmpStartPos;
    }

    /*
    *�ړ��\�ȍ��W�̃��X�g���擾����
    */
    Dictionary<int, int[]> GetPosition(int[] _PosA)
    {
        //�ǐ��̂��ߍ��W��ϐ��Ɋi�[
        int x = _PosA[0];
        int y = _PosA[1];

        //�ړ���������2���x,y���W�����v�Z
        List<int[]> position = new List<int[]> {
            new int[] {x, y + 2},
            new int[] {x, y - 2},
            new int[] {x + 2, y},
            new int[] {x - 2, y}
        };

        //�ړ��������Ɉړ���̍��W���͈͓����ǂł��邩�𔻒肷��
        //�^�ł���΁A�ԋp�p���X�g�ɒǉ�����
        Dictionary<int, int[]> positions = position.Where(p => !isOutOfRange(p[0], p[1]) && walls[p[0], p[1]] == 0)
                                                   .Select((p, i) => new { p, i })
                                                   .ToDictionary(p => p.i, p => p.p);
        //�ړ��\�ȏꏊ�����݂��Ȃ��ꍇnull��Ԃ�
        return positions.Count() != 0 ? positions : null;
    }

    //�^����ꂽx�Ay���W���͈͊O�̏ꍇ�^��Ԃ�
    bool isOutOfRange(int x, int y)
    {
        return (x < 0 || y < 0 || x >= max || y >= max);
    }

    //�p�����[�^�ɉ����ăI�u�W�F�N�g�𐶐�����
    [PunRPC]
    void BuildDungeon()
    {
        int[,] _walls = walls;
        //�c��1�}�X���傫�����[�v���񂵁A�ǂƂ���
        for (int i = -1; i <= max; i++)
        {
            for (int j = -1; j <= max; j++)
            {
                //�͈͊O�A�܂��͕ǂ̏ꍇ�ɕǃI�u�W�F�N�g�𐶐�����
                if (isOutOfRange(i, j)
                    || _walls[i, j] == 0)
                {
                    photonView.RPC(nameof(RPCCreateWall), RpcTarget.AllBuffered, new Vector3(i, 0, j));
                }
                else if (_walls[i, j] == 2)
                {
                    photonView.RPC(nameof(RPCCreateObstacle), RpcTarget.AllBuffered, new Vector3(i, 0, j));
                }
            }
        }
    }

    [PunRPC]
    public void RPCCreateWall(Vector3 targetPos)
    {
        Instantiate(wall, targetPos, Quaternion.identity);
    }

    [PunRPC]
    public void RPCCreateObstacle(Vector3 targetPos)
    {
        Instantiate(wall_destroyable, targetPos, Quaternion.identity);
    }

    public void CreateFloor()
    {
        for (int i = -1; i <= max; i++)
        {
            for (int j = -1; j <= max; j++)
            {
                //�S�Ă̏ꏊ�ɏ��I�u�W�F�N�g�𐶐�
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
    public void RPCCreateCoin(Vector3 targetPos)
    {
        Instantiate(coin, targetPos, coin.transform.rotation);
    }

    [PunRPC]
    public void RPCCreateBomb(Vector3 targetPos)
    {
        targetPos.y = -0.1f;
        Instantiate(bomb, targetPos, bomb.transform.rotation);
    }

    // �}�X�^�[�̂ݎ��s
    public (int x, int z, int type) CreatePoint()
    {
        (int x, int z, int type) point;
        do
        {
            point.x = Random.Range(0, max);
            point.z = Random.Range(0, max);
        } while (Physics.OverlapSphere(new Vector3(point.x, -0.3f, point.z), 0).Length > 0);
        point.type = Random.Range(0, 2);
        return point;
    }

    public void RPCCreateTrap(int posX, int posZ, int trapType, bool transparent)
    {
        if (Physics.OverlapSphere(new Vector3(posX, -0.3f, posZ), 0).Length <= 0)
        {
            GameObject trapObj = Instantiate(trap, new Vector3(posX, -0.45f, posZ), Quaternion.identity) as GameObject;
            trapObj.transform.parent = trapContainer.transform;

            if (trapType == 0) trapObj.GetComponentInChildren<TextMeshPro>().text = "R";
            if (transparent)
            {
                trapObj.GetComponentInChildren<Renderer>().material = transparentMaterial;
                trapObj.GetComponentInChildren<TextMeshPro>().color = new Color(0, 0, 0, 0);
            }
        }
    }

}