using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    //These are the player's Variables, the raw info that defines them
    
    //The Rigidbody2D is a component that gives the player physics, and is what we use to move
    public Rigidbody2D RB;

    //TextMeshPro is a component that draws text on the screen.
    //We use this one to show our score.
    public TextMeshPro ScoreText;

    //We use this one to show our health.
    public TextMeshPro HealthText;
    
    //This will control how fast the player moves
    public float Speed = 5;
    
    //This is how many points we currently have
    public int Score = 0;
    public float startTime;
    public float scoreTimer;

    //This is the current player health
    public int Health = 20;
    
    //Float to track cooldown time for the dash
    public float dashTimer = 0f;
    public float dashReset = 0f;


    //Start automatically gets triggered once when the objects turns on/the game starts
    void Start()
    {
        //During setup we call UpdateScore to make sure our score text looks correct
        UpdateScore();
        //We call UpdateHealth to make sure our health text looks corrent
        UpdateHealth(0);
    }

    //Update is a lot like Start, but it automatically gets triggered once per frame
    //Most of an object's code will be called from Update--it controls things that happen in real time
    void Update()
    {
        // Call scoreByTime function
        scoreByTime();

        if (Score >= 50)
        {
            Win();
        }

        //Check to see if dash is on cooldown
        if(dashTimer > 2f)
        {
            dashReset -= Time.deltaTime;
        }

        if(dashReset <= 0f)
        {
            dashTimer = 0f;
            dashReset = 2f;
        }
        
        float currentSpeed = Speed;

        if(dashTimer <= 2f)
        {
            
            //Sprinting function
            if(Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed)
            {
                currentSpeed = Speed * 2f;
                dashTimer += Time.deltaTime;
            }
        }

        else 
        {
            currentSpeed = Speed;
        }
          

        
        
        //Finally, I take that variable and I feed it to the component in charge of movement
        Vector3 velocity = inputDir(currentSpeed);
        RB.linearVelocity = velocity;
    }

    //prefix(scope) [return type] name(parameters)
    private Vector3 inputDir(float currentSpeed)
    {

        //The code below controls the character's movement
        //First we make a variable that we'll use to record how we want to move
        Vector2 vel = new Vector2(0,0);
        
        //Then we use if statement to figure out what that variable should look like
        //If I hold the right arrow key, the player should move right. . .
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            vel.x = currentSpeed;
        }
        //If I hold the left arrow, the player should move left. . .
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            vel.x = -currentSpeed;
        }
        //If I hold the up arrow, the player should move up. . .
        if (Keyboard.current.upArrowKey.isPressed)
        {
            vel.y = currentSpeed;
        }
        //If I hold the down arrow, the player should move down. . .
        if (Keyboard.current.downArrowKey.isPressed)
        {
            vel.y = -currentSpeed;
        }

        return vel;
    }

    // Function meant to increase score the longer you stay alive
    void scoreByTime()
    {
        //timer starts at 0
        startTime = 0;

        //score timer increase as time goes on
        scoreTimer += Time.deltaTime;

        // Once score timer reaches 5...
        if (scoreTimer >= 5)
        {
            // Add 1 to score
            Score ++;
            // Update score text
            UpdateScore();
            // Reset score timer
            scoreTimer = 0;
        }
    }

    //This gets called whenever you bump into another object, like a wall or coin.
    private void OnCollisionEnter2D(Collision2D other)
    {
        //This checks to see if the thing you bumped into had the Hazard tag
        //If it does...
        if (other.gameObject.CompareTag("Hazard"))
        {
            //Take damage
            UpdateHealth(-1);
            //Check to see if health is 0 or lower
            if(Health <= 0){

                //Run your 'you lose' function!
                Die();
            }
            
        }
        
        //This checks to see if the thing you bumped into has the CoinScript script on it
        CoinScript coin = other.gameObject.GetComponent<CoinScript>();
        //If it does, run the code block belows
        if (coin != null)
        {
            //Tell the coin that you bumped into them so they can self destruct or whatever
            coin.GetBumped();
            //Make your score variable go up by one. . .
            Score++;
            //And then update the game's score text
            UpdateScore();
        }
    }

    //This function updates the game's score text to show how many points you have
    //Even if your 'score' variable goes up, if you don't update the text the player doesn't know
    public void UpdateScore()
    {
        ScoreText.text = "Score: " + Score;
    }

    //This function updates the player health on screen
    public void UpdateHealth(int deltaHealth)
    {
        Health += deltaHealth;
        HealthText.text = "Health: " + Health;
    }

    //If this function is called, the player character dies. The game goes to a 'Game Over' screen.
    public void Die()
    {
        SceneManager.LoadScene("Game Over");
    }

    //If this function is called, the player character wins. The game goes to a 'Game Win' screen.
    public void Win()
    {
        SceneManager.LoadScene("Game Win");
    }
}
