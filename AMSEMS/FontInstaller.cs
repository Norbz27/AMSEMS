using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;

public class FontInstaller
{
    // Constants for font installation
    private const int WM_FONTCHANGE = 0x001D;
    private const int HWND_BROADCAST = 0xFFFF;

    // External method for broadcasting WM_FONTCHANGE message
    [DllImport("user32.dll", SetLastError = true)]
    private static extern int SendMessage(int hWnd, int Msg, int wParam, int lParam);

    // External method for adding font to the system
    [DllImport("gdi32.dll", EntryPoint = "AddFontResource", SetLastError = true)]
    private static extern int AddFontResource(string lpszFilename);

    // External method for removing font from the system
    [DllImport("gdi32.dll", EntryPoint = "RemoveFontResource", SetLastError = true)]
    private static extern int RemoveFontResource(string lpFileName);

    // Install font method
    public static bool InstallFont(string fontFilePath)
    {
        try
        {
            // Check if the font file exists
            if (!File.Exists(fontFilePath))
            {
                Console.WriteLine("Font file not found.");
                return false;
            }

            // Install the font
            int result = AddFontResource(fontFilePath);

            // Broadcast WM_FONTCHANGE message to notify other windows of the font change
            SendMessage(HWND_BROADCAST, WM_FONTCHANGE, 0, 0);

            if (result == 0)
            {
                Console.WriteLine("Font installation failed.");
                return false;
            }

            Console.WriteLine("Font installed successfully.");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return false;
        }
    }

    // Uninstall font method
    public static bool UninstallFont(string fontFilePath)
    {
        try
        {
            // Check if the font file exists
            if (!File.Exists(fontFilePath))
            {
                Console.WriteLine("Font file not found.");
                return false;
            }

            // Remove the font
            int result = RemoveFontResource(fontFilePath);

            // Broadcast WM_FONTCHANGE message to notify other windows of the font change
            SendMessage(HWND_BROADCAST, WM_FONTCHANGE, 0, 0);

            if (result == 0)
            {
                Console.WriteLine("Font uninstallation failed.");
                return false;
            }

            Console.WriteLine("Font uninstalled successfully.");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return false;
        }
    }
}
