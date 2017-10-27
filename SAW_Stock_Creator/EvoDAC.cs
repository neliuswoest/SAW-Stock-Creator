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
using System.Configuration;


namespace WooCommerceIntegration
{
    public class EvoDAC
    {
        public static string SDservername = ConfigurationManager.AppSettings["EvolutionServerName"];
        public static string SDinitialCatalog = ConfigurationManager.AppSettings["EvolutionDatabase"];
        public static string SDuserName = ConfigurationManager.AppSettings["EvolutionUserName"];
        public static string SDpassword = ConfigurationManager.AppSettings["EvolutionPassword"];
        public static string LogFilePath = ConfigurationManager.AppSettings["LogFilePath"];

        public static DataTable ResultTable(string script)
        {
            DataTable dt = new DataTable(); //Create ne datatable to store script in that holds the query result
            dt.Clear();

            SqlConnectionStringBuilder cb = new SqlConnectionStringBuilder();
            cb.DataSource = SDservername;
            cb.InitialCatalog = SDinitialCatalog;
            cb.UserID = SDuserName;
            cb.Password = SDpassword;

            try
            {
                using (SqlConnection lSqlCon = new SqlConnection(cb.ToString()))
                {
                    lSqlCon.Open();
                    BusinessLogic.WriteToLogFile(DateTime.Now.ToString() + ": " + "Connection to Evolution database opened success.");
                    using (SqlCommand lSqlCmd = new SqlCommand())
                    {
                        lSqlCmd.Connection = lSqlCon;
                        lSqlCmd.CommandType = CommandType.Text;
                        lSqlCmd.CommandText = script;
                        lSqlCmd.ExecuteNonQuery();
                        SqlDataAdapter da = new SqlDataAdapter(lSqlCmd);
                        BusinessLogic.WriteToLogFile(DateTime.Now.ToString() + ": " + script + "\nExecuted and returned: ");
                        da.Fill(dt);
                        BusinessLogic.WriteToLogFile(DateTime.Now.ToString() + ": \nDatatable dt. ");
                    }
                    try
                    {
                        lSqlCon.Close();
                        BusinessLogic.WriteToLogFile(DateTime.Now.ToString() + ": " + "Connection to Evolution database closed success.");
                    }
                    catch (Exception ex) //If this connection does not close it will result in zombi connections.
                    {
                        BusinessLogic.WriteToLogFile(DateTime.Now.ToString() + ": " + ex.ToString());
                        //Force connection closure by sending modified packet header. Phase 2 Development.
                    }
                }
            }
            catch (Exception ex)
            {
                BusinessLogic.WriteToLogFile(DateTime.Now.ToString() + ": " + ex.ToString());
            }
            return dt;
        }


        public static void WriteToLogFile(string message)
        {
            try
            {
                string path = ConfigurationManager.AppSettings[LogFilePath]; //Path as set up in the configuration file
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(message);
                }
            }
            catch (Exception ex)
            {
                throw; //Throwing an anhandled exeption because if the writing to the log file fails then i cannot write it to the log file
                //Send an email to Heine if this occurs - Phase 2 development.
            }
        }
    }
}

