using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Properties")]
    [SerializeField] private float movementSpeed = 10f;

    [Header("Mouse Look")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float rotationLerpSpeed = 15f;

    [Header("Shoot Properties")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;

    private CharacterController cc;
    private PlayerStates playerStates;
    private LevelManager levelManager;
    private float nextFireTime;
    private float fixedY;

    void Awake()
    {
        playerStates = GetComponent<PlayerStates>();
        cc = GetComponent<CharacterController>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        fixedY = transform.position.y;

        if (playerCamera == null)
            Debug.LogError("PlayerController: please assign the playerCamera field.");
    }

    void OnEnable() => playerStates.Shoot += ShootBullet;
    void OnDisable() => playerStates.Shoot -= ShootBullet;

    void Update()
    {
        if (!playerStates.IsAlive) return;

        HandleMovement();
        RotateTowardsMouse();
        HandleShooting();
    }

    private void HandleMovement()
    {
        // Read input axes
        float v = Input.GetAxis("Vertical Movement");
        float h = Input.GetAxis("Horizontal Movement");

        // Build a movement vector in local space
        Vector3 move = (transform.forward * v + transform.right * h);
        if (move.sqrMagnitude > 1f) move.Normalize();

        // Move, preserving the controller's built-in collision
        cc.Move(move * movementSpeed * Time.deltaTime);
    }

    private void RotateTowardsMouse()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        if (ground.Raycast(ray, out float enter))
        {
            Vector3 hit = ray.GetPoint(enter);
            Vector3 dir = hit - transform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.001f)
            {
                Quaternion target = Quaternion.LookRotation(dir, Vector3.up);
                transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    target,
                    Time.deltaTime * rotationLerpSpeed
                );
            }
        }
    }

    private void HandleShooting()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFireTime)
        {
            playerStates.Shoot.Invoke();
            nextFireTime = Time.time + playerStates.FireRate;
        }
    }

    private void ShootBullet()
    {
        Instantiate(
            bulletPrefab,
            bulletSpawnPoint.position,
            bulletSpawnPoint.rotation
        );
    }

    void LateUpdate()
    {
        // after all movement, force Y back to fixedY
        Vector3 pos = transform.position;
        pos.y = fixedY;
        transform.position = pos;
    }
}
