using MagazynApp.Data;

namespace MagazynApp
{
    public partial class App : Application
    {

        public static DatabaseService Database { get; private set; }
        public App(DatabaseService database)
        {
            InitializeComponent();
            Database = database;
            Task.Run( async () =>  await Database.InitalizeAsync());

            MainPage = new AppShell();
        }
    }
}
