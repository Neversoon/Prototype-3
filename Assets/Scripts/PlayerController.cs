using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float score = 0;
    private Rigidbody playerRb;
    private Animator playerAnim;
    private AudioSource playerAudio;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    public float jumpForce = 600.0f;
    public float doubleJumpForce = 600.0f;
    public float gravityModifier;
    public bool isOnGround = true;
    public bool gameOver = false;
    private bool doubleJump = false;
    public bool boostMode = false;
    public Transform startingPoint;
    public float lerpSpeed;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        gameOver = true;
        StartCoroutine(PlayIntro());
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log("Score: " + score);
        PlayerMovement();
    }
    void PlayerStart()
    {
        playerAnim.SetFloat("Speed_f", 0.4f);
        transform.Translate(Vector3.right * Time.deltaTime * 3);
    }
    void PlayerMovement()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver)
        {
            playerAnim.SetTrigger("Jump_trig");
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSound, 1.0f);
            doubleJump = false;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !doubleJump && !isOnGround)
        {
            playerAnim.Play("Running_Jump", 3, 0f);
            doubleJump = true;
            playerRb.AddForce(Vector3.up * jumpForce / 2, ForceMode.Impulse);
            playerAudio.PlayOneShot(jumpSound, 1.0f);
        }
        if (Input.GetKey(KeyCode.D) && isOnGround)
        {
            playerAnim.SetBool("Run_Boost", true);
            boostMode = true;
        }
        else
        {
            playerAnim.SetBool("Run_Boost", false);
            boostMode = false;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !gameOver)
        {
            doubleJump = false;
            isOnGround = true;
            dirtParticle.Play();
        }
        else if (collision.gameObject.CompareTag("Obstracle"))
        {
            gameOver = true;
            Debug.Log("Game over");
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            explosionParticle.Play();
            dirtParticle.Stop();
            playerAudio.PlayOneShot(crashSound, 1.0f);
        }

    }
    IEnumerator PlayIntro()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startingPoint.position;
        float journeyLength = Vector3.Distance(startPos, endPos);
        float startTime = Time.time;
        float distanceCovered = (Time.time - startTime) * lerpSpeed;
        float fractionOfJourney = distanceCovered / journeyLength;
        GetComponent<Animator>().SetFloat("Speed_f", 0.5f);
        while (fractionOfJourney < 1)
        {
            distanceCovered = (Time.time - startTime) * lerpSpeed;
            fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(startPos, endPos,
            fractionOfJourney);
            yield return null;
        }
        GetComponent<Animator>().SetFloat("Speed_f", 1.5f);
        gameOver = false;
    }
}
