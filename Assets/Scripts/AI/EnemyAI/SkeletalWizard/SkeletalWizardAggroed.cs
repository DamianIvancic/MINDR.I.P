using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalWizardAggroed : State<SkeletalWizard> {

    private static SkeletalWizardAggroed _instance;

    public static SkeletalWizardAggroed Instance
    {
        get
        {
            if (_instance == null)
            {
                new SkeletalWizardMove();
            }

            return _instance;
        }
    }

    public override void EnterState(SkeletalWizard owner)
    {
        throw new System.NotImplementedException();
    }

    public override void ExitState(SkeletalWizard owner)
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateAI(SkeletalWizard owner)
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateAnimator(SkeletalWizard owner)
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateMovement(SkeletalWizard owner)
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState(SkeletalWizard owner)
    {
        throw new System.NotImplementedException();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
