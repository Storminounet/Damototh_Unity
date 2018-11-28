using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringArm : MonoBehaviour 
{
    private Vector3 _offset;

    private P_CameraData _cData;

    protected void Awake ()
    {
        _offset = transform.localPosition;
        _cData = GetComponentInParent<P_References>().CameraData;
	}
	
	protected void Update ()
    {
        RayHandler();
	}

    RaycastHit hit;
    Ray ray;
    void RayHandler()
    {
        hit = new RaycastHit();
        ray = new Ray(transform.parent.position, transform.parent.TransformDirection(_offset.normalized));
        
        Physics.Raycast(ray, out hit, _offset.magnitude, WorldData.DefaultSolidLayer);

        if (hit.collider != null)
        {
            Vector3 warpedPoint = hit.point
            - ray.direction * _cData.DirectionDisplacementfactor
            + hit.normal * _cData.NormalDisplacementFactor;

            float parentToPointDist = (transform.parent.position - warpedPoint).magnitude;
            float parentToTransformDist = (transform.localPosition).magnitude;

            if (parentToPointDist < parentToTransformDist)
            {
                transform.position = warpedPoint;
            }
            else
            {
                Vector3 newLocalPos = Vector3.MoveTowards(transform.localPosition, _offset, _cData.ReturnToPosSpeed * WorldData.DeltaTime);

                if (newLocalPos.magnitude < parentToPointDist)
                {
                    transform.localPosition = newLocalPos;
                }
            }

        }
        else
        {
            GoBackToOffset();
        }
    }

    private void GoBackToOffset()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, _offset, _cData.ReturnToPosSpeed * WorldData.DeltaTime);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.parent.position, transform.position);

        Gizmos.color = Color.red;
        if (hit.collider != null)
            Gizmos.DrawWireSphere(hit.point, 0.1f);
        Gizmos.DrawLine(transform.parent.position, transform.parent.position + transform.parent.TransformDirection(_offset.normalized) * _offset.magnitude);
    }
#endif

}