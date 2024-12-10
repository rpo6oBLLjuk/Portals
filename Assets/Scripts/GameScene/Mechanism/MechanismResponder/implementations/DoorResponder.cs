using CustomInspector;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DoorResponder : MechanismResponderPrototype
{
    [SerializeField] private List<Door> doors;


    public override void Activated()
    {
        foreach(Door door in doors)
        {
            door.OpenDoor();
        }
    }

    public override void Deactivated()
    {
        foreach (Door door in doors)
        {
            door.CloseDoor();
        }
    }
}

[Serializable]
public class Door
{
    [SerializeField] private Transform door;
    [SerializeField] private float durationTime;

    [HorizontalGroup(true)] public Vector3 openPosition;
    [HorizontalGroup] public Vector3 closePosition;

    [HorizontalGroup(true)] public Vector3 openRotation;
    [HorizontalGroup] public Vector3 closeRotation;


    public void OpenDoor()
    {
        door.DOLocalMove(openPosition, durationTime);
        door.DOLocalRotate(openRotation, durationTime);
    }

    public void CloseDoor()
    {
        door.DOLocalMove(closePosition, durationTime);
        door.DOLocalRotate(closeRotation, durationTime);
    }
}
