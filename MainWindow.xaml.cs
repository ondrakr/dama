using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wpf_DAMA
{
    public partial class MainWindow : Window
    {
        private Ellipse vybrane_pole;
        private bool bily_hrac = true;
        private bool[,] sachovniceCerna = new bool[8, 8];
        private bool[,] sachovniceBila = new bool[8, 8];
        private bool[,] sachovnice = new bool[8, 8];
        private bool[,] damaCerna = new bool[8, 8];
        private bool[,] damaBila = new bool[8, 8];

        public MainWindow()
        {
            InitializeComponent();
            VytvorPole();
        }

        private void VytvorPole()
        {
            //Černý
            for (int radek = 0; radek < 3; radek++)
            {
                int prvniSloupek = 0;
                if (radek % 2 == 0)
                {
                    prvniSloupek = 1;
                }
                for (int sloupek = prvniSloupek; sloupek < 8; sloupek += 2)
                {
                    sachovniceCerna[radek, sloupek] = true;
                }
            }

            // Bílý
            for (int radek = 5; radek < 8; radek++)
            {   
                int prvniSloupek = 0;
                if (radek % 2 == 0)
                {
                    prvniSloupek = 1;
                }
                for (int sloupek = prvniSloupek; sloupek < 8; sloupek += 2)
                {
                    sachovniceBila[radek, sloupek] = true;
                }
            }
        }

        private void Vyber(object sender, MouseButtonEventArgs e)
        {
            vybrane_pole = sender as Ellipse;
            Grid pozice = vybrane_pole.Parent as Grid;

            int radek = Grid.GetRow(vybrane_pole);
            int sloupek = Grid.GetColumn(vybrane_pole);

            if ((bily_hrac && sachovniceBila[radek, sloupek]) || (!bily_hrac && sachovniceCerna[radek, sloupek]) ||
                (bily_hrac && damaBila[radek, sloupek]) || (!bily_hrac && damaCerna[radek, sloupek]))
            {
                vybrane_pole.Fill = Brushes.Green;

                Ellipse nabidka1 = pozice.FindName("nabidka1") as Ellipse;
                Ellipse nabidka2 = pozice.FindName("nabidka2") as Ellipse;

                if (nabidka1 != null)
                    nabidka1.Visibility = Visibility.Hidden;

                if (nabidka2 != null)
                    nabidka2.Visibility = Visibility.Hidden;

                int maxRadek = sachovniceBila.GetLength(0);
                int maxSloupek = sachovniceBila.GetLength(1);

                //nabidka levo
                if (nabidka1 != null && sloupek > 0)
                {
                    nabidka1.Visibility = Visibility.Visible;
                    if (bily_hrac)
                    {
                        if (damaBila[radek, sloupek] || radek == 0)
                        {
                            nabidka1.Visibility = Visibility.Hidden;
                            if (radek - 1 >= 0 && sloupek - 1 >= 0 && !sachovniceBila[radek - 1, sloupek - 1] && !sachovniceCerna[radek - 1, sloupek - 1])
                            {
                                nabidka1.Visibility = Visibility.Visible;
                                Grid.SetRow(nabidka1, radek - 1);
                                Grid.SetColumn(nabidka1, sloupek - 1);
                            }
                        }
                        else
                        {
                            if (radek - 2 >= 0 && sloupek - 2 >= 0 &&
                                sachovniceCerna[radek - 1, sloupek - 1] && !sachovniceCerna[radek - 2, sloupek - 2])
                            {
                                Grid.SetRow(nabidka1, radek - 2);
                                Grid.SetColumn(nabidka1, sloupek - 2);
                            }
                            else if ((radek - 2 >= 0 && sloupek - 2 >= 0 && sachovniceCerna[radek - 1, sloupek - 1] && sachovniceCerna[radek - 2, sloupek - 2]) ||
                                     (radek - 1 >= 0 && sloupek - 1 >= 0 && sachovniceCerna[radek - 1, sloupek - 1]) ||
                                     (radek - 1 >= 0 && sloupek - 1 >= 0 && sachovniceBila[radek - 1, sloupek - 1]) ||
                                     (radek - 2 >= 0 && sloupek - 2 >= 0 && sachovniceCerna[radek - 2, sloupek - 2] && sloupek >= 2 && sachovniceCerna[radek - 1, sloupek - 1]))
                            {
                                nabidka1.Visibility = Visibility.Hidden;
                            }
                            else if (radek - 1 >= 0 && sloupek - 1 >= 0)
                            {
                                Grid.SetRow(nabidka1, radek - 1);
                                Grid.SetColumn(nabidka1, sloupek - 1);
                            }
                        }
                    }
                    else //hraje cerny hrac
                    {
                        if (damaCerna[radek, sloupek] || radek == maxRadek - 1)
                        {
                            nabidka1.Visibility = Visibility.Hidden;
                            if (radek + 1 < maxRadek && sloupek - 1 >= 0 && !sachovniceBila[radek + 1, sloupek - 1] && !sachovniceCerna[radek + 1, sloupek - 1])
                            {
                                nabidka1.Visibility = Visibility.Visible;
                                Grid.SetRow(nabidka1, radek + 1);
                                Grid.SetColumn(nabidka1, sloupek - 1);
                            }
                        }
                        else
                        {
                            if (radek + 2 < maxRadek && sloupek - 2 >= 0 &&
                                sachovniceBila[radek + 1, sloupek - 1] && !sachovniceBila[radek + 2, sloupek - 2])
                            {
                                Grid.SetRow(nabidka1, radek + 2);
                                Grid.SetColumn(nabidka1, sloupek - 2);
                            }
                            else if ((radek + 2 < maxRadek && sloupek - 2 >= 0 && sachovniceBila[radek + 1, sloupek - 1] && sachovniceBila[radek + 2, sloupek - 2]) ||
                                     (radek + 1 < maxRadek && sloupek - 1 >= 0 && sachovniceBila[radek + 1, sloupek - 1]) ||
                                     (radek + 1 < maxRadek && sloupek - 1 >= 0 && sachovniceCerna[radek + 1, sloupek - 1]) ||
                                     (radek + 2 < maxRadek && sloupek - 2 >= 0 && sachovniceBila[radek + 2, sloupek - 2] && sachovniceBila[radek + 1, sloupek - 1] && sachovniceCerna[radek + 2, sloupek - 2] && sloupek >= 2))
                            {
                                nabidka1.Visibility = Visibility.Hidden;
                            }
                            else if (radek + 1 < maxRadek && sloupek - 1 >= 0)
                            {
                                Grid.SetRow(nabidka1, radek + 1);
                                Grid.SetColumn(nabidka1, sloupek - 1);
                            }
                        }
                    }
                }

                //nabidka pravo
                if (nabidka2 != null && sloupek < maxSloupek - 1)
                {
                    nabidka2.Visibility = Visibility.Visible;
                    if (bily_hrac)
                    {
                        if (damaBila[radek, sloupek] || radek == 0)
                        {
                            nabidka2.Visibility = Visibility.Hidden;
                            if (radek - 1 >= 0 && sloupek + 1 < maxSloupek && !sachovniceBila[radek - 1, sloupek + 1] && !sachovniceCerna[radek - 1, sloupek + 1])
                            {
                                nabidka2.Visibility = Visibility.Visible;
                                Grid.SetRow(nabidka2, radek - 1);
                                Grid.SetColumn(nabidka2, sloupek + 1);
                            }
                        }
                        else
                        {
                            if (radek - 2 >= 0 && sloupek + 2 < maxSloupek &&
                                sachovniceCerna[radek - 1, sloupek + 1] && !sachovniceCerna[radek - 2, sloupek + 2])
                            {
                                Grid.SetRow(nabidka2, radek - 2);
                                Grid.SetColumn(nabidka2, sloupek + 2);
                            }
                            else if ((radek - 2 >= 0 && sloupek + 2 < maxSloupek && sachovniceCerna[radek - 1, sloupek + 1] && sachovniceCerna[radek - 2, sloupek + 2]) ||
                                     (radek - 1 >= 0 && sloupek + 1 < maxSloupek && sachovniceCerna[radek - 1, sloupek + 1]) ||
                                     (radek - 1 >= 0 && sloupek + 1 < maxSloupek && sachovniceBila[radek - 1, sloupek + 1]) ||
                                     (radek - 2 >= 0 && sloupek + 2 < maxSloupek && sachovniceCerna[radek - 2, sloupek + 2] && sachovniceCerna[radek - 1, sloupek + 1]))
                            {
                                nabidka2.Visibility = Visibility.Hidden;
                            }
                            else if (radek - 1 >= 0 && sloupek + 1 < maxSloupek)
                            {
                                Grid.SetRow(nabidka2, radek - 1);
                                Grid.SetColumn(nabidka2, sloupek + 1);
                            }
                        }
                    }
                    else //hraje cerny hrac
                    {
                        if (damaCerna[radek, sloupek] || radek == maxRadek - 1)
                        {
                            nabidka2.Visibility = Visibility.Hidden;
                            if (radek + 1 < maxRadek && sloupek + 1 < maxSloupek && !sachovniceBila[radek + 1, sloupek + 1] && !sachovniceCerna[radek + 1, sloupek + 1])
                            {
                                nabidka2.Visibility = Visibility.Visible;
                                Grid.SetRow(nabidka2, radek + 1);
                                Grid.SetColumn(nabidka2, sloupek + 1);
                            }
                        }
                        else
                        {
                            if (radek + 2 < maxRadek && sloupek + 2 < maxSloupek &&
                                sachovniceBila[radek + 1, sloupek + 1] && !sachovniceBila[radek + 2, sloupek + 2])
                            {
                                Grid.SetRow(nabidka2, radek + 2);
                                Grid.SetColumn(nabidka2, sloupek + 2);
                            }
                            else if ((radek + 2 < maxRadek && sloupek + 2 < maxSloupek && sachovniceBila[radek + 1, sloupek + 1] && sachovniceBila[radek + 2, sloupek + 2]) ||
                                     (radek + 1 < maxRadek && sloupek + 1 < maxSloupek && sachovniceBila[radek + 1, sloupek + 1]) ||
                                     (radek + 1 < maxRadek && sloupek + 1 < maxSloupek && sachovniceCerna[radek + 1, sloupek + 1]) ||
                                     (radek + 2 < maxRadek && sloupek + 2 < maxSloupek && sachovniceBila[radek + 2, sloupek + 2] && sachovniceBila[radek + 1, sloupek + 1]))
                            {
                                nabidka2.Visibility = Visibility.Hidden;
                            }
                            else if (radek + 1 < maxRadek && sloupek + 1 < maxSloupek)
                            {
                                Grid.SetRow(nabidka2, radek + 1);
                                Grid.SetColumn(nabidka2, sloupek + 1);
                            }
                        }
                    }
                }

            }
        }

        private void Posun(object sender, MouseButtonEventArgs e)
        {
            Ellipse vybrana_nabidka = sender as Ellipse;
            if (vybrana_nabidka != null && vybrane_pole != null)
            {
                Grid pozice = vybrana_nabidka.Parent as Grid;

                if (pozice != null)
                {
                    int radek = Grid.GetRow(vybrana_nabidka);
                    int sloupek = Grid.GetColumn(vybrana_nabidka);

                    int stary_radek = Grid.GetRow(vybrane_pole);
                    int stary_sloupek = Grid.GetColumn(vybrane_pole);

                    if (Math.Abs(radek - stary_radek) == 2 && Math.Abs(sloupek - stary_sloupek) == 2)
                    {
                        int preskoceny_radek = (radek + stary_radek) / 2;
                        int preskoceny_sloupek = (sloupek + stary_sloupek) / 2;
                        Zmizeni(preskoceny_radek, preskoceny_sloupek, pozice);
                    }

                    if (bily_hrac)
                    {
                        sachovniceBila[stary_radek, stary_sloupek] = false;
                        sachovniceBila[radek, sloupek] = true;
                        if (radek == 0)
                        {
                            damaBila[radek, sloupek] = true;
                        }
                    }
                    else
                    {
                        sachovniceCerna[stary_radek, stary_sloupek] = false;
                        sachovniceCerna[radek, sloupek] = true;
                        if (radek == 7)
                        {
                            damaCerna[radek, sloupek] = true;
                        }
                    }

                    Grid.SetRow(vybrane_pole, radek);
                    Grid.SetColumn(vybrane_pole, sloupek);

                    if (vybrane_pole.Name.Contains("cerne"))
                        vybrane_pole.Fill = Brushes.Black;

                    if (vybrane_pole.Name.Contains("bile"))
                        vybrane_pole.Fill = Brushes.White;

                    Ellipse nabidka1 = pozice.FindName("nabidka1") as Ellipse;
                    Ellipse nabidka2 = pozice.FindName("nabidka2") as Ellipse;

                    if (nabidka1 != null)
                        nabidka1.Visibility = Visibility.Hidden;

                    if (nabidka2 != null)
                        nabidka2.Visibility = Visibility.Hidden;

                    TextBlock hrac = FindName("Hrac") as TextBlock;

                    if (bily_hrac)
                    {
                        hrac.Text = "Černá";
                    }
                    else
                    {
                        hrac.Text = "Bílá";
                    }

                    bily_hrac = !bily_hrac;
                }
            }
        }

        private void Zmizeni(int radek, int sloupek, Grid pozice)
        {
            Ellipse preskocena = null;

            foreach (UIElement element in pozice.Children)
            {
                if (element is Ellipse)
                {
                    Ellipse pole = element as Ellipse;
                    if (Grid.GetRow(pole) == radek && Grid.GetColumn(pole) == sloupek)
                    {
                        preskocena = pole;
                        break;
                    }
                }
            }

            if (preskocena != null)
            {
                pozice.Children.Remove(preskocena);

                if (bily_hrac)
                {
                    sachovniceCerna[radek, sloupek] = false;
                }
                else
                {
                    sachovniceBila[radek, sloupek] = false;
                }
            }
        }
    }
}
