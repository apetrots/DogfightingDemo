using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    public float MoveSpeed = 80.0f;
    public float TurnSpeed = 10.0f;

    public float LaserForce = 20.0f;

    public float DamageCooldown = 0.25f;

    public GameObject laserPrefab;
    // can specifically reference the transform of a gameobject
    public Transform laserSpawnPoint;

    public int HealthPoints = 4;

    public SpriteRenderer DamageOverlay;

    public Sprite[] DamageSprites;

    Player target;

    Rigidbody2D rb2d;


    // timer to store when the next time this player can be damaged is,
    // if damageTimer > 0 then invulnerable to collision damage...
    float damageTimer = 0.0f;

    void Start()
    {
    // physics 1st pass 
        rb2d = GetComponent<Rigidbody2D>();

        target = FindObjectOfType<Player>();
    }

    void Damage(int hitPoints)
    {
        HealthPoints -= hitPoints;
        if (HealthPoints <= 0)
        {
            // game over!
            print("dead!");
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

    void OnCollisionStay2D(Collision2D coll)
    {
        Asteroid ast = coll.gameObject.GetComponent<Asteroid>();
        if (ast != null && damageTimer <= 0.0f)
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
    }

    void Update()
    {
        UpdateDamageOverlay();

        if (damageTimer > 0.0f)
            damageTimer -= Time.deltaTime;

        // NonPhysicsMove();
        if (Input.GetButtonDown("Jump"))
        {
            FireLaser(LaserForce);
        }
    }

    void FixedUpdate()
    {
        Vector3 pointingDir = transform.up;
        rb2d.AddForce(pointingDir * MoveSpeed);


        // turning towards player
        Vector2 dirToTarget = (target.transform.position - transform.position).normalized;

        float angle = Vector2.SignedAngle(pointingDir, dirToTarget);

        rb2d.AddTorque(TurnSpeed * Mathf.Sign(angle));
    }
}
