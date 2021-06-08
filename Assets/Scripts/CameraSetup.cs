using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraSetup : MonoBehaviour
{

    public Bird activeBird;

    private CinemachineTargetGroup cinemachineTargetGroup;
    private Monster[] monsterTargets;


    void Awake()
    {
        cinemachineTargetGroup = GetComponentInChildren<CinemachineTargetGroup>();
        monsterTargets = GameObject.FindObjectsOfType<Monster>();
        activeBird = GameObject.FindObjectOfType<Bird>();

        cinemachineTargetGroup.AddMember(activeBird.transform, 1.3f, 2);
        monsterTargets.ToList().ForEach(monster => cinemachineTargetGroup.AddMember(monster.transform, 1, 1));
    }

}
