using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    public Animator camAnim;
    public Camera playerCamera;

    public float playerSpeed = 17f;
    public float momentumDamping = 10f;

    private CharacterController pCC;
    private AudioSource walkSource;
    private bool isWalking;
    private Vector3 planeNorm;
    private Vector3 inputVector;
    private Vector3 flatVector;
    private Vector3 movementVector;
    private float gravity = -10f;



    // Start is called before the first frame update
    void Start()
    {
        pCC = GetComponent<CharacterController>();
        planeNorm = new Vector3(0,1,0);
        walkSource = GetComponent<AudioSource>();
        pCC.enableOverlapRecovery = true;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        MovePlayer();
        camAnim.SetBool("isWalking", isWalking);
        if (isWalking && !walkSource.isPlaying)
        {
            walkSource.Play();
        }
        else if(!isWalking && walkSource.isPlaying)
        {
            walkSource.Stop();
        }
    }

    void GetInput()
    {
        //if holding down wasd moves char
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            inputVector.Normalize();
            inputVector = transform.TransformDirection(inputVector);

            isWalking = true;//enables head bob
        }
        //else if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    gameObject.transform.SetPositionAndRotation(new Vector3(0,1.33f,-2.16f),Quaternion.identity);
        //}
        //if not holding wasd slows down char
        else
        {
            inputVector = Vector3.MoveTowards(inputVector, Vector2.zero, momentumDamping * Time.deltaTime);

            isWalking = false;//disables head bob
        }
        flatVector = Vector3.ProjectOnPlane(inputVector, planeNorm);
        flatVector.Normalize();
        movementVector = (flatVector*playerSpeed)+ (Vector3.up*gravity);

    }

    void MovePlayer()
    {
        pCC.Move(movementVector*Time.deltaTime);
    }

}
