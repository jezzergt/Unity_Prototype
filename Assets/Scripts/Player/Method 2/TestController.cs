using UnityEngine;
using System.Collections;

public class TestController: MonoBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 6f;
    public float sensitivity = 2f;

    CharacterController player;
    Animator animator;

    public GameObject eyes;

    float moveFB;
    float moveLR;

    float rotX;
    float rotY;


    void Start()
    {
        player = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        moveFB = Input.GetAxis("Vertical") * walkSpeed;
        animator.SetFloat("Walk_Forward", moveFB);

        moveLR = Input.GetAxis("Horizontal") * walkSpeed;

        
        animator.SetFloat("Walk_Right", moveLR);
        
        

        rotX = Input.GetAxis("Mouse X") * sensitivity;
        rotY -= Input.GetAxis("Mouse Y") * sensitivity;

        rotY = Mathf.Clamp(rotY, -60f, 60f);

        Vector3 movement = new Vector3(moveLR, 0, moveFB);
        transform.Rotate(0, rotX, 0);
        eyes.transform.localRotation = Quaternion.Euler(rotY, 0, 0);
        //eyes.transform.Rotate (-rotY, 0, 0);

        movement = transform.rotation * movement;
        player.Move(movement * Time.deltaTime);

    }
}