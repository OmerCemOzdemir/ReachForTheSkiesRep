using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class PlayerRPGController : MonoBehaviour
{
    private Rigidbody2D playerCharacterRigidbody;
    [SerializeField] private Camera mainCamera;

    [SerializeField] private float moveSpeed;
    [SerializeField] private InputActionReference inputAction;
    [SerializeField] private Animator playerAnimator;

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

    /* 
            playerAnimator.SetBool("WalkSide", true);
            playerAnimator.SetBool("WalkFront", false);
            playerAnimator.SetBool("WalkBack", false);
            playerAnimator.SetBool("Idle", false);
     */

    private void HandleAnimation()
    {

        if (movementDirection.x > 0)
        {
            playerAnimator.Play("WalkSide");
            transform.localScale = new Vector3(1, 1, 1);

        }
        else if (movementDirection.x < 0)
        {
            playerAnimator.Play("WalkSide");
            transform.localScale = new Vector3(-1, 1, 1);

        }
        else
        {
            if (movementDirection.y < 0)
            {
                playerAnimator.Play("WalkForward");

            }
            else if (movementDirection.y > 0)
            {
                playerAnimator.Play("WalkBackward");

            }
            else
            {
                playerAnimator.Play("Idle");

            }
        }
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

        HandleAnimation();

    }
}


/*
 
   if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(mainCamera.ScreenToWorldPoint(Input.mousePosition));
            setLocation.position = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }

 
 */