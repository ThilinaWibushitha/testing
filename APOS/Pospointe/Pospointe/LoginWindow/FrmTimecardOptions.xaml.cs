using Pospointe.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Pospointe.LoginWindow
{
    /// <summary>
    /// Interaction logic for FrmTimecardOptions.xaml
    /// </summary>
    public partial class FrmTimecardOptions : Window
    {
        string lastact = "";
        public TimeCardUserResp empdata;
        public FrmTimecardOptions(TimeCardUserResp rsp)
        {
            InitializeComponent();
            empdata = rsp;
            LblWelcomeMessage.Text = $"Welcome, {rsp.employee.firstName} {rsp.employee.lastName}!";
            //Last Activity : Checkout at 5:00PM (02/11/2024)
            if (rsp.lastActivity != null)
            {
                LblLastactive.Text = $"Last Activity : {GetStatusinWords(rsp.lastActivity.punchType ?? 0)} - {rsp.lastActivity.timeStamp.ToString("h:mmtt (MM/dd/yyyy)")}";
            }
            else
            {
                LblLastactive.Text = $"Last Activity : No clock-in recorded";
            }
        }

        private static string GetStatusinWords(int no)
        {
            if (no == 0) { return "Unknown"; }
            if (no == 1) { return "Clock-IN"; }
            if (no == 2) { return "Clock-OUT"; }
            if (no == 3) { return "Break-IN"; }
            if (no == 4) { return "Break-Out"; }
            return "Unknown";
        }

        private async void BtnClkIn_Click(object sender, RoutedEventArgs e)
        {
            await UpdateLog(1);
        }

        private async void BtnClkOut_Click(object sender, RoutedEventArgs e)
        {
            await UpdateLog(2);
        }

        private async void BtnBrkIn_Click(object sender, RoutedEventArgs e)
        {
            await UpdateLog(3);
        }

        private async void BtnBrkOut_Click(object sender, RoutedEventArgs e)
        {
            await UpdateLog(4);
        }

        private async Task UpdateLog(int punchtype)
        {
            if (empdata?.lastActivity != null)
            {
                int lastPunch = empdata.lastActivity.punchType ?? 0;

                if (punchtype == 1 && lastPunch == 1)
                {
                    ShowCustomMessage("Clock-In Attempt", "You have already clocked in. No further action is needed.");
                    this.Close();
                    return;
                }
                if (punchtype == 3 && lastPunch == 3)
                {
                    ShowCustomMessage("Break Already Started", "You are already on a break.");
                    this.Close();
                    return;
                }
                if (punchtype == 2 && lastPunch == 2)
                {
                    ShowCustomMessage("Clock-Out Attempt", "You have already clocked out.");
                    this.Close();
                    return;
                }
                if (punchtype == 4 && lastPunch == 4) 
                {
                    ShowCustomMessage("Break Already Ended", "You have already returned from break.");
                    this.Close();
                    return;
                }
            }

            string resp = TimeCardService.UpdateData(punchtype, empdata.employee.empId);

            if (resp == "success")
            {
                ShowCustomMessage("Action Confirmed", "Your time log has been successfully updated.", false);
                this.Close();
            }
            else
            {
                ShowCustomMessage("Update Failed", $"An error occurred: {resp}", true);
            }
        }



        private void ShowCustomMessage(string header, string message, bool isError = false)
        {
            FrmCustommessage frmCustommessage = new FrmCustommessage
            {
                LblHeader = { Text = header },
                LblMessage = { Text = message },
                IsError = isError
            };
            frmCustommessage.ShowDialog();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        //private async Task UpdateLog(int punchtype)
        //{
        //   // MessageBox.Show(empdata.employee.empId.ToString());
        //  string resp =  TimeCardService.UpdateData(punchtype, empdata.employee.empId);

        //    if (resp == "success")
        //    {
        //        FrmCustommessage frmCustommessage = new FrmCustommessage
        //        {
        //            LblHeader = { Text = "Alart" },
        //            LblMessage = { Text = "Updated Successfully!" },
        //            IsError = false
        //        };
        //        frmCustommessage.ShowDialog();
        //        this.Close();

        //    }
        //    else
        //    {
        //        FrmCustommessage frmCustommessage = new FrmCustommessage
        //        {
        //            LblHeader = { Text = "Error" },
        //            LblMessage = { Text = resp },
        //            IsError = true
        //        };
        //        frmCustommessage.ShowDialog();


        //    }


        //}


    }
}
