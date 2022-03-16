using UnityEngine;
using UnityEditor;

public class DrawGizmoSample
{
    [DrawGizmo(GizmoType.NotInSelectionHierarchy |
               GizmoType.InSelectionHierarchy |
               GizmoType.Selected |
               GizmoType.Active |
               GizmoType.Pickable)]
    private static void CustomOnDrawGizmos(TargetExample target, GizmoType gizmoType)
    {
        Gizmos.color = Color.white;
        Gizmos.DrawCube(target.transform.position, Vector3.one);
    }

    [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.Active)]
    private static void OnDrawGizmosSelected(TargetExample target, GizmoType gizmoType)
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(target.transform.position, Vector3.one);
    }
    
}