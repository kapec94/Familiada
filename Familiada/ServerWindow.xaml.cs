using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

namespace Familiada
{
    public partial class ServerWindow : Window
    {
        internal ClientWindow client = null;

        internal RoundData round = null;

        internal int pointsA = 0;
        internal int pointsB = 0;

        int winnerPoints = 0;

        int currentPage = 0;
        Page[] pages = null;

        public ServerWindow()
        {
            InitializeComponent();

            client = new ClientWindow();

            pages = new Page[5];
            pages[0] = new DataSelectPage(this);
            pages[1] = new NormalGamePage(this);
            pages[2] = new FinalGamePage(this);
            pages[3] = new FinalGamePage(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Navigate(pages[0]);
            client.Show();
        }

        private void LoadData(int set)
        {
            Object[] data = (set == 1 ? data1 : (set == 2 ? data2 : data3));
            round = new RoundData();

            for (int n = 0; n < 10; n += 2)
            {
                Question q = new Question((String)data[n]);
                Object[] ans = ((Object[])data[n + 1]);
                for (int a = 0; a < ans.Length; a += 2)
                {
                    q.AddAnswer((String)ans[a], (int)ans[a + 1]);
                }

                round.normal[n / 2] = q;
            }
            for (int f = 10; f < 20; f += 2)
            {
                Question q = new Question((String)data[f]);
                Object[] ans = ((Object[])data[f + 1]);
                for (int a = 0; a < ans.Length; a += 2)
                {
                    q.AddAnswer((String)ans[a], (int)ans[a + 1]);
                }

                round.final[(f - 10) / 2] = q;
            }
        }

        internal void NextPage(params Object[] args)
        {
            List<int?> points;
            switch (currentPage)
            {
                case 0:
                    LoadData((int)args[0]);                    
                    break;

                case 1:
                    winnerPoints = Math.Max(pointsA, pointsB);
                    break;

                case 2:
                    points = args[0] as List<int?>;
                    points.ForEach(delegate(int? i) { if (i.HasValue) winnerPoints += i.Value; });
                    break;

                case 3:
                    points = args[0] as List<int?>;
                    points.ForEach(delegate(int? i) { if (i.HasValue) winnerPoints += i.Value; });
                    this.client.LoadLastPage();
                    return;
            }

            MainFrame.Navigate(pages[++currentPage]);
        }

        #region Data
        static Object[] data1 = new Object[]
        {
            "Najbardziej popularne imię męskie",
            new Object[]
            {
                "Paweł", 25,
                "Mateusz", 24,
                "Adam", 16,
                "Piotr", 15,
                "Maciej", 10,
                "Tomek", 10
            },
            "Jaki przedmiot szkolny przydaje się w życiu.",
            new Object[]
            {
                "Kanapka", 33,
                "Matematyka", 17,
                "J. Angielski", 16,
                "W-F", 15,
                "Żaden", 11,
                "PO", 7
            },
            "O czym najczęściej zapominamy?",
            new Object[] 
            {
                "Zadanie domowe", 48,
                "Klucze", 15,
                "Urodziny", 12,
                "Sprawdzian", 9,
                "Obowiązki domowe", 8,
                "Mundurek", 7
            },
            "Co robimy, gdy jesteśmy smutni?",
            new Object[] 
            {
                "Płaczemy", 34,
                "Jemy", 25,
                "Pijemy (Kubusia)", 20,
                "Słuchamy muzyki", 14,
                "Uczymy się", 7
            },
            "Wymień książkę napisaną przez Henryka Sienkiewicza",
            new Object[] 
            {
                "Potop", 41,
                "W pustyni i w puszczy", 36,
                "Quo Vadis", 23
            },

            "Wymień najcieplejsze państwo w Europie",
            new Object[] 
            {
                "Hiszpania", 57,
                "Włochy", 22,
                "Grecja", 9,
                "Rosja", 9,
                "Polska", 3
            },
            "Podaj tytuł najbardziej znanej polskiej komedii.",
            new Object[] 
            {
                "Chłopaki nie płaczą", 31,
                "Seksmisja", 23,
                "Sami Swoi", 21,
                "Miś", 17,
                "Dzień Świra", 7
            },
            "Podaj nazwę zielonego owocu",
            new Object[] 
            {
                "Limonka", 30,
                "Kiwi", 27,
                "Jabłko", 25,
                "Niedojrzały banan", 14,
                "Awokado", 4
            },
            "Co musisz zabrać ze sobą w góry?",
            new Object[] 
            {
                "Plecak", 35,
                "Buty", 29,
                "Prowiant", 25,
                "Kompas", 8,
                "Telefon", 3
            },
            "Jakie znasz drapieżne zwierze?",
            new Object[] 
            {
                "Lew", 34,
                "Tygrys", 25,
                "Wilk", 21,
                "Gepard", 15,
                "Wąż", 5
            }
        };

        Object[] data2 = new Object[]
        {
            "Więcej niż jedno zwierzę.",
            new Object[]
            {
                "Lama", 27,
                "Owca", 20,
                "Stado", 18,
                "Zoo", 18,
                "Zwierzęta", 9,
                "Wataha", 8
            },
            "Z czym jemy kanapkę?",
            new Object[] 
            {
                "Ser", 28,
                "Masło", 22,
                "Szynka", 21,
                "Chleb", 18,
                "Nutella", 7,
                "Sałata", 4
            },
            "Ma  skrzydła i nie lata.", 
            new Object[] 
            {
                "Pingwin", 39,
                "Kura", 32,
                "Struś", 10,
                "Tupolew", 7,
                "Adam Małysz", 7,
                "Kiwi", 5
            },
            "Bez jakiego urządzenia nie mógłbyś się obyć?",
            new Object[] 
            {
                "Telefon", 57,
                "Komputer", 30,
                "Urządzenia RTV", 7,
                "Odtwarzacz muzyki", 3,
                "Młot pneumatyczny", 3
            },
            "Co robią ludzie, kiedy myślą, że nikt ich nie widzi?",
            new Object[] 
            {
                "Dłubią w nosie", 63,
                "Masturbują się", 20,
                "Paczą", 18
            },

            "Podaj nazwisko najbardziej znanego polskiego aktora",
            new Object[] 
            {
                "Pazura", 33,
                "Adamczyk", 18,
                "Linda", 17,
                "Karolak", 16,
                "Żmijewski", 16
            },
            "Który kwiat ma najmocniejszy zapach?",
            new Object[] 
            {
                "Róża", 37,
                "Lilia", 20,
                "Bez", 17,
                "Konwalia", 14,
                "Konopie", 12
            },
            "Najbardziej popularne imię żeńskie?",
            new Object[] 
            {
                "Anna", 53,
                "Ola", 18,
                "Kasia", 14,
                "Ewa", 8,
                "Maria", 7
            },
            "Podaj jeden z kolorów.",
            new Object[] 
            {
                "Niebieski", 27,
                "Zielony", 21,
                "Różowy", 18,
                "Czarny", 17,
                "Czerwony", 17,
            },
            "Owoc z którego wyciskamy nektar.",
            new Object[] 
            {
                "Pomarańcza", 56,
                "Brzoskwinia", 16,
                "Cytryna", 13,
                "Jabłko", 10,
                "Czekolada", 5
            }
        };

        Object[] data3 = new Object[]
        {
            "Najczęściej cytowana „gwiazda”?",
            new Object[] 
            {
                "Doda", 44,
                "Betlejemska", 15,
                "P. Coelho", 12,
                "J. Krupa", 11,
                "Pablo z Mielina", 10,
                "Lech Roch Pawlak", 8,
            },
            "Jak się nazywa zwierzę żyjące na ziemi, które osiąga 30 metrów długości?",
            new Object[] 
            {
                "Żyrafa", 32,
                "Waleń", 25,
                "Wąż", 18,
                "Smok", 16,
                "Chomik", 7,
                "Jamnik", 6
            },
            "Wymień najbardziej znany środek transportu",
            new Object[] 
            {
                "Samochód", 57,
                "Autobus", 13,
                "Pociąg", 12,
                "Nogi", 8,
                "Rower", 6,
                "Hulajnoga", 5,
            },
            "Czym się bawią dzieci w piaskowinicy?",
            new Object[] 
            {
                "Łopatkami", 46,
                "Piaskiem", 34,
                "Kupami", 8,
                "Zabawkami", 7,
                "Rękoma", 5
            },
            "Podaj przykład państwa bez dostępu do morza.",
            new Object[] 
            {
                "Czechy", 67,
                "Polska", 20,
                "Watykan", 13,
            },

            "Podaj niebezpieczny zawód.",
            new Object[] 
            {
                "Strażak", 27,
                "Nauczyciel", 20,
                "Górnik", 18,
                "Stajenny Augiasza", 18,
                "Policjant", 9,
                "Śmieciarz", 8
            },

            "Do czego dodajemy czekoladę?",
            new Object[] 
            {
                "Ciasta", 58,
                "Lodów", 19,
                "Do wszystkiego", 9,
                "Do kawy", 8,
                "Do zupy", 6
            },
            "Jaka dyscyplina jest najsłynniejsza na świecie",
            new Object[] 
            {
                "Sportowa", 36,
                "Piłka nożna", 32,
                "Opierdaling", 12,
                "Biegi", 12,
                "Gry w bierki", 8
            },
            "Najcięższa rzecz w twoim domu to",
            new Object[] 
            {
                "Szafa", 41,
                "Telewizor", 21,
                "Ściana", 13,
                "Teściowa", 13,
                "Auto", 12
            },
            "Co kojarzy ci się z Francją?",
            new Object[] 
            {
                "Paryż", 33,
                "Żaby", 27,
                "Wino", 18,
                "Ślimaki", 12,
                "Wieża Eiffla", 10
            }
        };

        #endregion


    }
}
