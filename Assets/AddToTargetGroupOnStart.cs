using Cinemachine;
using System.Linq;
using UnityEngine;

public class AddToTargetGroupOnStart : MonoBehaviour
{
    public CinemachineTargetGroup targetGroup;

    void Start()
    {
        AddSelfToTargetGroup();
    }

    void AddSelfToTargetGroup()
    {
        var targets = targetGroup.m_Targets.ToList();   
        var target = new CinemachineTargetGroup.Target
        {
            target = transform,
            radius = 3.0f,
            weight = 1.0f
        };
        targets.Add(target);
        targetGroup.m_Targets = targets.ToArray();
    }
}
