using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChange : MonoBehaviour
{
    private Renderer r;
    [SerializeField, ColorUsage(false, true)]
    private Color badcolor;
    [SerializeField, ColorUsage(false, true)]
    private Color normalcolor;
    [SerializeField, ColorUsage(false, true)]
    private Color goodcolor;

    [SerializeField]
    private bool OriginalMatCheckFlag;
    [SerializeField]
    private GameObject badparticle;
    [SerializeField]
    private GameObject normalparticle;
    [SerializeField]
    private GameObject goodparticle;

    private bool badflag, normalflag, goodflag;
    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Renderer>(); //Rendererコンポーネントを取得（Material取得のため）
        r.material.EnableKeyword("_EMISSION");
        badflag = true;
        normalflag = true;
        goodflag = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameStatusManager.Instance.GetGameState() == GameStatusManager.GameState.BAD)
        {
            if(badflag)
            {
                badflag = false;
                normalflag = true;

                if(badparticle != null)
                {
                    GameObject ps;
                    ps = Instantiate(badparticle, transform.position, Quaternion.AngleAxis(-90f,new Vector3(1.0f,0.0f,0.0f)));
                    Destroy(ps, 6.0f);
                }
            }
            if(OriginalMatCheckFlag)
            {
                r.material.SetColor("_Color", badcolor);
            }
            else
            {
                r.material.SetColor("_EmissionColor", badcolor);
            }
            
        }
        if (GameStatusManager.Instance.GetGameState() == GameStatusManager.GameState.NORMAL)
        {
            if(normalflag)
            {
                badflag = true;
                normalflag = false;
                goodflag = true;

                if(normalparticle != null)
                {
                    GameObject ps;
                    ps = Instantiate(normalparticle, transform.position, Quaternion.AngleAxis(-90f, new Vector3(1.0f, 0.0f, 0.0f)));
                    Destroy(ps, 6.0f);
                }
            }
            if (OriginalMatCheckFlag)
            {
                r.material.SetColor("_Color", normalcolor);
            }
            else
            {
                r.material.SetColor("_EmissionColor", normalcolor);
            }
        }
        if (GameStatusManager.Instance.GetGameState() == GameStatusManager.GameState.GOOD)
        {
            if(goodflag)
            {
                normalflag = true;
                goodflag = false;

                if(goodparticle != null)
                {
                    GameObject ps;
                    ps = Instantiate(goodparticle, transform.position, Quaternion.AngleAxis(-90f, new Vector3(1.0f, 0.0f, 0.0f)));
                    Destroy(ps, 6.0f);
                }
            }
            if (OriginalMatCheckFlag)
            {
                r.material.SetColor("_Color", goodcolor);
            }
            else
            {
                r.material.SetColor("_EmissionColor", goodcolor);
            }
        }
    }
}
