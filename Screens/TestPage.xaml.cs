using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using static FurniManager.Screens.TestPage;

namespace FurniManager.Screens
{
    /// <summary>
    /// Interaction logic for TestPage.xaml
    /// </summary>
    public partial class TestPage : Page
    {
        
        
        public TestPage()
        {
            InitializeComponent();
        }

    }
    public class MyClass
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public double Salary { get; set; }
        public MyClass()
        {

        }
        public MyClass(string n, int a, double s)
        {
            Name = n;
            Age = a;
            Salary = s;
        }
    }
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        private ObservableCollection<MyClass> _myCollection { get; set; }

        public ObservableCollection<MyClass> MyCollection
        {
            get { return _myCollection; }
            set
            {
                _myCollection = value;
                NotifyPropertyChanged(nameof(MyCollection));
            }
        }
        private ObservableCollection<Source1> _source1 { get; set; }
        public ObservableCollection<Source1> Source1
        {
            get { return _source1; }
            set { _source1 = value; NotifyPropertyChanged(nameof(Source1)); }
        }
        public ViewModel()
        {
            _source1 = new ObservableCollection<Source1>();
            _myCollection = new ObservableCollection<MyClass>();
            SetupSource();

        }
        private void SetupSource()
        {
            _source1.Add(new Source1(2));
            _source1.Add(new Source1(3));
            _source1.Add(new Source1(5));
            _source1.Add(new Source1(8));
            _myCollection.Add(new MyClass("name1", 12, 30.3));
            _myCollection.Add(new MyClass("name2", 22, 30.3));
            _myCollection.Add(new MyClass("name3", 32, 30.3));
            _myCollection.Add(new MyClass("name3", 32, 30.3));
        }


    }
    public class Source1
    {
        public int Number { get; set; }
        public Source1(int n)
        {
            Number = n;
        }
    }
}
