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
//using System.Windows.Media;


namespace DataUploaderApp;

public partial class AddWindow : Window
{
    private Article _RedArticle = null;
    private ObservableCollection<Bitmap> _Photos;

    private string? _PictureFile = null;//_RedClient != null ? _RedClient.Photo : null; //�����������, ������� ���������� ����� ������ (���� ��� � ���� ����, ����� null)
    private string? _SelectedImage = null; //��������� �����������

    private string _SelectedAudioPath = null;
    private string _SelectedAudioName = null;
   
    public AddWindow()
    {
        _RedArticle = new Article();
        InitializeComponent();
        cbox_cathegory.ItemsSource = Helper.DefaultDbContext.Cathegories.ToList();
        grid_addWindow.DataContext = _RedArticle;
        tblock_header.Text = "�������� ������";
        btn_confirm.Content = "��������";
        cbox_cathegory.SelectedIndex = 0;



        //_Photos = new ObservableCollection<Bitmap>(_RedArticle.IdPhotos.Select(x => new Bitmap($"Assets/{x.Photo1}")));
        //lbox_images.ItemsSource = _Photos.ToList();
    }

    public AddWindow(Article redArticle)
    {
        _RedArticle = redArticle;
        InitializeComponent(); 
        cbox_cathegory.ItemsSource = Helper.DefaultDbContext.Cathegories.ToList();
        grid_addWindow.DataContext = _RedArticle;
        tblock_header.Text = "������������� ������";
        btn_confirm.Content = "���������";
        btn_del.IsVisible = true;

        lbox_images.ItemsSource = _RedArticle.IdPhotos.ToList();

        _Photos = new ObservableCollection<Bitmap>(_RedArticle.IdPhotos.Select(x => new Bitmap($"Assets/{x.Photo1}")));
        //lbox_images.ItemsSource = _Photos.ToList();
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

    private void Button_Confirm(object? sender, Avalonia.Interactivity.RoutedEventArgs e)//��������� ��� ��������
    {
        if (tbox_name.Text != null && tbox_description.Text != null && tbox_name.Text != "" && tbox_description.Text != "")
        {
            if (_RedArticle.Id != 0)
            {
                if(_SelectedAudioPath != null && _SelectedAudioName != null)
                    System.IO.File.Copy(_SelectedAudioPath, $"Assets/{_SelectedAudioName}", true);
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

    private async void Button_AddImage(object? sender, Avalonia.Interactivity.RoutedEventArgs e)//���������� ����
    {
        OpenFileDialog dialog = new(); //�������� ����������
        dialog.Filters.Add(fileFilterPictures); //���������� �������

        string[] result = await dialog.ShowAsync(this); //����� �����
        if (result == null || result.Length == 0 || new System.IO.FileInfo(result[0]).Length > 2000000)
            return;//���� ������� ��������� ��� ������ ����� ����� ��������� 2 ��, �� �������� �� ����� �������

        string imagePath = result[0];
        string imageName = System.IO.Path.GetFileName(imagePath); //��������� ����� �����

        _Photos.Add(new Bitmap(imagePath));
        
        //System.IO.File.Copy(result[0], $"Assets/{imageName}", true); //����������� ����� � ����� �������



    }

    private readonly FileDialogFilter fileFilterPictures = new() //������ ��� ����������
    {
        Extensions = new List<string>() { "jpg", "png" }, //��������� ����������, ������������ � ����������
        Name = "����� �����������" //���������
    };

    private readonly FileDialogFilter fileFilterAudio = new() //������ ��� ����������
    {
        Extensions = new List<string>() { "mp3", "wav" }, //��������� ����������, ������������ � ����������
        Name = "����� �����������" //���������
    };

    private void Button_Click_3(object? sender, Avalonia.Interactivity.RoutedEventArgs e)//�������� �����������
    {
    }

    private void Button_Delete(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _RedArticle.IdPhotos.Clear();
        Helper.DefaultDbContext.Articles.Remove(_RedArticle);
        Helper.DefaultDbContext.SaveChanges();
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        Close();
    }

    private async void Button_AddAudio(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        OpenFileDialog dialog = new(); //�������� ����������
        dialog.Filters.Add(fileFilterAudio); //���������� �������

        string[] result = await dialog.ShowAsync(this); //����� �����
        if (result == null || result.Length == 0)
            return;

        _SelectedAudioPath = result[0];
        _SelectedAudioName = System.IO.Path.GetFileName(_SelectedAudioPath); //��������� ����� �����

        btn_playAudio.IsVisible = true;
        btn_deleteAudio.IsVisible = true;
        tblock_previewAudio.IsVisible = true;
        tblock_previewAudio.Text = _SelectedAudioName;
    }

    private void Button_PlayAudio(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        //var player = new MediaPlayer();
        //Process.Start("wmplayer.exe", _SelectedAudioPath);
        System.Diagnostics.Process.Start(new ProcessStartInfo
        {
            FileName = "C:\\Program Files (x86)\\Windows Media Player\\wmplayer.exe",
            Arguments = _SelectedAudioPath
        });
    }
}