using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[DisallowMultipleComponent]
public class Daemon : MonoBehaviour
{

    #region Internals, Set up in unity
    [SerializeField]
    internal SelectionCircle selectionCircle;
    [SerializeField]
    internal DaemonBody body;
    #endregion

    #region Stats
    public int Health = 60;
    public float WalkSpeed = 1;
    public float RotateSpeed = 15;
    #endregion

    #region Internal State
    private DaemonGame game;
    private Daemon enemy;

    internal Collider lastBumped;
    internal Vector3 lastBumpedImpulse;

    #endregion

    #region Publicly Visible State;
    public DaemonType daemonType;
    #endregion

    // TODO EnemyTypes

    // TODO AllyTypes

    #region Actions
    [FormerlySerializedAs("OnIdle2")]
    public DaemonActionList OnIdle;

    public DaemonActionList OnFoundEnemy;

    public DaemonActionList OnHuntingEnemy;

    public DaemonActionList OnCloseToEnemy;

    public DaemonActionList OnAttack;

    public DaemonActionList OnAttackMelee;

    [FormerlySerializedAs("OnCollision2")]
    public DaemonActionList OnCollision;

    public DaemonActionList OnDie;
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
        Player,
        Imp,
        Demon,
        Zombie
    }

    public enum InterruptState
    {
        // Interrupts
        EnemySpotted, // Or enemy target?
        CollidedWithWall,
        CollidedWithDaemon,
        CollidedWithEnemy,
        CollidedWithHurt,
    }

    private HashSet<InterruptState> unhandledInterrupts = new HashSet<InterruptState>();

    public DaemonState activeState;

    public Coroutine activeStateMachine;

    // Start is called before the first frame update
    void Start()
    {
        game = FindObjectOfType<DaemonGame>();

        KickTheStateMachine();
    }

    public void Hurt(int hitpoints)
    {
        Health -= hitpoints;
        Interrupt(InterruptState.CollidedWithHurt);
    }

    private void KickTheStateMachine()
    {
        if (activeStateMachine != null)
        {
            StopCoroutine(activeStateMachine);
        }
        body.animator.StopPlayback();
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        activeStateMachine = StartCoroutine(HackyStateMachine());
    }

    IEnumerator HackyStateMachine()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (activeState == DaemonState.Dead)
            {
                break;
            }

            if (Health <= 0)
            {
                yield return OnDie.DoListOfActions(this);
                activeState = DaemonState.Dead;
                continue;
            }

            CheckUnhandledInterrupts();

            // TODO Enemy detection bits

            // TODO Collision Handling bits
            if (DaemonState.HandleCollision == activeState)
            {
                yield return OnCollision.DoListOfActions(this);
                if (DaemonState.HandleCollision == activeState)
                {
                    activeState = DaemonState.Idle;
                }
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

    private void Interrupt(InterruptState interruptState)
    {
        bool IsValidInterrupt = false;
        DaemonState newDaemonState = DaemonState.Idle;
        switch (interruptState)
        {
            case InterruptState.CollidedWithWall:
                IsValidInterrupt = true; //(activeState == DaemonState.Hunting);
                newDaemonState = DaemonState.HandleCollision;
                break;
            case InterruptState.CollidedWithDaemon:
                IsValidInterrupt = true; //(activeState == DaemonState.Hunting);
                newDaemonState = DaemonState.HandleCollision;
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
                    Interrupt(InterruptState.CollidedWithDaemon);
                    break;

                // TODO Enemy is different?
            }
        }

    }

    private void OnMouseUpAsButton()
    {
        game.SelectDaemon(this);
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
