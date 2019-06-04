using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicProjectalScript : MonoBehaviour
{
    public ParticleSystem sparks;
    public GameObject projectile;

    private bool coliided=false;
    public float force = 10.0f;
    public float killingRange = 0.25f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!colision())//move forward while emiting
        {
            transform.Translate(Vector3.forward * Time.deltaTime*20);
            sparks.Emit(10);
        }
        else
        {
            if(coliided==false)
            {
                sparks.Emit(1000);//short particles burst
                coliided = true;
            }
            else
            {
                if(sparks.particleCount<10)
                {
                    Destroy(gameObject);//kill projectile
                }
                else
                {
                    magneticBehavior();//draw particles to projectile
                }

            }
        }
    }
    private bool colision()
    {
        if (transform.position.x < 10)
            return false;
        else
            return true;
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
}
