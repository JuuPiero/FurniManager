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

namespace FurniManager.Screens.Categories
{
    /// <summary>
    /// Interaction logic for CreateCategory.xaml
    /// </summary>
    public partial class CreateCategory : Page
    {
        public CreateCategory()
        {
            InitializeComponent();
        }

        private void OnCreate(object sender, RoutedEventArgs e)
        {
            var name = NameTextBox.Text;
            var desc = DescriptionTextBox.Text;

            Category newCategory = new Category { 
                Name = name,
                Description = desc
            };

            using var db = new ApplicationDbContext();
            db.Categories.Add(newCategory);
            db.SaveChanges();

            MessageBox.Show("Create new Category successfully");
            NameTextBox.Text = "";
            DescriptionTextBox.Text = "";
        }
    }
}
