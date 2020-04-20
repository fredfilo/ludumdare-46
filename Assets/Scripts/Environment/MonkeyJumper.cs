using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyJumper : MonoBehaviour
{
    // PROPERTIES
    // -------------------------------------------------------------------------

    [SerializeField] private Types.PathTo m_pathTo;
    
    // ACCESSORS
    // -------------------------------------------------------------------------

    public Types.PathTo pathTo => m_pathTo;
}
