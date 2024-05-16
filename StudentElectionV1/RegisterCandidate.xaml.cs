using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace StudentElectionV1
{
    
    public partial class RegisterCandidate : Window
    {
        string username;
        string password;
        string database;
        string server;
        public MySqlConnection conn;
        public MySqlDataAdapter adapter;
        public MySqlDataReader cursor;
        public MySqlCommand cmd;
        public string sql = "";
        public DataTable dataTable;
        public DataSet dt;
        public RegisterCandidate()
        {
            InitializeComponent();
            Connect2DB();
            showCandData();            
        }

        private void mySQLConnection()
        {
            string connStr = $"server={server}; password={password};username={username}; database={database}";
            conn = new MySqlConnection(connStr);
            conn.Open();
        }
        public void Connect2DB()
        {
            server = "sql6.freesqldatabase.com";
            database = "sql6706151";
            username = "sql6706151";
            password = "grvvR5WC8y";
            // server = "sql6.freesqldatabase.com";
            // database = "sql6700091";
            // username = "sql6700091";
            // password = "G4b8ziiLVv";
            mySQLConnection();
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            this.Hide();
            main.Show();
        }

        private void showCandData()
        {
            DataTable dt = new DataTable();
            // sql = "SELECT * FROM candidates";
            sql = "SELECT candidates.ID, candidates.VOTERSID, candidates.POSITION, candidates.PARTY,voters.LASTNAME, voters.FIRSTNAME, candidates.WITHDRAWN FROM candidates INNER JOIN voters ON candidates.votersid=voters.id ORDER BY candidates.PARTY";
            using (MySqlDataAdapter da = new MySqlDataAdapter(sql, conn))
            {
                da.Fill(dt);
            }
            candidateList.ItemsSource = dt.DefaultView;
            
            
        }

        private void getVoterData()
        {
            DataTable dt = new DataTable();
            sql = "SELECT ID, LASTNAME, FIRSTNAME FROM voters ORDER BY LASTNAME, FIRSTNAME, MIDDLENAME";
            using (MySqlDataAdapter da = new MySqlDataAdapter(sql, conn))
            {
                da.Fill(dt);                
            }
            
            votersList.ItemsSource = dt.DefaultView;
            
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
           
           // check for blank entries
           if (cmbParty.Text.Contains("Not Set") || cmbPosition.Text.Contains("Not Set")
                || cmbParty.Text=="" || cmbPosition.Text=="") 
           {
                txtCandidateText.Foreground = Brushes.Red;
                txtCandidateText.FontSize = 14;
                txtCandidateText.Text = "Position and party cannot be blank!";
                candLogo.Source = new BitmapImage(new Uri(@"images/warning.png", UriKind.Relative));
                txtVoterID.Visibility = Visibility.Hidden;
           } else 
           {                
                txtCandidateText.Foreground = Brushes.Black;
                txtCandidateText.Text = "Select a name";
                txtCandidateText.FontSize = 25;
                txtVoterID.Visibility = Visibility.Visible;
                txtCandidID.Visibility = Visibility.Hidden;

                // hide last and first name bounded to candidate table
                txtLastName.Visibility = Visibility.Hidden;
                txtFirstName.Visibility = Visibility.Hidden;
                txtFirstNameV.Visibility = Visibility.Visible;
                txtLastNameV.Visibility = Visibility.Visible;
                getVoterData();
                // attempt to change the last name and firstname bindings                

                // hide operational buttons
                spFile.Visibility = Visibility.Hidden;
                spEdit.Visibility = Visibility.Hidden;
                spWithdraw.Visibility = Visibility.Hidden;
                spSearch.Visibility = Visibility.Hidden;
                spSearchBox.Visibility = Visibility.Hidden;
                candLogo.Source = new BitmapImage(new Uri(@"images/candidate.png",UriKind.Relative));
                candLogo.Visibility = Visibility.Hidden;
                blueBorder.Width = 700;
                blueBorder.HorizontalAlignment= HorizontalAlignment.Left;
                candidateList.Width = 700;
                candidateList.HorizontalAlignment = HorizontalAlignment.Left;   
                candidateList.IsEnabled= false;                
                votersList.Visibility = Visibility.Visible;
                btnSaveCand.Visibility = Visibility.Visible;
                btnCancelCand.Visibility = Visibility.Visible;                
            }
        }

        private void btnCancelCand_Click(object sender, RoutedEventArgs e)
        {
            txtCandidateText.Foreground = Brushes.Black;            
            txtVoterID.Visibility = Visibility.Hidden;
            txtCandidID.Visibility = Visibility.Visible;
            getVoterData();
            // show operational buttons
            spFile.Visibility = Visibility.Visible;
            spEdit.Visibility = Visibility.Visible;
            spWithdraw.Visibility = Visibility.Visible;
            spSearch.Visibility = Visibility.Visible;
            spSearchBox.Visibility = Visibility.Visible;
            candLogo.Source = new BitmapImage(new Uri(@"images/candidate.png", UriKind.Relative));
            candLogo.Visibility = Visibility.Visible;            
            blueBorder.HorizontalAlignment = HorizontalAlignment.Left;
            candidateList.Width = 1150;
            blueBorder.Width = 850;
            candidateList.HorizontalAlignment = HorizontalAlignment.Left;
            candidateList.IsEnabled = true;
            votersList.Visibility = Visibility.Hidden;
            btnSaveCand.Visibility = Visibility.Hidden;
            btnCancelCand.Visibility = Visibility.Hidden;
            // clear fields
            cmbPosition.Text = "";
            cmbParty.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtCandidateText.Text = "Candidate Registration";
            txtCandidateText.FontSize = 25;
            txtFirstNameV.Visibility = Visibility.Hidden;
            txtLastNameV.Visibility = Visibility.Hidden;
            txtFirstName.Visibility = Visibility.Visible;
            txtLastName.Visibility = Visibility.Visible;
            // let us see if this Activate event, refreshes the window
            this.Activate();

        }

        

        private void candidateList_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            string sql = "SELECT * FROM voters WHERE id=@id";
            var id = txtVoterID2.Text;
            cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            var myResult = cmd.ExecuteReader();
            myResult.Read();
            if (myResult.HasRows)
            {
                txtLastName.Text = myResult.GetValue(1).ToString().Trim();
                txtFirstName.Text = myResult.GetValue(2).ToString().Trim();                
            }
            myResult.Close();
        }

        private void btnSaveCand_Click(object sender, RoutedEventArgs e)
        {
            var id=txtVoterID.Text;

            // check if a name has already been selected first
            if (txtFirstNameV.Text=="" || txtLastNameV.Text=="")
            {
                txtLastNameV.BorderBrush = Brushes.Red;
                txtFirstNameV.BorderBrush = Brushes.Red;
                return;             
            }
            sql = "INSERT INTO candidates(votersid,position,party,withdrawn) VALUES(@id,@position,@party,1)";
            // the 1 at the end indicates that this newly added candidate is active, with zero meaning withdrawn
            cmd = new MySqlCommand(sql, conn);
            var position = cmbPosition.Text;
            var party = cmbParty.Text;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@position", position);
            cmd.Parameters.AddWithValue("@party", party);            
            cursor = cmd.ExecuteReader();
            // at this point, we should be able to ADD data to our table (CANDIDATES)
            cursor.Close();
            // just housekeeping
            // ok clean bill, we may save
            txtCandidateText.Foreground = Brushes.Black;
            txtCandidateText.Text = "Register a Candidate";
            spFile.Visibility = Visibility.Visible;
            spEdit.Visibility = Visibility.Visible;
            spWithdraw.Visibility = Visibility.Visible;
            spSearch.Visibility = Visibility.Visible;
            spSearchBox.Visibility = Visibility.Visible;
            candLogo.Source = new BitmapImage(new Uri(@"images/candidate.png", UriKind.Relative));
            candLogo.Visibility = Visibility.Visible;
            blueBorder.HorizontalAlignment = HorizontalAlignment.Left;
            candidateList.Width = 1150;
            blueBorder.Width = 850;
            candidateList.HorizontalAlignment = HorizontalAlignment.Left;
            candidateList.IsEnabled = true;
            votersList.Visibility = Visibility.Hidden;
            btnSaveCand.Visibility = Visibility.Hidden;
            btnCancelCand.Visibility = Visibility.Hidden;
            txtLastName.BorderBrush = Brushes.Black;
            txtFirstName.BorderBrush = Brushes.Black;
            txtFirstNameV.Visibility = Visibility.Hidden;
            txtLastNameV.Visibility = Visibility.Hidden;
            txtFirstName.Visibility = Visibility.Visible;
            txtLastName.Visibility = Visibility.Visible;
            // adjust display
            showCandData();

        }

        private void votersList_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtCandidateText.Foreground = Brushes.Black;
            txtCandidateText.Text = "Candidate Registration";
            txtVoterID.Visibility = Visibility.Hidden;
            txtCandidID.Visibility = Visibility.Visible;

            // show operational buttons            

            // check if selected thing is already a candidate
            string sql = "SELECT * FROM candidates WHERE votersid=@id";
            var id = txtVoterID.Text;
            cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            var myResult = cmd.ExecuteReader();
            myResult.Read();
            if (myResult.HasRows)
            {                
                // flag if already a candidate
                txtCandidateText.Foreground = Brushes.Red;
                txtCandidateText.Text = "Already a candidate";
                candLogo.Source = new BitmapImage(new Uri(@"images/warning.png", UriKind.Relative));                
                
            }
            else
            {
                // ok clean bill, we may save
                txtCandidateText.Foreground = Brushes.Black;
                txtCandidateText.Text = "Candidate registration";
                spFile.Visibility = Visibility.Visible;
                spEdit.Visibility = Visibility.Visible;
                spWithdraw.Visibility = Visibility.Visible;
                spSearchBox.Visibility = Visibility.Visible;
                candLogo.Source = new BitmapImage(new Uri(@"images/candidate.png", UriKind.Relative));
                candLogo.Visibility = Visibility.Visible;
                blueBorder.HorizontalAlignment = HorizontalAlignment.Left;
                candidateList.Width = 1150;
                blueBorder.Width = 850;
                candidateList.HorizontalAlignment = HorizontalAlignment.Left;
                candidateList.IsEnabled = true;
                votersList.Visibility = Visibility.Hidden;
                btnSaveCand.Visibility = Visibility.Hidden;
                btnCancelCand.Visibility = Visibility.Hidden;
                txtFirstNameV.Visibility = Visibility.Hidden;
                txtLastNameV.Visibility = Visibility.Hidden;
                txtFirstName.Visibility = Visibility.Visible;
                txtLastName.Visibility = Visibility.Visible;
            }
            myResult.Close();

        }

        private void btnWithdraw_Click(object sender, RoutedEventArgs e)
        {
            // first remove non-relevant buttons
            spFile.Visibility = Visibility.Hidden;
            spEdit.Visibility = Visibility.Hidden;
            spWithdraw.Visibility = Visibility.Hidden;
            spSearch.Visibility = Visibility.Hidden;
            spSearchBox.Visibility = Visibility.Hidden;
            borderWithdraw.Visibility = Visibility.Visible;            

        }

        private void btnCancelWithdraw_Click(object sender, RoutedEventArgs e)
        {
            spFile.Visibility = Visibility.Visible;
            spEdit.Visibility = Visibility.Visible;
            spWithdraw.Visibility = Visibility.Visible;
            spSearch.Visibility = Visibility.Visible;
            spSearchBox.Visibility = Visibility.Visible;
            borderWithdraw.Visibility = Visibility.Hidden;
        }

        private void btnConfirmWithdraw_Click(object sender, RoutedEventArgs e)
        {
            sql = "UPDATE candidates SET withdrawn=0 WHERE ID=@id";             
            cmd = new MySqlCommand(sql, conn);
            var id = txtCandidID.Text.Trim();
            cmd.Parameters.AddWithValue("@id", id);            
            cursor = cmd.ExecuteReader();                        
            cursor.Close();

            // clean up
            spFile.Visibility = Visibility.Visible;
            spEdit.Visibility = Visibility.Visible;
            spWithdraw.Visibility = Visibility.Visible;
            spSearch.Visibility = Visibility.Visible;
            spSearchBox.Visibility = Visibility.Visible;
            borderWithdraw.Visibility = Visibility.Hidden;
            showCandData();
        }
    }
}
