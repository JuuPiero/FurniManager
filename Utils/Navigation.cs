using FurniManager.Screens.Products;
using System.Windows;
using System.Windows.Controls;

namespace FurniManager.Utils
{
    public class Navigation
    {
        public static void Navigate(Page page)
        {
            MainWindow main = (MainWindow)Application.Current.MainWindow;

            main.GetNavigation().Navigate(page);
        }
    }
}
