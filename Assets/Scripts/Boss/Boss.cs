using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

    public static Boss Instance { private set; get; }

    public Player player;
    public Animator animator;

    public GameObject robotPrefab;
    public GameObject robotBrincadorPrefab;
    public Transform spawner1;
    public Transform spawner2;
    public Transform spawner3;
    public Transform spawner4;

    public GameObject bossBulletPrefab;
    public Transform bulletSpawner;

    public LayerMask playerMask;
    public Transform attackCenter;
    public float attackRange;

    private AIState farAttack, closeAttack, spawnAttack, noAttack;
    private AISymbol enemyFar, enemyClose, randomSpawn, idle;
    private AIState state;
    private MonoBehaviour currentBehaviour;

    private float distanceFromPlayer;
    public float life;

    void Start() {

        Instance = this;
        player = Player.Instance;
        StartCoroutine(CalculateDistance());

        farAttack = new AIState("FAR", typeof(FarAttack));
        closeAttack = new AIState("CLOSE", typeof(CloseAttack));
        spawnAttack = new AIState("SPAWN", typeof(SpawnAttack));
        noAttack = new AIState("IDLE", typeof(Idle));

        enemyFar = new AISymbol("FAR");
        enemyClose = new AISymbol("CLOSE");
        randomSpawn = new AISymbol("SPAWN");
        idle = new AISymbol("IDLE");

        farAttack.AddTransition(enemyClose, closeAttack);
        farAttack.AddTransition(randomSpawn, spawnAttack);
        farAttack.AddTransition(idle, noAttack);

        closeAttack.AddTransition(enemyFar, farAttack);
        closeAttack.AddTransition(randomSpawn, spawnAttack);
        closeAttack.AddTransition(idle, noAttack);

        spawnAttack.AddTransition(enemyClose, closeAttack);
        spawnAttack.AddTransition(enemyFar, farAttack);
        spawnAttack.AddTransition(idle, noAttack);

        noAttack.AddTransition(enemyClose, closeAttack);
        noAttack.AddTransition(enemyFar, farAttack);
        noAttack.AddTransition(randomSpawn, spawnAttack);

        state = noAttack;

        currentBehaviour = gameObject.AddComponent(state.Behaviour) as MonoBehaviour;

        SoundManagerScript.PlaySound("boss");
    }

    private void ApplyTransition(AISymbol symbol) {
        AIState temp = state.ApplySymbol(symbol);
        if (temp != state) {
            state = temp;
        }
    }

    private IEnumerator CalculateDistance() {
        while (true) {
            distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
            yield return new WaitForSeconds(1f);
        }
    }

    public void StateDone(bool death = false) {
        //Destroy current state
        if (state.Name.Equals("IDLE")) {
            Destroy(GetComponent<Idle>());
        } else if (state.Name.Equals("CLOSE")) {
            Destroy(GetComponent<CloseAttack>());
        } else if (state.Name.Equals("FAR")) {
            Destroy(GetComponent<FarAttack>());
        } else if (state.Name.Equals("SPAWN")) {
            Destroy(GetComponent<SpawnAttack>());
        }

        if (!death) {
            //Select other state
            if (Random.Range(0, 1f) < 0.85) {
                if (distanceFromPlayer <= 7) {
                    ApplyTransition(enemyClose);
                } else {
                    if (distanceFromPlayer <= 25 && distanceFromPlayer > 15) {
                        ApplyTransition(enemyFar);
                    } else {
                        ApplyTransition(randomSpawn);
                    }
                }
            } else {
                ApplyTransition(idle);
            }

            //Set state
            currentBehaviour = gameObject.AddComponent(state.Behaviour) as MonoBehaviour;
        } else {
            animator.SetTrigger("IsDeath");
            SoundManagerScript.PlaySound("bossdeath");
            StartCoroutine(EndGame());
        }
    }

    public void TakeDamage(float damage) {
        if (life > 0) {
            life -= damage;
        } else {
            life = 0;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player") {
            player.takeDmg(20);
            player.BossHit(transform.forward, 600, 500);
        }
    }

    private IEnumerator EndGame() {
        yield return new WaitForSeconds(5f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        LoadScenes.Instance.WinGame();
    }
}
