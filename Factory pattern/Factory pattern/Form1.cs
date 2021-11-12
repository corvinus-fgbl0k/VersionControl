using Factory_pattern.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Factory_pattern
{
    public partial class Form1 : Form
    {
        private List<Ball> _balls = new List<Ball>();
        private BallFactory _factory;
        public BallFactory Factory {
            get { return _factory; }
            set { _factory = value; } 
        }
        public Form1()
        {
            InitializeComponent();
            Factory = new BallFactory();
        }

        private void createTimer_Tick(object sender, EventArgs e)
        {
            var ball = Factory.CreateNew();
            _balls.Add(ball);
            ball.Left = -ball.Width;
            mainPanel.Controls.Add(ball);
        }

        private void conveyorTimer_Tick(object sender, EventArgs e)
        {
            var maxPosition = 0;
            foreach (var item in _balls)
            {
                item.MoveBall();
                if (item.Left>maxPosition)
                {
                    maxPosition = item.Left;
                }
                if (maxPosition>1000)
                {
                    var oldestball = _balls[0];
                    mainPanel.Controls.Remove(oldestball);
                    _balls.Remove(oldestball);
                }
            }
        }
    }
}
