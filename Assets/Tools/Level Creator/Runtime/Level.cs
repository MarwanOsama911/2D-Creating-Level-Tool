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
        [SerializeField] private GameObject[] levelPieces;

        public const int GRID_SIZE = 1;

        private readonly Color normalColor = Color.gray;
        private readonly Color selectColor = Color.yellow;


        #region Singletone

        private static Level instance;

        public static Level Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<Level>();
                }

                return instance;
            }
        }

        #endregion

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

        public GameObject[] LevelPieces
        {
            get => levelPieces;
            set => levelPieces = value;
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

        public Vector3 WorldToGridCoordinates(Vector3 point)
        {
            var gridPoint = new Vector3((int)((point.x - transform.position.x) / GRID_SIZE),
                (int)((point.y - transform.position.y) / GRID_SIZE), 0f);
            return gridPoint;
        }

        public Vector3 GridToWorldCoordinates(int cols, int rows)
        {
            var worldPoint = new Vector3(transform.position.x + (cols * GRID_SIZE + GRID_SIZE / 2f),
                transform.position.y + (rows * GRID_SIZE + GRID_SIZE / 2f), 0f);
            return worldPoint;
        }

        public bool IsInsideGridBounds(Vector3 point)
        {
            var minX = transform.position.x;
            var maxX = minX + TotalColumns * GRID_SIZE;
            var minY = transform.position.y;
            var maxY = minY + TotalRows * GRID_SIZE;

            return (point.x >= minX && point.x <= maxX && point.y >= minY && point.y <= maxY);
        }

        public bool IsInsideGridBounds(int col, int row)
        {
            return (col >= 0 && col <= TotalColumns - 1 && row >= 0 && row <= TotalRows - 1);
        }
    }
}