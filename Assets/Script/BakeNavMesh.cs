using NavMeshPlus.Components;
using UnityEngine;

public class BakeNavMesh : MonoBehaviour
{
    [SerializeField] private NavMeshSurface navMeshSurface;
    [SerializeField] private bool checker = false;

    private void LateUpdate()
    {
        if (checker)
        BuildNavMesh();
    }

    public void BuildNavMesh()
    {
        navMeshSurface.BuildNavMesh();
    }
}
