using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using DataUploaderApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static Avalonia.Media.Imaging.Bitmap;
using static DataUploaderApp.SavingDate;

namespace DataUploaderApp;

public partial class AddWindow : Window
{
    private Article _RedArticle = null;
    private ObservableCollection<Bitmap> _Photos;

    private string? _PictureFile = null;//_RedClient != null ? _RedClient.Photo : null; //изображение, которое изначально имеет объект (если оно у него есть, иначе null)
    private string? _SelectedImage = null; //выбранное изображение
   
    public AddWindow()
    {
        _RedArticle = new Article();
        InitializeComponent();
        cbox_cathegory.ItemsSource = Helper.DefaultDbContext.Cathegories.ToList();
        grid_addWindow.DataContext = _RedArticle;
        tblock_header.Text = "Добавить запись";
        btn_confirm.Content = "Добавить";
        cbox_cathegory.SelectedIndex = 0;
        _Photos = new ObservableCollection<Bitmap>(_RedArticle.IdPhotos.Select(x => new Bitmap($"Assets/{x.Photo1}")));
        lbox_images.ItemsSource = _Photos.ToList();
    }

    public AddWindow(Article redArticle)
    {
        _RedArticle = redArticle;
        InitializeComponent(); 
        cbox_cathegory.ItemsSource = Helper.DefaultDbContext.Cathegories.ToList();
        grid_addWindow.DataContext = _RedArticle;
        tblock_header.Text = "Редактировать запись";
        btn_confirm.Content = "Сохранить";
        btn_del.IsVisible = true;
        _Photos = new ObservableCollection<Bitmap>(_RedArticle.IdPhotos.Select(x => new Bitmap($"Assets/{x.Photo1}")));
        lbox_images.ItemsSource = _Photos.ToList();
    }


    private void VivodInf()
    {
        tblok_id.Text += _RedArticle.Id;
        tbox_name.Text = _RedArticle.Name;
        tbox_description.Text = _RedArticle.Description;
        tboxlink.Text = _RedArticle.Video;
        
    }
    private void ReturnToMain(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();
    }

    private void Button_Confirm(object? sender, Avalonia.Interactivity.RoutedEventArgs e)//Сохранить или добавить
    {
        if (tbox_name.Text != null && tbox_description.Text != null)
        {
            if (_RedArticle.Id != 0)
            {
                _RedArticle.Name = tbox_name.Text;
                _RedArticle.Description = tbox_description.Text;
                _RedArticle.Video = tboxlink.Text;
                _RedArticle.IdCathegorie = cbox_cathegory.SelectedIndex + 1;
                
                Helper.DefaultDbContext.SaveChanges();
                new MainWindow().Show();
                Close();
            }
            else
            {
                _RedArticle.Id = Helper.DefaultDbContext.Articles.ToList().Max(x => x.Id)+1;
                _RedArticle.AddDate = System.DateOnly.FromDateTime(DateTime.Now);

                Helper.DefaultDbContext.Articles.Add(_RedArticle);

                Helper.DefaultDbContext.SaveChanges();

                new MainWindow().Show();
                Close();
            }
        }
    }

    private async void Button_Click_2(object? sender, Avalonia.Interactivity.RoutedEventArgs e)//добавление фото
    {
        OpenFileDialog dialog = new(); //Открытие проводника
        dialog.Filters.Add(fileFilter); //Применение фильтра

        string[] result = await dialog.ShowAsync(this); //Выбор файла
        if (result == null || result.Length == 0 || new System.IO.FileInfo(result[0]).Length > 2000000)
            return;//Если закрыть проводник или размер файла будет превышать 2 МБ, то картинка не будет выбрана

        string imagePath = result[0];
        string imageName = System.IO.Path.GetFileName(imagePath); //получение имени файла

        _Photos.Add(new Bitmap(imagePath));
        
        //System.IO.File.Copy(result[0], $"Assets/{imageName}", true); //Копирование файла в папку ассетов



    }

    private readonly FileDialogFilter fileFilter = new() //Фильтр для проводника
    {
        Extensions = new List<string>() { "jpg", "png" }, //доступные расширения, отображаемые в проводнике
        Name = "Файлы изображений" //пояснение
    };

    private void Button_Click_3(object? sender, Avalonia.Interactivity.RoutedEventArgs e)//Удаление изображения
    {
    }

    private void Button_Delete(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {

    }
}