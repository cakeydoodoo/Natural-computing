using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMovement : MonoBehaviour {

    //private Transform follow;
    //private Vector3 lastPosition;
    EnemySpawn enemySpawn;
    Rigidbody rb;

    //enemy movement variables
    float moveSpeed = 0.05f;
    float turnSpeed;
    
    GameObject Target;
    GameObject[] targetEnemy;
    GameObject[] targetFollow;
    GameObject[] targetFollower;

    bool playerSeen;
    Color colour;
    public LayerMask layerMask;

	// Use this for initialization
	void Start () {
        StartCoroutine(randomMovement());
        rb = GetComponent<Rigidbody>();
        Target = GameObject.Find("Player");
        targetEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        targetFollow = GameObject.FindGameObjectsWithTag("Following");
        targetFollower = GameObject.FindGameObjectsWithTag("Follower");
    }
	
	// Update is called once per frame
	void Update () {
        //layerMask = ~layerMask;
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        Debug.Log(Physics.Raycast(ray, out hit, 1));

        //draws the ray cast
        Debug.DrawRay(ray.origin, ray.direction, colour);


        if (targetOpen() == true)
        {
            FollowPlayer();
            //changes an objects tag if it starts following the player
            gameObject.tag = "Following";
        }

        else if (enemyOpen() ==true)
        {
            FollowEnemy();
            //gameObject.tag = "Follower";

        }
        /*
        else if (FollowerOpen() == true )
        {
            FollowFollower();
            gameObject.tag = "Enemy";
        }
        */
        else
        {
            Moving();
            
        }
        

    }


    private bool targetOpen()
    {
        Vector3 targetPosition = new Vector3(Target.transform.position.x, Target.transform.position.y, Target.transform.position.z);
        RaycastHit hit1;
        if (Physics.SphereCast(transform.position, 0.1f, targetPosition - transform.position, out hit1, Mathf.Infinity))
        {
            //the spherecast will hit anything with a collider but will return true if it hits anything with the name tank.
            if (hit1.collider.gameObject.name.Contains("Player"))
            {
                return true;

            }
        }
        return false;
    }

    //returns true if a sphere cast hits an object with the tag following
    private bool enemyOpen()
    {

        foreach (GameObject go in targetFollow)
        {

                Vector3 targetPosition = new Vector3(go.transform.position.x, go.transform.position.y, go.transform.position.z);


                RaycastHit hit;
                if (Physics.SphereCast(transform.position, 0.1f, targetPosition - transform.position, out hit, Mathf.Infinity))
                {
                    //the spherecast will hit anything with a collider but will return true if it hits anything with the name tank.
                    if (hit.collider.gameObject.tag.Contains("Following"))
                    {
                        return true;
                    }
                }
            
        }
        return false;
    }
    /*
    private bool FollowerOpen()
    {

        foreach (GameObject go in targetFollow)
        {

            Vector3 targetPosition = new Vector3(go.transform.position.x, go.transform.position.y, go.transform.position.z);


            RaycastHit hit;
            if (Physics.SphereCast(transform.position, 0.1f, targetPosition - transform.position, out hit, Mathf.Infinity))
            {
                //the spherecast will hit anything with a collider but will return true if it hits anything with the name tank.
                if (hit.collider.gameObject.tag.Contains("Follower"))
                {
                    return true;
                }
            }

        }
        return false;
    }
    */


    //follows the player nad turns toward the player

    void FollowPlayer()
    {
        //changes the colour of the raycast
        colour = Color.red;
        //follows other enemies
        Vector3 distance = Target.transform.position - transform.position;
        Vector3 direction = distance.normalized;
        Vector3 move = direction * moveSpeed;

        transform.LookAt(Target.transform);
        rb.MovePosition(rb.position + move);
    }

    //enemy with tag ENemy follows enemies with tag following
    void FollowEnemy()
    {
        colour = Color.yellow;

        foreach (GameObject go in targetFollow)
        {

                Vector3 distance = go.transform.position - transform.position;
                Vector3 direction = distance.normalized;
                Vector3 move = direction * moveSpeed;

                transform.LookAt(go.transform);

                rb.MovePosition(rb.position + move);
            }
        
        
    }
    /*
    void FollowFollower()
    {
        colour = Color.cyan;
        foreach (GameObject go1 in targetEnemy)
        {


            foreach (GameObject go in targetFollower)
            {

                Vector3 distance = go.transform.position - go1.transform.position;
                Vector3 direction = distance.normalized;
                Vector3 move = direction * moveSpeed;

                transform.LookAt(go1.transform);

                rb.MovePosition(rb.position + move);
            }
        }

    }
    */


    //moves the object forward and rotates the object
    void Moving()
    {
        colour = Color.green;

        Vector3 move = transform.forward * moveSpeed;
        rb.MovePosition(rb.position + move);

        float turn = turnSpeed * 180 * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);

    }


    //generates a random number for turning and changes every second and tries to stop the enemies from colliding into the walls
    IEnumerator randomMovement()
    {
        turnSpeed = 0;
        yield return new WaitForSeconds(1);
        turnSpeed = Random.Range(-2f, 2f);
        yield return new WaitForSeconds(0.5f);
        turnSpeed = 0;
        yield return new WaitForSeconds(1);
        turnSpeed = Random.Range(-2f, 2f);
        yield return new WaitForSeconds(0.5f);
        turnSpeed = 0;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 left = transform.TransformDirection(Vector3.left);

        if (Physics.Raycast(transform.position, right, 5, layerMask))
        {
            moveSpeed = 0;
            yield return new WaitForSeconds(1);
            turnSpeed = Random.Range(-2f, 0);
            moveSpeed = 0.05f;
        }
        if (Physics.Raycast(transform.position, forward, 5, layerMask))
        {
            moveSpeed = 0;
            yield return new WaitForSeconds(1);
            turnSpeed *= -turnSpeed;
            moveSpeed = 0.05f;
        }

        if (Physics.Raycast(transform.position, left, 5, layerMask))
        {
            moveSpeed = 0;
            yield return new WaitForSeconds(1);
            turnSpeed = Random.Range(0f, 2f);
            yield return new WaitForSeconds(1);
            moveSpeed = 0.05f;
        }


    }


}

