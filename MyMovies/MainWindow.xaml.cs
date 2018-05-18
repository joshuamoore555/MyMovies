using System;
using System.Collections.Generic;
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
using MyMovies.MovieRetriever;

using System.IO;
using Microsoft.VisualBasic.FileIO;
using Shell32;
using WMPLib;
using System.Text.RegularExpressions;

namespace MyMovies
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        Dictionary<string, string> names;
        Dictionary<string, string> filepaths;
        List<string> files;

        public MainWindow()
        {
            InitializeComponent();
            Max.Text = "240";
            Min.Text = "15";
            
            UpdateMovieList(15, 2000,"");

        }

        public void ClearLists()
        {
            names.Clear();
            filepaths.Clear();
            files.Clear();
            List.Items.Clear();
        }

        private void UpdateMovieList(int min, int max, string search)
        {
            names = new Dictionary<string, string>();
            filepaths = new Dictionary<string, string>();


            FileRetriever fileRetriever = new FileRetriever();
            files = fileRetriever.GetMovies(min, max);

            foreach (var file in files)
            {
                var name = System.IO.Path.GetFileNameWithoutExtension(file);
                name = name.Replace(".", " ");
                if (!filepaths.ContainsKey(name))
                {
                    if (search != null && search != "")
                    {
                        bool contains = StringExtensions.Contains(name, search, StringComparison.OrdinalIgnoreCase);
                        if (contains)
                        {
                            filepaths.Add(name, file);
                            List.Items.Add(name);
                            names.Add(file, name);
                        }
                    }
                    else
                    {
                        filepaths.Add(name, file);
                        List.Items.Add(name);
                        names.Add(file, name);
                    }
                }
            }
            var buttonList = files.Take(6);
            var buttons = new List<Button>{ Button1, Button2, Button3, Button4, Button5, Button6 };
            var text = new List<TextBlock> {Text1,Text2,Text3,Text4,Text5,Text6 };
            for(int i = 0; i < buttons.Count; i++)
            {

                var name = System.IO.Path.GetFileNameWithoutExtension(buttonList.ElementAt(i));
                text[i].Text = name ;
            }
        }

        

        public void LoadMovie(string name)
        {
            Title.Text = name;
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            var files = Title.Text;
            var splitFiles = files.Split('.');
            for(int i = 0; i < splitFiles.Length-1; i++)
            {
                string s = splitFiles[i];
                System.Diagnostics.Process.Start(filepaths[s]);
            }
  
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(List.SelectedItem != null)
            {
                var items = List.SelectedItems;

                StringBuilder s = new StringBuilder();
                foreach (var item in items)
                {
                    s.Append(item.ToString() + ".");
                }
                Title.Text = s.ToString();
            }
            else
            {
                List.SelectedItem = "";
                Title.Text = "";
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (filepaths.ContainsKey(Title.Text))
            {
                var filepath = filepaths[Title.Text];
                files.Remove(filepath);
                names.Remove(filepath);
                FileSystem.DeleteFile(filepath, UIOption.AllDialogs, RecycleOption.SendToRecycleBin, UICancelOption.ThrowException);
                List.Items.Remove(Title.Text);
                filepaths.Remove(Title.Text);
                List.Items.Refresh();
                Title.Text = "Deleted";
            }
            else
            {
                Title.Text = "Does not exist";
            }
            
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            int x = 0;
            int y = 0;
            string search = SearchBar.Text;
            if (Int32.TryParse(Min.Text, out x) && Int32.TryParse(Max.Text, out y))
            {
                Status.Text = "Minimum Time of " + Min.Text + "\nMaximum Time of " + Max.Text;
                ClearLists();
                UpdateMovieList(x, y, search);
                List.Items.Refresh();

            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private void MovieChecked(object sender, RoutedEventArgs e)
        {
            Min.Text = "80";
            Max.Text = "340";
        }

        private void TVShowChecked(object sender, RoutedEventArgs e)
        {
          
            Min.Text = "10";
            Max.Text = "79";
        }


    }
}
