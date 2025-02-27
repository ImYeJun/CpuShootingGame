using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletManager : MonoBehaviour
{
    public static PlayerBulletManager Instance { get; private set; }
    private List<GameObject> bulletPool = new List<GameObject>();
    [SerializeField] private GameObject equippedBullet;
    [SerializeField] private int initialBulletSpawnCnt;
    [SerializeField] private int supplyBulletCnt;
    [SerializeField] private int maxBulletCnt;
    [SerializeField] private int activatedBulletCnt;
    [SerializeField] private int bulletIterator;

    private void Awake()
    {
        if (Instance != null){
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        maxBulletCnt = initialBulletSpawnCnt;
        bulletIterator = 0;

        for (int i = 0; i < maxBulletCnt; i++){
            GameObject spawnedBullet = Instantiate(equippedBullet, transform.position, Quaternion.identity);
            spawnedBullet.transform.SetParent(transform);

            //The Bullet is aligned with X axis. So need to be rotated
            Quaternion bulletAngle = Quaternion.Euler(0,0,90);
            spawnedBullet.transform.rotation = bulletAngle;

            bulletPool.Add(spawnedBullet);

            spawnedBullet.SetActive(false);
        }
    }

    public GameObject GetBullet(){

        if (activatedBulletCnt >= maxBulletCnt){
            for (int i = 0; i < supplyBulletCnt; i++){
                GameObject spawnedBullet = Instantiate(equippedBullet, transform.position, Quaternion.identity);
                spawnedBullet.transform.SetParent(transform);

                bulletPool.Add(spawnedBullet);

                //The Bullet is aligned with X axis. So need to be rotated
                Quaternion bulletAngle = Quaternion.Euler(0,0,90);
                spawnedBullet.transform.rotation = bulletAngle;

                spawnedBullet.SetActive(false);
            }

            maxBulletCnt += supplyBulletCnt;
        }

        activatedBulletCnt++;
        bulletIterator++;
        bulletIterator %= maxBulletCnt;

        return bulletPool[bulletIterator];
    }

    public void ReturnBullet(GameObject returnedBullet){
        Type equipedBulletType = equippedBullet.GetType();
        Type returnedBulletType = returnedBullet.GetType();

        if (returnedBulletType != equipedBulletType){
            Debug.Log($"Wrong Bullet was returned ({returnedBullet} is not {equipedBulletType})");
            return;
        }
        returnedBullet.transform.SetParent(transform);
        returnedBullet.transform.position = transform.position;

        returnedBullet.SetActive(false);

        activatedBulletCnt--;
    }
}
