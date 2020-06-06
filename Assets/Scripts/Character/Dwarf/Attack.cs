using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float damage;
    public float baseKB;
    public float KBGrowth;
    public Vector3 velocity;
    private Vector2 direction;
    public string attackType;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            if (other.gameObject.GetComponent<PlayerController>().getIsGrounded() && attackType == "spike") {
                direction = velocity.normalized;
            } else if(attackType == "spike") {
                direction = -velocity.normalized;
            } else if (attackType == "swipe") {
                direction.x = (other.gameObject.GetComponent<Rigidbody2D>().transform.position.x - gameObject.transform.position.x);
                direction.y = velocity.y;
                direction = direction.normalized;
            }

            other.gameObject.GetComponent<PlayerController>().Knockback(direction, baseKB, KBGrowth, damage);
        }
    }
}
