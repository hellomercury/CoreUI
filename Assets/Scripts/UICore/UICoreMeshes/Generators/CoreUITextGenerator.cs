﻿using System;
using System.Collections.Generic;
using Assets.Scripts.UICore.StylesSystem.Styles.Font;
using UnityEngine;

namespace Assets.Scripts.UICore.UICoreMeshes.Generators
{
    [Serializable]
    public class CoreUITextGenerator : IDisposable
    {
        [SerializeField] private float _horizontalOffset;
        [SerializeField] private float _verticalOffset;
        [SerializeField] private CoreUIFont _font;
        [SerializeField] private Vector3[] _vertices;
        [SerializeField] private int[] _triangles;
        [SerializeField] private Vector2[] _uv;
        [SerializeField] private Color[] _colors;
        [SerializeField] private string _text;
        [SerializeField] private Color _color;
        private IDictionary<char, Action> _symbolHandlers;

        public Vector3[] Vertices { get { return _vertices; } }
        public int[] Triangles { get { return _triangles; } }
        public Vector2[] UV { get { return _uv; } }
        public Color Color { get { return _color; } }
        public Color[] Colors { get { return _colors; } }
        public string Text { get { return _text; } }
        public bool Inited { get { return _font != null && _symbolHandlers != null; } }

        public void Init(CoreUIFont font)
        {
            _text = string.Empty;
            _font = font;
            ResetOffsets();
            InitHandlers();
        }

        public void Update()
        {
            
        }

        public void GenerateMeshData(string text, Color color)
        {
            if (_text.Equals(text)) return;
            _text = text;
            _color = color;
            ResetMeshData(text);
            GenerateVertices(text);
        }

        public void UpdateColors(Color color)
        {
            if (_color == color) return;
            _color = color;
            for (var index = 0; index < _text.Length; index++)
                if (!CheckSymbolForHandler(_text[index])) GenerateColors(index*4);
        }

        private void InitHandlers()
        {
            _symbolHandlers = new Dictionary<char, Action>()
            {
                {' ', () => _horizontalOffset += _font.Space },
                {' ', () => _horizontalOffset += _font.Space },
                {'	', () => _horizontalOffset += _font.Space*4 },
                {'\n', () => ShiftLine() },
                {'\r', () => { }},
            };
        }

        private void GenerateVertices(string text)
        {
            for (var index = 0; index < text.Length; index++)
            {
                var c = text[index];
                if (CheckSymbolForHandler(c))
                {
                    HandleSymbol(c);
                    continue;
                }
                var symbol = _font.GetSymbol(c);
                GenerateSymbolVertices(index, symbol);
                _horizontalOffset += symbol.Width + _font.Interval;
            }
        }

        private bool CheckSymbolForHandler(char symbol)
        {
            return _symbolHandlers.ContainsKey(symbol);
        }

        private void HandleSymbol(char symbol)
        {
            _symbolHandlers[symbol].Invoke();
        }
        
        private void ResetMeshData(string text)
        {
            ResetOffsets();
            _vertices = new Vector3[text.Length * 4];
            _triangles = new int[text.Length * 6];
            _uv = new Vector2[text.Length * 4];
            _colors = new Color[text.Length * 4];
        }

        private void ResetOffsets()
        {
            _horizontalOffset = 0;
            _verticalOffset = -_font.FontHeight;
        }

        private void ShiftLine()
        {
            _horizontalOffset = 0;
            _verticalOffset -= _font.FontHeight;
        }

        private void GenerateSymbolVertices(int symbolIndex, SymbolDescription symbol)
        {
            var verticesStartIndex = symbolIndex*4;
            _vertices[verticesStartIndex] = new Vector3(_horizontalOffset, _verticalOffset - symbol.VerticalOffset);
            _vertices[verticesStartIndex + 1] = new Vector3(_horizontalOffset, _verticalOffset + symbol.Height - symbol.VerticalOffset);
            _vertices[verticesStartIndex + 2] = new Vector3(_horizontalOffset + symbol.Width, _verticalOffset + symbol.Height - symbol.VerticalOffset);
            _vertices[verticesStartIndex + 3] = new Vector3(_horizontalOffset + symbol.Width, _verticalOffset - symbol.VerticalOffset);

            _uv[verticesStartIndex] = new Vector2(symbol.UV.X, symbol.UV.Y);
            _uv[verticesStartIndex + 1] = new Vector2(symbol.UV.X, symbol.UV.MaxY);
            _uv[verticesStartIndex + 2] = new Vector2(symbol.UV.MaxX, symbol.UV.MaxY);
            _uv[verticesStartIndex + 3] = new Vector2(symbol.UV.MaxX, symbol.UV.Y);
             
            var triangleStartIndex = symbolIndex*6;
            _triangles[triangleStartIndex] = verticesStartIndex;
            _triangles[triangleStartIndex + 1] = verticesStartIndex + 1;
            _triangles[triangleStartIndex + 2] = verticesStartIndex + 2;
            _triangles[triangleStartIndex + 3] = verticesStartIndex;
            _triangles[triangleStartIndex + 4] = verticesStartIndex + 2;
            _triangles[triangleStartIndex + 5] = verticesStartIndex + 3;

            GenerateColors(verticesStartIndex);
        }

        private void GenerateColors(int index)
        {
            _colors[index] = _color;
            _colors[index + 1] = _color;
            _colors[index + 2] = _color;
            _colors[index + 3] = _color;
        }

        public void Dispose()
        {
            _vertices = null;
            _triangles = null;
            _uv = null;
            _colors = null;
        }
    }
}
