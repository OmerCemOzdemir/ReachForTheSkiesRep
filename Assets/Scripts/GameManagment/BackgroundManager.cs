using UnityEngine;

public class BackgroundManager : MonoBehaviour
{

    [Range(-1f, 1f)][SerializeField] private float scrollSpeed = 0.5f;
    private float distance;
    private Material mat;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        distance += Time.deltaTime * scrollSpeed;
        mat.SetTextureOffset("_MainTex", -Vector2.down * distance);
    }

}


/*
  offset += (Time.deltaTime * scrollSpeed) / 10f;
        mat.SetTextureOffset("_MainTex", new Vector2(0, offset));
 
 */

/*
     [SerializeField] private GameObject[] backgroundSlow;
    [SerializeField] private GameObject[] backgroundFast;

    [SerializeField] private GameObject upPos;
    [SerializeField] private GameObject downPos;

    [SerializeField] private Transform tempPos;

    private Vector3 teleportPos;

    private void Awake()
    {
        teleportPos = tempPos.position;
    }


    private void MoveBackground(float speed, GameObject[] backgrounds, float Z)
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            if (backgrounds[i].transform.position.y <= downPos.transform.position.y)
            {
                backgrounds[i].transform.position = teleportPos;
                // Debug.Log("Teleport Background");
            }
            backgrounds[i].transform.position += new Vector3(0, 0.01f * speed, 0);

        }


    }

    private void Update()
    {
       // MoveBackground(-2, backgroundFast, 11);
        MoveBackground(-1, backgroundSlow, 10);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Screen Height: " + Screen.height);
        }

    }

 
 */