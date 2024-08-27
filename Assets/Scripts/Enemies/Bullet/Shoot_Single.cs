using UnityEngine;

public class ShootOnFrameFive : StateMachineBehaviour
{
    private Warrior warrior;
    private bool hasShooted = false;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Obtén el componente Warrior desde el GameObject que tiene el Animator
        warrior = animator.GetComponent<Warrior>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (warrior != null)
        {
            if (stateInfo.normalizedTime >= 0.5f && !hasShooted)
            {
                warrior.Shoot();
                hasShooted = true;
            }

        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Reinicia hasShot cuando el estado sale
        hasShooted = false;
    }
}
