using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp276
{
    public partial class Form1 : Form
    {
        private class MovingObject
        {
            public Point P;
            public Point V;
            public int S = 20;
            public Brush C;
        }

        private List<MovingObject> objects = new List<MovingObject>();
        private System.Windows.Forms.Timer timer;
        private Random random = new Random();

        public Form1()
        {
            InitializeComponent();

            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer,
                true);
            this.UpdateStyles();

            timer = new System.Windows.Forms.Timer { Interval = 20 };
            timer.Tick += Timer_Tick;

            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        { 
            for (int i = 0; i < 10000; i++)
            {
                objects.Add(new MovingObject
                {
                    P = new Point(
                        random.Next(ClientSize.Width),
                        random.Next(ClientSize.Height)),
                    V = new Point(
                        random.Next(-5, 6),
                        random.Next(-5, 6)),
                    C = new SolidBrush(
                        Color.FromArgb(
                            random.Next(256),
                            random.Next(256),
                            random.Next(256)))
                });
            }

            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            MoveObjects();
            Invalidate();
        }

        private void MoveObjects()
        {
            foreach (var obj in objects)
            {
                obj.P = new Point(
                    obj.P.X + obj.V.X,
                    obj.P.Y + obj.V.Y);

                if (obj.P.X < 0 || obj.P.X > ClientSize.Width - obj.S)
                    obj.V = new Point(-obj.V.X, obj.V.Y);

                if (obj.P.Y < 0 || obj.P.Y > ClientSize.Height - obj.S)
                    obj.V = new Point(obj.V.X, -obj.V.Y);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            foreach (var obj in objects)
            {
                e.Graphics.FillEllipse(
                    obj.C,
                    obj.P.X,
                    obj.P.Y,
                    obj.S,
                    obj.S);
            }
        }
    }
}
