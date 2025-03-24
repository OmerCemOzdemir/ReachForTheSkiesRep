using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRPGController : MonoBehaviour
{
    private Rigidbody2D playerCharacterRigidbody;
    [SerializeField] private Camera mainCamera;

    [SerializeField] private float moveSpeed;
    [SerializeField] private InputActionReference inputAction;

    private Vector2 movementDirection;

    private void Awake()
    {
        playerCharacterRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Move()
    {
        movementDirection = inputAction.action.ReadValue<Vector2>();
        //playerCharacterRigidbody.MovePosition(transform.position + setLocation.position * Time.deltaTime * playerMoveSpeed);
        playerCharacterRigidbody.linearVelocity = new Vector2(movementDirection.x * moveSpeed, movementDirection.y * moveSpeed);

    }

    private void Update()
    {
        if (PlayerRPGUIControls.randomEnemyEncounter)
        {
            playerCharacterRigidbody.linearVelocity = new Vector2(0, 0);
        }
        else
        {
            Move();

        }

    }
}


/*
 
   if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(mainCamera.ScreenToWorldPoint(Input.mousePosition));
            setLocation.position = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }

 
 */