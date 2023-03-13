using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFunctionRedirect : MonoBehaviour
{
    
    void CallAttackEnd()
    {
        GetComponentInParent<PlayerMovement>().AttackEnd();

    }

    void CallBiteEnd()
    {
        GetComponentInParent<PlayerMovement>().BiteEnd();

    }

    public void CallClawSound1()

    {
        GetComponentInParent<PlayerMovement>().ClawSound1();
    }

    public void CallClawSound2()

    {
        GetComponentInParent<PlayerMovement>().ClawSound2();
    }
    public void CallClawSound3()

    {
        GetComponentInParent<PlayerMovement>().ClawSound3();
    }

   public void CallHeavyStepRStart()

    {
        GetComponentInParent<PlayerMovement>().HeavyStepRStart();
    }

    public void CallHeavyStepLStart()

    {
        GetComponentInParent<PlayerMovement>().HeavyStepLStart();
    }

    public void CallHeavyStepSoundR()

    {
        GetComponentInParent<PlayerMovement>().HeavyStepSoundR();
    }

    public void CallHeavyStepSoundL()

    {
        GetComponentInParent<PlayerMovement>().HeavyStepSoundL();
    }

    public void CallHeavyStepREnd()

    {
        GetComponentInParent<PlayerMovement>().HeavyStepREnd();
    }

    public void CallHeavyStepLEnd()

    {
        GetComponentInParent<PlayerMovement>().HeavyStepLEnd();
    }

    public void CallClawStartR()
    {
        GetComponentInParent<PlayerMovement>().ClawStart();

    }

    public void CallClawStartL()
    {
        GetComponentInParent<PlayerMovement>().ClawStart();

    }

    public void CallClawStart()
    {
        GetComponentInParent<PlayerMovement>().ClawStart();

    }


}
