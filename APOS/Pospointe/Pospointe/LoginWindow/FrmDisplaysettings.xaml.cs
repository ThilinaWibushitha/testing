using Pospointe.Helpers;
using Pospointe.LocalData;
using Pospointe.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Pospointe.LoginWindow;

/// <summary>
/// Interaction logic for FrmDisplaysettings.xaml
/// </summary>
public partial class FrmDisplaysettings : UserControl
{
    private List<TblDepartment> departments;
    private TblDepartment selectedDepartment;
    private TblDepartment managementDepartment;
    private TblDepartment savedManagementDepartment;
    private List<TblItem> allDepartmentItems;
    private List<TblItem> filteredItems;
    private List<SelectedItemInfo> selectedItems = new List<SelectedItemInfo>();
    private bool isUpdatingDropdowns = false;

    public FrmDisplaysettings()
    {
        InitializeComponent();
    }

    // Helper class to track selected items with their department
    public class SelectedItemInfo
    {
        public TblItem Item { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentId { get; set; }
        public string ButtonLabel { get; set; } // Added for Button 1/Button 2 display
    }

    private void TglCusdis_Checked(object sender, RoutedEventArgs e)
    {
        Properties.Settings.Default.CustomerDisplay = true;
        Properties.Settings.Default.Save(); // Save the setting
    }

    private void TglCusdis_Unchecked(object sender, RoutedEventArgs e)
    {
        Properties.Settings.Default.CustomerDisplay = false;
        Properties.Settings.Default.Save(); // Save the setting
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        // Load toggle state
        TglCusdis.IsChecked = Properties.Settings.Default.CustomerDisplay;

        // Load screen list
        var screens = WpfScreenHelper.Screen.AllScreens;
        CmbScreens.Items.Clear();

        foreach (var screen in screens)
        {
            CmbScreens.Items.Add(screen.DeviceName);
        }

        // Set saved selected screen
        if (!string.IsNullOrEmpty(Properties.Settings.Default.CDisplayID))
        {
            CmbScreens.SelectedItem = Properties.Settings.Default.CDisplayID;
        }

        // Load departments
        LoadDepartments();

        // Load previously saved selected items
        LoadSavedSelectedItems();

        // Load previously saved department for management section
        LoadSavedManagementDepartment();

        // Load tax rates
        LoadTaxRates();
    }

    private void BtnUpdatecus_Click(object sender, RoutedEventArgs e)
    {
        Properties.Settings.Default.CustomerDisplay = TglCusdis.IsChecked == true;

        if (CmbScreens.SelectedItem != null)
        {
            Properties.Settings.Default.CDisplayID = CmbScreens.SelectedItem.ToString();
        }

        Properties.Settings.Default.Save();

        if (SaveTaxRates())
        {
            ShowCustomMessage("Success", "Settings updated successfully.\n\nCustomer Display and Tax Rates have been saved.");
        }
    }

    /// <summary>
    /// Load departments from database (same source as FrmPosMain)
    /// </summary>
    private void LoadDepartments()
    {
        try
        {
            using (var context = new PosDb1Context())
            {
                // Load visible departments ordered by ListOrder (same as FrmPosMain)
                departments = context.TblDepartments
                    .Where(x => x.Visible == "OK")
                    .OrderBy(item => item.ListOrder)
                    .ToList();

                // Populate both dropdowns
                CmbDepartmentSelect.ItemsSource = departments;
                CmbManagementDepartment.ItemsSource = departments;

                if (departments.Any())
                {
                    isUpdatingDropdowns = true;
                    CmbDepartmentSelect.SelectedIndex = 0;
                    CmbManagementDepartment.SelectedIndex = 0;
                    isUpdatingDropdowns = false;
                }
            }
        }
        catch (Exception ex)
        {
            ShowCustomMessage("Error", $"Error loading departments: {ex.Message}");
        }
    }

    /// <summary>
    /// Handle item selection department change
    /// </summary>
    private void CmbDepartmentSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (CmbDepartmentSelect.SelectedItem is TblDepartment department)
        {
            selectedDepartment = department;
            LoadDepartmentItems();
            TxtSearchItems.Text = string.Empty;
        }
    }

    /// <summary>
    /// Clear placeholder text when search box gets focus
    /// </summary>
    private void TxtSearchItems_GotFocus(object sender, RoutedEventArgs e)
    {

        if (TxtSearchItems.Text == "Type to search items...")
        {
            TxtSearchItems.Text = string.Empty;
        }
        //TouchKeyboardHelper.OpenKeyboard();
        var keyb = new FrmKeyboard();
        var result = keyb.ShowDialog();

        if (result == true)
        {
            if (result == true && !string.IsNullOrWhiteSpace(keyb.returnvalue))
            {
                TxtSearchItems.Text = keyb.returnvalue;
                TxtSearchItems.CaretIndex = TxtSearchItems.Text.Length; 
            }

        }
    }

    /// <summary>
    /// Load items for the selected department
    /// </summary>
    private void LoadDepartmentItems()
    {
        if (selectedDepartment == null)
        {
            LstDepartmentItems.ItemsSource = null;
            TxtItemCount.Text = "Select a department to view items";
            return;
        }

        try
        {
            using (var context = new PosDb1Context())
            {
                // Load items for the selected department
                allDepartmentItems = context.TblItems
                    .Where(item => item.ItemDeptId == selectedDepartment.DeptId
                        && item.Visible == "OK"
                        && (item.IsModifer == false || item.IsModifer == null))
                    .OrderBy(item => item.ListOrder)
                    .ToList();

                filteredItems = new List<TblItem>(allDepartmentItems);
                LstDepartmentItems.ItemsSource = filteredItems;

                UpdateItemCount();
            }
        }
        catch (Exception ex)
        {
            ShowCustomMessage("Error", $"Error loading items: {ex.Message}");
        }
    }

    /// <summary>
    /// Handle search text changes - filter items as user types
    /// </summary>
    private void TxtSearchItems_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (allDepartmentItems == null || !allDepartmentItems.Any())
            return;

        string searchText = TxtSearchItems.Text?.ToLower() ?? string.Empty;

        // Skip if placeholder text
        if (searchText == "type to search items...")
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(searchText))
        {
            filteredItems = new List<TblItem>(allDepartmentItems);
        }
        else
        {
            filteredItems = allDepartmentItems
                .Where(item => item.ItemName.ToLower().Contains(searchText) ||
                               item.ItemId.ToLower().Contains(searchText))
                .ToList();
        }

        LstDepartmentItems.ItemsSource = null;
        LstDepartmentItems.ItemsSource = filteredItems;
        UpdateItemCount();
    }

    /// <summary>
    /// Handle item selection from the available items list
    /// </summary>
    private void LstDepartmentItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LstDepartmentItems.SelectedItem is TblItem selectedItem && selectedDepartment != null)
        {
            // Check if already selected
            if (selectedItems.Any(si => si.Item.ItemId == selectedItem.ItemId))
            {
                ShowCustomMessage("Info", "This item is already selected.");
                LstDepartmentItems.SelectedIndex = -1;
                return;
            }

            // Check max limit
            if (selectedItems.Count >= 2)
            {
                ShowCustomMessage("Limit Reached", "You can only select up to 2 items.");
                LstDepartmentItems.SelectedIndex = -1;
                return;
            }

            // Determine which button slot is available
            bool hasButton1 = selectedItems.Any(si => si.ButtonLabel == "Button 1");
            bool hasButton2 = selectedItems.Any(si => si.ButtonLabel == "Button 2");

            string buttonLabel;
            if (!hasButton1)
            {
                buttonLabel = "Button 1";
            }
            else if (!hasButton2)
            {
                buttonLabel = "Button 2";
            }
            else
            {
                // This shouldn't happen due to count check above
                buttonLabel = $"Button {selectedItems.Count + 1}";
            }

            // Add to selected items with appropriate button label
            selectedItems.Add(new SelectedItemInfo
            {
                Item = selectedItem,
                DepartmentName = selectedDepartment.DeptName,
                DepartmentId = selectedDepartment.DeptId,
                ButtonLabel = buttonLabel
            });

            UpdateSelectedItemsDisplay();
            LstDepartmentItems.SelectedIndex = -1;
        }
    }

    /// <summary>
    /// Remove item from selected list
    /// </summary>
    private void BtnRemoveItem_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is SelectedItemInfo itemInfo)
        {
            selectedItems.Remove(itemInfo);
            UpdateSelectedItemsDisplay();
        }
    }

    /// <summary>
    /// Update the selected items display
    /// </summary>
    private void UpdateSelectedItemsDisplay()
    {
        LstSelectedItems.ItemsSource = null;
        LstSelectedItems.ItemsSource = selectedItems;

        if (selectedItems.Count == 0)
        {
            TxtSelectionInfo.Text = "No items selected";
        }
        else if (selectedItems.Count == 1)
        {
            TxtSelectionInfo.Text = "1 item selected (1 more allowed)";
        }
        else
        {
            TxtSelectionInfo.Text = "2 items selected (Maximum reached)";
        }
    }

    /// <summary>
    /// Save Items to POS button click
    /// </summary>
    private void BtnSaveItems_Click(object sender, RoutedEventArgs e)
    {
        if (selectedItems.Count == 0)
        {
            ShowCustomMessage("Warning", "Please select at least one item to save.");
            return;
        }

        // Find items by their button labels
        var button1Item = selectedItems.FirstOrDefault(si => si.ButtonLabel == "Button 1");
        var button2Item = selectedItems.FirstOrDefault(si => si.ButtonLabel == "Button 2");

        // Save Button 1 item to Btn1Tag
        if (button1Item != null)
        {
            Properties.Settings.Default.Btn1Tag = $"XITEM:{button1Item.Item.ItemId}";
        }
        else
        {
            Properties.Settings.Default.Btn1Tag = string.Empty;
        }

        // Save Button 2 item to Btn3Tag
        if (button2Item != null)
        {
            Properties.Settings.Default.Btn3Tag = $"XITEM:{button2Item.Item.ItemId}";
        }
        else
        {
            Properties.Settings.Default.Btn3Tag = string.Empty;
        }

        Properties.Settings.Default.Save();

        // Show confirmation
        string itemsList = string.Join("\n", selectedItems.Select(si => $"- {si.ButtonLabel}: {si.Item.ItemName}"));
        ShowCustomMessage("Items Saved", $"Items saved to POS buttons:\n\n{itemsList}\n\nThese will appear as Item buttons on the POS screen.");
    }

    /// <summary>
    /// Save Department Management button click
    /// </summary>
    private void BtnSaveDepartmentManagement_Click(object sender, RoutedEventArgs e)
    {
        if (CmbManagementDepartment.SelectedItem is not TblDepartment department)
        {
            ShowCustomMessage("Warning", "Please select a department to save.");
            return;
        }

        // Save department to Btn2Tag (Department button in POS)
        Properties.Settings.Default.Btn2Tag = $"XDEPT:{department.DeptId}";
        Properties.Settings.Default.Save();

        // Update status display
        TxtManagementStatus.Text = $"Saved: {department.DeptName}";
        TxtManagementStatus.Foreground = Brushes.Green;

        // Show confirmation
        ShowCustomMessage("Department Saved", $"Department '{department.DeptName}' has been saved to POS Department button.");
    }

    /// <summary>
    /// Update Selection button click
    /// </summary>
    private void BtnUpdateSelection_Click(object sender, RoutedEventArgs e)
    {
        if (selectedItems.Count == 0)
        {
            ShowCustomMessage("Info", "No items selected to update.");
            return;
        }

        // Group items by department
        var itemsByDept = selectedItems.GroupBy(si => si.DepartmentName);

        string summary = "Selection Updated:\n\n";
        foreach (var group in itemsByDept)
        {
            summary += $"{group.Key}:\n";
            foreach (var item in group)
            {
                summary += $"  - {item.Item.ItemName} (${item.Item.ItemPrice:F2})\n";
            }
            summary += "\n";
        }

        ShowCustomMessage("Update Successful", summary);
    }

    /// <summary>
    /// Update the item count display
    /// </summary>
    private void UpdateItemCount()
    {
        if (selectedDepartment == null)
        {
            TxtItemCount.Text = "Select a department to view items";
        }
        else if (filteredItems == null || !filteredItems.Any())
        {
            TxtItemCount.Text = $"No items found in '{selectedDepartment.DeptName}'";
        }
        else
        {
            TxtItemCount.Text = $"Showing {filteredItems.Count} of {allDepartmentItems.Count} items";
        }
    }

    /// <summary>
    /// Load previously saved selected items from settings
    /// </summary>
    private void LoadSavedSelectedItems()
    {
        selectedItems.Clear();

        try
        {
            using (var context = new PosDb1Context())
            {
                // Load item from Btn1Tag
                if (!string.IsNullOrEmpty(Properties.Settings.Default.Btn1Tag))
                {
                    string btn1Tag = Properties.Settings.Default.Btn1Tag;
                    if (btn1Tag.StartsWith("XITEM:"))
                    {
                        string itemId = btn1Tag.Substring(6); // Remove "XITEM:" prefix
                        var item = context.TblItems.FirstOrDefault(x => x.ItemId == itemId);
                        if (item != null)
                        {
                            var dept = context.TblDepartments.FirstOrDefault(d => d.DeptId == item.ItemDeptId);
                            selectedItems.Add(new SelectedItemInfo
                            {
                                Item = item,
                                DepartmentName = dept?.DeptName ?? "Unknown",
                                DepartmentId = item.ItemDeptId,
                                ButtonLabel = "Button 1" // First saved item gets Button 1
                            });
                        }
                    }
                }

                // Load item from Btn3Tag
                if (!string.IsNullOrEmpty(Properties.Settings.Default.Btn3Tag))
                {
                    string btn3Tag = Properties.Settings.Default.Btn3Tag;
                    if (btn3Tag.StartsWith("XITEM:"))
                    {
                        string itemId = btn3Tag.Substring(6); // Remove "XITEM:" prefix
                        var item = context.TblItems.FirstOrDefault(x => x.ItemId == itemId);
                        if (item != null)
                        {
                            var dept = context.TblDepartments.FirstOrDefault(d => d.DeptId == item.ItemDeptId);
                            selectedItems.Add(new SelectedItemInfo
                            {
                                Item = item,
                                DepartmentName = dept?.DeptName ?? "Unknown",
                                DepartmentId = item.ItemDeptId,
                                ButtonLabel = "Button 2" // Second saved item gets Button 2
                            });
                        }
                    }
                }

                // Update the display
                UpdateSelectedItemsDisplay();
            }
        }
        catch (Exception ex)
        {
            ShowCustomMessage("Error", $"Error loading saved items: {ex.Message}");
        }
    }

    /// <summary>
    /// Load previously saved department for management section from settings
    /// </summary>
    private void LoadSavedManagementDepartment()
    {
        try
        {
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Btn2Tag))
            {
                string btn2Tag = Properties.Settings.Default.Btn2Tag;
                if (btn2Tag.StartsWith("XDEPT:"))
                {
                    string deptId = btn2Tag.Substring(6); // Remove "XDEPT:" prefix

                    // Find and select the department in the dropdown
                    var savedDept = departments?.FirstOrDefault(d => d.DeptId == deptId);
                    if (savedDept != null)
                    {
                        CmbManagementDepartment.SelectedItem = savedDept;
                        TxtManagementStatus.Text = $"Saved: {savedDept.DeptName}";
                        TxtManagementStatus.Foreground = Brushes.Green;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowCustomMessage("Error", $"Error loading saved department: {ex.Message}");
        }
    }

    /// <summary>
    /// Show custom message box with beautiful design
    /// </summary>
    private void ShowCustomMessage(string header, string message)
    {
        CustomMessageHeader.Text = header;
        CustomMessageText.Text = message;
        CustomMessageOverlay.Visibility = Visibility.Visible;
        CustomMessageBorder.Visibility = Visibility.Visible;
    }

    /// <summary>
    /// Hide custom message box
    /// </summary>
    private void HideCustomMessage()
    {
        CustomMessageOverlay.Visibility = Visibility.Collapsed;
        CustomMessageBorder.Visibility = Visibility.Collapsed;
    }

    /// <summary>
    /// Custom message OK button click
    /// </summary>
    private void BtnCustomMessageOk_Click(object sender, RoutedEventArgs e)
    {
        HideCustomMessage();
    }

    private void TxtTaxAmount_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        // Allow only numbers and decimal point
        var textBox = sender as TextBox;
        var fullText = textBox.Text.Insert(textBox.SelectionStart, e.Text);

        e.Handled = !IsValidDecimal(fullText);
    }

    /// <summary>
    /// Validate if string is a valid decimal number
    /// </summary>
    private bool IsValidDecimal(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return true;

        return decimal.TryParse(text, out decimal result) && result >= 0 && result <= 100;
    }

    /// <summary>
    /// Load tax rates from database
    /// </summary>
    private void LoadTaxRates()
    {
        try
        {
            using (var context = new PosDb1Context())
            {
                // Load Take Away Tax (TaxNo = 1)
                var takeAwayTax = context.TblTaxRates.FirstOrDefault(t => t.TaxNo == 1);
                if (takeAwayTax != null)
                {
                    TxtTakeAwayTaxAmount.Text = takeAwayTax.TaxRate?.ToString("0.##") ?? "0";
                }

                // Load Dine-In Tax (TaxNo = 2)
                var dineInTax = context.TblTaxRates.FirstOrDefault(t => t.TaxNo == 2);
                if (dineInTax != null)
                {
                    TxtDineinTaxAmount.Text = dineInTax.TaxRate?.ToString("0.##") ?? "0";
                }
            }
        }
        catch (Exception ex)
        {
            ShowCustomMessage("Error", $"Error loading tax rates: {ex.Message}");
        }
    }

    /// <summary>
    /// Save tax rates to database
    /// </summary>
    private bool SaveTaxRates()
    {
        try
        {
            using (var context = new PosDb1Context())
            {
                // Parse tax values
                if (!decimal.TryParse(TxtTakeAwayTaxAmount.Text, out decimal takeAwayTax))
                {
                    ShowCustomMessage("Invalid Input", "Please enter a valid Take Away Tax Amount.");
                    return false;
                }

                if (!decimal.TryParse(TxtDineinTaxAmount.Text, out decimal dineInTax))
                {
                    ShowCustomMessage("Invalid Input", "Please enter a valid Dine-In Tax Amount.");
                    return false;
                }

                // Validate range
                if (takeAwayTax < 0 || takeAwayTax > 100)
                {
                    ShowCustomMessage("Invalid Range", "Take Away Tax Amount must be between 0 and 100.");
                    return false;
                }

                if (dineInTax < 0 || dineInTax > 100)
                {
                    ShowCustomMessage("Invalid Range", "Dine-In Tax Amount must be between 0 and 100.");
                    return false;
                }

                // Update or Insert Take Away Tax (TaxNo = 1)
                var takeAwayTaxRecord = context.TblTaxRates.FirstOrDefault(t => t.TaxNo == 1);
                if (takeAwayTaxRecord != null)
                {
                    takeAwayTaxRecord.TaxRate = takeAwayTax;
                }
                else
                {
                    context.TblTaxRates.Add(new TblTaxRate
                    {
                        TaxNo = 1,
                        TaxRate = takeAwayTax
                    });
                }

                // Update or Insert Dine-In Tax (TaxNo = 2)
                var dineInTaxRecord = context.TblTaxRates.FirstOrDefault(t => t.TaxNo == 2);
                if (dineInTaxRecord != null)
                {
                    dineInTaxRecord.TaxRate = dineInTax;
                }
                else
                {
                    context.TblTaxRates.Add(new TblTaxRate
                    {
                        TaxNo = 2,
                        TaxRate = dineInTax
                    });
                }

                context.SaveChanges();
                return true;
            }
        }
        catch (Exception ex)
        {
            ShowCustomMessage("Error", $"Error saving tax rates: {ex.Message}");
            return false;
        }
    }

    private void TxtTakeAwayTaxAmount_GotFocus(object sender, RoutedEventArgs e)
    {
        FrmNumbpad numpad = new FrmNumbpad();
        if (numpad.ShowDialog() == true)
        {
            TxtTakeAwayTaxAmount.Text = numpad.returnvalue;
        }
    }
    private void TxtDineInTaxAmount_GotFocus(object sender, RoutedEventArgs e)
    {
        FrmNumbpad numpad = new FrmNumbpad();
        if (numpad.ShowDialog() == true)
        {
            TxtDineinTaxAmount.Text = numpad.returnvalue;
        }
    }

}