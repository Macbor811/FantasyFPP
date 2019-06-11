using System.Collections.Generic;
using UnityEngine;

public class bloodSplash : MonoBehaviour
{
    public GameObject bloodDropPrerfab;
    public int numberOfParticles = 100;
    public float speed=1.0f;
    public float gravity = 8.0f;
    public float noise = 1.5f;
    public float lifetime = 1.0f;
    public float lifetimeNoise = 0.1f;

    private List<BloodDrop> particles=new List<BloodDrop>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberOfParticles; i++)
        {
            particles.Add(new BloodDrop(bloodDropPrerfab, transform.position, transform.rotation, new Vector3(speed+Random.Range(-noise,noise), Random.Range(-noise, noise), Random.Range(-noise, noise)), new Vector3(0, -8+ Random.Range(-noise, noise),0),transform,lifetime+Random.Range(-lifetimeNoise,lifetimeNoise)));
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].Update();
            if(!particles[i].isAlive())
            {
                int a = Random.Range(0, 10);
                if(a == 0)
                        particles[i]=new BloodDrop(bloodDropPrerfab, transform.position, transform.rotation, new Vector3(speed + Random.Range(-noise, noise), Random.Range(-noise, noise), Random.Range(-noise, noise)), new Vector3(0, -8 + Random.Range(-noise, noise), 0), transform, lifetime + Random.Range(-lifetimeNoise, lifetimeNoise));
                else
                    particles.RemoveAt(i);
                if (particles.Count == 0)
                    Destroy(gameObject);
            }
        }
    }
}

public class BloodDrop:Object
{
    private GameObject bloodDrop;
    private Vector3 veloclity;
    private Vector3 acceleration;
    private float lifetime = 0;
    private float lifeLength;

    public BloodDrop(GameObject bloodDropPrefab, Vector3 position,Quaternion rotation, Vector3 veloclity, Vector3 acceleration,Transform parent, float lifeLength)
    {
        this.veloclity = veloclity;
        this.acceleration = acceleration;
        this.lifeLength = lifeLength;
        bloodDrop = bloodDropPrefab;
        bloodDrop = Instantiate(bloodDropPrefab, position, Quaternion.identity,parent);
    }

    public void Update()
    {
        bloodDrop.transform.localPosition += veloclity * Time.deltaTime;
        veloclity += acceleration * Time.deltaTime;
        lifetime += Time.deltaTime;
    }
    public bool isAlive()
    {
        if (lifetime < lifeLength)
            return true;
        else
        {
            Destroy(bloodDrop);
            return false;
        }
    }
}
