//De Castro - A324

using UnityEngine;

public class LaunchCannonball : MonoBehaviour       //(1) the group of objects this is attached to is the CANNON
{
    private Vector2 _initialVelocity;
    private Vector2 _initialPosition;
    private bool _isLaunched;
    private int ballCount=0;

    private float _time;

    [SerializeField] private GameObject projectilePrefab; //reference to cannonball prefab
    [SerializeField] private Transform launchPoint; //launch point of cannonball
    [SerializeField] private Rigidbody _cannonball; //reference to cannonball prefab's rb comp


    [Range(5, 250)] [SerializeField] private float _power;  
    [Range(0, 90)] [SerializeField] private float _angle;

    //for debug log purposes
    private float currPower;    
    private float currAngle;    

    [SerializeField] private Transform cannonBase;
    [SerializeField] private Transform cannonBarrel;

    private Quaternion initialCannonBaseRotation;
    private Quaternion initialCannonBarrelRotation;

    //SETTERS and GETTERS for the debug log
    public void SetIsLaunched(bool value)
    {
        _isLaunched = value;
    }

    public bool GetIsLaunched()
    {
        return _isLaunched;
    }

    public void SetBallCount()
    {
        ballCount++;
    }
    public int GetBallCount()
    {
        return ballCount;
    }

    public void SetAngle(){
        currAngle = _angle;
    }
    public float GetAngle(){
        return currAngle;
    }

    public void SetPower(){
        currPower = _power;
    }
    public float GetPower(){
        return currPower;
    }

     private void Start()
    {
        //store the initial local rotations of the cannon base and barrel for its eventual rotation
        initialCannonBaseRotation = cannonBase.localRotation;
        initialCannonBarrelRotation = cannonBarrel.localRotation;
    }

    public void Launch()
    {
        SetBallCount();
         Debug.Log("Cannonball "+GetBallCount()+" launched!");
        //instantiate the cannonball at launch point with the same rotation as the catapult
        GameObject projectile = Instantiate(projectilePrefab, launchPoint.position, launchPoint.rotation);

        LocationOnContact cannonballTrigger = projectile.GetComponent<LocationOnContact>();

        //set initial position of the cannonball (where it was instantiated)
        if (cannonballTrigger != null)
        {
            cannonballTrigger.SetInitialPosition(launchPoint.position); // Set the launch position
        }

        //access and assign a variable to cannonball rb for later
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        //calculate the initial velocity based on the set angle and power
        if(_power < 5)      //makes it realistic to start at this much power for a cannon
        {
            _power = 5;
        }
        _initialVelocity = new Vector2(
            Mathf.Cos(_angle * Mathf.PI / 180) * _power,
            Mathf.Sin(_angle * Mathf.PI / 180) * _power
        );

        //set the initial velocity of cannonball rb
        projectileRb.velocity = new Vector3(_initialVelocity.x, _initialVelocity.y, 0);

        _initialPosition = new Vector2(_cannonball.position.x, _cannonball.position.y);
        _isLaunched = true;

        SetAngle();
        SetPower();
    }

    private float KinematicEquation(float acceleration, float velocity, float position, float time) //usage of the KINEMATIC FORMULA
                                                            //both from video AND corrected from assignment content (3)
    {
        return (0.5f * acceleration * time * time) + (velocity * time) + position;
    }

     private void RotateCannon()                //rotate the cannon in real-time when manipulating ANGLE value (visual)
    {
        float totalAngle = _angle;

        cannonBase.localRotation = initialCannonBaseRotation * Quaternion.Euler(-totalAngle,0,0);
        cannonBarrel.localRotation = initialCannonBarrelRotation * Quaternion.Euler(0, 0, -totalAngle);
    }


    private void Update()
    {
        RotateCannon();

        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && !_isLaunched)   //(2)ADD and INSTANTIATE on LMB/SPACE
        {
            Launch();
        }

        if (_isLaunched)
        {
            _time += Time.deltaTime;

            float newRockX = KinematicEquation(0, _initialVelocity.x, _initialPosition.x, _time);
            float newRockY = KinematicEquation(-9.81f, _initialVelocity.y, _initialPosition.y, _time);

            // Update the position of the cannonball based on the kinematic equations
            _cannonball.position = new Vector3(newRockX, newRockY, _cannonball.position.z);
        }
    }
}