using System.Threading.Tasks;
using Avalonia.Controls;

namespace AvaloniaColorNames;

internal abstract class Toolkit
{
    public static void MessageBox(string content)
    {
        Window window = new() { Content = new TextBlock { Text = content } };
        window.Classes.Add("MessageBox");
        window.Tapped += (_, _) => window.Close();
        window.Loaded += async (_, _) =>
        {
            await Task.Delay(content.Length * 200);
            window.Close();
        };
        window.Show();
    }
}