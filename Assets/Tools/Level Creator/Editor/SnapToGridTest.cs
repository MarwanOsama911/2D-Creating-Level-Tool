using Tools.LevelCreator.Runtime;
using UnityEngine;

namespace Tools.LevelCreator.Editor
{
    [ExecuteInEditMode]
    public class SnapToGridTest : MonoBehaviour
    {
        void Update()
        {
            var gridCoordinate = Level.Instance.WorldToGridCoordinates(transform.position);
            transform.position = Level.Instance.GridToWorldCoordinates((int) gridCoordinate.x, (int) gridCoordinate.y);
        }

        private void OnDrawGizmos()
        {
            var oldColor = Gizmos.color;
            Gizmos.color = (Level.Instance.IsInsideGridBounds(transform.position)) ? Color.green : Color.red;
            Gizmos.DrawCube(transform.position,Vector3.one*Level.GRID_SIZE);
            Gizmos.color = oldColor;
        }
    }
}