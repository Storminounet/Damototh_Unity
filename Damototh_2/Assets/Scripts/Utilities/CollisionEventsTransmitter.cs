using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollisionEventsTransmitter : MonoBehaviour 
{
    [SerializeField] private int _parentNumber = 1;
    private void OnCollisionEnter(Collision collision)
    {
        GetParent().SendMessage("OnCollisionEnter", collision, SendMessageOptions.DontRequireReceiver);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GetParent().SendMessage("OnCollisionEnter2D", collision, SendMessageOptions.DontRequireReceiver);

    }
    private void OnCollisionExit(Collision collision)
    {
        GetParent().SendMessage("OnCollisionExit", collision, SendMessageOptions.DontRequireReceiver);

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        GetParent().SendMessage("OnCollisionExit2D", collision, SendMessageOptions.DontRequireReceiver);

    }
    private void OnCollisionStay(Collision collision)
    {
        GetParent().SendMessage("OnCollisionStay", collision, SendMessageOptions.DontRequireReceiver);

    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        GetParent().SendMessage("OnCollisionStay2D", collision, SendMessageOptions.DontRequireReceiver);

    }
    private void OnTriggerEnter(Collider other)
    {
        GetParent().SendMessage("OnTriggerEnter", other, SendMessageOptions.DontRequireReceiver);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetParent().SendMessage("OnTriggerEnter2D", collision, SendMessageOptions.DontRequireReceiver);
    }
    private void OnTriggerExit(Collider other)
    {
        GetParent().SendMessage("OnTriggerExit", other, SendMessageOptions.DontRequireReceiver);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        GetParent().SendMessage("OnTriggerExit2D", collision, SendMessageOptions.DontRequireReceiver);

    }
    private void OnTriggerStay(Collider other)
    {
        GetParent().SendMessage("OnTriggerStay", other, SendMessageOptions.DontRequireReceiver);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        GetParent().SendMessage("OnTriggerStay2D", collision, SendMessageOptions.DontRequireReceiver);
    }

    private Transform GetParent()
    {
        Transform t = transform.parent;
        for (int i = 1; i < _parentNumber; i++)
        {
            t = t.parent;
        }

        return t;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_parentNumber < 1)
        {
            _parentNumber = 1;
        }
    }
#endif
}
