using NavMeshPlus.Components;
using UnityEngine;

public class BakeNavMesh : MonoBehaviour
{
    [SerializeField] private float delayTime;
    [SerializeField] private float time;
    [SerializeField] private NavMeshSurface navMeshSurface;

    private void LateUpdate()
    {
        if(time >= delayTime)
        {
            BuildNavMesh();
            Destroy(this);
        }
        else
        {
            time += Time.deltaTime;
        }
    }

    public void BuildNavMesh()
    {
        navMeshSurface.BuildNavMesh();
    }
}
