using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerController : MonoBehaviour {
    public enum MovementType {
        Strafe, Rotational
    }
    
    #region Fields
    #region Serialized
    [Header("Configuration")]
    [SerializeField]
    private GameObject settings;
    [Header("Controls")]
    [SerializeField]
    private string horizontalBinding;
    [SerializeField]
    private string verticalBinding;
    [SerializeField]
    private string hookBinding;
    [SerializeField]
    private string dashBinding;
    
    [Header("Other")]
    [SerializeField]
    private bool isBlue;
    [SerializeField, Tooltip("Units per second")]
    private float maxSpeed;
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private MovementType movementType = MovementType.Strafe;
    [SerializeField, Tooltip("Respawn duration in seconds")]
    private float respawnDuration = 1;
    [SerializeField, Tooltip("World prefab storage")]
    private GameObject prefabs;
    [SerializeField]
    private GameObject ImpactParticle;
    [SerializeField]
    private Material dissolveMaterial;

    [Header("SFX")]
    [SerializeField]
    private AudioClip ImpactClip;
    [SerializeField]
    private AudioClip DashClip;
    [SerializeField] 
    private AudioClip HookClip;  

    #endregion

    private AudioSource sfx;
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;
    private Material[] initialMaterials;
    private Material[] respawnMaterials;

    #region Movement
    private StrafeMovement strafeScheme;
    private RotationalMovement rotateScheme;
    private MovementScheme MovementScheme {
        get {
            switch (movementType) {
                case MovementType.Strafe:
                    return strafeScheme;
                case MovementType.Rotational:
                    return rotateScheme;
                default:
                    return null;
            }
        }
    }
    #endregion

    #region Settings
    private InputBindings bindings;
    private MovementSettings movementSettings;
    private GameplaySettings gameplaySettings;
    #endregion

    public Player Player { get; } = new Player();

    private bool isOnFloor;
    #endregion

    private void Awake() {
        Assert.IsNotNull(prefabs, "No prefab game object given!");
        movementSettings = new MovementSettings()
                        .SetMaxSpeed(maxSpeed)
                        .SetAcceleration(acceleration);
        bindings = new InputBindings()
                        .SetHorizontal(horizontalBinding)
                        .SetVertical(verticalBinding)
                        .SetHook(hookBinding)
                        .SetDash(dashBinding);
        strafeScheme = new StrafeMovement(Player, movementSettings, bindings);
        rotateScheme = new RotationalMovement(movementSettings, bindings);

        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
        initialMaterials = transform.Find("NeoPlayer").GetComponent<MeshRenderer>().materials;
        respawnMaterials = new Material[initialMaterials.Length];
    }

    private void Start() {
        sfx = GetComponent<AudioSource>();

        GameObject hookPrefab = prefabs.transform.Find("Hook")?.gameObject;
        Assert.IsNotNull(hookPrefab, "No Hook prefab exists!");

        gameplaySettings = settings.GetComponent<Settings>()?.Gameplay ?? new GameplaySettings();
        Player.Initialize(gameObject, hookPrefab, bindings, movementSettings, gameplaySettings);
        ImpactParticle.GetComponent<Renderer>().enabled = false;

        Material respawnMaterial = new Material(dissolveMaterial);
        respawnMaterial.SetColor("_emission_color", Player.IsBlue() ? Colors.BLUE_COLOR : Colors.RED_COLOR);
        for (int i = 0; i < respawnMaterials.Length; i++) {
            respawnMaterials[i] = respawnMaterial;
        }
    }

    private void Update() {
        CheckFalling();
        Player.Update(Time.deltaTime);
    }

    private void FixedUpdate() {
        if (Player.State.IsOff(Player.States.Falling)) {
            MovementScheme?.Update(transform);
        }

        float friction = 0.9f;
        Vector3 vel = GetComponent<Rigidbody>().velocity;
        vel.Set(vel.x * friction, vel.y, vel.z * friction);
        GetComponent<Rigidbody>().velocity = vel;
    }

    private void OnCollisionEnter(Collision col) {
        bool isPlayer = col.gameObject.GetComponent<PlayerController>() != null;
        if (Player.State.IsOn(Player.States.Dashing) && isPlayer) {
            Push(col.gameObject);
            ImpactParticle.GetComponent<Renderer>().enabled = true;
            ImpactParticle.GetComponent<ParticleSystem>().Play();
            sfx.clip = ImpactClip;
            sfx.Play();
            
        }

        if (Player.State.IsOn(Player.States.Pushed)) {
            GetStunned();
        }

        if (col.gameObject.tag =="DeathFloor") {
            StartRestart();
        }
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Floor") {
            isOnFloor = true;
        }
    }

    private void OnTriggerExit(Collider collider) {
        if (collider.gameObject.tag == "Floor") {
            isOnFloor = false;
        }
    }

    private void CheckFalling() {
        bool isDashing = Player.State.IsOn(Player.States.Dashing);
        if (isOnFloor || isDashing)  { Player.State.Off(Player.States.Falling); }
        else                         { Player.State.On(Player.States.Falling); }

        if (Player.HasOrb && Player.State == Player.States.Falling) {
            Player.DropOrb(null);
        }
    }

    private void StartRestart() {
        Rigidbody body = GetComponent<Rigidbody>();
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        MeshRenderer mesh = transform.Find("NeoPlayer").GetComponent<MeshRenderer>();
        mesh.materials = respawnMaterials;
        StartCoroutine("UpdateRespawn", mesh);
    }

    private IEnumerator UpdateRespawn(MeshRenderer mesh) {
        // Note: Don't use 0, it results in weird glow of the gun material
        float value = 0.0001f;
        while (value < 1) {
            foreach (Material mat in mesh.materials) {
                mat.SetFloat("_progress", value);
            }
            value += Time.deltaTime / respawnDuration;
            yield return null;
        }
        Respawn();
        StopAllCoroutines();
    }

    private void Respawn() {
        transform.rotation = spawnRotation;
        transform.position = spawnPosition;
        MeshRenderer mesh = transform.Find("NeoPlayer").GetComponent<MeshRenderer>();
        mesh.materials = initialMaterials;
    }

    public void OnOrbPickup() {
        StartCoroutine("AddScore");
    }

    public void OnOrbDrop() {
        StopCoroutine("AddScore");
    }

    private IEnumerator AddScore() {
        while (true) {
            G.Instance.Round.AddPoint(isBlue);
            yield return new WaitForSeconds(0.25f);
        }
    }

    private void Push(GameObject target) {
        Rigidbody body = GetComponent<Rigidbody>();
        DashAbility dash = Player.GetAbility<DashAbility>();
        Rigidbody otherBody = target.GetComponent<Rigidbody>();
        Player otherPlayer = target.GetComponent<PlayerController>().Player;

        otherPlayer.State.On(Player.States.Pushed);
        G.Instance.Pipeline.New().
            Func(() => {
                Vector3 force = otherBody.velocity.normalized * 0.5f * dash.Force;
                otherBody.AddForce(force, ForceMode.Impulse);
            })
            .Delay(0.8f)
            .Func(() => {
                otherPlayer.State.Off(Player.States.Pushed);
            });

        body.velocity = Vector3.zero;
        Player.State.Off(Player.States.Dashing);

        if (otherPlayer.HasOrb) {
            otherPlayer.DropOrb(otherBody.transform.position);
        }
    }

    private void GetStunned() {
        Player.State.Off(Player.States.Pushed);
        Player.State.On(Player.States.Stunned);
        G.Instance.Pipeline.New().Delay(gameplaySettings.StunDuration).Func(() => Player.State.Off(Player.States.Stunned));
    }

    public bool IsBlue() {
        return isBlue;
    }
}
