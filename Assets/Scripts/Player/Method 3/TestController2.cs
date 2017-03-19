using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class TestController2 : MonoBehaviour {

    [SerializeField]
    private float walkSpeed = 2f;
    [SerializeField]
    private float runSpeed = 6f;
    [SerializeField]
    private float lookSensitivity = 3f;

    private PlayerMotor motor;
    Animator anim;

    int jumpHash = Animator.StringToHash("Jump");

    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //Calculate movement velocity as a 3D vector
        float _zMov = Input.GetAxisRaw("Vertical");
        float _xMov = Input.GetAxisRaw("Horizontal");
        
        Vector3 _moveHorzintal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;

        //Final movement vector
        Vector3 _velocity = (_moveHorzintal + _movVertical).normalized * walkSpeed;
        motor.Move(_velocity);

        //Calculate rotation as a 3D vector (turning around)
        float _yRot = Input.GetAxisRaw("Mouse X");
        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;
        motor.Rotate(_rotation);

        //Calculate camera rotation as a 3D vector (turning around)
        float _xRot = Input.GetAxisRaw("Mouse Y");
        Vector3 _cameraRotation = new Vector3(_xRot, 0f, 0f) * lookSensitivity;
        motor.RotateCamera(_cameraRotation);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger(jumpHash);
        }

       
        /* if (_zMov != 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        } */
    }
}
