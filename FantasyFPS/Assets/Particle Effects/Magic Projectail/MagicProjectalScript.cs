using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicProjectalScript : MonoBehaviour
{
    public ParticleSystem sparks;

    private bool collided=false;
    public float speed = 20.0f;
    public float force = 10.0f;
    public float killingRange = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<DealDamage>().IsActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!collided)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            sparks.Emit(10);
        }
        else
        {
            if (sparks.particleCount < 10)
            {
                Destroy(gameObject);//kill projectile
            }
            else
            {
                magneticBehavior();//draw particles to projectile
            }

        }
    }

    private void magneticBehavior()
    {
        ParticleSystem.Particle[] particles=new ParticleSystem.Particle[sparks.particleCount];
        sparks.GetParticles(particles);
        for(int i=0;i<particles.Length;i++)
        {
            Vector3 direction = transform.position - particles[i].position;
            if (direction.x<killingRange&&direction.y<killingRange&&direction.z<killingRange)
            {
                particles[i].remainingLifetime = 0;
            }
            direction.Normalize();
            particles[i].velocity += (direction * force) * Time.deltaTime;

        }
        sparks.SetParticles(particles);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (!collided && collision.gameObject.tag == "Enemy")
        //{
        //    collision.gameObject.GetComponent<TakeDamage>().takeDamage(GetComponent<DealDamage>().damage);
        //}
        sparks.Emit(1000);
        collided = true;
    }
}
