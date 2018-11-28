using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum InteractingState
{
    None,
    Drinking,
    Activatin
}

public class P_InteractionController : P_Component
{
    public P_InteractionController(P_References playerReferences, P_PlayerController master) : base(playerReferences, master) { }

    private bool _selectingSomething = false;
    private IInteractable _selectedInteractable;
    private InteractingState _interactingState = InteractingState.None;
    private InteractableType _interactableType { get { return _selectedInteractable.InteractableType; } }

    private Transform _InteractCircle { get { return pRefs.InteractCircle; } }

    public InteractingState InteractingState { get { return _interactingState; } }

    public override void MainUpdate()
    {
        ComputeSelectedInteractable();
        UpdateInteractCircleTransform();
    }
    private void ComputeSelectedInteractable()
    {
        Collider[] colls = Physics.OverlapSphere(Position, ItData.DetectionDistance, WorldData.InteractableLayer);

        float distance = Mathf.Infinity;
        float distanceTemp;
        IInteractable currentInteractable = null;
        IInteractable currentInteractableTemp = null;

        for (int i = 0; i < colls.Length; i++)
        {
            currentInteractableTemp = colls[i].transform.GetComponentInParent<IInteractable>();
            if (currentInteractableTemp != null)
            {
                distanceTemp = (currentInteractableTemp.InteractPosition - Position).sqrMagnitude;
                if (distanceTemp < distance)
                {
                    currentInteractable = currentInteractableTemp;
                    distance = distanceTemp;
                }
            }
        }

        if (currentInteractable != null && currentInteractable != _selectedInteractable)
        {
            _selectedInteractable = currentInteractable;
            _selectingSomething = true;

            OnSelectInteractable(_selectedInteractable);
        }
        else if (currentInteractable == null && _selectedInteractable != null)
        {
            _selectedInteractable = null;
            _selectingSomething = false;

            OnDeselectInteractable();
        }
    }
    private void UpdateInteractCircleTransform()
    {
        if (_selectingSomething == true)
        {
            if (_InteractCircle.gameObject.activeSelf == false)
            {
                _InteractCircle.gameObject.SetActive(true);
            }

            _InteractCircle.position = _selectedInteractable.InteractPosition;
            _InteractCircle.LookAt(pRefs.CamTransform);
        }
        else if (_InteractCircle.gameObject.activeSelf == true)
        {
            _InteractCircle.gameObject.SetActive(false);
        }
        
    }

    //Utilities
    private void Interact()
    {
        _selectedInteractable.Interact();

        _selectedInteractable = null;
        _selectingSomething = false;
    }

    //Events
    private void OnSelectInteractable(IInteractable interactable)
    {

    }

    private void OnDeselectInteractable()
    {

    }
}