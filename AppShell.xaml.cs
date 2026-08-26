namespace MurisHaseljic472
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(TabPage), typeof(TabPage));
        }
    }
}