using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Attack Options")]
    [SerializeField]
    private LayerMask mascaraJugador;
    [SerializeField]
    private Transform centroAtaque;
    [Space(10)]
    [SerializeField]
    private float radioVision = 10f,
                  ataqueCooldown = 1f,
                  rangoAtaque = 0.5f,
                  radioBrinco = 0;
    [SerializeField]
    private int dmgPorGolpe = 10;

    public bool isBrincador=false,
                isNormal=true,
                isVeneno=false;

    [SerializeField]
    private GameObject ammoDrop, moneyDrop,healthDrop;

    private Transform jugador;
    private NavMeshAgent agent;
    private Animator anim;
    private CapsuleCollider col;

    private Rigidbody rb;

    private int health = 5;
    private float timeSinceHit,
                  timeSinceBrinco;

    private bool dead = false,
                 canJump = true;

    private float speed,acc;

    void Start() {
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        timeSinceHit = 0f;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        col = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        speed = agent.speed;
        acc = agent.acceleration;
        SoundManagerScript.PlaySound("enemy");

        if (isBrincador)
        {
            health = Mathf.CeilToInt(health / 2f);
        }
    }

    void Update()
    {
        if (dead)
        {
            return;
        }
        //TODO: Poner en corrutina
        float distancia = Vector3.Distance(jugador.position,transform.position);

        if(Vector3.Distance(Vector3.up * transform.position.y, Vector3.up * jugador.position.y) <= 3 && agent.speed>speed)
        {
            agent.speed = speed;
            agent.acceleration = acc;
        }

        if (distancia <= radioVision)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("RobotArmature|Robot_Jump"))
            {
                canJump = false;
                Collider[] hitted = Physics.OverlapSphere(centroAtaque.position, rangoAtaque, mascaraJugador);
                if (timeSinceHit <= 0)
                {
                    foreach (Collider golpeado in hitted)
                    {
                        Player tmp = golpeado.gameObject.GetComponent<Player>();
                        if (tmp != null)
                        {
                            tmp.takeDmg(dmgPorGolpe * 2);
                            timeSinceHit = 1;
                        }
                    }
                }

                if (health <= 0)
                {
                    SoundManagerScript.PlaySound("death");
                    GameManager.Instance.AddKill();
                    anim.SetTrigger("Die");
                    Destroy(gameObject, 2f);
                    dead = true;
                    col.enabled = false;
                    if (Random.Range(0f, 1f) <= 0.5f)
                    {
                        if (Random.Range(0f, 1f) <= 0.5f)
                        {
                            Instantiate(ammoDrop, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
                        }
                        else
                        {
                            Instantiate(moneyDrop, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
                        }

                    }

                }

                return;
            }else if (!canJump)
            {
                canJump = true;
                GetComponent<NavMeshAgent>().enabled = true;
                rb.isKinematic = true;
                timeSinceHit = 0;
            }

            if (!rb.isKinematic)
            {
                return;
            }

            agent.SetDestination(jugador.position);

            if (distancia <= agent.stoppingDistance-0.1f && isNormal)
            {
                anim.SetBool("Correr", false);
                transform.LookAt(new Vector3(jugador.position.x,transform.position.y,jugador.position.z));
                //Ataque
                if (timeSinceHit <= 0f)
                {
                    anim.SetTrigger("Ataque");
                    ataque();
                    timeSinceHit = ataqueCooldown;
                }
                else
                {
                    timeSinceHit -= Time.deltaTime;
                }

            }
            else if (distancia<=radioBrinco && isBrincador && canJump){
                transform.LookAt(new Vector3(jugador.position.x, transform.position.y, jugador.position.z));

                if (timeSinceBrinco <= 0)
                {
                    ataqueBrinco();
                }
                else
                {
                    if (distancia >= agent.stoppingDistance+0.25f)
                    {
                        anim.SetBool("Correr", true);
                    }
                    else
                    {
                        anim.SetBool("Correr", false);
                    }
                    timeSinceBrinco -= Time.deltaTime;
                }
                

            }
            else
            {
                anim.SetBool("Correr", true);
            }
        }
        else
        {
            anim.SetBool("Correr", false);
        }

        if (health <= 0) {
            SoundManagerScript.PlaySound("death");
            GameManager.Instance.AddKill();
            GetComponent<NavMeshAgent>().enabled = true;
            rb.isKinematic = true;
            anim.SetTrigger("Die");
            Destroy(gameObject,2f);
            dead = true;
            agent.isStopped=true;
            col.enabled = false;
            if (Random.Range(0f, 1f) <= 0.5f)
            {
                if (Random.Range(0f, 1f) <= 0.5f)
                {
                    Instantiate(moneyDrop, new Vector3(transform.position.x, transform.position.y, transform.position.z),transform.rotation);
                }
                else
                {
                    if(Random.Range(0f, 1f) <= 0.5f)
                    {
                        Instantiate(ammoDrop, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
                    }
                    else
                    {
                        Instantiate(healthDrop, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
                    }
                   
                }
                
            }
            
        }
        
    }

    public void ataque()
    {
        Collider[] hitted = Physics.OverlapSphere(centroAtaque.position, rangoAtaque, mascaraJugador);
        foreach (Collider golpeado in hitted)
        {
            Player tmp = golpeado.gameObject.GetComponent<Player>();
            if (tmp != null)
            {
                tmp.takeDmg(dmgPorGolpe);
            }
        }
    }

    public void ataqueBrinco()
    {
        anim.SetBool("Correr", false);
        anim.SetTrigger("AtaqueBrinco");

        GetComponent<NavMeshAgent>().enabled = false;
        rb.isKinematic = false;
        rb.AddForce(transform.forward.x*400, 250, transform.forward.z*400);
        timeSinceBrinco = 2.5f;

    }

    public void TakeDamage(int dmg) {
        health -= dmg;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Jump" && agent.speed <= speed)
        {
            agent.speed = agent.speed * 10;
            agent.acceleration = acc * 3;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioVision);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(centroAtaque.position, rangoAtaque);

        Gizmos.color = Color.blue;
        float theta = 0;
        float x = radioBrinco * Mathf.Cos(theta);
        float y = radioBrinco * Mathf.Sin(theta);
        Vector3 pos = transform.position + new Vector3(x, 0, y);
        Vector3 newPos = pos;
        Vector3 lastPos = pos;
        for (theta = 0.1f; theta < Mathf.PI * 2; theta += 0.1f)
        {
            x = radioBrinco * Mathf.Cos(theta);
            y = radioBrinco * Mathf.Sin(theta);
            newPos = transform.position + new Vector3(x, 0, y);
            Gizmos.DrawLine(pos, newPos);
            pos = newPos;
        }
        Gizmos.DrawLine(pos, lastPos);
    }
}
