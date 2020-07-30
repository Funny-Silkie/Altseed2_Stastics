using Altseed2;
using Altseed2.Stastics;
using NUnit.Framework;
using System;

namespace Test
{
    class LineGraph
    {
        private static readonly Font font_GenYo = Font.LoadDynamicFontStrict("Resources/GenYoMinJP-Bold.ttf", 30);
        private static readonly Font font_mplus = Font.LoadDynamicFontStrict("Resources/mplus-1m-regular.ttf", 30);
        [Test]
        public void TestMono()
        {
            Engine.Initialize("Test", 960, 720);
            var graph = new LineGraphMono()
            {
                AxisColor = new Color(0, 0, 0),
                BackColor = new Color(255, 255, 255),
                GraphArea = new RectF(100, 50, 450, 450),
                LabelColor = new Color(0, 0, 0),
                LabelFont = Font.LoadDynamicFontStrict("Resources/GenYoMinJP-Bold.ttf", 30),
                LabelX = "Index",
                LabelY = "Value",
                MaxY = 10f,
                MinY = 0f,
                Size = new Vector2F(600, 600),
                ValueColor = new Color(0, 0, 0),
                ValueFont = Font.LoadDynamicFontStrict("Resources/GenYoMinJP-Bold.ttf", 30)
            };
            Engine.AddNode(graph);
            var source = new float[20];
            for (int i = 0; i < source.Length; i++) source[i] = MathF.Pow(i, 0.5f);
            graph.MaxY = System.Linq.Enumerable.Max(source);
            graph.MinY = System.Linq.Enumerable.Min(source);
            graph.AddData(source, new Color(255, 100, 100), 3f);
            while (Engine.DoEvents())
            {
                if (Engine.Keyboard.GetKeyState(Key.S) == ButtonState.Push) Engine.Graphics.SaveScreenshot($"SS/{nameof(TestMono)}.png");
                Engine.Update();
            }
            Engine.Terminate();
        }
        [Test]
        public void TestDouble()
        {
            Engine.Initialize("Test", 960, 720);
            var graph = new LineGraphDouble()
            {
                AxisColor = new Color(0, 0, 0),
                BackColor = new Color(255, 255, 255),
                GraphArea = new RectF(100, 50, 450, 450),
                LabelColor = new Color(0, 0, 0),
                LabelFont = Font.LoadDynamicFontStrict("Resources/GenYoMinJP-Bold.ttf", 30),
                MaxX = 20f,
                MaxY = 20f,
                MinX = 0f,
                MinY = 0f,
                Size = new Vector2F(600, 600),
                ValueColor = new Color(0, 0, 0),
                ValueFont = Font.LoadDynamicFontStrict("Resources/GenYoMinJP-Bold.ttf", 30)
            };
            Engine.AddNode(graph);
            //var source = new Vector2F[20];
            //for (int i = 0; i < source.Length; i++) source[i] = new Vector2F(i * 2, i * i);
            //graph.MaxX = System.Linq.Enumerable.Max(source, x => x.X);
            //graph.MaxY = System.Linq.Enumerable.Max(source, x => x.Y);
            //graph.MinX = System.Linq.Enumerable.Min(source, x => x.X);
            //graph.MinY = System.Linq.Enumerable.Min(source, x => x.Y);
            //graph.AddData(source, new Color(255, 100, 100), 3f);
            graph.AddData(new[] { new Vector2F(15f, 0f), new Vector2F(14f, 4f), new Vector2F(12f, 7f), new Vector2F(10f, 10f), new Vector2F(7f, 12f), new Vector2F(4f, 14f), new Vector2F(0, 15f) }, new Color(255, 100, 100), 3f);
            while (Engine.DoEvents())
            {
                if (Engine.Keyboard.GetKeyState(Key.S) == ButtonState.Push) Engine.Graphics.SaveScreenshot($"SS/{nameof(TestDouble)}.png");
                Engine.Update();
            }
            Engine.Terminate();
        }
    }
}
