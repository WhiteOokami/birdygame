using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class propane : MonoBehaviour
{
    public float blastRadius;
    public float force;
    public LayerMask LayerToHit;
    public GameObject ExplosionEffect;
    public AudioSource _explosion;
    public AudioClip _explosionSound;
    public float explodeWhenVelocity = 10f;

    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    _explosion.clip = _explosionSound;
        //    _explosion.Play();
        //    explode();
        //}
    }

    void explode()
    {

        _explosion.clip = _explosionSound;
        _explosion.Play();

        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position,blastRadius,LayerToHit);

        foreach(Collider2D obj in objects)
        {
            Vector2 direction = obj.transform.position - transform.position;
            obj.GetComponent<Rigidbody2D>().AddForce(direction * force);
        }

        CameraShaker.Instance.ShakeOnce(3,3,0.1f,1f);
        GameObject ExplosionEffectIns = Instantiate(ExplosionEffect,transform.position,Quaternion.identity);
        Destroy(ExplosionEffectIns,10);
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,blastRadius);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 velocity = collision.relativeVelocity;
        if (velocity.x+velocity.y > explodeWhenVelocity)
            explode();
    }
}
