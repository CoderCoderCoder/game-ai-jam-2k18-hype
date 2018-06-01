using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public enum GameState
    {
        NOT_STARTED = 0,
        LOADING = 100,
        GAME_OVER = 200
    };

    private GameState gameState;
    public GameObject projectile;
    public GameObject crosshair;

    public Text scoreText;
    public Text treasureText;
    public Text livesText;

    public float weaponCool = 0.5f;
    private float weaponTimer = 0.0f;
    public float moveSpeed = 0.0f;
    public float deathTime = 2.0f;
    private float deathTimer = 0.0f;
    public int lives = 3;

    private SpriteRenderer sprite;
    private Animator anim;
    private Vector3 lastAim = Vector3.left;

    private bool cooling = false;
    private bool dead = false;
    private bool facingRight = false;
    private bool earnedLevelLife = false;

    private int totalLevelTreasure;
    private int treasureRemaining;
    private int score = 0;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Respawn()
    {
        dead = false;
        deathTimer = 0.0f;

        transform.position = Vector3.zero;
        anim.ResetTrigger("Die");
        anim.Play("PlayerSwim");
    }

    public void SetNumTreasures(int numTreasures)
    {
        totalLevelTreasure = treasureRemaining = numTreasures;
    }

    void FixedUpdate ()
    {
        if(!dead)
        {
            Vector3 motionAmount = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if (motionAmount == Vector3.zero)
                return;

            if(motionAmount.x != 0.0f)
                lastAim = new Vector3(motionAmount.x, 0.0f, 0.0f).normalized;

            motionAmount = moveSpeed * Time.deltaTime * motionAmount.normalized;

            if (motionAmount.x > 0)
                sprite.flipX = facingRight = true;
                
            else if (motionAmount.x < 0)
                sprite.flipX = facingRight = false;

            transform.position += motionAmount;
        } 
	}

    private void Update()
    {
        if(gameState == GameState.NOT_STARTED)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {

            }
        }

        if(dead)
        {
            deathTimer += Time.deltaTime;

            if(deathTimer > deathTime)
                Respawn();
        }
        else if(Input.GetKeyDown(KeyCode.Return))
        {
            CaveGenerator caveGen = FindObjectOfType<CaveGenerator>();
            GrammarGenerator grammarGen = FindObjectOfType<GrammarGenerator>();
            caveGen.Generate();
            grammarGen.Generate();
            earnedLevelLife = false;
            Respawn();
        }
        else
        {
            bool leftArrow = Input.GetKey(KeyCode.LeftArrow);
            bool rightArrow = Input.GetKey(KeyCode.RightArrow);
            bool upArrow = Input.GetKey(KeyCode.UpArrow);
            bool downArrow = Input.GetKey(KeyCode.DownArrow);

            if(leftArrow || rightArrow || upArrow || downArrow)
            {
                crosshair.SetActive(true);
                lastAim = Vector3.zero;

                if (leftArrow)
                    lastAim.x = -2.0f;
                else if (rightArrow)
                    lastAim.x = 2.0f;
                if (upArrow)
                    lastAim.y = 1.0f;
                else if (downArrow)
                    lastAim.y = -1.0f;

                crosshair.transform.localPosition = lastAim + Vector3.up;

                lastAim.Normalize();
            }
            else
            {
                lastAim = (facingRight) ? Vector3.right : Vector3.left;
                crosshair.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.Space) && !cooling)
            {
                cooling = true;
                Projectile newProjectile = (Instantiate(projectile) as GameObject).GetComponent<Projectile>();
                newProjectile.Launch(transform.position + Vector3.up + lastAim, lastAim);
                weaponTimer = 0.0f;
            }

            if (cooling)
            {
                weaponTimer += Time.deltaTime;

                if (weaponTimer >= weaponCool)
                    cooling = false;
            }
        }
    }

    private void OnGUI()
    {
        livesText.text = lives.ToString();
        treasureText.text = treasureRemaining.ToString();
        scoreText.text = score.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            anim.SetTrigger("Die");
            score = Mathf.Clamp(score - 1000, 0, score);
            dead = true;
            --lives;
            deathTimer = 0.0f;
        }
        else if(collision.CompareTag("Pickup"))
        {
            score += 200;
            --treasureRemaining;

            if(treasureRemaining <= 0.2f * totalLevelTreasure && !earnedLevelLife)
            {
                earnedLevelLife = true;
                ++lives;
            }

            Destroy(collision.gameObject);
        }
    }
}
