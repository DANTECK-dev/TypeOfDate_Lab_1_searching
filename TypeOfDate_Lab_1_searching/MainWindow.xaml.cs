using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

namespace TypeOfDate_Lab_1_searching
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public static class Extensions
    {
        public static T[] SubArray<T>(this T[] array, int offset, int length)
        {
            return array.Skip(offset)
                        .Take(length)
                        .ToArray();
        }
    }

    public partial class MainWindow : Window
    {
        private int _count = 0;
        private int _search = 0;

        public MainWindow()
        {
            InitializeComponent();
            //Def();
        }

        private void Show_Error(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Def()
        {
            if (Sequential_Search_L != null) Sequential_Search_L.Content = "None";
            if (Binary_Search_L != null) Binary_Search_L.Content = "None";
            if (Sequential_Massive_L != null) Sequential_Massive_L.Content = "None";
            if (Binary_Massive_L != null) Binary_Massive_L.Content = "None";
            _count = 0;
            _search = 0;
        }

        private void Count_TB_TextChanged(object sender, TextChangedEventArgs e)
        {
            Def();

        }

        private void Searching_TB_TextChanged(object sender, TextChangedEventArgs e)
        {
            Def();

        }

        private static void GetRandomArr(ref int[] arr_1, ref int[] arr_2, int min, int max)
        {
            Random random = new Random();
            for (int i = 0; i < arr_1.Length; i++)
            {
                var num = random.Next(min, max);

                if (arr_1.Contains(num))
                {
                    i--;
                }
                else
                {
                    arr_1[i] = num;
                    arr_2[i] = num;
                }
            }
        }

        private int Sequential_Search(ref int[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == _search) 
                    return i;
            }
            return -1;
        }

        /*int FindIndex(List<int> arr, int el)
        {
            if (arr.Count == 0)
            {
                return 0;
            }
            if (arr[arr.Count - 1] < el)
            {
                return arr.Count;
            }
            int i = 0;
            int j = arr.Count;
            while (i + 1 < j)
            {
                int middle = i + (j - i) / 2;
                if (arr[middle] >= el)
                {
                    j = middle;
                }
                else
                {
                    i = middle;
                }
            }
            return arr[i] >= el ? i : j;
        }

        private void Binary_Sort(ref int[] arr)
        {
            List<int> sort = new List<int>();
            int index;
            for (int i = 0; i < arr.Length; i++)
            {
                index = FindIndex(sort, arr[i]);
                sort.Insert(index, arr[i]);
            }
            arr = sort.ToArray();
        }

        private int Binary_Search(ref int[] arr, int first, int last, int searched)
        {
            Binary_Sort(ref arr);

            if (first <= last)
            {
                int mid = (first + last) / 2;
                if (arr[mid] == searched) 
                    return mid;
                if (arr[mid] > searched) return Binary_Search(ref arr, first, mid - 1, searched);
                if (arr[mid] < searched) return Binary_Search(ref arr, mid + 1, last, searched);
            }

            //Show_Error("Искомый элемент не найден (бинарный поиск)");
            return -1;
        }*/

        private void Start_BT_Click(object sender, RoutedEventArgs e)
        {
            Def();

            if (int.TryParse(Count_TB.Text.Replace("_", String.Empty), out _))
            {
                _count = int.Parse(Count_TB.Text.Replace("_", String.Empty));
            }
            else
            {
                Show_Error("Введите количество элементов масива");
                return;
            }

            if (int.TryParse(Searching_TB.Text.Replace("_", String.Empty), out _))
            {
                _search = int.Parse(Searching_TB.Text.Replace("_", String.Empty));
            }
            else
            {
                Show_Error("Введите искомый элемент масива");
                return;
            }

            if (_count <= 0)
            {
                Show_Error("Введите количество элементов больше нуля");
                return;
            }

            if (_search <= 0)
            {
                Show_Error("Введите искомый элемент больше нуля");
                return;
            }

            int[] Sequential_Massive = new int[_count];
            int[] Binary_Massive = new int[_count];

            GetRandomArr(ref Sequential_Massive, ref Binary_Massive, 1, _count*2);

            var stopWatch = Stopwatch.StartNew();
            int sequential_index = Sequential_Search(ref Sequential_Massive);
            stopWatch.Stop();

            Sequential_Search_L.Content = stopWatch.Elapsed + " ns";
            /*
            stopWatch.Restart();
            Binary_Sort(ref Binary_Massive);
            stopWatch.Stop();

            Binary_Search_L.Content = "Сортировка - " + stopWatch.Elapsed + " ns\n";

            stopWatch.Restart();
            int binary_index = Binary_Search(ref Binary_Massive, 0, _count - 1, _search);
            stopWatch.Stop();

            Binary_Search_L.Content += "+ Поиск - " + stopWatch.Elapsed + " ns";
            */

            stopWatch.Restart();
            Array.Sort(Binary_Massive);
            stopWatch.Stop();
            Binary_Search_L.Content = "Сортировка - " + stopWatch.Elapsed + " ns\n";

            stopWatch.Restart();
            int binary_index = Array.BinarySearch(Binary_Massive, _search);
            stopWatch.Stop();
            Binary_Search_L.Content += "Поиск - " + stopWatch.Elapsed + " ns";

            if (sequential_index > 0 || binary_index > 0)
            {
                Sequential_Massive_L.Content = String.Join(',', Sequential_Massive.SubArray(sequential_index - 5, 10));
                Binary_Massive_L.Content = String.Join(',', Binary_Massive.SubArray(binary_index - 5, 10));
            }
            else
            {
                Show_Error("Искомый элемент не найден");
            }
        }
    }
}
