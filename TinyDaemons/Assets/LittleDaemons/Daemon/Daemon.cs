using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[DisallowMultipleComponent]
public class Daemon : MonoBehaviour
{

    private static int _daemonCounter = 0;

    #region Internals, Set up in unity
    [SerializeField]
    internal SelectionCircle selectionCircle;
    [SerializeField]
    internal DaemonBody body;
    #endregion

    #region Stats
    public string Name;
    public int Health = 60;
    public int TotalHealth = 60;
    public float WalkSpeed = 1;
    public float RotateSpeed = 15;
    #endregion

    #region Internal State
    private DaemonGame game;

    internal Collider lastBumped;
    internal Vector3 lastBumpedImpulse;

    internal bool isMelee; // is delivering a melee attack

    #endregion

    #region Publicly Visible State;
    public DaemonType daemonType;
    public List<DaemonType> enemyTypes;

    public Daemon enemy;
    #endregion

    // TODO EnemyTypes

    // TODO AllyTypes

    #region Actions
    [FormerlySerializedAs("OnIdle2")]
    public DaemonActionList OnIdle;

    public DaemonActionList OnHuntingEnemy;

    public DaemonActionList OnAttack;

    [FormerlySerializedAs("OnCollision2")]
    public DaemonActionList OnCollision;

    public DaemonActionList OnHurt;

    public DaemonActionList OnDie;

    // Property to fetch all actions in one swoop
    public DaemonActionList[] AllActions {
        get
        {
            return new DaemonActionList[]{
                OnIdle,
                OnHuntingEnemy,
                OnAttack,
                OnCollision,
                OnHurt,
                OnDie
            };
        }
    }
    #endregion

    public enum DaemonState
    {
        Idle,
        // Patrolling, no need. Idle covers a patrol
        Hunting,
        Attacking,
        HandleCollision,
        Melee,
        Hurt,

        // DEADBEEF
        Dead
    }

    public enum DaemonType
    {
        None,
        Doomed,
        Imp,
        Demon,
        Zombie,

        // Robo Boxers
        Red,
        Blue
    }

    public enum InterruptState
    {
        // Interrupts
        EnemySpotted, // Or enemy target?
        CollidedWithWall,
        CollidedWithDaemon,
        CollidedWithEnemy,
        CollidedWithHurt
    }

    private HashSet<InterruptState> unhandledInterrupts = new HashSet<InterruptState>();

    public DaemonState activeState;

    public Coroutine activeStateMachine;

    // Start is called before the first frame update
    void Start()
    {
        // Hokey hack to give them full HP
        TotalHealth = Mathf.Max(Health, TotalHealth);
        Health = TotalHealth;

        Name = $"{daemonType} #{++_daemonCounter:00}";

        game = DaemonGame.GetInstance();

        if (enemyTypes == null || enemyTypes.Count == 0)
        {
            AutopopulateEnemyTypes();
        }

        KickTheStateMachine();
    }

    private void AutopopulateEnemyTypes()
    {
        enemyTypes = new List<DaemonType>();

        // Just slap it in there
        if (daemonType == DaemonType.None)
        {
            // nothing
        }
        else if (daemonType == DaemonType.Doomed)
        {
            enemyTypes = System.Enum.GetValues(typeof(DaemonType))
                .Cast<DaemonType>()
                .Where(dType => dType != DaemonType.Doomed && dType != DaemonType.None)
                .ToList();
        }
        else
        {
            enemyTypes.Add(DaemonType.Doomed);
        }
    }

    public void Hurt(int hitpoints)
    {
        Health -= hitpoints;
        Interrupt(InterruptState.CollidedWithHurt);
    }

    private void KickTheStateMachine()
    {
        // Do clear-ups of machines
        foreach (var act in AllActions)
        {
            act.NotPerforming();
        }

        StopTheStateMachine();

        activeStateMachine = StartCoroutine(HackyStateMachine());
    }

    private void StopTheStateMachine()
    {
        if (activeStateMachine != null)
        {
            StopCoroutine(activeStateMachine);
            ResetDaemonBody();
            activeStateMachine = null;
        }
    }

    private void ResetDaemonBody()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        body.animator.StopPlayback();
    }

    IEnumerator HackyStateMachine()
    {
        while (true)
        {
            // May as well reset while we're here
            ResetDaemonBody();

            yield return new WaitForEndOfFrame();

            // Definitely dead
            if (Health <= 0)
            {
                yield return OnDie.DoListOfActions(this);
                activeState = DaemonState.Dead;
                game.DeselectSelf(this);
                continue;
            }

            CheckUnhandledInterrupts();

            // TODO 'Hurt' parts

            if (DaemonState.HandleCollision == activeState)
            {
                yield return OnCollision.DoListOfActions(this);
                if (DaemonState.HandleCollision == activeState)
                {
                    activeState = DaemonState.Idle;
                }
            }
            else if (DaemonState.Attacking == activeState)
            {
                // TODO - Check range for melee or not ? Nah...
                yield return OnAttack.DoListOfActions(this);
                activeState = enemy == null ? DaemonState.Idle : DaemonState.Hunting;
            }
            else if (DaemonState.Hunting == activeState)
            {
                if (!enemy || enemy.Health <= 0)
                {
                    enemy = null;
                    activeState = DaemonState.Idle;
                    continue;
                }
                yield return OnHuntingEnemy.DoListOfActions(this);
            }
            else if (DaemonState.Idle == activeState && enemy && enemy.Health > 0)
            {
                activeState = DaemonState.Hunting;
                continue;
            }
            else if (DaemonState.Idle == activeState)
            {
                yield return OnIdle.DoListOfActions(this);
                // Will loop forever, waiting for interrupts
            }
            else
            {
                Debug.LogWarning($"Tried handling a situation, but failed state={activeState}. Switching to Idle");
                activeState = DaemonState.Idle;
            }
        }

        // TODO Explode into bits.
    }

    public void Interrupt(InterruptState interruptState)
    {
        bool IsValidInterrupt = false;
        DaemonState newDaemonState = DaemonState.Idle;
        switch (interruptState)
        {
            case InterruptState.CollidedWithWall:
                IsValidInterrupt = !isMelee; //(activeState == DaemonState.Hunting);
                newDaemonState = DaemonState.HandleCollision;
                break;
            case InterruptState.CollidedWithDaemon:
                IsValidInterrupt = true; //(activeState == DaemonState.Hunting);
                newDaemonState = DaemonState.HandleCollision;
                break;
            case InterruptState.CollidedWithHurt:
                IsValidInterrupt = true;
                newDaemonState = Health > 0 ? DaemonState.Hurt : DaemonState.Dead;
                break;
        }

        if (IsValidInterrupt)
        {
            unhandledInterrupts.Remove(interruptState);
            activeState = newDaemonState;
            KickTheStateMachine();
        }
        else
        {
            unhandledInterrupts.Add(interruptState);
        }
    }

    private void CheckUnhandledInterrupts()
    {
        if (unhandledInterrupts.Count > 0)
        {
            // Force another interrupt to see if things change
            InterruptState unhandled = unhandledInterrupts.First();
            Interrupt(unhandled);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Wall" || collision.collider.tag == "Daemon")
        {
            lastBumped = collision.collider;
            lastBumpedImpulse = collision.impulse;

            Debug.Log($"Bumped into {collision.collider.tag}.");

            switch (collision.collider.tag)
            {
                case "Wall":
                    Interrupt(InterruptState.CollidedWithWall);
                    break;
                case "Daemon":
                    Daemon d = collision.collider.GetComponent<Daemon>();
                    if( enemyTypes.Contains(d.daemonType) )
                    {
                        Interrupt(InterruptState.CollidedWithEnemy);
                    }
                    else
                    {
                        Interrupt(InterruptState.CollidedWithDaemon);
                    }    
                    break;

                // TODO Enemy is different?
            }
        }

    }

    private void OnMouseUpAsButton()
    {
        game.SelectDaemon(this);
        game.myLearning.HasClickedDaemon = true;
    }

    private void OnMouseEnter()
    {
        selectionCircle.Hover();
    }

    private void OnMouseExit()
    {
        selectionCircle.Unhover();
    }

}
