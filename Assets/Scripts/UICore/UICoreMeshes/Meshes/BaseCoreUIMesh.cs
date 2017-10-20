﻿using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.UICore.StylesSystem.Styles;
using UnityEngine;

namespace Assets.Scripts.UICore.UICoreMeshes.Meshes
{
   public abstract class BaseCoreUIMesh
    {
        private Mesh _mesh;
        protected Rect _rect;
        private List<Vector3> _vertices;
        private List<Vector2> _uv;
        private List<int> _triangles;
        private List<Vector3> _normals; 

        public Mesh Mesh { get { return _mesh; } }

        public List<Vector3> Vertices
        {
            get { return _vertices; }
            set { _vertices = value; }
        }

        public List<Vector2> Uv
        {
            get { return _uv; }
            set { _uv = value; }
        }

        public List<int> Triangles
        {
            get { return _triangles; }
            set { _triangles = value; }
        }

        public float X
        {
            get { return _rect.x; }
             set { _rect.y = value; }
        }

        public float Y
        {
            get { return _rect.y; }
             set { _rect.y = value; }
        }

        public float Width
        {
            get { return _rect.width; }
            set { _rect.width = value; }
        }

        public float Height
        {
            get { return _rect.height; }
            set { _rect.height = value; }
        }

        protected BaseCoreUIMesh()
        {
            _mesh = new Mesh();
            _vertices = new List<Vector3>();
            _uv = new List<Vector2>();
            _triangles = new List<int>();
            _normals = new List<Vector3>();
        }

        public void Init(BaseStyle style, Rect rect)
        {
            _rect = rect;
            Generate(style);
        }

        protected abstract void Generate(BaseStyle style);

        protected void PushUV(float uvx, float uvy)
        {
            PushUV(new Vector2(uvx, uvy));    
        }

        protected void PushUV(Vector2 uv)
        {
            _uv.Add(uv);    
        }

        protected void PushVertice(float x, float y)
        {
            PushVertice(new Vector3(x, y));
        }

        protected void PushVertice(Vector3 vertice)
        {
            _vertices.Add(vertice);    
        }

        protected void PushVertice(float x, float y, float uvx, float uvy)
        {
            PushVertice(new Vector3(x, y), new Vector2(uvx, uvy));
        }

        protected void PushVertice(Vector3 vertice, Vector2 uv)
        {
            _vertices.Add(vertice);
            _uv.Add(uv);
            //_normals.Add(Vector3.forward);
        }

        protected void UpdateMeshInfo()
        {
            _mesh.SetVertices(_vertices);
            _mesh.SetUVs(0, _uv);
            _mesh.SetTriangles(_triangles, 0);
            //_mesh.SetNormals(_normals);
        }
    }
}
