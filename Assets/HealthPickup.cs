using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int Health = 1;

    void OnTriggerEnter2D(Collider2D coll)
    {
        var player = coll.gameObject.GetComponent<Player>();
        if (player)
        {
            player.HealthPoints = Mathf.Clamp(player.HealthPoints + Health, 0, 4);

            Destroy(this.gameObject);
        }
    }
}
