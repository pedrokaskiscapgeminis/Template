using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationList
{
    None,
    Clapping,
    Waving,
    Capoeira,
    Salute,
    Defeated,
    TwistedDance
}
public class EventWheel : NetworkTransform
{
    public Animator animator;

    AnimationList animationToPlay = AnimationList.None;
    
    public void setAnimation1()
    {
        animationToPlay = AnimationList.Clapping;
        Debug.Log(animationToPlay);
    }

    public void setAnimation2()
    {
        animationToPlay = AnimationList.Waving;
        Debug.Log(animationToPlay);
    }

    public void setAnimation3()
    {
        animationToPlay = AnimationList.Capoeira;
        Debug.Log(animationToPlay);
    }

    public void setAnimation4()
    {
        animationToPlay = AnimationList.Salute;
        Debug.Log(animationToPlay);
    }

    public void setAnimation5()
    {
        animationToPlay = AnimationList.Defeated;
        Debug.Log(animationToPlay);
    }

    public void setAnimation6()
    {
        animationToPlay = AnimationList.TwistedDance;
        Debug.Log(animationToPlay);
    }


    public override void Render()
    {
        if (animationToPlay != AnimationList.None) { 
            if (animator == null) { animator = this.gameObject.transform.parent.GetComponentInChildren<Animator>(); };
            animator.SetInteger("AnimationWheel",(int)animationToPlay);

            //Set the value to zero to end animation
            animationToPlay = AnimationList.None;
        }
        else
        {
            if (animator == null) { animator = this.gameObject.transform.parent.GetComponentInChildren<Animator>(); };
            animator.SetInteger("AnimationWheel", (int)animationToPlay);
        }
    }
}
