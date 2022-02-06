using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    private float speed = 30;
    private float leftBound = -15;
    private PlayerController playerControllerScript;
    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerControllerScript.boostMode){
            speed = 60;
        }
        else{
            speed = 30;
        }
        if(playerControllerScript.gameOver == false){
        playerControllerScript.score += Time.deltaTime * speed;
        transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
        if(transform.position.x < leftBound && gameObject.CompareTag("Obstracle")){
            Destroy(gameObject);
        }
    }
}
