using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    // PROPERTIES
    // -------------------------------------------------------------------------

    [SerializeField] private Types.PathNodeType m_nodeType;
    [SerializeField] private List<PathLink> m_links;
}
