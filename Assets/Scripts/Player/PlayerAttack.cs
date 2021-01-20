using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float damage;
    public float hitEffect;
    public float baseKnockback;
    public float knockbackGrowth;
    public float hitLagDuration;
    public bool isInHitFreeze;
    public Vector2 directionalInfluence;
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

            //hitfreeze method to calculate DI
            //store player's previous swipe direction for 3 frame buffer and use that vector to subtract from the attack direction
            other.gameObject.GetComponent<PlayerController>()._isInHitLag = true;
            gameObject.GetComponentInParent<PlayerController>()._isInHitLag = true;

            StartCoroutine(HitFreeze(other, (int)((damage / 3 + 3) * hitEffect)));
        }
    }

    internal IEnumerator HitFreeze(Collider2D other, int hitLag) {
        yield return StartCoroutine(Assets.Scripts.Helper.WaitFor.Frames(hitLag));
        directionalInfluence = other.gameObject.GetComponent<PlayerController>()._bufferedDirection;
        other.gameObject.GetComponent<PlayerCollisionScript>().Knockback(direction, baseKnockback, knockbackGrowth, damage, directionalInfluence);
        other.gameObject.GetComponent<PlayerController>()._isInHitLag = false;
        gameObject.GetComponentInParent<PlayerController>()._isInHitLag = false;
    }
}
