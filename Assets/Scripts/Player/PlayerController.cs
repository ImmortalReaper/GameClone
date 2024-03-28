using BayatGames.SaveGameFree;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDataPersistence
{
    [Header("Player Data")]
    [SerializeField] CharacterController characterController;
    [SerializeField] float gravityMultiplier = 0.3f; 
    [SerializeField] float playerSpeed = 3f;
    [SerializeField] string groundTag = "Ground";

    [Header("Joystick Data")]
    [SerializeField] FloatingJoystick joystick;

    [Header("Animator Settings")]
    [SerializeField] Animator animator;
    [SerializeField] string animatorSpeedParameterName;
    [SerializeField] string animatorMoveParameterName;
    [SerializeField] float animationDampTime = 0.2f;

    [Header("Data Persistence")]
    [SerializeField] string positionIndetifier = "playerPosition";
    [SerializeField] string rotationIndetifier = "playerRotation";

    void Awake() => Load();

    void OnApplicationPause(bool pause) => Save();

    void OnEnable() => joystick.OnBehaviorChange += SetIsMoving;

    void OnDestroy() => joystick.OnBehaviorChange -= SetIsMoving;

    void SetIsMoving(bool obj) => animator.SetBool(animatorMoveParameterName, obj);

    void FixedUpdate()
    {
        PlayerMovement();
        animator.SetFloat(animatorSpeedParameterName, joystick.Direction.magnitude, animationDampTime, Time.deltaTime);
    }

    void PlayerMovement()
    {
        Vector3 direction = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
        Vector3 move = direction * playerSpeed * Time.fixedDeltaTime;

        if (!characterController.isGrounded) { move += Physics.gravity * Time.deltaTime * gravityMultiplier; }

        Vector3 nextPosition = transform.position + move + Vector3.up * characterController.height;
        RaycastHit[] hits = Physics.RaycastAll(nextPosition, Vector3.down, 50f);

        bool willLandOnGround = false;
        foreach (RaycastHit hit in hits)
        {
            Debug.DrawRay(nextPosition, Vector3.down * hit.distance, Color.yellow);
            if (hit.collider.CompareTag(groundTag))
            {
                willLandOnGround = true;
                break;
            }
        }

        if (willLandOnGround) { characterController.Move(move); }

        if (direction != Vector3.zero) { transform.rotation = Quaternion.LookRotation(direction); }
    }

    public void Save()
    {
        SaveGame.Save(positionIndetifier, transform.position);
        SaveGame.Save(rotationIndetifier, transform.rotation);
    }

    public void Load()
    {
        characterController.enabled = false;
        if (SaveGame.Exists(positionIndetifier)) { transform.position = SaveGame.Load(positionIndetifier, transform.position); }
        if (SaveGame.Exists(rotationIndetifier)) { transform.rotation = SaveGame.Load(rotationIndetifier, transform.rotation); }
        characterController.enabled = true;
    }
}
