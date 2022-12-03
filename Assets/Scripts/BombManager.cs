using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    public GameObject explosionPrefab; // 爆発エフェクト
    public LayerMask levelMask;
    private bool exploded = false; // すでに爆発している場合 true

    // 他のオブジェクトがこの爆弾に当たったら呼び出される
    public void OnTriggerEnter(Collider other)
    {
        // まだ爆発していない、
        // かつ、この爆弾にぶつかったオブジェクトが爆発エフェクトの場合
        if (!exploded && other.CompareTag("Explosion"))
        {
            // 2 重に爆発処理が実行されないように
            // すでに爆発処理が実行されている場合は止める
            CancelInvoke("Explode");

            // 爆発する
            Explode();
        }
    }

    // 爆弾が爆発する時の処理
    public void Explode()
    {
        // 爆弾の位置に爆発エフェクトを作成
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // 爆弾を非表示にする
        GetComponent<MeshRenderer>().enabled = false;
        // 爆風を広げる
        StartCoroutine(CreateExplosions(Vector3.forward)); // 上に広げる
        StartCoroutine(CreateExplosions(Vector3.right)); // 右に広げる
        StartCoroutine(CreateExplosions(Vector3.back)); // 下に広げる
        StartCoroutine(CreateExplosions(Vector3.left)); // 左に広げる
        transform.Find("Cylinder").gameObject.SetActive(false);

        // 0.3 秒後に非表示にした爆弾を削除
        Destroy(gameObject, 0.3f);
    }

    // 爆風を広げる
    private IEnumerator CreateExplosions(Vector3 direction)
    {
        // 2 マス分ループする
        for (int i = 1; i < 3; i++)
        {
            // ブロックとの当たり判定の結果を格納する変数
            RaycastHit hit;

            // 爆風を広げた先に何か存在するか確認
            Physics.Raycast
            (
                transform.position + new Vector3(0, 0.5f, 0),
                direction,
                out hit,
                i,
                levelMask
            );

            // 爆風を広げた先に何も存在しない場合
            if (!hit.collider)
            {
                // 爆風を広げるために、
                // 爆発エフェクトのオブジェクトを作成
                Instantiate
                (
                    explosionPrefab,
                    transform.position + (i * direction),
                    explosionPrefab.transform.rotation
                );
            }
            // 爆風を広げた先にオブジェクトが存在する場合
            else if (hit.collider.CompareTag("Destroyable"))
            {
                Instantiate
                (
                    explosionPrefab,
                    transform.position + (i * direction),
                    explosionPrefab.transform.rotation
                );
                break;
            }
            else
            {
                // 爆風はこれ以上広げない
                break;
            }

            // 0.05 秒待ってから、次のマスに爆風を広げる
            yield return new WaitForSeconds(0.05f);
        }
    }

}
