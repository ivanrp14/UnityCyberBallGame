using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotoController : MonoBehaviour
{
    private PlayerInputManager input;
    private CharacterController controller;
    public bool ballMode, isGrounded;

    private Animator animator;
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float ballSpeed = 13f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float RotationSpeed = 10f;
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool canSlide = false;
    [SerializeField] private float dragFactor = 0.95f;
    [SerializeField] private float lastXAddition = 10f;
    [SerializeField] private float lastX;
    public Vector3 velocity;
    public bool useGravity = true;
    [Header("Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.1f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private Transform rayCheck;
    [SerializeField] private float rayCheckDistance = 0.1f;
    [Header("Camera")]
    [SerializeField]
    private GameObject robot;
    [SerializeField] private Transform cameraFollowTarget;
    public Health health;
    private Destroy destroy;
    float xRotation, yRotation;
    const float SPEED_MULTIPLIER = 100f;
    Dash dash;
    Damage damage;
    [SerializeField] private ParticleSystem jumpFx;


    void Start()
    {
        damage = GetComponent<Damage>();
        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInputManager>();
        animator = GetComponentInChildren<Animator>();
        health = GetComponent<Health>();
        destroy = GetComponent<Destroy>();
        lastX = transform.position.x - 2;
        robot = GameObject.Find("Robot");
        dash = GetComponent<Dash>();
    }

    void Update()
    {
        CheckMode();
        if (ballMode)
        {

            Move(ballSpeed);
            damage.canDoDamage = true;


        }
        else
        {
            damage.canDoDamage = false;
            Move(moveSpeed);
        }
        Rotate();
        GroundAndGravity();
        if (input.jump)
        {
            Jump();
        }
        input.jump = false;
        if (!canMove)
        {
            velocity.x = 0f;
        }
        // Aplicar la velocidad al CharacterController
        controller.Move(velocity * Time.deltaTime);

    }

    void Move(float s)
    {

        RayCheck();
        Vector3 inputDir = new Vector3(input.move.x, 0, 0);
        if (input.move.magnitude > 0 && canMove)
        {
            // Movimiento normal basado en la entrada del jugador
            velocity.x = inputDir.x * s;
        }
        else if (canMove)
        {
            // Aplicar desaceleración cuando no hay entrada del jugador
            velocity.x *= dragFactor;
        }


        // Actualizar las animaciones según la velocidad
        animator.SetFloat("speed", Math.Abs(Mathf.Round(velocity.x * SPEED_MULTIPLIER)));
        animator.SetFloat("yVelocity", velocity.y);
    }
    void CheckXPosition()
    {
        if (transform.position.x < lastX && input.move.x < 0)
        {
            canMove = false;
        }

        else
        {
            canMove = true;
        }

        float newLastX = transform.position.x - lastXAddition;
        lastX = newLastX > lastX ? newLastX : lastX;

    }

    void CheckMode()
    {
        if (input.switchMode && isGrounded)
        {
            ballMode = !ballMode;
            if (ballMode)
            {
                TransformToBall();
            }
            else
            {
                TransformToRobot();
            }
            input.switchMode = false;

        }
        animator.SetBool("ballMode", ballMode);
    }

    void Rotate()
    {
        float targetYRotation = input.move.x >= 0 ? 90f : -90f;
        Quaternion targetRotation = Quaternion.Euler(0, targetYRotation, 0);

        robot.transform.rotation = Quaternion.Slerp(
            robot.transform.rotation,
            targetRotation,
            Time.deltaTime * RotationSpeed
        );
    }

    public void Jump()
    {
        if (isGrounded || canSlide)
        {
            if (jumpFx != null)
            {
                jumpFx.Play();
            }
            velocity.y = Mathf.Sqrt(2f * gravity * jumpHeight);
            velocity.x += input.move.x * 2f;
            if (canSlide)
            {
                velocity.y *= 1.5f;
            }
            input.jump = false;
            animator.SetBool("isJumping", true);
        }
    }

    bool IsGrounded()
    {
        RaycastHit hit;
        if (Physics.SphereCast(groundCheck.position, groundDistance / 2f, -transform.up, out hit, groundDistance, groundMask))
        {
            if (((1 << hit.collider.gameObject.layer) & groundMask) != 0)
            {
                return true;
            }

        }

        return false;
    }

    void GroundAndGravity()
    {

        isGrounded = IsGrounded();
        if (isGrounded)
        {
            if (velocity.y < 0)
            {
                velocity.y = -2f;
            }
        }
        else if (useGravity)
        {
            velocity.y -= gravity * Time.deltaTime;
        }

        animator.SetBool("isGrounded", isGrounded);
    }
    void RayCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(rayCheck.position, rayCheck.forward, out hit, rayCheckDistance, wallMask))
        {
            canMove = false;
            canSlide = true;

        }
        else if (canMove)
        {
            canMove = true;
            canSlide = false;
        }
    }
    public float GetLookDirection()
    {
        return robot.transform.rotation.eulerAngles.y <= 90 ? 1f : -1f;
    }
    public void TransformToBall()
    {
        ballMode = true;

        input.listenMovement = false;
        input.move.x = GetLookDirection();

    }
    public void TransformToRobot()
    {
        ballMode = false;
        health.canTakeDamage = true;

        input.listenMovement = true;
    }
    /// <summary>
    /// OnControllerColliderHit is called when the controller hits a
    /// collider while performing a Move.
    /// </summary>
    /// <param name="hit">The ControllerColliderHit data associated with this collision.</param>
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.TryGetComponent(out IDestroyable destroyable) && hit.gameObject.tag == "Box")
        {

            float maxAngle = 150f;
            // Obtener la normal del punto de contacto (dirección del golpe)
            Vector3 hitNormal = hit.normal;

            // Obtener la dirección hacia la que está mirando el objeto (su frente)
            Vector3 objectForward = robot.transform.forward;

            // Calcular el ángulo entre la dirección del golpe y el frente del objeto
            float angle = Vector3.Angle(hitNormal, objectForward);

            // Si el ángulo es menor o igual que el máximo permitido, destruir el objeto
            if (angle > maxAngle && dash.isDashing)
            {
                destroyable.Dstry();

            }
        }
        else if (hit.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {

            if (hit.gameObject.tag == "Button" && hit.gameObject.TryGetComponent(out IActivable activable))
            {
                activable.Activate();
                isGrounded = true;
                ballMode = false;
                velocity.y = 0;
            }
            /*
            // Obtener la normal del suelo en el punto de colisión
            Vector3 hitNormal = hit.normal;

            // Calcular la rotación alrededor del eje Z para alinear con la normal del suelo
            float angleZ = Mathf.Atan2(hitNormal.x, hitNormal.y) * Mathf.Rad2Deg;

            // Mantener las rotaciones actuales en los ejes X e Y
            float currentXRotation = robot.transform.localRotation.eulerAngles.z;
            float currentYRotation = robot.transform.localRotation.eulerAngles.y;

            // Crear la rotación solo alrededor del eje Z
            Quaternion targetRotation = Quaternion.Euler(angleZ, currentYRotation, currentXRotation);

            // Ajustar la rotación del robot para que se alinee con la normal del suelo
            robot.transform.localRotation = targetRotation;

            // Ajustar la posición del robot para que los pies toquen el suelo
            Vector3 offset = new Vector3(0, -controller.height / 2f, 0); // Asumiendo que el centro del CharacterController está en el medio del robot
            Vector3 adjustedPosition = hit.point + hitNormal * (controller.height / 2f - 0.1f); // Ajustar ligeramente por debajo del punto de contacto

            // Ajustar la posición del robot
            robot.transform.position = adjustedPosition + offset;*/
        }


    }




}
