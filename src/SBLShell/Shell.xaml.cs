using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SBLShell
{

    public interface IWindowHostedControl
    {
        public HostWindow HostWindow { get; }
    }

    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : IWindowHostedControl
    {
        public Shell()
        {
            InitializeComponent();
        }
        public HostWindow HostWindow { get; set; }
    }

    public class HostWindow
    {
        internal Window ActualHostWindow { get; }
        public IntPtr HostHandle => new WindowInteropHelper(ActualHostWindow).EnsureHandle();

        public HostWindow()
        {
            ActualHostWindow = new Window();
        }

        public HostWindow(IntPtr host)
        {
            HwndSource hWnd = HwndSource.FromHwnd(host);
            if(hWnd == null) throw new ArgumentException("Handle does not hold a valid host");
            var window = hWnd.RootVisual as Window;
            ActualHostWindow = window ?? throw new ArgumentException("Handle does not hold a valid host");
        }
    }
}
