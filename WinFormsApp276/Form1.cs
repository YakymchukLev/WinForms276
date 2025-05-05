using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp276
{
    public partial class Form1 : Form
    {
        // Внутрішній клас, що представляє рухомий об'єкт
        private class MovingObject
        {
            public Point P;  // Поточна позиція об'єкта (X,Y)
            public Point V;  // Вектор швидкості (по горизонталі та вертикалі)
            public int S = 20;  // Розмір об'єкта (діаметр кола)
            public Brush C;   // Колір об'єкта
        }

        // Список усіх рухомих об'єктів
        private List<MovingObject> objects = new List<MovingObject>();
        // Таймер для анімації
        private System.Windows.Forms.Timer timer;
        // Генератор випадкових чисел для ініціалізації об'єктів
        private Random random = new Random();

        // Hалаштовує критично важливі параметри
        public Form1()
        {
            InitializeComponent();

            // Налаштування подвійної буферизації для плавної анімації
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer,
                true);
            this.UpdateStyles();

            // Ініціалізація таймера з інтервалом 20 мс (~50 кадрів/сек)
            timer = new System.Windows.Forms.Timer { Interval = 20 };
            timer.Tick += Timer_Tick;  // Підписка на подію таймера

            // Підписка на подію завантаження форми
            this.Load += Form1_Load;
        }

        // Обробник події завантаження форми
        private void Form1_Load(object sender, EventArgs e)
        {
            // Створення рухомих об'єктів з випадковими параметрами
            for (int i = 0; i < 2; i++)
            {
                objects.Add(new MovingObject
                {
                    // Випадкова початкова позиція в межах форми
                    P = new Point(
                        random.Next(ClientSize.Width),
                        random.Next(ClientSize.Height)),
                    // Випадкова швидкість (-5 до 5 по кожній осі)
                    V = new Point(
                        random.Next(-5, 6),
                        random.Next(-5, 6)),
                    // Випадковий колір (RGB)
                    C = new SolidBrush(
                        Color.FromArgb(
                            random.Next(256),
                            random.Next(256),
                            random.Next(256)))
                });
            }

            timer.Start();  // Запуск анімації
        }

        // Обробник події таймера (викликається кожні 20 мс)
        private void Timer_Tick(object sender, EventArgs e)
        {
            MoveObjects();  // Оновлення позицій об'єктів
            Invalidate();   // Запит на перемалювання форми
        }

        // Метод для оновлення позицій всіх об'єктів
        private void MoveObjects()
        {
            foreach (var obj in objects)
            {
                // Оновлення позиції (додавання вектора швидкості)
                obj.P = new Point(
                    obj.P.X + obj.V.X,
                    obj.P.Y + obj.V.Y);

                // Перевірка зіткнення з лівою/правою межею форми
                if (obj.P.X < 0 || obj.P.X > ClientSize.Width - obj.S)
                    obj.V = new Point(-obj.V.X, obj.V.Y);  // Зміна напрямку по X

                // Перевірка зіткнення з верхньою/нижньою межею форми
                if (obj.P.Y < 0 || obj.P.Y > ClientSize.Height - obj.S)
                    obj.V = new Point(obj.V.X, -obj.V.Y);  // Зміна напрямку по Y
            }
        }

        // Метод для малювання форми та всіх об'єктів
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);  // Виклик базового методу

            // Малювання кожного об'єкта у вигляді заповненого кола
            foreach (var obj in objects)
            {
                e.Graphics.FillEllipse(
                    obj.C,       // Колір
                    obj.P.X,     // Позиція X
                    obj.P.Y,     // Позиція Y
                    obj.S,       // Ширина
                    obj.S);      // Висота (коло, тому ширина=висота)
            }
        }
    }
}