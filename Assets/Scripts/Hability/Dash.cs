using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : Hability
{
    public ParticleSystem Fx;
    public float dashDuration;
    private float dashTime;
    private CharacterController characterController;
    public bool isDashing;
    [SerializeField] private float dashSpeed = 10f;
    private Health health;
    [SerializeField] private ParticleSystem dashFx;

    // El método Start será llamado automáticamente por Unity
    protected override void Start()  // Usar override para sobrescribir el método Start de la clase base
    {
        base.Start();  // Llama al Start de la clase base para ejecutar su lógica
        isDashing = false;
        health = GetComponent<Health>();



    }
    protected override void Update()  // Usar override para sobrescribir el método Start de la clase base
    {

        if (Input.GetKeyDown(KeyCode.D))
        {
            Use();
        }
        base.Update();  // Llama al Update de la clase base para ejecutar su lógica
        if (isDashing)
        {
            health.canTakeDamage = false;
            if (dashTime > 0)
            {
                dashTime -= Time.deltaTime;
                controller.TransformToBall();

            }
            else
            {
                isDashing = false;
                Fx.Stop();

                controller.TransformToRobot();
            }


        }
        else
        {
            health.canTakeDamage = true;
        }



    }

    public override void ApplyAugment(float augmentValue)
    {
        // Implementa cómo el dash debería mejorar con el aumento de nivel
        Debug.Log("Dash mejorado con valor de aumento: " + augmentValue);
    }

    public override void ExecuteHability()
    {
        // Implementa la lógica específica del dash
        Debug.Log("Dash ejecutado");

        // Si tienes un efecto de partículas, lo reproduces
        if (Fx != null)
        {
            Fx.gameObject.transform.position = controller.transform.position;
            Fx.transform.rotation = controller.transform.rotation;
            Fx.Play();
            if (dashFx != null)
            {
                dashFx.Play();
            }
        }
        isDashing = true;
        dashTime = dashDuration;
        controller.velocity.x += dashSpeed * Time.deltaTime * controller.GetLookDirection();
        controller.velocity.y = 2 * dashSpeed * Time.deltaTime;
        // Aquí podrías añadir lógica para mover al personaje rápidamente durante 'dashDuration'
        // Por ejemplo, podrías usar un Coroutine para manejar la duración del dash
    }
}
