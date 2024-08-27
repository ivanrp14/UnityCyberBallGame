using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance { get; private set; }

    [SerializeField]
    private GameObject bulletPrefab, bulletPlayer;


    [SerializeField]
    private int initialPoolSize = 10;

    private ObjectPool<Bullet> bulletPool;
    private ObjectPool<Bullet> bulletPoolPlayer;

    private void Awake()
    {
        Instance = this;
        bulletPool = new ObjectPool<Bullet>(bulletPrefab.GetComponent<Bullet>(), initialPoolSize, transform);
        bulletPoolPlayer = new ObjectPool<Bullet>(bulletPlayer.GetComponent<Bullet>(), initialPoolSize, transform);
    }

    public Bullet GetBullet()
    {
        return bulletPool.Get();
    }
    public Bullet GetBulletPlayer()
    {
        return bulletPoolPlayer.Get();
    }

    public void ReturnBullet(Bullet bullet)
    {
        bulletPool.ReturnToPool(bullet);
    }
}
