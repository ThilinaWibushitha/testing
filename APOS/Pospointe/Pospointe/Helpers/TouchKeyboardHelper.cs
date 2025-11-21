using System.Diagnostics;
using System.IO;

namespace Pospointe.Helpers;

public static class TouchKeyboardHelper
{
    private static readonly string TabTipPath =
        @"C:\Program Files\Common Files\Microsoft Shared\ink\TabTip.exe";
    public static void OpenKeyboard()
    {
        try
        {
            if (File.Exists(TabTipPath))
            {
                ProcessStartInfo psi = new ProcessStartInfo()
                {
                    FileName = TabTipPath,
                    UseShellExecute = true,
                    Verb = "open"
                };
                Process.Start(psi);
            }
            else
            {
                ProcessStartInfo psi = new ProcessStartInfo()
                {
                    FileName = "osk.exe",
                    UseShellExecute = true,
                    Verb = "open"
                };
                Process.Start(psi);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Failed to open touch keyboard: " + ex.Message);
        }
    }
}
