using Unity.VisualScripting;
using UnityEngine;

public class Player_Combat : Entity_Combat
{
    [Header("Counter attack details")]
    [SerializeField] private float counterRecovvery;
    public bool CounterAttackPergormed()
    {

        bool hasPerformedCounter = false;
        foreach (var target in GetDetectedColliders())
        {
            ICounterable counterable = target.GetComponent<ICounterable>();

            if(counterable == null) 
            {
                continue;
            }
            if(counterable.CanBeCountered)
            {

                counterable.HandleCounter();
                hasPerformedCounter = true;
            }
        }

        return hasPerformedCounter;
    }

    public float GetCounterRecoveryDuration() => counterRecovvery;
}
