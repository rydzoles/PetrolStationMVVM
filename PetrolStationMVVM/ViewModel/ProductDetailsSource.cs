using HtmlAgilityPack;
using PetrolStationMVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Xml;
using System.Net.NetworkInformation;
using System.Windows;

namespace PetrolStationMVVM.ViewModel
{
    public class ProductDetailsSource : INotifyPropertyChanged
    {
        private static IList<HtmlNode> mCrudePathProductsfromWeb { get; set; } //= GetCrudeProductsFromWeb(BaseUrl);
        private const string BaseUrl = "https://www.lotos.pl/144/dla_biznesu/hurtowe_ceny_paliw";
        public static ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();
        public event PropertyChangedEventHandler? PropertyChanged;
        public string NoINternernet { get; set; }
        public double MySelectedItem { get; set; }
        public double mySelectedItem
        {
            get
            {
                MySelectedItem = 8;
                return MySelectedItem; }

            set
            {
                MySelectedItem = value;
                OnPropertyChanged("MySelectedItem");

            }
        }
        public ProductDetailsSource()
        {
            try
            {
                var Web = new HtmlWeb();
                var document = Web.Load(BaseUrl);
                mCrudePathProductsfromWeb = document.QuerySelectorAll("tbody td");
                if (mCrudePathProductsfromWeb == null)
                    SaveProductToTextFile();
            }
            catch (Exception ex)
            {
                NoINternernet = "ładujesz stare dane" + ex.Message;
            }
            finally
            {
                GetProductFromTextFile();
            }
        }
        public void OnPropertyChanged(string pathName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(pathName));
        }
        public ObservableCollection<Product> products
        {
            get
            {
               
                return Products;
            }
            set
            {
                Products = value;
                OnPropertyChanged(nameof(Products));
            }
        }
        public static void SaveProductToTextFile()
        {
            var counter = 0;
            List<string> lines = new List<string>();

            foreach (var tableRow in mCrudePathProductsfromWeb)
            {
                if (counter < 6)
                {
                    lines.Add(tableRow.InnerText.ToString());
                }
                else
                    break;
                counter++;
            }
            File.WriteAllLines("FuelMenu.txt", lines);
        }
        public static ObservableCollection<Product> GetProductFromTextFile()
        {
            var lines = File.ReadLines("FuelMenu.txt");
            bool IsNameOrNofalse = false;
            Product product = new Product();
            foreach (var item in lines)
            {
                if (IsNameOrNofalse == false)
                {
                    IsNameOrNofalse = true;
                    product.Name = item;
                }
                else
                {
                    product.Price = double.Parse(item);
                    var currentPRoduct = new Product
                    {
                        Name = product.Name,
                        Price = product.Price,
                    };
                    Products.Add(currentPRoduct);
                    IsNameOrNofalse = false;
                }
            }
            return Products;
        }
    }
}
