using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIRangedAttackDetector : MonoBehaviour
{
    public UnityEvent<GameObject> OnPlayerDetected;
    [field: SerializeField]
    public bool PlayerDetected { get; private set; }
    public Vector2 DirectionToTarget => (target.transform.position - transform.position).normalized;

    [Header("OverlapBox parameters")]
    [SerializeField] Vector2 detectorSize = Vector2.one;
    [SerializeField] Vector2 detectorOriginOffset = Vector2.zero;
    [SerializeField] LayerMask detectorLayerMask;

    [Header("Cooldown parameters")]
    [SerializeField] float detectionCooldown = 1f;
    private float cooldownTimer;

    [Header("Gizmo parameters")]
    [SerializeField] Color gizmoIdleColor = new Color(0f, 1f, 0f, 0.2f); // Green
    [SerializeField] Color gizmoDetectedcolor = new Color(1f, 0f, 0f, 0.2f); // Red
    [SerializeField] bool showGizmos = true;

    GameObject target;

    public GameObject Target
    {
        get => target;
        private set
        {
            target = value;
            PlayerDetected = target != null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
        else
        {
            Collider2D collider = Physics2D.OverlapBox((Vector2)transform.position + detectorOriginOffset, detectorSize, 0, detectorLayerMask);
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
            Gizmos.color = gizmoIdleColor;
            if (PlayerDetected)
            {
                Gizmos.color = gizmoDetectedcolor;

            }
            Gizmos.DrawCube((Vector2)transform.position + detectorOriginOffset, detectorSize);
        }
    }
}
