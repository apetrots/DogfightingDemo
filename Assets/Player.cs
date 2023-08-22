using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
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

    Rigidbody2D rb2d;

    // timer to store when the next time this player can be damaged is,
    // if damageTimer > 0 then invulnerable to collision damage...
    float damageTimer = 0.0f;

    void Start()
    {
    // physics 1st pass 
        rb2d = GetComponent<Rigidbody2D>();
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
        print(spriteIndex);
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
        PhysicsMove();
    }

    void PhysicsMove()
    {
        float xDir = Input.GetAxis("Horizontal");
        float yDir = Input.GetAxis("Vertical");

    // 1st pass: straight ahead!
        Vector3 pointingDir = transform.up;
        rb2d.AddForce(pointingDir * MoveSpeed * yDir);
    // 2nd pass: turning with physics
        rb2d.AddTorque(TurnSpeed * -xDir);
    }

    void NonPhysicsMove()
    {
        float xDir = Input.GetAxis("Horizontal");
        float yDir = Input.GetAxis("Vertical");

    // 1st pass
        // Vector3 newPos = transform.position;
        // newPos.x += xDir * MoveSpeed;
        // newPos.y += yDir * MoveSpeed;
        // transform.position = newPos;

    // 2nd pass: condense
        // transform.position += new Vector3(xDir * MoveSpeed, yDir * MoveSpeed, 0.0f);

    // 3rd pass: delta time
        // transform.position += new Vector3(xDir * MoveSpeed * Time.deltaTime, yDir * MoveSpeed * Time.deltaTime, 0.0f);

    // 4th pass: condense and explain vector math, vector * scalar is element-wise
        // transform.position += new Vector3(xDir, yDir, 0.0f) * MoveSpeed * Time.deltaTime;

    // 5th pass: normalize vector
        // Vector3 moveDir = new Vector3(xDir, yDir, 0.0f);
        // transform.position += moveDir.normalized * MoveSpeed * Time.deltaTime;

    // 6th pass: rotation of ship and going towards where pointing
        // Vector3 rotation = transform.eulerAngles;

        // // z-axis: 2d rotation
        // rotation.z += xDir * TurnSpeed * Time.deltaTime; 

        // transform.eulerAngles = rotation;

    // 7th pass: rotation direction fix and movement forward
        Vector3 rotation = transform.eulerAngles;

        // z-axis: 2d rotation
        rotation.z += (-xDir) * TurnSpeed * Time.deltaTime; 

        transform.eulerAngles = rotation;

        // .up variable is based on how the transform is currently rotated
        Vector3 moveDir = transform.up;

        // moveDir = direction of movement (forward), yDir = which way the player is inputting to move, 
        // MoveSpeed = base move speed 
        transform.position += moveDir * yDir * MoveSpeed * Time.deltaTime;

    }
}
