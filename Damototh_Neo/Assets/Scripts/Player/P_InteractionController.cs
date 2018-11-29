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
    private EntityController _selectedEntity;
    private InteractingState _interactingState = InteractingState.None;

    private Coroutine _drinkCoroutine = null;

    private InteractableType _interactableType { get { return _selectedInteractable.InteractableType; } }
    private Transform _InteractCircle { get { return pRefs.InteractCircle; } }

    public bool SelectingSomething { get { return _selectingSomething; } }
    public EntityController SelectedEntity { get { return _selectedEntity; } }
    public IInteractable SelectedInteractable { get { return _selectedInteractable; } }
    public InteractingState InteractingState { get { return _interactingState; } }

    public override void MainUpdate()
    {
        ComputeSelectedInteractable();
        UpdateInteractCircleTransform();
        CheckInteract();
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
            if (currentInteractableTemp != null && currentInteractableTemp.CanBeInteracted == true)
            {
                if (GetInteractLookAngle(currentInteractableTemp) < ItData.InteractionAngle * 0.5f)
                {
                    distanceTemp = (currentInteractableTemp.InteractPosition - Position).sqrMagnitude;
                    if (distanceTemp < distance)
                    {
                        currentInteractable = currentInteractableTemp;
                        distance = distanceTemp;
                    }
                }
            }
        }

        if (currentInteractable != null && currentInteractable != _selectedInteractable)
        {
            _selectedInteractable = currentInteractable;
            _selectedEntity = _selectedInteractable as EntityController;
            _selectingSomething = true;

            OnSelectInteractable(_selectedInteractable);
        }
        else if (currentInteractable == null && _selectedInteractable != null)
        {
            _selectedInteractable = null;
            _selectedEntity = null;
            _selectingSomething = false;

            OnDeselectInteractable();
        }
    }
    private void UpdateInteractCircleTransform()
    {
        if (_selectingSomething == true && master.CanPerformActions == true)
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
    private void CheckInteract()
    {
        if (master.Interact == true && master.CanPerformActions == true && _selectingSomething == true)
        {
            Interact();
        }
    }

    //Coroutines
    private IEnumerator DrinkCoroutine()
    {
        _interactingState = InteractingState.Drinking;
        yield return new WaitForSeconds(master.IsInCombat ? ItData.InsideCombatDrinkTime : ItData.OutsideCombatDrinkTime);
        _interactingState = InteractingState.None;
    }

    //Utilities
    private void Interact()
    {
        master.OnInteract(_selectedInteractable);
        _selectedInteractable.Interact();

        switch (_selectedInteractable.InteractableType)
        {
            case InteractableType.Mechanism:
                break;
            case InteractableType.Corpse:

                if (_drinkCoroutine != null)
                {
                    master.StopCoroutine(_drinkCoroutine);
                }
                master.StartCoroutine(DrinkCoroutine());

                if (_selectedEntity != null)
                {
                    master.OnDrinkCorpse(_selectedEntity);
                }
                else
                {
                    master.OnDrinkThing((IDrinkable)_selectedInteractable);
                }
                break;
        }

        _selectedInteractable = null;
        _selectingSomething = false;
    }
    private float GetInteractLookAngle(IInteractable interactable)
    {
        Vector3 dir1 = (interactable.InteractPosition - Position).SetY(0);
        Vector3 dir2 = master.MovementController.LastMoveDirection;

        return Vector3.Angle(dir1, dir2);
    }

    //Events
    private void OnSelectInteractable(IInteractable interactable)
    {

    }

    private void OnDeselectInteractable()
    {

    }
}