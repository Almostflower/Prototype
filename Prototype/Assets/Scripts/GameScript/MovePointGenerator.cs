using UnityEngine;

public class MovePointGenerator : MonoBehaviour
{
    [SerializeField]
    private int _genereteNum = 30;

    // Start is called before the first frame update
    void Start()
    {
        var parent = new GameObject("MovePoints");
        for(int i = 0; i <= _genereteNum; i++)
        {
            var nextMovePoint = new GameObject();
            nextMovePoint.name = ("MovePoint(" + i + ")");
            nextMovePoint.tag = "MovePoint";
            nextMovePoint.layer = 9;
            var sp = nextMovePoint.AddComponent<SphereCollider>();
            sp.isTrigger = true;
            nextMovePoint.AddComponent<NextMovePosition>();
            var rb = nextMovePoint.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezePosition;
            nextMovePoint.transform.parent = parent.transform;
            nextMovePoint.transform.position = new Vector3(Random.Range(-38, 38), 0, Random.Range(-38, 38));
        }
    }
}
