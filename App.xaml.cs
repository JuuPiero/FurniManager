using FurniManager.Data;
using FurniManager.Screens;
using FurniManager.Windows;
using System.Linq;
using System.Windows;

namespace FurniManager
{
    public partial class App : Application
    {   
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using (var db = new ApplicationDbContext())
            {
                bool ex = db.Users.Any(u => u.Email == "admin");

                if(!ex)
                {
                    db.Users.Add(new Models.User
                    {
                        Name = "admin",
                        Email = "admin",
                        Password = "admin",
                        Role = "admin",
                    });
                    db.SaveChanges();
                }
            }

            LoginWindow login = new LoginWindow();
            login.Show();
        }
    }
}