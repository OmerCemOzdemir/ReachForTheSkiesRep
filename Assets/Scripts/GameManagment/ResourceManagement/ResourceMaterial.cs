using System.Collections;
using UnityEngine;

public class ResourceMaterial : MonoBehaviour
{
    private Resource resourceType = Resource.Organic;
    public Resource ResourceType { get => resourceType; set => resourceType = value; }
    public int ResourceValue { get => resourceValue; set => resourceValue = value; }

    private Transform playerTransform;
    [SerializeField] private GameObject model;
    [SerializeField] private ParticleSystem effect;
    [SerializeField] private AudioSource sound;

    [SerializeField] private float speedOfResource;
    [SerializeField] private Sprite organicSprite;
    [SerializeField] private Sprite metalSprite;
    [SerializeField] private Sprite chemicalSprite;
    private bool pickUp = false;
    private int resourceValue = 1;
    private float distance;

    private void Start()
    {
        switch (resourceType)
        {
            case Resource.Organic:
                model.GetComponent<SpriteRenderer>().sprite = organicSprite;
                break;
            case Resource.MetalScrap:
                model.GetComponent<SpriteRenderer>().sprite = metalSprite;
                break;
            case Resource.Chemical:
                model.GetComponent<SpriteRenderer>().sprite = chemicalSprite;
                break;
        }

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            switch (resourceType)
            {
                case Resource.Organic:
                    SaveData.instance.organicMaterials += resourceValue;
                    StartCoroutine(DestroyDelay());
                    break;
                case Resource.MetalScrap:
                    SaveData.instance.metalScrapMaterials += resourceValue;
                    StartCoroutine(DestroyDelay());

                    break;
                case Resource.Chemical:
                    SaveData.instance.chemicalMaterials += resourceValue;
                    StartCoroutine(DestroyDelay());

                    break;
            }

            UnityEngine.Debug.Log("Resources: Chem: " + SaveData.instance.chemicalMaterials + " Metal: " + SaveData.instance.metalScrapMaterials + " Organic: " + SaveData.instance.organicMaterials);
        }
    }

    IEnumerator DestroyDelay()
    {
        sound.Play();
        pickUp = true;
        effect.Play();
        GetComponent<CircleCollider2D>().enabled = false;
        model.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }


    private void Update()
    {
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        distance = Vector2.Distance(transform.position, playerTransform.position);
        Vector2 direction = playerTransform.position - transform.position;
        if (!pickUp)
        {
            if (distance < 4)
            {
                transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speedOfResource * Time.deltaTime);
            }
            else
            {
                transform.position += new Vector3(0, -0.01f, 0);

            }

        }


    }


}

public enum Resource
{
    Organic,
    MetalScrap,
    Chemical,
}