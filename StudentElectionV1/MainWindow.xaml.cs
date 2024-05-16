using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace StudentElectionV1
{
    
    public partial class MainWindow : Window
    {
        DispatcherTimer clockTimer = new DispatcherTimer();
        RegisterVoter registerVoter = new RegisterVoter();
        public string isAdmin = "";
        public MainWindow()
        {
            InitializeComponent();
            // format date and time
            isAdmin = txtTemp.Text;
            txtDateDisplay.Text = DateTime.Now.ToString("dddd MMMM dd, yyyy");
            clockTimer.Interval = TimeSpan.FromSeconds(1);
            clockTimer.Tick += ClockTimerEngine;
            clockTimer.Start();

            // check user access rights

            if (isAdmin =="0")            
            {
                // zero means they are not administrators but mere operators
                btnHouseKeeping.Visibility = Visibility.Hidden;
                txtUserManagement.Visibility = Visibility.Hidden;
                btnElec.Visibility = Visibility.Hidden;
                txtVotingProper.Visibility = Visibility.Hidden;
            } else
            {
                Console.WriteLine(isAdmin);
            }
        }

        private void ClockTimerEngine(object? sender, EventArgs e)
        {
            txtTimeDisplay.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnVoterReg_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            registerVoter.Activate();
            registerVoter.Show();
        }

        private void btnHouseKeeping_Click(object sender, RoutedEventArgs e)
        {            
            if (txtTemp.Text.Trim()=="1")
            {
                RegisterUser userWindow = new RegisterUser();
                userWindow.Show();
                this.Hide();
            } else
            {
                txtUserManagement.Text = "Action not authorized";
                imgHouseKeeping.Source = new BitmapImage(new Uri(@"/images/notallowed.png", UriKind.Relative));
            }
        }

        private void btnElec_Click(object sender, RoutedEventArgs e)
        {
            if (txtTemp.Text.Trim() == "1")
            {
                VotingProper votingWindow = new VotingProper();
                votingWindow.Show();
                this.Hide();
            }
            else
            {
                txtVotingProper.Text = "Action not authorized";
                imgVotingProper.Source = new BitmapImage(new Uri(@"/images/notallowed.png", UriKind.Relative));
            }
        }

        private void btnCandReg_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            RegisterCandidate candidate = new RegisterCandidate();
            candidate.Show();
        }
    }
}