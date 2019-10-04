using UnityEngine;

public struct FPS
{
    public float updateinterval;
    public float m_accum;
    public float m_frames;
    public float m_timeleft;
    public float m_fps;
}

public class UpdateManager : MonoBehaviour
{
    private FPS debugfps;

    private int behaviourcount_;
    private BaseMonoBehaviour[] behaviours;
    private static UpdateManager instance;

    public UpdateManager()
    {
        instance = this;
        debugfps.updateinterval = 0.5f;
    }

    private void Update()
    {
        for(int i = 0; i < ColliderDebuger.Instance.GetMaxColliderDraw(); i++)
        {
            ColliderDebuger.Instance.ColliderDrawStart(i);
        }

        if(behaviourcount_ > 0)
        {
            for(var i = 0; i < behaviours.Length; i++)
            {
                if(behaviours[i] == null || !behaviours[i].isActiveAndEnabled)
                {
                    continue;
                }
                behaviours[i].UpdateNormal();
            }
        }
    }

    private void FixedUpdate()
    {
        if(behaviourcount_ > 0)
        {
            for(var i = 0; i < behaviours.Length; i++)
            {
                if (behaviours[i] == null || !behaviours[i].isActiveAndEnabled)
                {
                    continue;
                }

                behaviours[i].UpdateFixed();
            }
        }
    }

    private void LateUpdate()
    {
        if(behaviourcount_ > 0)
        {
            for(var i = 0; i < behaviours.Length; i++)
            {
                if(behaviours[i] == null || !behaviours[i].isActiveAndEnabled)
                {
                    continue;
                }

                behaviours[i].UpdateLate();
            }
        }
    }
    public static void AddBehaviour(BaseMonoBehaviour behaviour)
    {
        if(instance.behaviours == null)
        {
            instance.behaviours = new BaseMonoBehaviour[1];
        }
        else
        {
            System.Array.Resize(ref instance.behaviours, instance.behaviours.Length + 1);
        }

        instance.behaviours[instance.behaviours.Length - 1] = behaviour;
        instance.behaviourcount_ = instance.behaviours.Length;
    }

    public static void RemoveBehaviour(BaseMonoBehaviour behaviour)
    {
        int addAtIndex = 0;
        BaseMonoBehaviour[] tempBehavioursArray = new BaseMonoBehaviour[instance.behaviours.Length - 1];

        for(int i = 0; i < instance.behaviours.Length; i++)
        {
            if(instance.behaviours[i] == null)
            {
                continue;
            }
            else if(instance.behaviours[i] == behaviour)
            {
                continue;
            }

            tempBehavioursArray[addAtIndex] = instance.behaviours[i];
            addAtIndex++;
        }

        instance.behaviours = new BaseMonoBehaviour[tempBehavioursArray.Length];

        for(int i = 0; i < tempBehavioursArray.Length; i++)
        {
            instance.behaviours[i] = tempBehavioursArray[i];
        }

        instance.behaviourcount_ = instance.behaviours.Length;
    }

    private void OnGUI()
    {
        GUILayout.Label("FPS :" + debugfps.m_fps.ToString("f2"));
    }
}
