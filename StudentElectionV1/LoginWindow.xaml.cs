using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using Mysqlx.Resultset;
using System;
using System.Collections.Generic;
using System.Data;
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
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StudentElectionV1
{
    
    public partial class LoginWindow : Window
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
        MainWindow bintana = new MainWindow();
        public LoginWindow()
        {
            InitializeComponent();
            Connect2DB();            
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

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

            // string sql = "SELECT COUNT(*) FROM users WHERE USERNAME=@username AND USERPASS=@userpass";
            string sql2 = "SELECT * FROM users WHERE USERNAME=@username AND USERPASS=@userpass";            
            cmd = new MySqlCommand(sql2, conn);            
            var username = txtUserName.Text;
            var userpass = txtUserPass.Password.ToString();                        
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@userpass", userpass);
            var myResult = cmd.ExecuteReader();

            // int count = Convert.ToInt32(cmd.ExecuteScalar());

            // execute another query


            myResult.Read();

            // if (count != 0);
            if (myResult.HasRows)
            {                
                bintana.Show();
                bintana.txtTemp.Text = myResult.GetValue(4).ToString().Trim();
                myResult.Close();
                conn.Close();
                this.Close();
            }
            else
            {
                myResult.Close();
                errorMessage.Visibility = Visibility.Visible;
               
            }                       
        }

        /*   private int EncodePass(string pass2Encode)
        {
            int encoded = 0;
            char temp;
            int i = pass2Encode.Length;            
            for (int j = 0; j <i; j++)
            {                
                temp = pass2Encode[j];
                encoded = encoded + Asc(temp);
            }
            return encoded;
        }

       private int Asc(char String)
        {
            int int32 = Convert.ToInt32(String);
            if (int32 < 128)
                return int32;
            try
            {
                Encoding fileIoEncoding = Encoding.Default;
                char[] chars = new char[1] { String };
                if (fileIoEncoding.IsSingleByte)
                {
                    byte[] bytes = new byte[1];
                    fileIoEncoding.GetBytes(chars, 0, 1, bytes, 0);
                    return (int)bytes[0];
                }
                byte[] bytes1 = new byte[2];
                if (fileIoEncoding.GetBytes(chars, 0, 1, bytes1, 0) == 1)
                    return (int)bytes1[0];
                if (BitConverter.IsLittleEndian)
                {
                    byte num = bytes1[0];
                    bytes1[0] = bytes1[1];
                    bytes1[1] = num;
                }
                return (int)BitConverter.ToInt16(bytes1, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string CeasarCipher(string test)
          {
              string encoded="";            

              char[] highlet = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M' };
              char[] lowlet = { 'Z', 'Y', 'X', 'W', 'V', 'U', 'T', 'S', 'R', 'Q', 'P', 'O', 'N' };

              test = test.ToUpper();
              int el = test.Length;
              for (int i=0; i<el; i++) 
              {
                  for (int k=0; k<13; k++) 
                  {                    
                      if (Asc(test[i]) > 64 && Asc(test[i]) < 78)
                      {
                          if (test[i] == highlet[k]) 
                          {
                              encoded = encoded + lowlet[k];
                              break;                        
                          }                        
                      }
                      else
                      {
                          if (Asc(test[i]) == 32)
                          {
                              encoded = encoded + " ";
                              break;
                          }
                          else
                          {
                              if (test[i] == lowlet[k])
                              {
                                  encoded = encoded + highlet[k];
                                  break;
                              }
                          } // == 32
                      }  // > 64 and < 78
                  }  // for k
              } // for i

              return encoded; 
          }*/
    }
}
