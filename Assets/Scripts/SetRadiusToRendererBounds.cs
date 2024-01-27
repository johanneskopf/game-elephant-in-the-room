using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class SetRadiusToRendererBounds : MonoBehaviour
{
    public Renderer[] PlayerRenderer;
    public float RadiusOffset = 0.0f;

    void Start()
    {
        var targets = new List<CinemachineTargetGroup.Target>();
        for (int i = 0; i < PlayerRenderer.Length; i++)
        {
            // Calculate the center and radius of the renderer's bounding box to fit the target group.
            Vector3 center = PlayerRenderer[i].bounds.center;
            float radius = PlayerRenderer[i].bounds.extents.magnitude;

            // Assign the calculated center and radius to the Target Group.
            CinemachineTargetGroup.Target target = new CinemachineTargetGroup.Target
            {
                target = PlayerRenderer[i].transform,
                radius = radius + RadiusOffset,
                weight = 1.0f
            };
            targets.Add(target);
        }
        var targetGroup = GetComponent<CinemachineTargetGroup>();
        targetGroup.m_Targets = targets.ToArray();
    }
}
