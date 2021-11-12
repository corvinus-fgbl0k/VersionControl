using Factory_pattern.Abstractions;
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
        private List<Toy> _toys = new List<Toy>();
        private IToyFactory _factory;

        private Toy _nextToy;
        public IToyFactory Factory {
            get { return _factory; }
            set
            {
                _factory = value;
                DisplayNext();
            }
        }

        private void DisplayNext()
        {
            if (_nextToy!=null)
            {
                Controls.Remove(_nextToy);
            }
            _nextToy = Factory.CreateNew();
            _nextToy.Top = label1.Top + label1.Height + 20;
            _nextToy.Left = label1.Left;
            Controls.Add(_nextToy);
        }

        public Form1()
        {
            InitializeComponent();
            Factory = new CarFactory();
        }

        private void createTimer_Tick(object sender, EventArgs e)
        {
            var toy = Factory.CreateNew();
            _toys.Add(toy);
            toy.Left = -toy.Width;
            mainPanel.Controls.Add(toy);
        }

        private void conveyorTimer_Tick(object sender, EventArgs e)
        {
            var maxPosition = 0;
            foreach (var toy in _toys)
            {
                toy.MoveToy();
                if (toy.Left>maxPosition)
                {
                    maxPosition = toy.Left;
                }
                if (maxPosition>1000)
                {
                    var oldesttoy = _toys[0];
                    mainPanel.Controls.Remove(oldesttoy);
                    _toys.Remove(oldesttoy);
                }
            }
        }

        private void btnCar_Click(object sender, EventArgs e)
        {
            Factory = new CarFactory();
        }

        private void btnBall_Click(object sender, EventArgs e)
        {
            Factory = new BallFactory()
            {
                BallColor = btnColor.BackColor
            };
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var coloPicker = new ColorDialog();

            coloPicker.Color = button.BackColor;
            if (coloPicker.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            button.BackColor = coloPicker.Color;
        }
        private void btnColor2_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var coloPicker = new ColorDialog();

            coloPicker.Color = button.BackColor;
            if (coloPicker.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            button.BackColor = coloPicker.Color;
        }

        private void btnPresent_Click(object sender, EventArgs e)
        {
            Factory = new PresentFactory()
            {
                BoxColor = btnColor.BackColor,
                RibbonColor = btnColor2.BackColor
            };
        }
    }
}
