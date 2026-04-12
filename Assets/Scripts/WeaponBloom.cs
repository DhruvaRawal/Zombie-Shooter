using UnityEngine;

public class WeaponBloom : MonoBehaviour
{
    [SerializeField] float defaultBloomAngle = 3;
    [SerializeField] float walkBloomMultiplier = 1.5f;
    [SerializeField] float crouchBloomMultiplier = 0.5f;
    [SerializeField] float sprintBloomMultiplier = 2f;
    [SerializeField] float adsBloomMultiplier = 0.5f;

    MovementStateManager movement;
    AimStateManager aiming;

    float currentBloom;

    void Start()
    {
        movement = GetComponentInParent<MovementStateManager>();
        aiming = GetComponentInParent<AimStateManager>();
    }

    public Vector3 BloomAnngle(Transform barrelPos)
    {
        if (movement.currentstate == movement.idle) currentBloom = defaultBloomAngle;
        else if (movement.currentstate == movement.walk) currentBloom = defaultBloomAngle * walkBloomMultiplier;
        else if (movement.currentstate == movement.run) currentBloom = defaultBloomAngle * sprintBloomMultiplier;
        else if (movement.currentstate == movement.crouch)
        {
            if (movement.dir.magnitude == 0) currentBloom = defaultBloomAngle * crouchBloomMultiplier;
            else currentBloom = defaultBloomAngle * crouchBloomMultiplier * walkBloomMultiplier;
        }

        if (aiming.CurrentState == aiming.Aim) currentBloom *= adsBloomMultiplier;

        float randX = Random.Range(-currentBloom, currentBloom);
        float randY = Random.Range(-currentBloom, currentBloom);
        float randZ = Random.Range(-currentBloom, currentBloom);

        Vector3 randomRotation = new Vector3(randX, randY, randZ);
        return barrelPos.localEulerAngles + randomRotation;
    }
}
