using System;
using System.Collections.Generic;

namespace Altseed2.Stastics
{
    /// <summary>
    /// X軸にインデックス，Y軸に値をとる折れ線グラフのクラス
    /// </summary>
    [Serializable]
    public class LineGraphMono : LineGraphBase
    {
        private protected override IEnumerable<LineBase> Lines => lines;
        private readonly HashSet<Line> lines = new HashSet<Line>();
        internal int MaxX
        {
            get => _maxX;
            set
            {
                if (_maxX == value) return;
                _maxX = value;
                text_maxX.Text = value.ToString();
                text_maxX.AdjustSize();
                AssignUpdate();
            }
        }
        private int _maxX;
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
        /// <see cref="LineGraphMono"/>の新しいインスタンスを生成する
        /// </summary>
        public LineGraphMono()
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
        public Line AddData(float[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data), "引数がnullです");
            var result = new Line(data);
            AddChildNode(result);
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
        public Line AddData(float[] data, Color color, float thickness)
        {
            if (data == null) throw new ArgumentNullException(nameof(data), "引数がnullです");
            var result = new Line(data, color, thickness);
            AddChildNode(result);
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
            var max = 0;
            foreach (var l in lines)
                if (max < l.Data.Length - 1)
                    max = l.Data.Length - 1;
            MaxX = max;
            return true;
        }
        /// <summary>
        /// <see cref="LineGraphMono"/>で描画される線のクラス
        /// </summary>
        [Serializable]
        public sealed class Line : LineBase
        {
            private LineGraphMono graph;
            /// <summary>
            /// 描画するデータを取得または設定する
            /// </summary>
            public float[] Data
            {
                get => _data;
                set
                {
                    if (_data == value) return;
                    _data = value ?? Array.Empty<float>();
                    if (graph != null && graph.MaxX < value.Length - 1 && value.Length > 0) graph.MaxX = value.Length - 1;
                    AssignUpdate();
                }
            }
            private float[] _data;
            /// <summary>
            /// 既定の色，太さを持った<see cref="Line"/>の新しいインスタンスを生成する
            /// </summary>
            /// <param name="data">プロットするデータ</param>
            public Line(float[] data) : base()
            {
                Data = data;
            }
            /// <summary>
            /// 指定した色，太さを持った<see cref="Line"/>の新しいインスタンスを生成する
            /// </summary>
            /// <param name="data">プロットするデータ</param>
            /// <param name="color">線の色</param>
            /// <param name="thickness">線の太さ</param>
            public Line(float[] data, Color color, float thickness) : base(color, thickness)
            {
                Data = data;
            }
            protected override void OnAdded()
            {
                if (Parent is LineGraphMono l)
                {
                    graph = l;
                    if (graph.MaxX < _data.Length - 1 && _data.Length > 0) graph.MaxX = _data.Length - 1;
                    AssignUpdate();
                }
            }
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
                var array = _data.Length == 1 ? new LineNode[1] : new LineNode[_data.Length - 1];
                var x = graph.MaxX == 0 ? 0 : graph.GraphArea.Width / graph.MaxX;
                var y = graph.GraphArea.Height / (graph._maxY - graph._minY);
                var positions = new Vector2F[_data.Length];
                for (int i = 0; i < _data.Length; i++) positions[i] = new Vector2F(x * i + graph.GraphArea.X, graph.GraphArea.Y + graph.GraphArea.Height - y * (_data[i] - graph._minY));
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
        }
    }
}
