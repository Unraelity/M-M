using System.Collections;
using UnityEngine;

public class PlayerController : BaseController, IDamageable 
{
    [Header("Bindings")]
    [SerializeField] private OrientationController orientationController;
    [SerializeField] private MovementController movementController;
    [SerializeField] private AnimationController animController;
    [SerializeField] private GameObject hitBox;
    [SerializeField] private InteractionZone interactionZone;
    [Header("Damage Settings")]
    [SerializeField] private float maxHealth = 3f;
    [SerializeField] private float invinsibilityTime = 1f;
    [SerializeField] private string hitAnimParam = "Hit";
    [Header("Roll Settings")]
    [SerializeField] private float rollTime = 0.5f;
    [SerializeField] private float rollTiles = 1.525f;
    private float health;
    // graph variables
    private CommandGraph commandGraph;
    private Ability currAbility;
    private Ability nextAbility;
    // ability declarations
    private IdleAbility idleAbility;
    private MoveUpAbility moveUpAbility;
    private MoveDownAbility moveDownAbility;
    private MoveRightAbility moveRightAbility;
    private MoveLeftAbility moveLeftAbility;
    private MoveRightUpAbility moveRightUpAbility;
    private MoveRightDownAbility moveRightDownAbility;
    private MoveLeftUpAbility moveLeftUpAbility;
    private MoveLeftDownAbility moveLeftDownAbility;
    private RollAbility rollAbility;
    private RollUpAbility rollUpAbility;
    private RollDownAbility rollDownAbility;
    private RollRightAbility rollRightAbility;
    private RollLeftAbility rollLeftAbility;
    private RollRightUpAbility rollRightUpAbility;
    private RollRightDownAbility rollRightDownAbility;
    private RollLeftUpAbility rollLeftUpAbility;
    private RollLeftDownAbility rollLeftDownAbility;
    private WeaponAbility weaponAbility;
    private InteractAbility interactAbility;
    private PauseAbility pauseAbility;

    public GameObject HitBox => hitBox;
    public Weapon EquippedWeapon;

    private void Awake()
    {
        if (orientationController == null)
        {

            orientationController = GetComponent<OrientationController>();

            if (orientationController == null)
            {
                Debug.LogError("No Orientation Controller atached to Player");
            }
        }

        if (movementController == null)
        {

            movementController = GetComponent<MovementController>();

            if (movementController == null)
            {
                Debug.LogError("No Movement Controller atached to Player");
            }
        }

        if (animController == null)
        {

            animController = GetComponent<AnimationController>();

            if (animController == null)
            {
                Debug.LogError("No Animation Controller atached to Player");
            }
        }
        
        if (interactionZone == null) {

            interactionZone = GetComponentInChildren<InteractionZone>();

            if (interactionZone == null) {
                Debug.LogError("No Interaction Zone atached to Player");
            }
        }
    }

    private void Start()
    {
        health = maxHealth;

        CreateAbilities();
        InitializeGraph();

        currAbility = idleAbility;
        nextAbility = currAbility;
    }

    private void Update()
    {
        nextAbility = commandGraph.GetNextAbility(currAbility);

        // exit current and enter next ability if it is different than current
        if (nextAbility != null)
        {
            if (nextAbility is IBidirectionalAbility bidirectional)
            {
                bidirectional.SetReturnAbility(currAbility);
            }
            else
            {
                currAbility.ExitAbility();
            }
            nextAbility.EnterAbility();
            currAbility = nextAbility;
        }
    }

    private void FixedUpdate()
    {
        if (currAbility != null)
        {
            currAbility.ExecuteAbility();
        }
    }

    public void TakeDamage(float damage)
    {
        currAbility.ExitAbility();
        StartCoroutine(Invinsibility());
        currAbility = idleAbility;
        currAbility.EnterAbility();

        // make coroutine for invinsibility frame
        animController.SetAnimatorTrigger(hitAnimParam);
        health -= damage;

        if (health <= 0) {
            Die();
        }

    }

    private void CreateAbilities()
    {

        idleAbility = new IdleAbility(movementController);
        // moving
        moveUpAbility = new MoveUpAbility(movementController, animController);
        moveDownAbility = new MoveDownAbility(movementController, animController);
        moveRightAbility = new MoveRightAbility(movementController, animController);
        moveLeftAbility = new MoveLeftAbility(movementController, animController);
        moveRightUpAbility = new MoveRightUpAbility(movementController, animController);
        moveRightDownAbility = new MoveRightDownAbility(movementController, animController);
        moveLeftUpAbility = new MoveLeftUpAbility(movementController, animController);
        moveLeftDownAbility = new MoveLeftDownAbility(movementController, animController);
        // rolling
        rollAbility = new RollAbility(this, orientationController, movementController, animController, rollTime, rollTiles);
        rollUpAbility = new RollUpAbility(this, orientationController, movementController, animController, rollTime, rollTiles);
        rollDownAbility = new RollDownAbility(this, orientationController, movementController, animController, rollTime, rollTiles);
        rollRightAbility = new RollRightAbility(this, orientationController, movementController, animController, rollTime, rollTiles);
        rollLeftAbility = new RollLeftAbility(this, orientationController, movementController, animController, rollTime, rollTiles);
        rollRightUpAbility = new RollRightUpAbility(this, orientationController, movementController, animController, rollTime, rollTiles);
        rollRightDownAbility = new RollRightDownAbility(this, orientationController, movementController, animController, rollTime, rollTiles);
        rollLeftUpAbility = new RollLeftUpAbility(this, orientationController, movementController, animController, rollTime, rollTiles);
        rollLeftDownAbility = new RollLeftDownAbility(this, orientationController, movementController, animController, rollTime, rollTiles);
        // attacking
        weaponAbility = new WeaponAbility(this, animController);
        // interacting
        interactAbility = new InteractAbility(interactionZone);
        // pausing
        pauseAbility = new PauseAbility();
    }

    private void InitializeGraph()
    {

        commandGraph = new CommandGraph();

        bool isPressedCondition = true;

        // 4-directional movement
        commandGraph.AddAbilityTransition(idleAbility, Commands.MoveUp, moveUpAbility, isPressedCondition);                  // idle -> move up
        commandGraph.AddAbilityTransition(idleAbility, Commands.MoveDown, moveDownAbility, isPressedCondition);              // idle -> move down
        commandGraph.AddAbilityTransition(idleAbility, Commands.MoveRight, moveRightAbility, isPressedCondition);            // idle -> move right
        commandGraph.AddAbilityTransition(idleAbility, Commands.MoveLeft, moveLeftAbility, isPressedCondition);              // idle -> move left
        commandGraph.AddAbilityTransition(moveUpAbility, Commands.MoveUp, idleAbility, !isPressedCondition);                 // move up -> idle
        commandGraph.AddAbilityTransition(moveDownAbility, Commands.MoveDown, idleAbility, !isPressedCondition);             // move down -> idle
        commandGraph.AddAbilityTransition(moveRightAbility, Commands.MoveRight, idleAbility, !isPressedCondition);           // move right -> idle
        commandGraph.AddAbilityTransition(moveLeftAbility, Commands.MoveLeft, idleAbility, !isPressedCondition);             // move left -> idle 

        // 8-directional movement
        commandGraph.AddAbilityTransition(moveUpAbility, Commands.MoveRight, moveRightUpAbility, isPressedCondition);        // move up -> move right up
        commandGraph.AddAbilityTransition(moveRightAbility, Commands.MoveUp, moveRightUpAbility, isPressedCondition);        // move right -> move right up
        commandGraph.AddAbilityTransition(moveUpAbility, Commands.MoveLeft, moveLeftUpAbility, isPressedCondition);          // move up -> move left up
        commandGraph.AddAbilityTransition(moveLeftAbility, Commands.MoveUp, moveLeftUpAbility, isPressedCondition);          // move left -> move left up
        commandGraph.AddAbilityTransition(moveDownAbility, Commands.MoveRight, moveRightDownAbility, isPressedCondition);    // move down -> move right down
        commandGraph.AddAbilityTransition(moveRightAbility, Commands.MoveDown, moveRightDownAbility, isPressedCondition);    // move right -> move right down
        commandGraph.AddAbilityTransition(moveDownAbility, Commands.MoveLeft, moveLeftDownAbility, isPressedCondition);      // move down -> move left down
        commandGraph.AddAbilityTransition(moveLeftAbility, Commands.MoveDown, moveLeftDownAbility, isPressedCondition);      // move left -> move left down
        commandGraph.AddAbilityTransition(moveRightUpAbility, Commands.MoveRight, moveUpAbility, !isPressedCondition);       // move right up -> move up
        commandGraph.AddAbilityTransition(moveRightUpAbility, Commands.MoveUp, moveRightAbility, !isPressedCondition);       // move right up ->  move right
        commandGraph.AddAbilityTransition(moveLeftUpAbility, Commands.MoveLeft, moveUpAbility, !isPressedCondition);         // move left up -> move up
        commandGraph.AddAbilityTransition(moveLeftUpAbility, Commands.MoveUp, moveLeftAbility, !isPressedCondition);         // move left up -> move left
        commandGraph.AddAbilityTransition(moveRightDownAbility, Commands.MoveRight, moveDownAbility, !isPressedCondition);   // move right down -> move down
        commandGraph.AddAbilityTransition(moveRightDownAbility, Commands.MoveDown, moveRightAbility, !isPressedCondition);   // move right down -> move right
        commandGraph.AddAbilityTransition(moveLeftDownAbility, Commands.MoveLeft, moveDownAbility, !isPressedCondition);     // move left down -> move down
        commandGraph.AddAbilityTransition(moveLeftDownAbility, Commands.MoveDown, moveLeftAbility, !isPressedCondition);     // move left down -> move left

        // 2-directional rolling
        commandGraph.AddAbilityTransition(idleAbility, Commands.Roll, rollAbility, isPressedCondition);                      // idle -> roll
        commandGraph.AddAbilityTransition(rollAbility, rollAbility, idleAbility);                                            // roll -> idle

        // 4-directional rolling
        commandGraph.AddAbilityTransition(moveUpAbility, Commands.Roll, rollUpAbility);                                      // move up -> roll up
        commandGraph.AddAbilityTransition(moveDownAbility, Commands.Roll, rollDownAbility);                                  // move down -> roll down
        commandGraph.AddAbilityTransition(moveRightAbility, Commands.Roll, rollRightAbility);                                // move right -> roll right
        commandGraph.AddAbilityTransition(moveLeftAbility, Commands.Roll, rollLeftAbility);                                  // move left -> roll left
        commandGraph.AddAbilityTransition(rollUpAbility, rollUpAbility, idleAbility);                                        // roll up -> idle
        commandGraph.AddAbilityTransition(rollDownAbility, rollDownAbility, idleAbility);                                    // roll down -> idle
        commandGraph.AddAbilityTransition(rollRightAbility, rollRightAbility, idleAbility);                                  // roll right -> idle
        commandGraph.AddAbilityTransition(rollLeftAbility, rollLeftAbility, idleAbility);                                    // roll left -> idle

        // 8-directional rolling
        commandGraph.AddAbilityTransition(moveRightUpAbility, Commands.Roll, rollRightUpAbility);                            // move right up -> roll right up
        commandGraph.AddAbilityTransition(moveRightDownAbility, Commands.Roll, rollRightDownAbility);                        // move right down -> roll right down
        commandGraph.AddAbilityTransition(moveLeftUpAbility, Commands.Roll, rollLeftUpAbility);                              // move left up -> roll left up
        commandGraph.AddAbilityTransition(moveLeftDownAbility, Commands.Roll, rollLeftDownAbility);                          // move left down -> roll left down
        commandGraph.AddAbilityTransition(rollRightUpAbility, rollRightUpAbility, idleAbility);                              // roll right up -> idle
        commandGraph.AddAbilityTransition(rollRightDownAbility, rollRightDownAbility, idleAbility);                          // roll right down -> idle
        commandGraph.AddAbilityTransition(rollLeftUpAbility, rollLeftUpAbility, idleAbility);                                // roll left up -> idle
        commandGraph.AddAbilityTransition(rollLeftDownAbility, rollLeftDownAbility, idleAbility);                            // roll left down -> idle

        // attack
        commandGraph.AddAbilityTransition(idleAbility, Commands.UseWeapon, weaponAbility);                                   // idle -> use weapon
        commandGraph.AddAbilityTransition(moveUpAbility, Commands.UseWeapon, weaponAbility);                                 // move up -> use weapon
        commandGraph.AddAbilityTransition(moveDownAbility, Commands.UseWeapon, weaponAbility);                               // move down -> use weapon
        commandGraph.AddAbilityTransition(moveRightAbility, Commands.UseWeapon, weaponAbility);                              // move right -> use weapon
        commandGraph.AddAbilityTransition(moveDownAbility, Commands.UseWeapon, weaponAbility);                               // move left -> use weapon
        commandGraph.AddAbilityTransition(moveRightUpAbility, Commands.UseWeapon, weaponAbility);                            // move right up -> use weapon
        commandGraph.AddAbilityTransition(moveRightDownAbility, Commands.UseWeapon, weaponAbility);                          // move right down -> use weapon
        commandGraph.AddAbilityTransition(moveLeftUpAbility, Commands.UseWeapon, weaponAbility);                             // move left up -> use weapon
        commandGraph.AddAbilityTransition(moveLeftDownAbility, Commands.UseWeapon, weaponAbility);                           // move left down -> use weapon
        commandGraph.AddAbilityTransition(weaponAbility, Commands.UseWeapon, weaponAbility);                                 // use weapon -> use weapon
        commandGraph.AddAbilityTransition(weaponAbility, weaponAbility, idleAbility);                                        // use weapon -> idle

        // interact
        commandGraph.AddAbilityTransition(idleAbility, Commands.Interact, interactAbility);                                  // idle -> interact
        commandGraph.AddAbilityTransition(moveUpAbility, Commands.Interact, interactAbility);                                // move up -> interact
        commandGraph.AddAbilityTransition(moveDownAbility, Commands.Interact, interactAbility);                              // move down -> interact
        commandGraph.AddAbilityTransition(moveRightAbility, Commands.Interact, interactAbility);                             // move right -> interact
        commandGraph.AddAbilityTransition(moveDownAbility, Commands.Interact, interactAbility);                              // move left -> interact
        commandGraph.AddAbilityTransition(moveRightUpAbility, Commands.Interact, interactAbility);                           // move right up -> interact
        commandGraph.AddAbilityTransition(moveRightDownAbility, Commands.Interact, interactAbility);                         // move right down -> interact
        commandGraph.AddAbilityTransition(moveLeftUpAbility, Commands.Interact, interactAbility);                            // move left up -> interact
        commandGraph.AddAbilityTransition(moveLeftDownAbility, Commands.Interact, interactAbility);                          // move left down -> interact
        commandGraph.AddAbilityTransition(interactAbility, interactAbility, idleAbility);                                    // interact -> idle  

        // pause
        commandGraph.AddAbilityTransition(idleAbility, Commands.Pause, pauseAbility);                                        // idle -> pause
        commandGraph.AddAbilityTransition(moveUpAbility, Commands.Pause, pauseAbility);                                      // move up -> pause
        commandGraph.AddAbilityTransition(moveDownAbility, Commands.Pause, pauseAbility);                                    // move down -> pause
        commandGraph.AddAbilityTransition(moveRightAbility, Commands.Pause, pauseAbility);                                   // move right -> pause
        commandGraph.AddAbilityTransition(moveLeftAbility, Commands.Pause, pauseAbility);                                    // move left -> pause
        commandGraph.AddAbilityTransition(moveRightUpAbility, Commands.Pause, pauseAbility);                                 // move right up -> pause
        commandGraph.AddAbilityTransition(moveRightDownAbility, Commands.Pause, pauseAbility);                               // move right down -> pause
        commandGraph.AddAbilityTransition(moveLeftUpAbility, Commands.Pause, pauseAbility);                                  // move left up -> pause
        commandGraph.AddAbilityTransition(moveLeftDownAbility, Commands.Pause, pauseAbility);                                // move left down -> pause
        commandGraph.AddAbilityTransition(weaponAbility, Commands.Pause, pauseAbility);                                      // use weapon -> pause
        commandGraph.AddAbilityTransition(rollAbility, Commands.Pause, pauseAbility);                                        // roll up -> pause
        commandGraph.AddAbilityTransition(rollUpAbility, Commands.Pause, pauseAbility);                                      // roll up -> pause
        commandGraph.AddAbilityTransition(rollDownAbility, Commands.Pause, pauseAbility);                                    // roll down -> pause
        commandGraph.AddAbilityTransition(rollRightAbility, Commands.Pause, pauseAbility);                                   // roll right -> pause
        commandGraph.AddAbilityTransition(rollLeftAbility, Commands.Pause, pauseAbility);                                    // roll left -> pause
        commandGraph.AddAbilityTransition(rollRightUpAbility, Commands.Pause, pauseAbility);                                 // roll right up -> pause
        commandGraph.AddAbilityTransition(rollRightDownAbility, Commands.Pause, pauseAbility);                               // roll right down -> pause
        commandGraph.AddAbilityTransition(rollLeftUpAbility, Commands.Pause, pauseAbility);                                  // roll left up -> pause
        commandGraph.AddAbilityTransition(rollLeftDownAbility, Commands.Pause, pauseAbility);                                // roll left down -> pause
        commandGraph.AddAbilityTransition(pauseAbility, pauseAbility, Commands.Pause);                                       // pause -> any ability
    }

    public IEnumerator Invinsibility()
    {
        hitBox.SetActive(false);
        yield return new WaitForSeconds(invinsibilityTime);
        hitBox.SetActive(true);
    }

    public void Die() {
        Debug.Log("Player has died");
    }
}
