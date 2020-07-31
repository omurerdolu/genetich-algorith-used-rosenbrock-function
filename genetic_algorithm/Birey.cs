using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MehmetOmurErdolu_153311035
{
    public class Birey : IComparable<Birey>
    {
        public double[] kromozom;
        public double hata;

        private int genNo;
        private double minGen;
        private double maxGen;
        private double mutOrani;
        private double mutDegisim;

        static Random rnd = new Random(0);//normal rand
        static Random rndx_y = new Random();//x ve y değerlerimden dolayı.
        public Birey(int genNo, double minGen, double maxGen, double mutOrani, double mutDegisim)
        {
            this.genNo = genNo;
            this.minGen = minGen;
            this.maxGen = maxGen;
            this.mutOrani = mutOrani;
            this.mutDegisim = mutDegisim;
            this.kromozom = new double[genNo];
            double x, y;
            for (int i = 0; i < this.kromozom.Length; ++i)
            {
                ////////////////////////////////////
                //////Burada x ve y değerlerimi oluşturup kontrollerimi yapıyorum.
                x = rndx_y.NextDouble() * (maxGen - minGen) + minGen;
                y = rndx_y.NextDouble() * (maxGen - minGen) + minGen;
                ////////////////////////////////////
                //benden istenen nedir ? yani sınır fonksiyonum 
                if (100 * Math.Pow((Math.Pow((y - x), 2)), 2) + Math.Pow(1 - x, 2) > 100 * Math.Pow((Math.Pow((y-1 - x-1), 2)), 2) + Math.Pow(1 - x-1, 2)) { // Rosenbrock Fonksiyonuna sokuyorum.
                    this.kromozom[i] = 0.0004 * x * y;//yeni oluşan kromozomum tamamdır.
                }
            }
            this.hata = Genetic.Hata(this.kromozom);
        }
        public int CompareTo(Birey other) //IComparable interface'inden kalıtım aldığı için.
        {
            return 0;
        }
    }

}
