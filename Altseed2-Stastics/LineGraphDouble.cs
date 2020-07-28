using System;
using System.Collections;
using System.Collections.Generic;

namespace Altseed2.Stastics
{
    /// <summary>
    /// X，Y軸それぞれに値を持てる折れ線グラフのクラス
    /// </summary>
    [Serializable]
    public class LineGraphDouble : LineGraphBase
    {
        private protected override IEnumerable<LineBase> Lines => lines;
        private readonly HashSet<Line> lines = new HashSet<Line>();
        /// <summary>
        /// X軸最大値を取得または設定する
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">設定しようとした値が<see cref="MinX"/>以下</exception>
        public float MaxX
        {
            get => _maxX;
            set
            {
                if (_maxX == value) return;
                if (_minX >= value) throw new ArgumentOutOfRangeException(nameof(value), $"設定しようとした値がMin({_minX})以下です\n設定しようとした値：{value}");
                _maxX = value;
                text_maxX.Text = value.ToString();
                text_maxX.AdjustSize();
                AssignUpdate();
            }
        }
        private float _maxX = 1.0f;
        /// <summary>
        /// Y軸最大値を取得または設定する
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">設定しようとした値が<see cref="MinY"/>以下</exception>
        public float MaxY
        {
            get => _maxY;
            set
            {
                if (_maxY == value) return;
                if (_minY >= value) throw new ArgumentOutOfRangeException(nameof(value), $"設定しようとした値がMin({_minY})以下です\n設定しようとした値：{value}");
                _maxY = value;
                text_maxY.Text = value.ToString();
                text_maxY.AdjustSize();
                AssignUpdate();
            }
        }
        private float _maxY = 1.0f;
        /// <summary>
        /// X軸最小値を取得または設定する
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">設定しようとした値が<see cref="MaxX"/>以上</exception>
        public float MinX
        {
            get => _minX;
            set
            {
                if (_minX == value) return;
                if (_maxX <= value) throw new ArgumentOutOfRangeException(nameof(value), $"設定しようとした値がMax({_maxX})以上です\n設定しようとした値：{value}");
                _minX = value;
                text_minX.Text = value.ToString();
                text_minX.AdjustSize();
                AssignUpdate();
            }
        }
        private float _minX;
        /// <summary>
        /// Y軸最小値を取得または設定する
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">設定しようとした値が<see cref="MaxY"/>以上</exception>
        public float MinY
        {
            get => _minY;
            set
            {
                if (_minY == value) return;
                if (_maxY <= value) throw new ArgumentOutOfRangeException(nameof(value), $"設定しようとした値がMax({_maxY})以上です\n設定しようとした値：{value}");
                _minY = value;
                text_minY.Text = value.ToString();
                text_minY.AdjustSize();
                AssignUpdate();
            }
        }
        private float _minY;
        /// <summary>
        /// <see cref="LineGraphDouble"/>の新しいインスタンスを生成する
        /// </summary>
        public LineGraphDouble()
        {
            AxisThickness = 3f;
            BackColor = new Color(0, 0, 0);
            GraphArea = new RectF(100, 50, 250, 250);
            LabelX = "X";
            LabelY = "Y";
        }
        /// <summary>
        /// プロットするデータを追加する
        /// </summary>
        /// <param name="data">プロットするデータ</param>
        /// <exception cref="ArgumentNullException"><paramref name="data"/>がnull</exception>
        /// <returns>追加された線を表す<see cref="Line"/>のインスタンス</returns>
        public Line AddData(Vector2F[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data), "引数がnullです");
            var result = new Line(data);
            AddChildNode(result);
            lines.Add(result);
            return result;
        }
        /// <summary>
        /// プロットするデータを追加する
        /// </summary>
        /// <param name="data">プロットするデータ</param>
        /// <param name="color">線の色</param>
        /// <param name="thickness">線の太さ</param>
        /// <exception cref="ArgumentNullException"><paramref name="data"/>がnull</exception>
        /// <returns>追加された線を表す<see cref="Line"/>のインスタンス</returns>
        public Line AddData(Vector2F[] data, Color color, float thickness)
        {
            if (data == null) throw new ArgumentNullException(nameof(data), "引数がnullです");
            var result = new Line(data, color, thickness);
            AddChildNode(result);
            lines.Add(result);
            return result;
        }
        /// <summary>
        /// 線を削除する
        /// </summary>
        /// <param name="line">削除する線</param>
        /// <returns><paramref name="line"/>を削除出来たらtrue，それ以外でfalse</returns>
        public bool RemoveData(Line line)
        {
            if (!lines.Contains(line)) return false;
            back.RemoveChildNode(line);
            lines.Remove(line);
            return true;
        }
        /// <summary>
        /// <see cref="LineGraphDouble"/>で描画される線のクラス
        /// </summary>
        [Serializable]
        public sealed class Line : LineBase
        {
            private static readonly VectorComparer comparer = new VectorComparer();
            private LineGraphDouble graph;
            /// <summary>
            /// 描画するデータを取得または設定する
            /// </summary>
            public Vector2F[] Data
            {
                get => _data;
                set
                {
                    if (_data == value) return;
                    if (value == null || value.Length == 0) _data = Array.Empty<Vector2F>();
                    else
                    {
                        var array = (Vector2F[])value.Clone();
                        Array.Sort(array, comparer);
                        _data = array;
                    }
                    AssignUpdate();
                }
            }
            private Vector2F[] _data;
            /// <summary>
            /// 既定の色，太さを持った<see cref="Line"/>の新しいインスタンスを生成する
            /// </summary>
            /// <param name="data">プロットするデータ</param>
            public Line(Vector2F[] data) : base()
            {
                Data = data;
            }
            /// <summary>
            /// 指定した色，太さを持った<see cref="Line"/>の新しいインスタンスを生成する
            /// </summary>
            /// <param name="data">プロットするデータ</param>
            /// <param name="color">線の色</param>
            /// <param name="thickness">線の太さ</param>
            public Line(Vector2F[] data, Color color, float thickness) : base(color, thickness)
            {
                Data = data;
            }
            /// <inheritdoc/>
            protected override void OnAdded()
            {
                if (Parent is LineGraphDouble l)
                {
                    graph = l;
                    AssignUpdate();
                }
            }
            /// <inheritdoc/>
            protected override void OnRemoved()
            {
                if (graph != null)
                {
                    graph.lines.Remove(this);
                    graph = null;
                }
            }
            private protected override void UpdateNodes()
            {
                if (graph == null) return;
                for (int i = 0; i < Nodes.Length; i++) graph.back.RemoveChildNode(Nodes[i]);
                var array = _data.Length <= 1 ? new LineNode[1] : new LineNode[_data.Length - 1];
                var x = graph.GraphArea.Width / (graph._maxX - graph._minX);
                var y = graph.GraphArea.Height / (graph._maxY - graph._minY);
                var positions = new Vector2F[_data.Length];
                for (int i = 0; i < _data.Length; i++) positions[i] = new Vector2F((x - graph._minX) * _data[i].X + graph.GraphArea.X, graph.GraphArea.Y + graph.GraphArea.Height - y * (_data[i].Y - graph._minY));
                if (positions.Length == 1) array[0] = new LineNode()
                {
                    Color = Color,
                    Point1 = positions[0],
                    Point2 = positions[0],
                    Position = positions[0],
                    Thickness = Thickness
                };
                else
                {
                    for (int i = 1; i < positions.Length; i++)
                    {
                        array[i - 1] = new LineNode
                        {
                            Color = Color,
                            Thickness = Thickness
                        };
                        SetPos(array[i - 1], positions[i - 1], positions[i]);
                        graph.back.AddChildNode(array[i - 1]);
                    }
                }
                Nodes = array;
            }
            [Serializable]
            internal sealed class VectorComparer : IComparer<Vector2F>, IComparer
            {
                public int Compare(Vector2F x, Vector2F y) => x.X.CompareTo(y.X);
                int IComparer.Compare(object x, object y)
                {
                    if (x is null) return y is null ? 0 : -1;
                    if (y is null) return 1;
                    if (x is Vector2F vx && y is Vector2F vy) return Compare(vx, vy);
                    throw new ArgumentException($"サポートされていない型です\nx : {x.GetType()}, y : {y.GetType()}", $"{nameof(x)}, {nameof(y)}");
                }
            }
        }
    }
}
