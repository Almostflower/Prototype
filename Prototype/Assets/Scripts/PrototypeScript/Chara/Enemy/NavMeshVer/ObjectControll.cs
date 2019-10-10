using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ObjectControll : MonoBehaviour
{
    [SerializeField]
    private Transform m_target = null;

    private NavMeshAgent m_navAgent = null;

    private void Awake()
    {
        m_navAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if (m_target != null)
        {
            m_navAgent.destination = m_target.position;
        }
    }
}
