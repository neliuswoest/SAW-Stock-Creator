using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.IO;
using Pastel.Evolution;


namespace SAW_Stock_Creator
{
    public class EvoDAC
    {
        public static string EvolutionServerName = ConfigurationManager.AppSettings["EvolutionServerName"];
        public static string EvolutionInitialCatalog = ConfigurationManager.AppSettings["EvolutionDatabase"];
        public static string EvolutionUserName = ConfigurationManager.AppSettings["EvolutionUserName"];
        public static string EvolutionPassword = ConfigurationManager.AppSettings["EvolutionPassword"];

        public static string CommonServerName = ConfigurationManager.AppSettings["CommonServerName"];
        public static string CommonInitialCatalog = ConfigurationManager.AppSettings["CommonDatabase"];
        public static string CommonUserName = ConfigurationManager.AppSettings["CommonUserName"];
        public static string CommonPassword = ConfigurationManager.AppSettings["CommonPassword"];
        

        public static void WriteToLogFile(string message)
        {
                
            try
            {
                string path = ConfigurationManager.AppSettings["LogFilePath"];                
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(message);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void SDKConnector()
        {
            try
            {
                DatabaseContext.CreateCommonDBConnection(CommonServerName, CommonInitialCatalog, CommonUserName, CommonPassword, false);
                DatabaseContext.SetLicense("DE10110022", "5927195");
                DatabaseContext.CreateConnection(EvolutionServerName, EvolutionInitialCatalog, EvolutionUserName, EvolutionPassword, false);                
                WriteToLogFile(DateTime.Now.ToString() + " - Connection to Sage Evolution database successfull");
            }
            catch (Exception ex)
            {
                WriteToLogFile(DateTime.Now.ToString() + " - Connection to Sage Evolution database not successfull: " + ex.ToString());
                MessageBox.Show("Connection to Evolution failed. Please contact AboutIT on +27 (012) 460 1003", "Connection Failure");
                throw;
            }
        }

        public static void CommitSqlScript(string script)
        {
            SqlConnectionStringBuilder cb = new SqlConnectionStringBuilder();
            cb.DataSource = EvolutionServerName;
            cb.InitialCatalog = EvolutionInitialCatalog;
            cb.UserID = EvolutionUserName;
            cb.Password = EvolutionPassword;

            try
            {
                using (SqlConnection lSqlCon = new SqlConnection(cb.ToString()))
                {
                    lSqlCon.Open();
                    WriteToLogFile(DateTime.Now.ToString() + ": " + "Connection to Evolution database opened success.");
                    using (SqlCommand lSqlCmd = new SqlCommand())
                    {
                        lSqlCmd.Connection = lSqlCon;
                        lSqlCmd.CommandType = CommandType.Text;
                        lSqlCmd.CommandText = script;
                        lSqlCmd.ExecuteNonQuery();
                        SqlDataAdapter da = new SqlDataAdapter(lSqlCmd);
                        WriteToLogFile(DateTime.Now.ToString() + ": " + script + "\nExecuted and returned: ");
                    }
                    try
                    {
                        lSqlCon.Close();
                        WriteToLogFile(DateTime.Now.ToString() + ": " + "Connection to Evolution database closed success.");
                    }
                    catch (Exception ex) 
                    {
                        WriteToLogFile(DateTime.Now.ToString() + ": " + ex.ToString());
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteToLogFile(DateTime.Now.ToString() + ": " + ex.ToString());
            }
        }
    }
}