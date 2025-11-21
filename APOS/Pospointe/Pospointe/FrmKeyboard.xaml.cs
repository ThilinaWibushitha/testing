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
using System.Windows.Shapes;

namespace Pospointe
{
    /// <summary>
    /// Interaction logic for FrmKeyboard.xaml
    /// </summary>
    public partial class FrmKeyboard : Window
    {
        public string returnvalue { get; set; }
        private bool _isShiftPressed = false;
        private DateTime _lastTouchTime = DateTime.MinValue;

        public FrmKeyboard()
        {
            InitializeComponent();
            this.Loaded += Window_Loaded;  // ← ADD THIS LINE!
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Enable touch for all buttons and prevent mouse event promotion
            foreach (var button in FindVisualChildren<Button>(this))
            {
                button.PreviewTouchDown += Button_PreviewTouchDown;
                button.PreviewMouseDown += Button_PreviewMouseDown;
            }

            MyTextBox.Focus();
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

        private void Button_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            e.Handled = true;
            _lastTouchTime = DateTime.Now;

            if (sender is Button button)
            {
                button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        private void Button_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // If a touch event happened recently (within 500ms), ignore this mouse event
            if ((DateTime.Now - _lastTouchTime).TotalMilliseconds < 500)
            {
                e.Handled = true;
            }
        }

        private void Key_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string key = button.Content.ToString();
                // Handle Space
                if (key == "Space")
                {
                    MyTextBox.Text += " ";
                }
                // Handle Backspace
                else if (key == "Backspace")
                {
                    if (MyTextBox.Text.Length > 0)
                        MyTextBox.Text = MyTextBox.Text.Remove(MyTextBox.Text.Length - 1);
                }
                // Handle Enter
                else if (key == "Enter")
                {
                    returnvalue = MyTextBox.Text;
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    // Handle letters
                    if (char.IsLetter(key[0])) // Check if the key is a letter
                    {
                        MyTextBox.Text += _isShiftPressed ? key.ToUpper() : key.ToLower();
                    }
                    else
                    {
                        // Handle numbers and special characters (unaffected by Shift)
                        MyTextBox.Text += key;
                    }
                }
            }
        }

        // Event handler for Shift key
        private void Shift_Click(object sender, RoutedEventArgs e)
        {
            _isShiftPressed = !_isShiftPressed; // Toggle Shift state
            // Provide visual feedback for the Shift key state
            if (sender is Button shiftButton)
            {
                shiftButton.Background = _isShiftPressed
                    ? new SolidColorBrush(Colors.LightBlue) // Shift is active
                    : new SolidColorBrush(Colors.LightGray); // Shift is inactive
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}