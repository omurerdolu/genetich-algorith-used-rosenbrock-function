using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MehmetOmurErdolu_153311035
{
    class Genetic
    {
        static Random rnd = new Random(0); // static rnd
        public double[] Cozum(int popBoyut, int iterasyonSayisi, double min, double max, double caprazlamaOrani, double mutasyonOrani, double secim, int maxGen, double seckinlik)
        {
            Birey[] birey = new Birey[iterasyonSayisi];
            double[] enIyiCozum = new double[popBoyut]; // herhangi bir bireyin bulduğu en iyi çözümleri topluyorum.
            double EnIyiCikis = double.MaxValue; // aşağıda her yeni birey için kontrolü bununla sağlıyorum.
                                                 //ilk değeri bilmediğim için maxValue dedim.zaten ilk birey'de max değerle değişiyor.
            for (int i = 0; i < birey.Length; ++i)
            {
                birey[i] = new Birey(popBoyut, min, max, caprazlamaOrani, mutasyonOrani);
                if (birey[i].hata < EnIyiCikis)//Yeni popülasyona kendi verdiğim hata değerinden düşükse hata değeri array.copy ile yeni diziye at.(bestSolution)
                {
                    EnIyiCikis = birey[i].hata;
                    Array.Copy(birey[i].kromozom, enIyiCozum, popBoyut);
                }
            }

            // process
            int gen = 0;
            bool durum = false;
            while (gen < maxGen && durum == false)//5000 denemeden küçükse ve halen tamamlanmamışsa devam et.
            {
                Birey[] ebeveyn = Sec(2, birey, secim); //2 birey seçtiriyorum.
                Birey[] cocuk = MutVeyaCross(ebeveyn[0], ebeveyn[1], min, max, caprazlamaOrani, mutasyonOrani); // 2 genimi yolluyorum burada mutasyon ve crossover olması için
                KotuOlaniSil(cocuk[0], cocuk[1], birey);//popülasyondaki en kötü 2 sinin yerine yeni çocukları ekle.
                YeniBirey(birey, popBoyut, min, max, caprazlamaOrani, mutasyonOrani); // yeni bir birey ata ve 3. birey olsun.

                for (int i = iterasyonSayisi - 3; i < iterasyonSayisi; ++i) // 3. bireyi kontrol et
                {
                    if (birey[i].hata < EnIyiCikis)//3. birey EnIyiCikis durumundan daha küçükse al değilse alma.
                    {
                        
                        EnIyiCikis = birey[i].hata;
                        birey[i].kromozom.CopyTo(enIyiCozum, 0);//yeni bireye kromozomları atıyorum.
                        if (EnIyiCikis < seckinlik)
                        {
                            durum = true;
                        }
                    }
                }
                ++gen;
            }
            return enIyiCozum;
        }
        private static Birey[] Sec(int n, Birey[] populasyon, double secim)
        {
            
            int popBoyut = populasyon.Length;
            int[] index = new int[popBoyut];
            for (int i = 0; i < index.Length; ++i)
                index[i] = i;

            for (int i = 0; i < index.Length; ++i) // yerlerini karıştırıyorum. oluşacak n adet kromozomdan 2 aday alacağım çünkü.
            {                                        // ve indexlerin düzenli olduğundan sıralarını karıştırıyorum ki rasgele 2 si gelsin.
                int r = rnd.Next(i, index.Length);
                int gecici = index[r];
                index[r] = index[i];
            }

            int turnuvaSayisi = (int)(secim * popBoyut);
            if (turnuvaSayisi < n)
                turnuvaSayisi = n;
            Birey[] adaylar = new Birey[turnuvaSayisi];//aday 2 çocuğum.

            for (int i = 0; i < turnuvaSayisi; ++i)//az önce karıştırdığım indexes'tan ilk 2 elemanı aday olarak belirledim.
                adaylar[i] = populasyon[index[i]];
            Array.Sort(adaylar);

            Birey[] sonuc = new Birey[n];
            for (int i = 0; i < n; ++i)
                sonuc[i] = adaylar[i];
            //seçtiğim 2 aday genimi aldım ve geri yolluyorum.
            return sonuc;
        }
        private static Birey[] MutVeyaCross(Birey parent1, Birey parent2, double minGen, double maxGen, double caprazlamaOrani, double mutasyonDegisim) // crossover ve mutasyon
        {
            int GenNo = parent1.kromozom.Length;//crossover için.

            int cross = rnd.Next(0, GenNo - 1); // crossover noktası. Tamamen kromozomların değişmemesi için 1 çıkarıyorum.
            //Random bir crossover olayı gerçekleştireceğim eleman sayısı yani burada belirlediğim.
            Birey child1 = new Birey(GenNo, minGen, maxGen, caprazlamaOrani, mutasyonDegisim); // random kromozom
            Birey child2 = new Birey(GenNo, minGen, maxGen, caprazlamaOrani, mutasyonDegisim);

            for (int i = 0; i <= cross; ++i)
                child1.kromozom[i] = parent1.kromozom[i];
            for (int i = cross + 1; i < GenNo; ++i)
                child2.kromozom[i] = parent1.kromozom[i];
            for (int i = 0; i <= cross; ++i)
                child2.kromozom[i] = parent2.kromozom[i];
            for (int i = cross + 1; i < GenNo; ++i)
                child1.kromozom[i] = parent2.kromozom[i];

            Mutasyon(child1, maxGen, caprazlamaOrani, mutasyonDegisim);
            Mutasyon(child2, maxGen, caprazlamaOrani, mutasyonDegisim);

            child1.hata = Genetic.Hata(child1.kromozom);//hataları voilated olması gerekiyor onları ölçtük.
            child2.hata = Genetic.Hata(child2.kromozom);

            Birey[] sonuc = new Birey[2];
            sonuc[0] = child1;
            sonuc[1] = child2;

            return sonuc;
        }
        private static void Mutasyon(Birey child, double maxGen, double caprazlamaOrani, double mutasyonDegisim)
        {
            double hi = mutasyonDegisim * maxGen;
            double lo = -hi;
            for (int i = 0; i < child.kromozom.Length; ++i)
            {
                if (rnd.NextDouble() < caprazlamaOrani)//üretilen rnd sayı mutasyon oranından düşükse mutasyona uğra değilse uğrama.
                {
                    double delta = (hi - lo) * rnd.NextDouble() + lo;//mutasyon gerçekleşiyor delta ile.
                    child.kromozom[i] += delta;
                }
            }
        }
        private static void KotuOlaniSil(Birey child1, Birey child2, Birey[] populasyon)
        {
            //popülasyondaki en kötü 2 sinin yerine yeni çocukları ekle.
            int popBoyut = populasyon.Length;
            Array.Sort(populasyon);
            populasyon[popBoyut - 1] = child1;
            populasyon[popBoyut - 2] = child2;
            return;
        }
        private static void YeniBirey(Birey[] populasyon, int GenNo, double minGen, double maxGen, double mutOran, double mutDegisim)
        {
            //en kötü bireyi sil yeni bireyi oraya koy
            Birey immigrant = new Birey(GenNo, minGen, maxGen, mutOran, mutDegisim);
            int popBoyutu = populasyon.Length;
            populasyon[popBoyutu - 3] = immigrant; //3. en kötü bireyi değiştir.
        }
        public static double Hata(double[] x)
        {
            double trueMin = 0.0;
            double z = 0.0;
            for (int i = 0; i < x.Length; ++i)
                z += (x[i] * x[i]);
            return Math.Abs(trueMin - z);
        }
    }
}
