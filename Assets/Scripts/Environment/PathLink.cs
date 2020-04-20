using System;
using UnityEngine;

[Serializable]
public class PathLink
{
    // PROPERTIES
    // -------------------------------------------------------------------------

    [SerializeField] private Types.PathTo pathTo;
    [SerializeField] private Types.MovementType movementType;
    [SerializeField] private Transform nextNode;

}
