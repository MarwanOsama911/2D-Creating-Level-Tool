using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

namespace Platformer
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private int totalRows = 10;
        [SerializeField] private int totalColumns = 25;

        public const int GRID_SIZE = 1;

        private readonly Color normalColor = Color.gray;
        private readonly Color selectColor = Color.yellow;

        public int TotalRows
        {
            get => totalRows;
            set => totalRows = value;
        }

        public int TotalColumns
        {
            get => totalColumns;
            set => totalColumns = value;
        }

        private void GridFrameGizmos(int cols, int rows)
        {
            Gizmos.DrawLine(Vector3.zero, new Vector3(0f, rows * GRID_SIZE, 0f));
            Gizmos.DrawLine(Vector3.zero, new Vector3(cols * GRID_SIZE, 0f, 0f));
            Gizmos.DrawLine(new Vector3(cols * GRID_SIZE, 0f, 0f),
                new Vector3(cols * GRID_SIZE, rows * GRID_SIZE, 0f));
            Gizmos.DrawLine(new Vector3(0, rows * GRID_SIZE, 0f),
                new Vector3(cols * GRID_SIZE, rows * GRID_SIZE, 0f));
        }

        private void GridGizmo(int cols, int rows)
        {
            for (int i = 1; i < cols; i++)
                Gizmos.DrawLine(new Vector3(i * GRID_SIZE, 0f, 0f),
                    new Vector3(i * GRID_SIZE, rows * GRID_SIZE, 0f));

            for (int j = 1; j < rows; j++)
                Gizmos.DrawLine(new Vector3(0f, j * GRID_SIZE, 0f),
                    new Vector3(cols * GRID_SIZE, j * GRID_SIZE, 0f));
        }

        private void OnDrawGizmos()
        {
            var oldColor = Gizmos.color;
            Matrix4x4 oldMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;


            Gizmos.color = normalColor;
            GridGizmo(TotalColumns, TotalRows);
            GridFrameGizmos(TotalColumns, TotalRows);

            Gizmos.color = oldColor;
            Gizmos.matrix = oldMatrix;
        }

        private void OnDrawGizmosSelected()
        {
            var oldColor = Gizmos.color;
            Matrix4x4 oldMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.color = selectColor;
            GridFrameGizmos(TotalColumns, TotalRows);

            Gizmos.color = oldColor;
            Gizmos.matrix = oldMatrix;
        }
    }
}