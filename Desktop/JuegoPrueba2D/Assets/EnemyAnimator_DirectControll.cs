using UnityEngine;
using System.Collections;

// Definimos los estados del enemigo.
public enum EnemyState {
    Idle,
    Patrol,
    Chase,
    Attack,
    Evade,
    Skill,
    Dead
}

public class EnemyAI_Final : MonoBehaviour {

    public Transform player;

    public Transform[] patrolPoints;
    private int currentPatrolPointIndex = 0;

    private Animator enemyAnimator;

    public float patrolSpeed = 2.0f;
    public float chaseSpeed = 5.0f;
    public float evadeSpeed = 7.0f;

    public float chaseRange = 10f;
    public float attackRange = 2f;
    public float evadeRange = 3f;

    private EnemyState currentState = EnemyState.Idle;
    private EnemyState lastState = EnemyState.Idle;

    public float skillCooldown = 5f;
    private float lastSkillTime = 0f;

    // Nuevo: Bandera para saber si el enemigo está en una acción de un solo disparo (ataque, habilidad, evasión).
    private bool isPerformingAction = false;
    private int atkTimes = 0;

    void Awake() {
        enemyAnimator = GetComponent<Animator>();
    }

    void Start() {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null) {
            player = playerObject.transform;
        } else {
            Debug.LogWarning("No se encontró un objeto con la etiqueta 'Player'.");
        }
        SetState(EnemyState.Patrol); // Inicia en el estado de patrullaje.
    }

    void Update() {
        if (!player) return;

        UpdateStateAndMove();
    }

    void UpdateStateAndMove() {
        // Si el enemigo está realizando una acción de un solo disparo, no cambia de estado.
        if (isPerformingAction) {
            return;
        }
        
        lastState = currentState;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange) {
            SetState(EnemyState.Attack);
        } else if (distanceToPlayer <= evadeRange && Time.time > lastSkillTime + skillCooldown) {
            SetState(EnemyState.Evade);
        } else if (distanceToPlayer <= chaseRange) {
            SetState(EnemyState.Chase);
        } else if (Time.time > lastSkillTime + skillCooldown) {
            SetState(EnemyState.Skill);
        } else {
            SetState(EnemyState.Idle); // Nuevo estado de inactividad.
        }

        switch (currentState) {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Evade:
                Evade();
                break;
            case EnemyState.Idle:
                // No se mueve, simplemente está inactivo.
                break;
            case EnemyState.Skill:
            case EnemyState.Attack:
            case EnemyState.Dead:
                // El movimiento es manejado por la animación o por una corrutina.
                break;
        }
    }

    void SetState(EnemyState newState) {
        if (currentState == newState) return;
        
        currentState = newState;
        
        // Ejecutamos la acción del nuevo estado.
        ExecuteStateAction();
    }

    void ExecuteStateAction() {
        switch (currentState) {
            case EnemyState.Idle:
                enemyAnimator.CrossFade("idle_1", 0.2f);
                break;
            case EnemyState.Patrol:
                enemyAnimator.CrossFade("walk", 0.2f);
                break;
            case EnemyState.Chase:
                enemyAnimator.CrossFade("run", 0.2f);
                break;
            case EnemyState.Attack:
                isPerformingAction = true;
                // Reproduce el primer golpe
                enemyAnimator.Play("hit_1");
                break;
            case EnemyState.Evade:
                isPerformingAction = true;
                enemyAnimator.Play("evade_1");
                break;
            case EnemyState.Skill:
                isPerformingAction = true;
                if (Random.value > 0.5f) {
                    enemyAnimator.Play("skill_1");
                } else {
                    enemyAnimator.Play("skill_2");
                }
                lastSkillTime = Time.time;
                break;
            case EnemyState.Dead:
                enemyAnimator.Play("death");
                break;
        }
    }

    // Lógica de movimiento para cada estado.
    void Patrol() {
        if (patrolPoints.Length == 0) return;
        Transform targetWaypoint = patrolPoints[currentPatrolPointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, patrolSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.5f) {
            currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;
        }
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        if (direction != Vector3.zero) {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    void Chase() {
        transform.position = Vector3.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
        Vector3 direction = (player.position - transform.position).normalized;
        if (direction != Vector3.zero) {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    void Evade() {
        Vector3 evadeDirection = (transform.position - player.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + evadeDirection * 5f, evadeSpeed * Time.deltaTime);
    }
    
    void Skill() {
        // No hay lógica de movimiento en este método.
    }

    // Métodos de eventos de animación. Se llaman desde el Animator Controller.
    public void OnAnimationEnd() {
        // Este evento se debe llamar al final de las animaciones de un solo disparo (hit_1, evade_1, skill_1, skill_2).
        isPerformingAction = false;
    }

    public void OnHit1End() {
        // Nuevo: Llamado al final de la animación "hit_1".
        // Incrementa el contador de golpes para la siguiente animación.
        atkTimes++;
        if (atkTimes < 2) {
            enemyAnimator.Play("hit_2");
        } else {
            // Reinicia el contador y termina la secuencia de ataque.
            atkTimes = 0;
            isPerformingAction = false;
        }
    }
    
    public void Die() {
        SetState(EnemyState.Dead);
        this.enabled = false;
    }
}