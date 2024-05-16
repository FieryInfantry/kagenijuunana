using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography;
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
using System.Xml.Linq;
using System.Xml.Schema;

namespace StudentElectionV1
{
    
    public partial class VotingProper : Window
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
        public string voterID;
        MainWindow main = new MainWindow();
        public VotingProper()
        {
            InitializeComponent();
            Connect2DB();
            txtVoterNumber.Focus();
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

        private void txtVoterNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                
                string sql = "SELECT * FROM voters WHERE voternumber=@voternumber";
                var voternumber = int.Parse(txtVoterNumber.Text);
                cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@voternumber", voternumber);
                var myResult = cmd.ExecuteReader();
                myResult.Read();
                if (myResult.HasRows)
                {
                    // check if he/she already voted 
                    string alreadyVoted = myResult.GetValue(7).ToString();
                    if (alreadyVoted=="0")                    {                        
                        string lastname = myResult.GetValue(1).ToString().Trim();
                        string firstname = myResult.GetValue(2).ToString().Trim();
                        string middleinit = myResult.GetValue(3).ToString().Trim();
                        var justinit = middleinit[0];
                        txtVoterName.Foreground = Brushes.Black;
                        txtVoterName.Text = firstname + " " + justinit + ". " + lastname;
                        btnProceed.Visibility = Visibility.Visible;
                        txtSelectCandidate.Text = txtVoterName.Text + " ID: " + myResult.GetValue(0).ToString();
                        voterID = myResult.GetValue(0).ToString();
                        txtSelectionDirection.Visibility = Visibility.Visible;                    
                    } else
                    {
                        txtVoterName.Text = "You have already voted!";
                        myResult.Close();
                        return;
                    }
                    
                } 
                else 
                {
                    txtVoterName.Foreground = Brushes.Red;
                    txtVoterName.Text = "That is not a valid voter number";
                    btnProceed.Visibility = Visibility.Hidden;
                }
                myResult.Close();
            }
        }

        private void btnProceed_Click(object sender, RoutedEventArgs e)
        {
            borderGetVoterNumber.Visibility = Visibility.Hidden;
            txtSelectCandidate.Visibility = Visibility.Visible;
            txtSelectedCandidates.Visibility = Visibility.Visible;
            spSelection.Visibility = Visibility.Visible;
            spButtonControl.Visibility = Visibility.Visible;
            getData();
           
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {            
                        
            this.Hide();
            main.Show();
        }

        private void candidateList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var targetCandidate = candidateList.SelectedItem;
            if (btnResetSelection.Visibility == Visibility.Hidden)
            {
                btnResetSelection.Visibility = Visibility.Visible;
            }

            btnSubmitVote.IsEnabled = true;
            votersList.Items.Add(targetCandidate);
            candidateList.Items.Remove(targetCandidate);
            
        }

        private void votersList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var targetCandidate = votersList.SelectedItem;
            if (btnSubmitVote.Visibility==Visibility.Hidden)
            {
                btnSubmitVote.Visibility = Visibility.Visible;
            }
            candidateList.Items.Add(targetCandidate);
            votersList.Items.Remove(targetCandidate);
            
        }

        private void btnResetSelection_Click(object sender, RoutedEventArgs e)
        {
           
            votersList.Items.Clear();
            candidateList.Items.Clear();
            getData();
           
        }

        private void getData()
        // ********************** Get data to list boxes ********************************************************
        {
            // get voters name in a list
            string midinit = "";
            var fullname = new List<String>();
            string sql1 = "SELECT id, firstname, lastname, middlename FROM voters ORDER BY firstname, lastname";
            cmd = new MySqlCommand(sql1, conn);
            var myResult2 = cmd.ExecuteReader();
            while (myResult2.Read())
            {
                midinit = myResult2["middlename"].ToString();
                fullname.Add(myResult2["firstname"].ToString() + " " + midinit[0] + ". " + myResult2["lastname"].ToString() + "-" + myResult2["id"].ToString());
            }
            myResult2.Close();
            // better close this reader after we finish.

            sql = "SELECT * FROM candidates WHERE withdrawn=1 ORDER BY party";
            // Query the candidates table and exclude withdrawn entries            
            cmd = new MySqlCommand(sql, conn);
            var myResult = cmd.ExecuteReader();
            string candidateItem = "";
            string candidateName = "";
            string candidID = "";
            string myTemp = "";
            string tempVoterID = "";


            while (myResult.Read())
            {
                candidID = myResult["votersid"].ToString().Trim();
                
                // loop thru voter's list to find the candidate's name
                for (int i = 0; i < fullname.Count; i++)
                {
                    myTemp = fullname[i];
                    int charPos = myTemp.IndexOf("-");
                    tempVoterID = myTemp.Substring(charPos + 1);
                    if (candidID == tempVoterID)
                    {
                        // we have the voter's name ?
                        candidateItem = myTemp.Substring(0, charPos) + " " + myResult["position"].ToString() + " " + myResult["party"].ToString() + " ID: " + myResult["id"];
                        break;
                        // we take it; then we exit this loop
                    }
                }
                // finally, add the name (from voters table) , position and party (from candidates table) to the list
                candidateList.Items.Add(candidateItem);
            }

            myResult.Close();
        }

        private void btnSubmitVote_Click(object sender, RoutedEventArgs e)
        {
            if (votersList.Items.Count == 0)
            {
                btnSubmitVote.IsEnabled = false;
            }

            // get data from votersList
            for (int i= 0; i< votersList.Items.Count; i++)
            {
                var targetCandidate = votersList.Items[i].ToString();
                int charPos = targetCandidate.IndexOf(":");
                var tempCandidID = targetCandidate.Substring(charPos + 1);
                // txtSelectionDirection.Text = DateTime.Now.ToString("hh:mm tt");

                sql = "INSERT INTO votes (candidateid,votersid,timevoted) VALUES(@candID,@voterID,@timeVoted)";
                cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@candID", tempCandidID);
                cmd.Parameters.AddWithValue("@voterID", voterID);
                cmd.Parameters.AddWithValue("@timeVoted", DateTime.Now.ToString("hh:mm"));
                cursor = cmd.ExecuteReader();
                cursor.Close();
                
            }
            // mark voter as having already voted                
            var sql2 = "UPDATE voters SET voted=1 WHERE ID=" + voterID;
            var cmd2 = new MySqlCommand(sql2, conn);
            var cursor2 = cmd2.ExecuteReader();
            cursor2.Close();
            borderGetVoterNumber.Visibility = Visibility.Visible;
            txtSelectCandidate.Visibility = Visibility.Hidden;
            txtSelectedCandidates.Visibility = Visibility.Hidden;
            spSelection.Visibility = Visibility.Hidden;
            spButtonControl.Visibility = Visibility.Hidden;
            txtVoterNumber.Text = "";
            txtVoterName.Text = "";
            txtSelectionDirection.Visibility=Visibility.Hidden;
            btnProceed.Visibility = Visibility.Hidden;
        }
        private void btnTallyBoard_Click(object sender, RoutedEventArgs e)
        {
            var tempCandID = "";
            borderGetVoterNumber.Visibility = Visibility.Hidden;
            btnExit.Visibility = Visibility.Hidden;
            btnReturn2Voting.Visibility = Visibility.Visible;
            btnTallyBoard.Visibility = Visibility.Hidden;
            svElectionResult.Visibility = Visibility.Visible;
            txtAsOF.Text = txtAsOF.Text + " " + main.txtTimeDisplay.Text;


            // Define a method to execute the SQL queries and handle the results
            void ExecuteQueryAndHandleResults(string position, string party, TextBlock positionTextBlock)
            {
                // Query to check if there is a candidate
                sql = "SELECT candidates.id, voters.firstname, voters.lastname FROM candidates INNER JOIN voters ON candidates.votersid=voters.id WHERE position=@position AND party=@party AND WITHDRAWN=1";
                cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@position", position);
                cmd.Parameters.AddWithValue("@party", party);
                var result = cmd.ExecuteReader();

                if (result.HasRows)
                {
                    result.Read();
                    tempCandID = result.GetValue(0).ToString();
                    positionTextBlock.Text = result.GetValue(1).ToString().Trim() + " " + result.GetValue(2).ToString().Trim() + " (" + party + ")";
                }
                else
                {
                    positionTextBlock.Text = "NO candidate";
                }
                result.Close();

                // let us get the vote count
                sql = "SELECT COUNT(candidateid) AS votesGotten FROM votes WHERE candidateid=@candidateid";
                cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@candidateid", tempCandID);
                var voteCount = cmd.ExecuteScalar();
                positionTextBlock.Text = positionTextBlock.Text + " >> " + voteCount.ToString();
                tempCandID = "";
            }

            // Execute the method for Governor ALAB
            ExecuteQueryAndHandleResults("Governor", "ALAB", GovernorALAB);
            ExecuteQueryAndHandleResults("Governor", "ALAWS", GovernorALAWS);
            ExecuteQueryAndHandleResults("Governor", "IND", GovernorIND);

            // Execute the method for Vice Governor ALAB
            ExecuteQueryAndHandleResults("Vice Governor", "ALAB", ViceGovernorALAB);
            ExecuteQueryAndHandleResults("Vice Governor", "ALAWS", ViceGovernorALAWS);
            ExecuteQueryAndHandleResults("Vice Governor", "IND", ViceGovernorIND);
            // Execute the method for Secretary ALAB
            ExecuteQueryAndHandleResults("Secretary", "ALAB", SecretaryALAB);
            ExecuteQueryAndHandleResults("Secretary", "ALAWS", SecretaryALAWS);
            ExecuteQueryAndHandleResults("Secretary", "IND", SecretaryIND);

            ExecuteQueryAndHandleResults("Treasurer", "ALAB", TreasurerALAB);
            ExecuteQueryAndHandleResults("Treasurer", "ALAWS", TreasurerALAWS);
            ExecuteQueryAndHandleResults("Treasurer", "IND", TreasurerIND);

            ExecuteQueryAndHandleResults("Auditor", "ALAB", AuditorALAB);
            ExecuteQueryAndHandleResults("Auditor", "ALAWS", AuditorALAWS);
            ExecuteQueryAndHandleResults("Auditor", "IND", AuditorIND);

            ExecuteQueryAndHandleResults("BusinessManager", "ALAB", BusinessManagerALAB);
            ExecuteQueryAndHandleResults("BusinessManager", "ALAWS", BusinessManagerALAWS);
            ExecuteQueryAndHandleResults("BusinessManager", "IND", BusinessManagerIND);

            ExecuteQueryAndHandleResults("PRO", "ALAB", PROALAB);
            ExecuteQueryAndHandleResults("PRO", "ALAWS", PROALAWS);
            ExecuteQueryAndHandleResults("PRO", "IND", PROIND);

            ExecuteQueryAndHandleResults("ITRepresentative", "ALAB", ITRepresentativeALAB);
            ExecuteQueryAndHandleResults("ITRepresentative", "ALAWS", ITRepresentativeALAWS);
            ExecuteQueryAndHandleResults("ITRepresentative", "IND", ITRepresentativeIND);


            ExecuteQueryAndHandleResults("ECERepresentative", "ALAB", ECERepresentativeALAB);
            ExecuteQueryAndHandleResults("ECERepresentative", "ALAWS", ECERepresentativeALAWS);
            ExecuteQueryAndHandleResults("ECERepresentative", "IND", ECERepresentativeIND);

            ExecuteQueryAndHandleResults("CPERepresentative", "ALAB", CPERepresentativeALAB);
            ExecuteQueryAndHandleResults("CPERepresentative", "ALAWS", CPERepresentativeALAWS);
            ExecuteQueryAndHandleResults("CPERepresentative", "IND", CPERepresentativeIND);


        }



        private void btnReturn2Voting_Click(object sender, RoutedEventArgs e)
        {
            borderGetVoterNumber.Visibility = Visibility.Visible;
            btnExit.Visibility = Visibility.Visible;
            btnReturn2Voting.Visibility = Visibility.Hidden;
            btnTallyBoard.Visibility = Visibility.Visible;
            svElectionResult.Visibility = Visibility.Hidden;
        }
    }
}
