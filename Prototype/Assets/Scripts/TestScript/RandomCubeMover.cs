using UnityEngine;

public class RandomCubeMover : BaseMonoBehaviour
{
    float speed = 10.0f;
    Vector3 vel;
    
    protected override void Awake()
    {
        base.Awake();
    }

    public override void UpdateNormal()
    {
        vel = Random.onUnitSphere * speed;
        transform.Translate(vel * Time.deltaTime);
    }

}
