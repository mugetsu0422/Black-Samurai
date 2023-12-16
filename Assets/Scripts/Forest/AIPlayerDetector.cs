using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayerDetector : MonoBehaviour
{
    [field: SerializeField]
    public bool PlayerDetected { get; private set; }
    public Vector2 DirectionToTarget => (target.transform.position - transform.position).normalized;

    [Header("OverlapBox parameters")]
    [SerializeField] Vector2 detectorSize = Vector2.one;
    [SerializeField] Vector2 detectorOriginOffset = Vector2.zero;

    [SerializeField] float detectionDelay = 0.3f;
    [SerializeField] LayerMask detectorLayerMask;

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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DetectionCoroutine());
    }

    IEnumerator DetectionCoroutine()
    {
        yield return new WaitForSeconds(detectionDelay);
        PerformDetection();
        StartCoroutine(DetectionCoroutine());
    }

    public void PerformDetection()
    {
        Collider2D collider = Physics2D.OverlapBox((Vector2)transform.position + detectorOriginOffset, detectorSize, 0, detectorLayerMask);
        if (collider)
        {
            Target = collider.gameObject;
        }
        else
        {
            Target = null;
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
