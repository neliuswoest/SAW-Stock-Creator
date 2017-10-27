using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pastel.Evolution;

namespace SAW_Stock_Creator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            MessageBox.Show("Test");

        }
    }
}



//public class EvoDAC
//{
//    public static string SDservername = ConfigurationManager.AppSettings["EvolutionServerName"];
//    public static string SDinitialCatalog = ConfigurationManager.AppSettings["EvolutionDatabase"];
//    public static string SDuserName = ConfigurationManager.AppSettings["EvolutionUserName"];
//    public static string SDpassword = ConfigurationManager.AppSettings["EvolutionPassword"];

//    public static DataTable ResultTable(string script)
//    {
//        DataTable dt = new DataTable(); //Create ne datatable to store script in that holds the query result
//        dt.Clear();

//        SqlConnectionStringBuilder cb = new SqlConnectionStringBuilder();
//        cb.DataSource = SDservername;
//        cb.InitialCatalog = SDinitialCatalog;
//        cb.UserID = SDuserName;
//        cb.Password = SDpassword;

//        try