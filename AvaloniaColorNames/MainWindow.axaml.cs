using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Media;
using DrawingColor = System.Drawing.Color;

namespace AvaloniaColorNames;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        const int rgb = 180;
        var infos = typeof(Colors).GetProperties();

        // Get list
        var button_infos = (from info in infos
            let color = Color.Parse(info.Name)
            select new ButtonInfo
            {
                Index = color.R * 65536 + color.G * 256 + color.B,
                Hue = DrawingColor.FromName(info.Name).GetHue(),
                Name = info.Name,
                Background = new SolidColorBrush(color),
                Foreground = color is { R: < rgb, G: < rgb } or { R: < rgb, B: < rgb } or { G: < rgb, B: < rgb }
                    ? Brushes.White
                    : Brushes.Black
            }).ToList();

        // Order by
        button_infos = button_infos
            .OrderBy(info => info.Hue)
            .ThenBy(info => info.Index)
            .ToList();

        // Generate grid
        var length = Math.Sqrt(infos.Length);
        var width = (int)Math.Ceiling(length);
        for (var i = 0; i < width; i++)
        {
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
        }

        // Generate button
        var index = 0;
        foreach (var info in button_infos)
        {
            Button btn = new()
            {
                Content = info.Name,
                Foreground = info.Foreground,
                Background = info.Background
            };
            btn.Click += (_, _) =>
            {
                Clipboard.SetTextAsync(info.Name);
                Toolkit.MessageBox(info.Name);
            };
            Grid.SetRow(btn, index / width);
            Grid.SetColumn(btn, index % width);
            grid.Children.Add(btn);
            index++;
        }
    }
}

public class ButtonInfo
{
    public SolidColorBrush Background;
    public IImmutableSolidColorBrush Foreground;
    public float Hue;
    public int Index;
    public string Name;
}