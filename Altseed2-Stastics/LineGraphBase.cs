using System;
using System.Collections.Generic;

namespace Altseed2.Stastics
{
    /// <summary>
    /// 折れ線グラフの基底クラス
    /// </summary>
    [Serializable]
    public abstract class LineGraphBase : Node
    {
        private protected static readonly Font font_GenYo = Font.LoadDynamicFontStrict("Resources/GenYoMinJP-Bold.ttf", 30);
        private protected static readonly Font font_mplus = Font.LoadDynamicFontStrict("Resources/mplus-1m-regular.ttf", 30);
        private protected readonly RectangleNode back = new RectangleNode()
        {
            Size = new Vector2F(400, 400)
        };
        private readonly TextNode labelX = new TextNode()
        {
            Font = font_GenYo,
            Pivot = new Vector2F(0.5f, 0f),
            ZOrder = 1
        };
        private readonly TextNode labelY = new TextNode()
        {
            Angle = -90f,
            Font = font_GenYo,
            Pivot = new Vector2F(0.5f, 1.0f),
            ZOrder = 1
        };
        private readonly LineNode horizontalLine = new LineNode()
        {
            ZOrder = 1
        };
        private protected readonly TextNode text_maxX = new TextNode()
        {
            Font = font_GenYo,
            Pivot = new Vector2F(1f, 0f),
            Text = "0",
            ZOrder = 1
        };
        private protected readonly TextNode text_maxY = new TextNode()
        {
            Font = font_GenYo,
            Pivot = new Vector2F(1f, 0f),
            Text = "1",
            ZOrder = 1
        };
        private protected readonly TextNode text_minX = new TextNode()
        {
            Font = font_GenYo,
            Text = "0",
            ZOrder = 1
        };
        private protected readonly TextNode text_minY = new TextNode()
        {
            Font = font_GenYo,
            Pivot = new Vector2F(1f, 1f),
            Text = "0",
            ZOrder = 1
        };
        private readonly LineNode verticalLine = new LineNode()
        {
            ZOrder = 1
        };
        /// <summary>
        /// 縦軸，横軸の色を取得または設定する
        /// </summary>
        public Color AxisColor
        {
            get => horizontalLine.Color;
            set
            {
                horizontalLine.Color = value;
                verticalLine.Color = value;
            }
        }
        /// <summary>
        /// 縦軸，横軸の太さを取得または設定する
        /// </summary>
        public float AxisThickness
        {
            get => horizontalLine.Thickness;
            set
            {
                horizontalLine.Thickness = value;
                verticalLine.Thickness = value;
            }
        }
        /// <summary>
        /// 背景色を取得または設定する
        /// </summary>
        public Color BackColor { get => back.Color; set => back.Color = value; }
        /// <summary>
        /// グラフの領域を取得または設定する
        /// </summary>
        public RectF GraphArea
        {
            get => _graphArea;
            set
            {
                if (_graphArea == value) return;
                _graphArea = value;
                verticalLine.Point1 = value.Position;
                verticalLine.Point2 = new Vector2F(value.X, value.Y + value.Height);
                horizontalLine.Point1 = new Vector2F(value.X, value.Y + value.Height);
                horizontalLine.Point2 = value.Position + value.Size;
                labelX.Position = new Vector2F(value.X + value.Width / 2, (Size.Y + value.Y + value.Height) / 2);
                labelY.Position = new Vector2F(value.X / 2, value.Y + value.Height / 2);
                text_minX.Position = new Vector2F(value.X, value.Y + value.Height + AxisThickness);
                text_maxX.Position = new Vector2F(value.X + value.Width, value.Y + value.Height + AxisThickness);
                text_maxY.Position = new Vector2F(value.X - AxisThickness - 10f, value.Y);
                text_minY.Position = new Vector2F(value.X - AxisThickness - 10f, value.Y + value.Height);
                AssignUpdate();
            }
        }
        private RectF _graphArea;
        /// <summary>
        /// 見出しの色を取得または設定する
        /// </summary>
        public Color LabelColor
        {
            get => labelX.Color;
            set
            {
                labelX.Color = value;
                labelY.Color = value;
            }
        }
        /// <summary>
        /// 見出しのフォントを取得または設定する
        /// </summary>
        public Font LabelFont
        {
            get => labelX.Font;
            set
            {
                if (labelX.Font == value) return;
                labelX.Font = value;
                labelY.Font = value;
                labelX.AdjustSize();
                labelY.AdjustSize();
            }
        }
        /// <summary>
        /// X軸の見出しの文字列を取得または設定する
        /// </summary>
        public string LabelX
        {
            get => labelX.Text;
            set
            {
                labelX.Text = value;
                labelX.AdjustSize();
            }
        }
        /// <summary>
        /// Y軸の見出しの文字列を取得または設定する
        /// </summary>
        public string LabelY
        {
            get => labelY.Text;
            set
            {
                labelY.Text = value;
                labelY.AdjustSize();
            }
        }
        private protected abstract IEnumerable<LineBase> Lines { get; }
        /// <summary>
        /// グラフ左上の座標を取得または設定する
        /// </summary>
        public Vector2F Position { get => back.Position; set => back.Position = value; }
        /// <summary>
        /// グラフのサイズを取得または設定する
        /// </summary>
        public Vector2F Size
        {
            get => back.Size;
            set
            {
                back.Size = value;
                labelX.Position = new Vector2F(_graphArea.X + _graphArea.Width / 2, (value.Y + _graphArea.Y + _graphArea.Height) / 2);
            }
        }
        /// <summary>
        /// 値の色を取得または設定する
        /// </summary>
        public Color ValueColor
        {
            get => text_maxX.Color;
            set
            {
                text_maxX.Color = value;
                text_maxY.Color = value;
                text_minX.Color = value;
                text_minY.Color = value;
            }
        }
        /// <summary>
        /// 値の表示に用いるフォントを取得または設定する
        /// </summary>
        public Font ValueFont
        {
            get => text_maxX.Font;
            set
            {
                if (text_maxX.Font == value) return;
                text_maxX.Font = value;
                text_maxY.Font = value;
                text_minX.Font = value;
                text_minY.Font = value;
                text_maxX.AdjustSize();
                text_maxY.AdjustSize();
                text_minX.AdjustSize();
                text_minY.AdjustSize();
            }
        }
        /// <summary>
        /// <see cref="LineGraphBase"/>の新しいインスタンスを生成する
        /// </summary>
        protected LineGraphBase()
        {
            text_maxX.AdjustSize();
            text_maxY.AdjustSize();
            text_minX.AdjustSize();
            text_minY.AdjustSize();
            AddChildNode(back);
            back.AddChildNode(labelX);
            back.AddChildNode(labelY);
            back.AddChildNode(horizontalLine);
            back.AddChildNode(verticalLine);
            back.AddChildNode(text_maxX);
            back.AddChildNode(text_maxY);
            back.AddChildNode(text_minX);
            back.AddChildNode(text_minY);
        }
        private protected void AssignUpdate()
        {
             foreach (var line in Lines) line.AssignUpdate();
        }
        /// <summary>
        /// <see cref="LineGraphBase"/>で描画される線のクラス
        /// </summary>
        [Serializable]
        public abstract class LineBase : Node
        {
            private bool mustUpdated;
            /// <summary>
            /// 色を取得または設定する
            /// </summary>
            public Color Color
            {
                get => _color;
                set
                {
                    if (_color == value) return;
                    _color = value;
                    for (int i = 0; i < Nodes.Length; i++) Nodes[i].Color = value;
                }
            }
            private Color _color = new Color(255, 255, 255);
            internal LineNode[] Nodes { get; set; } = Array.Empty<LineNode>();
            /// <summary>
            /// 線の太さを取得または設定する
            /// </summary>
            public float Thickness
            {
                get => _thickness;
                set
                {
                    if (_thickness == value) return;
                    _thickness = value;
                    for (int i = 0; i < Nodes.Length; i++) Nodes[i].Thickness = value;
                }
            }
            private float _thickness = 3f;
            /// <summary>
            /// 既定の色，太さを持った<see cref="LineBase"/>の新しいインスタンスを生成する
            /// </summary>
            protected LineBase() { }
            /// <summary>
            /// 指定した色，太さを持った<see cref="LineBase"/>の新しいインスタンスを生成する
            /// </summary>
            /// <param name="color">線の色</param>
            /// <param name="thickness">線の太さ</param>
            protected LineBase(Color color, float thickness)
            {
                _thickness = thickness;
                _color = color;
            }
            internal void AssignUpdate() => mustUpdated = true;
            protected override void OnUpdate()
            {
                if (mustUpdated)
                {
                    UpdateNodes();
                    mustUpdated = false;
                }
            }
            private protected abstract void UpdateNodes();
        }
    }
}
