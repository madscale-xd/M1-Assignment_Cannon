//De Castro - A324

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationOnContact : MonoBehaviour
{
    private Vector3 _initialPosition; //variable for initial position of cannonball
    private LaunchCannonball launchy;
    private int ballCount = 0;
    private bool hasCollided = false; //flag to check if the cannonball has landed

    private void Start()
    {
        launchy = GameObject.Find("cannon (angle and force Changer)").GetComponent<LaunchCannonball>();
    }
    
    public void SetInitialPosition(Vector3 position)
    {
        _initialPosition = position; //store the instantiation point
    }

    //this method will be triggered when the cannonball enters the trigger collider (BONUS: LOGGING OUT DISTANCE TRAVELLED OF PROJECTILE)
    private void OnTriggerEnter(Collider other)
    {
        //check if the other object is the trigger to check the cannonball's landing
        if (other.CompareTag("LandingCheck") && !hasCollided) //ensuring that this is the FIRST contact with ground
        {
            //mark the cannonball as collided to prevent duplicate/faulty debug logs
            hasCollided = true;

            //calculate the distance from the cannonball's initial position to the point of contact (current position)
            float distance = Vector3.Distance(_initialPosition, transform.position);

            ballCount += 1;
            //log the distance produced by the choice of angle and power to the console
            Debug.Log("Cannonball " + launchy.GetBallCount() + " hit the ground! It went " + distance + " units from the point of origin " +
                "as it was launched with an angle of " + launchy.GetAngle() + " degrees and a launching power of " +
                launchy.GetPower() + " newtons!\nTap with your LEFT MOUSE BUTTON (LMB) or press SPACEBAR to launch another one!");
            
            launchy.SetIsLaunched(false);
        }
    }
}
