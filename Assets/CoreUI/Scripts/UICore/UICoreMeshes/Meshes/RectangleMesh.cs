﻿using System.Collections.Generic;
using UICore.StylesSystem.Styles;
using UnityEngine;

namespace UICore.UICoreMeshes.Meshes
{
    public class RectangleMesh : BaseCoreUIMesh
    {
        public override Texture2D Texture
        {
            get { return base.Texture; }
            set
            {
                base.Texture = value;
                if (value == null) return;
                Resize(Texture.width * _pixelWidth, Texture.height * _pixelWidth);
            }
        }

        protected override void Generate(BaseStyle style)
        {
            GenerateMesh();
        }

        private void GenerateMesh()
        {
            GenerateVertices();
            GenerateUV();   

            Triangles = new List<int>()
            {
                0, 1, 2,
                2, 1, 3,
            };
        }

        private void GenerateVertices()
        {
            PushVertice(0, 0);
            PushVertice(Width, 0);
            PushVertice(0, -Height);
            PushVertice(Width, -Height);
        }

        private void GenerateUV()
        {
            PushUV(0, 1);
            PushUV(1, 1);
            PushUV(0, 0);
            PushUV(1, 0);
        }

        protected override void ApplySize()
        {
            Clear();
            GenerateVertices();
            UpdatePositions();
        }
    }
}
