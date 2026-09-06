using System.Collections.Generic;
using UnityEngine;

public class CullingManager : MonoBehaviour
{
    public static CullingManager instance { get; private set; }

    private List<GameObject> fragmentList = new List<GameObject>();

    [SerializeField]
    private float alphaThreshold = 0.05f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        for (int i = fragmentList.Count - 1; i >= 0; i--)
        {
            var root = fragmentList[i];
            if (root == null)
            {
                fragmentList.RemoveAt(i);
                continue;
            }

            if (IsAlphaBelowThreshold(root))
            {
                Destroy(root);
                fragmentList.RemoveAt(i);
            }
        }
    }

    public void AddFragment(GameObject fragment)
    {
        fragmentList.Add(fragment);
    }

    private bool IsAlphaBelowThreshold(GameObject root)
    {
        var renderers = root.GetComponentsInChildren<Renderer>();
        foreach (var r in renderers)
        {
            if (r.material.HasProperty("_Color"))
            {
                float alpha = r.material.color.a;


                if (alpha > alphaThreshold)
                    return false;
            }
        }

        return true;
    }
}
