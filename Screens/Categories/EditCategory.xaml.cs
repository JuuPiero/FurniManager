using FurniManager.Data;
using FurniManager.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FurniManager.Screens.Categories;

public partial class EditCategory : Page
{
    public EditCategory(Category category)
    {
        InitializeComponent();
        DataContext = category;
    }

    private void OnUpdate(object sender, RoutedEventArgs e)
    {
        var category = DataContext as Category;
        using var db = new ApplicationDbContext();
        db.Categories.Update(category);
        db.SaveChanges();
        MessageBox.Show("Category updated succesfully");
    }
}