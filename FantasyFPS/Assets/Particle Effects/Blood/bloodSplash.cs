using System.Collections.Generic;
using UnityEngine;

public class bloodSplash : MonoBehaviour
{
    public GameObject bloodDropPrerfab;
    public int numberOfParticles = 100;
    public float speed=10.0f;
    public float gravity = 8.0f;
    public float noise = 1.0f;
    public float scale = 0.3f;
    public float scaleNoise = 0.1f;
    public float lifetime = 1.0f;
    public float lifetimeNoise = 0.1f;

    private List<BloodDrop> particles=new List<BloodDrop>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberOfParticles; i++)
        {
            BloodDrop tmp=new BloodDrop(bloodDropPrerfab, transform.position, transform.rotation ,transform);
            tmp.SetVelocity(new Vector3(speed + Random.Range(-noise, noise), Random.Range(-noise, noise), Random.Range(-noise, noise)));
            tmp.SetAcceleration(new Vector3(0, -8 + Random.Range(-noise, noise), 0));
            tmp.SetLifeLength(lifetime + Random.Range(-lifetimeNoise, lifetimeNoise));
            tmp.SetScale(scale+ Random.Range(-noise, noise));
            particles.Add(tmp);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].Update();
            if (!particles[i].isAlive())
            {
                int a = Random.Range(0, 10);
                if (a == 0)
                {
                    //particles[i] = new BloodDrop(bloodDropPrerfab, transform.position, transform.rotation,transform);
                    particles[i].SetPosition(transform.position);
                    particles[i].SetVelocity(new Vector3(speed + Random.Range(-noise, noise), Random.Range(-noise, noise), Random.Range(-noise, noise)));
                    particles[i].ResetLifeTime();
                }
                else
                {
                    particles[i].Destroy();
                    particles.RemoveAt(i);
                }
                if (particles.Count == 0)
                    Destroy(gameObject);
            }
        }
    }
}

public class BloodDrop:Object
{
    private GameObject bloodDrop;
    private Vector3 velocity;
    private Vector3 acceleration;
    private float lifetime = 0;
    private float lifeLength;

    public BloodDrop(GameObject bloodDropPrefab, Vector3 position,Quaternion rotation, Transform parent)
    {
        bloodDrop = bloodDropPrefab;
        bloodDrop = Instantiate(bloodDropPrefab, position, Quaternion.identity,parent);
    }

    public void ResetLifeTime() { this.lifetime = 0;  }
    public void SetPosition(Vector3 position) { this.bloodDrop.transform.position = position; }
    public void SetVelocity(Vector3 velocity){ this.velocity = velocity; }
    public void SetAcceleration(Vector3 acceleration) { this.acceleration = acceleration; }
    public void SetLifeLength(float lifeLength) { this.lifeLength = lifeLength;  }
    public void SetScale(float scale) { this.bloodDrop.transform.localScale=new Vector3(scale,scale,scale); }

    public void Update()
    {
        bloodDrop.transform.localPosition += velocity * Time.deltaTime;
        velocity += acceleration * Time.deltaTime;
        lifetime += Time.deltaTime;
    }
    public bool isAlive()
    {
        if (lifetime < lifeLength)
            return true;
        else
        {
            //Destroy(bloodDrop);
            return false;
        }
    }
    public void Destroy() { Destroy(bloodDrop); }
}
