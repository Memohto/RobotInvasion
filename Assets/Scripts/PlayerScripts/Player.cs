using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour {
    public static readonly float MIN_SPD = 5f;
    public static readonly float MAX_SPD = 7f;
    public static readonly int INIT_LIFE = 100;
    public static readonly int INIT_STAMINA = 100;

    public static Player Instance { private set; get; }

    [Header("Player Options")]
    [SerializeField]
    private Rigidbody rigidBody;
    [SerializeField]
    private Collider _collider;
    [SerializeField]
    private RawImage bloodImage;
    [SerializeField]
    private RawImage bloodBackgroundImage;
    [SerializeField]
    private GameObject boss;
    [Space(10)]
    [SerializeField]
    private float jumpForce = 200f;

    private Camera _camera;
    
    private float mouseSpd = GameManager.Instance.Sensibility;
    private float speed = Player.MIN_SPD;
    private int life = Player.INIT_LIFE;
    private int stamina = Player.INIT_STAMINA;
    private bool inStore = true;
    public bool withBoss = false;

    public Weapon Weapon { set; get; }
    public PlayerStates State { set; get; }

    private void Start() {
        Instance = this;
        Weapon = null;
        _camera = Camera.main;
    }

    private void Update() {
        //Movimiento
        float xDir = Input.GetAxis("Horizontal");
        float zDir = Input.GetAxis("Vertical");


        Vector3 xMove = transform.right * xDir;
        Vector3 zMove = transform.forward * zDir;

        Vector3 direction = (xMove + zMove).normalized;
        

        //Manage Speed
        if (Weapon != null) {
            if (Weapon.State == WeaponStates.Aim) {
                mouseSpd = GameManager.Instance.Sensibility * 0.5f;
                speed = Player.MIN_SPD / 3;
            } else {
                mouseSpd = GameManager.Instance.Sensibility;
                if (stamina > 0) {
                    if (Input.GetKey(KeyCode.LeftShift)) {
                        speed = Player.MAX_SPD;
                    } else {
                        speed = Player.MIN_SPD;
                    }
                } else {
                    speed = Player.MIN_SPD;
                }
            }
        } else {
            mouseSpd = GameManager.Instance.Sensibility;
            if (stamina > 0) {
                if (Input.GetKey(KeyCode.LeftShift)) {
                    speed = Player.MAX_SPD;
                } else {
                    speed = Player.MIN_SPD;
                }
            } else {
                speed = Player.MIN_SPD;
            }
        }
        

        //Manejar Estados
        if (direction.magnitude != 0) {
            if (speed == Player.MAX_SPD) {
                State = PlayerStates.Run;
                
            } else {
                State = PlayerStates.Move;
               
            }
        } else {
            State = PlayerStates.Idle;
        }

        //Manejar Stamina
        if (Input.GetKeyDown(KeyCode.LeftShift) && stamina > 0 && direction.magnitude != 0) {
            StartCoroutine("LowerStamina");
            StopCoroutine("Rest");
        } else if (Input.GetKeyUp(KeyCode.LeftShift)) {
            StopCoroutine("LowerStamina");
            StartCoroutine("Rest");
        }

        rigidBody.MovePosition(transform.position + ((xMove + zMove) * speed) * Time.deltaTime);

        //Salto
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()) {
            SoundManagerScript.PlaySound("jump");
            rigidBody.AddForce(transform.up * jumpForce);
        }

        //Disparo
        if (Weapon != null && !inStore) {
            if (Input.GetMouseButtonDown(0)) {
                Weapon.StartCoroutine("Shoot");
            } else if (Input.GetMouseButtonUp(0)) {
                Weapon.StopCoroutine("Shoot");
            }
        }
        

        //Giro
        if (!inStore) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            //Giro en X
            transform.Rotate(Vector3.up * mouseX * mouseSpd);

            //Giro en Y
            _camera.transform.Rotate(Vector3.left * mouseY * mouseSpd);
            //TODO: Clamp en y
        } else {
            if (Weapon != null) {
                Weapon.StopAllCoroutines();
            }
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private IEnumerator LowerStamina() {
        while (stamina > 0) {
            yield return new WaitForSeconds(0.05f);
            stamina -= 1;
            GameManager.Instance.Stamina = stamina;
        }
    }

    private IEnumerator Rest() { 
        while (stamina < 100) {
            yield return new WaitForSeconds(0.5f);
            if (State == PlayerStates.Idle) {
                stamina += 5;
                if (stamina > 100) stamina = 100;
            } else {
                stamina += 1;
            }
            GameManager.Instance.Stamina = stamina;
        }
    }

    private IEnumerator TakeDamageGUI() {
        SoundManagerScript.PlaySound("hit");
        bloodImage.enabled = true;
        bloodBackgroundImage.enabled = true;
        float cont = 0.4f;
        bloodBackgroundImage.color = new Color(255, 0, 0, cont);
        bloodImage.color = new Color(255, 0, 0, cont);
        while (cont >= 0) {
            yield return new WaitForSeconds(0.3f);
            bloodBackgroundImage.color = new Color(255, 0, 0, cont);
            bloodBackgroundImage.color = new Color(bloodBackgroundImage.color.r, bloodBackgroundImage.color.g, bloodBackgroundImage.color.b, cont);
            cont -= 0.1f;
        }
        bloodBackgroundImage.enabled = false;
        bloodImage.enabled = false;
    }

    public void takeDmg(int dmg) {
        StopCoroutine("TakeDamageGUI");
        StartCoroutine("TakeDamageGUI");
        life -= dmg;
        GameManager.Instance.Life = life;
        if (GameManager.Instance.Life <= 0) {
            GameManager.Instance.Life = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            LoadScenes.Instance.EndGame();
        }
    }

    public bool IsGrounded() {
        return Physics.Raycast(transform.position, Vector3.down, _collider.bounds.extents.y + 0.25f);
    }

    public void InStore(bool val) {
        inStore = val;
    }

    public void BossHit(Vector3 direction, float xForce, float yForce) {
        rigidBody.AddForce(direction * xForce + Vector3.up * yForce);
    }

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.tag == "Ammo") {
            if (Weapon != null) {
                if (Weapon is Gun) {
                    Ammo.Instance.AmmoTotal = Gun.maxAmmoCharger;
                } else if (Weapon is Revolver) {
                    Ammo.Instance.AmmoTotal = Revolver.maxAmmoCharger;
                } else if (Weapon is Rifle) {
                    Ammo.Instance.AmmoTotal = Rifle.maxAmmoCharger;
                }
            }
            GameManager.Instance.AmmoTotal = 10;
        } else if (collision.gameObject.tag == "Money") {
            GameManager.Instance.Money = Mathf.Round(Random.Range(50f, 100f));
        } else if (collision.gameObject.tag == "Health") {
            if (life <= 90) {
                life += 10;
            } else {
                life = 100;
            }
            GameManager.Instance.Life = life;
        } else if (collision.gameObject.tag == "Portal") {
            withBoss = true;
            GameBackgroundMusic.Instance.ChangeMusic();
            Instantiate(boss, new Vector3(146.6f, 6, -20), Quaternion.identity);
            transform.position = new Vector3(145, 7.8f, -50);
        }
    }
}

