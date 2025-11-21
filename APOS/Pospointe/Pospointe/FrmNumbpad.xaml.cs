using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Pospointe;

/// <summary>
/// Interaction logic for FrmNumbpad.xaml
/// </summary>
public partial class FrmNumbpad : Window
{

    public string returnvalue { get; set; }

    public string reqestmsg { get; set; }
    public string EnteredAmount { get; set; }

    private bool isPlaceholderActive = true;
    public FrmNumbpad()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (reqestmsg != null)
        {
            UserNameDisplay.Text = reqestmsg;
        }
        if (returnvalue != null)
        {
            TxtEnterAmount.Text = returnvalue;
        }

        // Enable touch for all buttons
        foreach (var button in FindVisualChildren<Button>(this))
        {
            button.PreviewTouchDown += Button_PreviewTouchDown;
        }

        TxtEnterAmount.Focus();
    }

    private void Button_PreviewTouchDown(object sender, TouchEventArgs e)
    {
        e.Handled = true;
        if (sender is Button button)
        {
            button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }
    }

    // Helper method to find all children of a type
    private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
    {
        if (depObj != null)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                if (child != null && child is T)
                {
                    yield return (T)child;
                }

                foreach (T childOfChild in FindVisualChildren<T>(child))
                {
                    yield return childOfChild;
                }
            }
        }
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            BtnEnter_Click(sender, e);
        }
    }


    protected override void OnPreviewTouchDown(TouchEventArgs e)
    {
        if (e.OriginalSource is Button btn)
        {
            btn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, btn));
            e.Handled = true;
        }

        base.OnPreviewTouchDown(e);
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void BtnEnter_Click(object sender, RoutedEventArgs e)
    {
        this.EnteredAmount = TxtEnterAmount.Text;
        returnvalue = TxtEnterAmount.Text;
        this.DialogResult = true;
        this.Close();
    }


    private void TxtEnterAmount_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (isPlaceholderActive && string.IsNullOrWhiteSpace(TxtEnterAmount.Text))
        {
            TxtEnterAmount.Text = "";
            TxtEnterAmount.Foreground = Brushes.Gray;
            isPlaceholderActive = true;
        }
    }

    private void BtnNumAmount(object sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            if (isPlaceholderActive)
            {
                TxtEnterAmount.Text = "";
                TxtEnterAmount.Foreground = Brushes.Black;
                isPlaceholderActive = false;
            }

            TxtEnterAmount.Text += button.Content.ToString();
        }
    }

    private void Btndelete_Click(object sender, RoutedEventArgs e)
    {
        if (TxtEnterAmount.Text.Length > 0)
        {
            TxtEnterAmount.Text = TxtEnterAmount.Text.Remove(TxtEnterAmount.Text.Length - 1);
            if (TxtEnterAmount.Text.Length == 0)
            {
                TxtEnterAmount.Text = "0";
            }

        }
    }

    private void BtnClose_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}
