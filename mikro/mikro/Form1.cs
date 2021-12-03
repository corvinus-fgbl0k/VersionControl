using mikro.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mikro
{
    public partial class Form1 : Form
    {
        List<Person> Population = new List<Person>();
        List<BirthProbability> BirthProbabilities = new List<BirthProbability>();
        List<DeathProbability> DeathProbabilities = new List<DeathProbability>();

        Random rng = new Random(1234);
        public Form1()
        {
            InitializeComponent();

            Population = GetPopulation(@"C:\Users\szuak\AppData\Local\Temp\nép.csv");
            BirthProbabilities = GetBirthProbabilities(@"C:\Users\szuak\AppData\Local\Temp\születés.csv");
            DeathProbabilities = GetDeathProbabilities(@"C:\Users\szuak\AppData\Local\Temp\halál.csv");

            for (int year = 2005; year <= 2024; year++)
            {
                for (int i = 0; i < Population.Count; i++)
                {
                    SimStep(year, Population[i]);
                }

                int nbrOfMales = (from x in Population
                                  where x.Gender == Gender.Male && x.IsAlive
                                  select x).Count();
                int nbrOfFemales = (from x in Population
                                    where x.Gender == Gender.Female && x.IsAlive
                                    select x).Count();
                Console.WriteLine(
                    string.Format("Év:{0} Fiúk:{1} Lányok:{2}", year, nbrOfMales, nbrOfFemales));
            }
        }
        public List<Person> GetPopulation(string csvpath)
        {
            List<Person> population = new List<Person>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    population.Add(new Person()
                    {
                        BirthYear = int.Parse(line[0]),
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[1]),
                        NbrOfChildren = int.Parse(line[2])
                    });
                }
            }

            return population;
        }
        public List<BirthProbability> GetBirthProbabilities(string csvpath)
        {
            List<BirthProbability> birth = new List<BirthProbability>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    birth.Add(new BirthProbability()
                    {
                        Year = int.Parse(line[0]),
                        NbrOfChildren = int.Parse(line[1]),
                        P = int.Parse(line[2])
                    });
                }
            }

            return birth;
        }
        public List<DeathProbability> GetDeathProbabilities(string csvpath)
        {
            List<DeathProbability> death = new List<DeathProbability>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    death.Add(new DeathProbability()
                    {
                        Year = int.Parse(line[0]),
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[1]),
                        P = int.Parse(line[2])
                    });
                }
            }

            return death;
        }
        private void SimStep(int year, Person person)
        {           
            if (!person.IsAlive) return;
            
            byte age = (byte)(year - person.BirthYear);

            double pDeath = (from x in DeathProbabilities
                             where x.Gender == person.Gender && x.Year == age
                             select x.P).FirstOrDefault();
            if (rng.NextDouble() <= pDeath)
                person.IsAlive = false;

            if (person.IsAlive && person.Gender == Gender.Female)
            {
                double pBirth = (from x in BirthProbabilities
                                 where x.Year == age
                                 select x.P).FirstOrDefault();
                if (rng.NextDouble() <= pBirth)
                {
                    Person újszülött = new Person();
                    újszülött.BirthYear = year;
                    újszülött.NbrOfChildren = 0;
                    újszülött.Gender = (Gender)(rng.Next(1, 3));
                    Population.Add(újszülött);
                }
            }
        }
        public void DisplayResults()
        {

        }
        private void button2_Click(object sender, EventArgs e)
        {
            DisplayResults();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = ofd.FileName;
            }
        }
    }
}
