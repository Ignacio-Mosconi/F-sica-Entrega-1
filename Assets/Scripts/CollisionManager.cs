using System;
using System.Collections.Generic;
using UnityEngine;

struct LayerKey
{
    public LayerMask layer;
    public int index;

    public LayerKey(LayerMask l, int i)
    {
        layer = l;
        index = i;
    }
}

public class CollisionManager : MonoBehaviour
{
    static CollisionManager instance;

    [SerializeField] LayerMask[] collisionLayers;

    Dictionary<LayerKey, List<CustomCollider2D>> colliders = new Dictionary<LayerKey, List<CustomCollider2D>>();

    void Awake()
    {
        if (Instance != this)
            Destroy(gameObject);
    }

    void Update()
    {
        foreach (LayerKey layerA in colliders.Keys)
        {
            foreach (LayerKey layerB in colliders.Keys)
            {
                if (layerB.index >= layerA.index)
                {
                    foreach (CustomBoxCollider2D boxA in colliders[layerA])
                    {
                        Vector2 posA = boxA.transform.position;

                        foreach (CustomBoxCollider2D boxB in colliders[layerB])
                        {
                            if (boxA != boxB)
                            {
                                Vector2 posB = boxB.transform.position;
                                Vector2 diff = posB - posA;

                                float minDistX = (boxA.BoundingBoxWidth + boxB.BoundingBoxWidth) * 0.5f;
                                float minDistY = (boxA.BoundingBoxHeight + boxB.BoundingBoxHeight) * 0.5f;

                                float deltaX = Mathf.Abs(diff.x);
                                float deltaY = Mathf.Abs(diff.y);

                                if (deltaX < minDistX && deltaY < minDistY)
                                {
                                    boxA.OnCollision.Invoke();
                                    boxB.OnCollision.Invoke();
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void RegisterCollider2D(LayerMask layer, CustomCollider2D collider)
    {
        LayerKey[] keys = new LayerKey[colliders.Keys.Count];
        colliders.Keys.CopyTo(keys, 0);

        LayerKey layerKey = Array.Find(keys, k => k.layer == layer);

        if (!colliders.ContainsKey(layerKey))
        {
            int nextIndex = keys.GetLength(0);
            layerKey = new LayerKey(layer, nextIndex);
            List<CustomCollider2D> colliderList = new List<CustomCollider2D>();
            
            colliders.Add(layerKey, colliderList);
        }

        colliders[layerKey].Add(collider);
    }

    public static CollisionManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<CollisionManager>();
                if (!instance)
                {
                    GameObject gameObj = new GameObject("CollisionManager");
                    instance = gameObj.AddComponent<CollisionManager>();
                }
            }

            return instance;
        }
    }
}