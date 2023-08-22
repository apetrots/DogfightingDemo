using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D coll)
    {
        // "coll.gameObject" is the other GameObject involved in the 
        // collision, colliding with the one this script is attached to.
        Laser laser = coll.gameObject.GetComponent<Laser>();

        // if GetComponent did not find the script on the other object, 
        // that means it's not a laser object and the variable is set to 
        // "null" or nothing 
        if (laser != null)
        {
            SplitIntoTwo();            
        }
    }

    void SplitIntoTwo()
    {
        // TODO need to spawn them next to eachother but on opposite sides of object, get bounds of collider
        
        // spawn in two copies of this asteroid
        GameObject split1 = Instantiate(this.gameObject, transform.position, transform.rotation);
        GameObject split2 = Instantiate(this.gameObject, transform.position, transform.rotation);

        // for the two copies, scale them by half so they're half as big
        split1.transform.localScale /= 2;
        split2.transform.localScale /= 2;

        // also scale their rigidbody masses accordingly
        split1.GetComponent<Rigidbody2D>().mass /= 2;
        split2.GetComponent<Rigidbody2D>().mass /= 2;

        // destroy original asteroid!
        Destroy(this.gameObject);
    }
    
    void Update()
    {
    }
}
