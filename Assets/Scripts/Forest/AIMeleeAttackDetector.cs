using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class AIMeleeAttackDetector : MonoBehaviour
{
    [SerializeField] LayerMask targetLayer;
    public UnityEvent<GameObject> OnPlayerDetected;

    [Header("OverlapCircle parameters")]
    [Range(1f, 10f)]
    [SerializeField] float radius;
    [SerializeField] Vector2 detectorOriginOffset = Vector2.zero;

    [Header("Cooldown parameters")]
    [SerializeField] float detectionCooldown = 2f;
    private float cooldownTimer;

    [Header("Gizmo parameters")]
    [SerializeField] Color gizmoColor = new Color(1f, 1f, 0f, 0.2f); // Purple
    [SerializeField] bool showGizmos = true;

    [field: SerializeField]
    public bool PlayerDetected { get; private set; }

    // Update is called once per frame
    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
        else
        {
            var collider = Physics2D.OverlapCircle((Vector2)transform.position + detectorOriginOffset, radius, targetLayer);
            PlayerDetected = collider != null;
            if (PlayerDetected)
            {
                OnPlayerDetected?.Invoke(collider.gameObject);
                cooldownTimer = detectionCooldown;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (showGizmos)
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawSphere((Vector2)transform.position + detectorOriginOffset, radius);
        }
    }
}
