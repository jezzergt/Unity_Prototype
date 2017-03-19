using UnityEngine;

    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private bool switchCam = false;
        [SerializeField]
        Camera firstPersonCamera;
        [SerializeField]
        Camera thirdPersonCamera;
        [SerializeField]
        private float m_WalkSpeed;
        [SerializeField]
        private float m_RunSpeed;
        [SerializeField]
        private float m_JumpHeight;
        [SerializeField]
        private float m_Gravity;
        [SerializeField]
        [Range(0, 1)]
        private float m_AirControlPercent;
        [SerializeField]
        private float m_TurnSmoothTime;
        [SerializeField]
        private float m_SpeedSmoothTime;

        float m_TurnSmoothVelocity;
        float m_SpeedSmoothVelocity;
        float m_CurrentSpeed;
        float m_VelocityY;

        private CharacterController m_Controller;
        private Transform m_Camera;

        private Vector2 m_Input;
        private Vector3 m_MoveDir;
        private Animator m_Animator;

        void Start()
        {

            m_Controller = GetComponent<CharacterController>();
            m_Camera = Camera.main.transform;
            m_Animator = GetComponent<Animator>();
            firstPersonCamera.GetComponent<Camera>().enabled = false;
            thirdPersonCamera.GetComponent<Camera>().enabled = true;
        }

        void Update()
        {
        // Input
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));                                  // Keyboard input "Horziontal" & "Vertical"
        Vector2 inputDir = input.normalized;                                                                                        // Turning our input Vector2 into a direction

        bool running = Input.GetKey(KeyCode.LeftShift);                                                                             // Turning on 'bool running' by keyboard input LeftShift

        Move(inputDir, running);

            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
            }

            // Camera Switch
            CameraSwitch();

            // Animator
            float animationSpeedPercent = ((running) ? m_CurrentSpeed / m_RunSpeed : m_CurrentSpeed / m_WalkSpeed * .5f);
            m_Animator.SetFloat("speedPercent", animationSpeedPercent, m_SpeedSmoothTime, Time.deltaTime);
        }

        void Move(Vector2 inputDir, bool running)
        {
            if (inputDir != Vector2.zero)
            {
                float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + m_Camera.eulerAngles.y;                                                                       // rotating around the Y axis multiplied by Atan2 'Dirction.x, Direction.y' multiplied by radions - degrees (Mathf.Rad2Deg)
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref m_TurnSmoothVelocity, GetModifiedSmoothTime(m_TurnSmoothTime));
            }

            float targetSpeed = ((running) ? m_RunSpeed : m_WalkSpeed) * inputDir.magnitude;                                                      // If we are 'running' then speed is equal to runSpeed otherwise speed is equal to walkSpeed. If there is no input the speed will be set to 0
            m_CurrentSpeed = Mathf.SmoothDamp(m_CurrentSpeed, targetSpeed, ref m_SpeedSmoothVelocity, GetModifiedSmoothTime(m_SpeedSmoothTime));

            m_VelocityY += Time.deltaTime * m_Gravity;
            Vector3 velocity = transform.forward * m_CurrentSpeed + Vector3.up * m_VelocityY;

            m_Controller.Move(velocity * Time.deltaTime);
            m_CurrentSpeed = new Vector2(m_Controller.velocity.x, m_Controller.velocity.z).magnitude;

            if (m_Controller.isGrounded)
            {
                m_VelocityY = 0;
            }
        }

        void Jump()
        {
            if (m_Controller.isGrounded)
            {
                float jumpVelocity = Mathf.Sqrt(-2 * m_Gravity * m_JumpHeight);
                m_VelocityY = jumpVelocity;

            }
        }

        float GetModifiedSmoothTime(float smoothTime)
        {
            if (m_Controller.isGrounded)
            {
                return smoothTime;
            }

            if (m_AirControlPercent == 0)
            {
                return float.MaxValue;
            }
            return smoothTime / m_AirControlPercent;
        }

        void CameraSwitch()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                switchCam = !switchCam;
            }

            if (switchCam == true)
            {
                firstPersonCamera.GetComponent<Camera>().enabled = false;
                firstPersonCamera.GetComponent<AudioListener>().enabled = false;

                thirdPersonCamera.GetComponent<Camera>().enabled = true;
                thirdPersonCamera.GetComponent<AudioListener>().enabled = true;
            }
            else
            {
                firstPersonCamera.GetComponent<Camera>().enabled = true;
                firstPersonCamera.GetComponent<AudioListener>().enabled = true;

                thirdPersonCamera.GetComponent<Camera>().enabled = false;
                thirdPersonCamera.GetComponent<AudioListener>().enabled = false;
            }
        }
    }

