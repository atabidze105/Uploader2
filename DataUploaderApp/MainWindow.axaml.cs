using Avalonia.Controls;
using DataUploaderApp.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace DataUploaderApp
{
    public partial class MainWindow : Window
    {
        private List<Article> _FoundedArticles = [];
        private List<Article> _Articles = Helper.DefaultDbContext.Articles
            .Include(x => x.IdPhotos)
            .Include(x => x.IdCathegorieNavigation)
            .ToList();
        private List<Cathegory> _Cathegories = Helper.DefaultDbContext.Cathegories.ToList();

        public MainWindow()
        {
            InitializeComponent();
            cbox_filtration.SelectedIndex = 0;
            cbox_selection.SelectedIndex = 0;
            tbox_searchbar.Text = "";
            Listins(_Articles);
            CboxSelectionInit();
            Searching();
        }

        private void OpenAddWindow(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            AddWindow addWindow = new AddWindow();
            addWindow.Show();
            this.Close();
        }
        private void Listins(List<Article> list)
        {
            lbox_items.ItemsSource = list.ToList();
        }

        private void CboxSelectionInit()
        {
            foreach (Cathegory c in _Cathegories)
            {
                cbox_selection.Items.Add(c.Name);
            }
        }

        private List<Article> Filtration(List<Article> articles)
        {
            List<Article> Filtrated = [];
            Filtrated.AddRange(articles);
            switch (cbox_filtration.SelectedIndex)
            {
                case 1:
                    return Filtrated.OrderBy(x => x.Name).ToList();
                case 2:
                    return Filtrated.OrderByDescending(x => x.Name).ToList();
                case 3:
                    return Filtrated.OrderBy(x => x.Id).ToList();
                case 4:
                    return Filtrated.OrderByDescending(x => x.Id).ToList();
                case 5:
                    return Filtrated.OrderByDescending(x => x.AddDate).ToList();
                case 6:
                    return Filtrated.OrderBy(x => x.AddDate).ToList();

            }
            return Filtrated;
        }

        private List<Article> SearchingBar()
        {
            if (tbox_searchbar.Text != "" && tbox_searchbar.Text != null)
            {
                List<Article> unsortedProducts = [];
                unsortedProducts.AddRange(_FoundedArticles.Count > 0 || cbox_selection.SelectedIndex != 0 ? Filtration(_FoundedArticles) : Filtration(_Articles));
                List<Article> productsBuffer = [];
                foreach (Article product in unsortedProducts)
                {
                    if (product.Name.Trim().ToLower().Contains(tbox_searchbar.Text.Trim().ToLower()))
                        productsBuffer.Add(product);
                }
                _FoundedArticles.Clear();
                _FoundedArticles.AddRange(productsBuffer);
             
                return Filtration(_FoundedArticles);
            }
            else
            {
                return _FoundedArticles.Count > 0 || cbox_selection.SelectedIndex != 0 ? Filtration(_FoundedArticles) : Filtration(_Articles); 
            }
        }

        private void Selection(List<Article> FiltratedArticle)
        {
            if (cbox_selection.SelectedItem != null)
            {
                _FoundedArticles.Clear();
                string? selectedItem = (cbox_selection.SelectedItem!.ToString())!;
                if (selectedItem != "По умолчанию")
                    _FoundedArticles.AddRange(FiltratedArticle.Where(x => x.IdCathegorieNavigation.Name == selectedItem));
            }
        }

        private void TextBox_KeyUp(object? sender, Avalonia.Input.KeyEventArgs e)
        {
            Searching();
        }

        private void Panel_DoubleTapped(object? sender, Avalonia.Input.TappedEventArgs e)
        {
            var article = lbox_items.SelectedItem as Article;
            new AddWindow(article).Show();
            Close();
        }

        private void ComboBox_Filtration(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
        {
            Searching();
        }

        private void ComboBox_Selection(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
        {
            Searching();
        }

        private void Searching()
        {
            List<Article> filtrated = [];
            filtrated.AddRange(Filtration(_Articles));
            Selection(filtrated);
            Listins(SearchingBar());
            tblock_itemsCount.Text = $"{((_FoundedArticles.Count > 0 || cbox_selection.SelectedIndex != 0 || tbox_searchbar.Text != "") ? _FoundedArticles.Count : _Articles.Count )}/{_Articles.Count}";
        }
    }
}