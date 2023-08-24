using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    public float MoveSpeed = 80.0f;
    public float TurnSpeed = 10.0f;

    public float LaserForce = 20.0f;

    public float DamageCooldown = 0.25f;
    public float DamageSpeedThreshold = 1.5f;
    public float LaserCooldown = 0.5f;
    
    public float AIShootMinAngle = 10.0f;
    public float AIShootMinDist = 8.0f;
    public float AICheckDistance = 8.0f;
    public float AIStopDistance = 3.0f;

    public GameObject laserPrefab;
    // can specifically reference the transform of a gameobject
    public Transform laserSpawnPoint;

    public int HealthPoints = 4;

    public SpriteRenderer DamageOverlay;

    public Sprite[] DamageSprites;

    public GameObject shipExplosionFX;
    public GameObject sparksFX;

    public GameObject droppedItem;

    Player target;

    Rigidbody2D rb2d;
    Collider2D collider;


    // timer to store when the next time this player can be damaged is,
    // if damageTimer > 0 then invulnerable to collision damage...
    float damageTimer = 0.0f;
    float laserTimer = 0.0f;

    void Start()
    {
    // physics 1st pass 
        rb2d = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();

        target = FindObjectOfType<Player>();
    }

    void Explode()
    {
        Instantiate(shipExplosionFX, transform.position, Quaternion.identity);
    }

    void Sparks()
    {
        Instantiate(sparksFX, transform.position, Quaternion.identity);
    }

    void Damage(int hitPoints)
    {
        HealthPoints -= hitPoints;
        if (HealthPoints <= 0)
        {
            Explode();
            if (droppedItem)
                Instantiate(droppedItem, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        else
        {
            Sparks();
        }

        UpdateDamageOverlay();
    }

    void UpdateDamageOverlay()
    {
        int spriteIndex = HealthPoints - 1;
        if (spriteIndex >= DamageSprites.Length || spriteIndex < 0)
            DamageOverlay.sprite = null;
        else
            DamageOverlay.sprite = DamageSprites[spriteIndex];
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Laser laser = coll.gameObject.GetComponent<Laser>();
        if (laser != null && damageTimer <= 0.0f)
        {
            Damage(1);

            damageTimer = DamageCooldown;
            return;
        }

        Asteroid ast = coll.gameObject.GetComponent<Asteroid>();
        Player player = coll.gameObject.GetComponent<Player>();
        if ((ast != null || player != null) && coll.relativeVelocity.magnitude >= DamageSpeedThreshold && damageTimer <= 0.0f)
        {
            Damage(1);

            damageTimer = DamageCooldown;
        }
    }

    void FireLaser(float force)
    {
        GameObject obj = Instantiate(laserPrefab, laserSpawnPoint.position, transform.rotation);
        var laserRB = obj.GetComponent<Rigidbody2D>();

        // ignore collision between the new laser's collider and the ship's collider
        Physics2D.IgnoreCollision(
                obj.GetComponent<Collider2D>(), 
                this.GetComponent<Collider2D>()
            );

        laserRB.AddForce(force * transform.up, ForceMode2D.Impulse);

        laserTimer = LaserCooldown;
    }

    void Update()
    {
        UpdateDamageOverlay();

        if (damageTimer > 0.0f)
            damageTimer -= Time.deltaTime;
        if (laserTimer > 0.0f)
            laserTimer -= Time.deltaTime;

        if (target == null)
            return;

        // decide if laser should be fired
        bool shouldFire = false;

        Vector3 pointingDir = transform.up;
        Vector2 dirToTarget = (target.transform.position - transform.position).normalized;

        float absAngle = Mathf.Abs(Vector2.SignedAngle(pointingDir, dirToTarget));

        if (target != null && absAngle <= AIShootMinAngle && Vector2.Distance(transform.position, target.transform.position) <= AIShootMinDist)
        {
            shouldFire = true;
        }

        if (laserTimer <= 0.0f && shouldFire)
        {
            FireLaser(LaserForce);
        }
    }

    float FindBestDirection()
    {
        Vector3 pointingDir = transform.up;

        // turning towards player
        Vector2 dirToTarget = (target.transform.position - transform.position).normalized;

        float angle = Vector2.SignedAngle(pointingDir, dirToTarget);

        RaycastHit2D[] hits = new RaycastHit2D[5];
        Physics2D.BoxCastNonAlloc(transform.position, collider.bounds.size, transform.rotation.z, pointingDir, hits, AIStopDistance, LayerMask.GetMask("Asteroids"));

        if (hits[0].collider != null)
        {
            return 1.0f;
        }

        return Mathf.Sign(angle);
    }

    void FixedUpdate()
    {
        if (target == null)
            return;

        Vector3 pointingDir = transform.up;

        RaycastHit2D hit = Physics2D.BoxCast(transform.position, collider.bounds.size, transform.rotation.z, pointingDir, AIStopDistance, LayerMask.GetMask("Asteroids", "Player"));
        if (!hit)
            rb2d.AddForce(pointingDir * MoveSpeed);
        else
            rb2d.AddForce(-pointingDir * 0.25f * MoveSpeed);

        rb2d.AddTorque(TurnSpeed * FindBestDirection());
    }
}
