using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(TankMovement))]
[RequireComponent(typeof(TankShooting))]
public class TankAnimation : MonoBehaviour
{
    Animator tankAnimator;
    Animator cannonAnimator;
    TankMovement tankMovement;
    TankShooting tankShooting;

   void Awake()
   {
       tankAnimator = GetComponent<Animator>();
       cannonAnimator = transform.GetChild(0).GetComponent<Animator>();
       tankMovement = GetComponent<TankMovement>();
       tankShooting = GetComponent<TankShooting>();

       tankShooting.OnFireStart.AddListener(PlayCannonFire);
   }

   void Update()
   {    
        tankAnimator.SetBool("Moving", tankMovement.IsMoving);
   }

    void PlayCannonFire()
    {
        cannonAnimator.SetTrigger("Has Fired");
    }

    public void PlayReceiveFire()
    {
        tankAnimator.SetTrigger("Has Received Fire");
    }

    public void DisableMovementAnimation()
    {
        tankMovement.IsMoving = false;
        tankAnimator.SetBool("Moving", false);
    }
}