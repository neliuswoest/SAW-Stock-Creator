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
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            comboBox1.SelectedIndex = 7;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string invNumber;
            string invDescription;
            string sellingPrice1;
            string inventoryItemID;

            if ((comboBox1.SelectedIndex < 1) || (String.IsNullOrEmpty(textBox1.Text)) || (String.IsNullOrEmpty(textBox2.Text)) || (String.IsNullOrEmpty(textBox3.Text)))
            {
                MessageBox.Show("You have to enter values in all fields.", "Error",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
            }
            else
            {
                try
                {
                    EvoDAC.SDKConnector(); //Create connection to DB using SDK
                    EvoDAC.WriteToLogFile(DateTime.Now.ToString() + " - Connection success - SDK "); //Record step in log file.
                    invNumber = textBox1.Text;
                    invDescription = textBox2.Text;
                    sellingPrice1 = textBox3.Text;
                    InventoryItem inventoryItem = new InventoryItem();
                    if (InventoryItem.FindByCode(invNumber) == -1) //Check if inventory item exists
                    {
                        try
                        {
                            //Create inventory Item
                            InventoryItem invItem = new InventoryItem();
                            PriceList p1 = new PriceList("Price List A");
                            invItem.Code = invNumber;
                            invItem.Description = invDescription;
                            invItem.IsWarehouseTracked = true;
                            invItem.SellingPrices[p1].PriceExcl = Convert.ToDouble(sellingPrice1);
                            invItem.Save();
                            inventoryItemID = InventoryItem.FindByCode(invNumber).ToString(); //Get the newly created inventory item ID.
                            inventoryItem.SellingPrices[p1].PriceExcl = Convert.ToDouble(sellingPrice1);
                            //This section update the "WhseStk" table and set the "Defaults" tick not selected.
                            string str1 = string.Format("where WHStockLink = {0} and WHWhseID = {1}", InventoryItem.FindByCode(invNumber), Warehouse.FindByCode("CPT"));
                            DatabaseContext.ExecuteCommandScalar(string.Format("Update WhseStk set WHUseInfoDefs = {0} {1}", 0, str1));
                            string str2 = string.Format("where WHStockLink = {0} and WHWhseID = {1}", InventoryItem.FindByCode(invNumber), Warehouse.FindByCode("DBN"));
                            DatabaseContext.ExecuteCommandScalar(string.Format("Update WhseStk set WHUseInfoDefs = {0} {1}", 0, str2));
                            string str3 = string.Format("where WHStockLink = {0} and WHWhseID = {1}", InventoryItem.FindByCode(invNumber), Warehouse.FindByCode("KIM"));
                            DatabaseContext.ExecuteCommandScalar(string.Format("Update WhseStk set WHUseInfoDefs = {0} {1}", 0, str3));
                            string str4 = string.Format("where WHStockLink = {0} and WHWhseID = {1}", InventoryItem.FindByCode(invNumber), Warehouse.FindByCode("GAU"));
                            DatabaseContext.ExecuteCommandScalar(string.Format("Update WhseStk set WHUseInfoDefs = {0} {1}", 0, str4));
                            string str5 = string.Format("where WHStockLink = {0} and WHWhseID = {1}", InventoryItem.FindByCode(invNumber), Warehouse.FindByCode("GEO"));
                            DatabaseContext.ExecuteCommandScalar(string.Format("Update WhseStk set WHUseInfoDefs = {0} {1}", 0, str5));

                            switch (comboBox1.SelectedItem.ToString())
                            {
                                case "ABBM":
                                    MessageBox.Show("ABBM");
                                    string str6 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'KIM-ABBM' where WHWhseID = 5 and WHStockLink = {0}", inventoryItemID);
                                    string str7 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'GAU-ABBM' where WHWhseID = 3 and WHStockLink = {0}", inventoryItemID);
                                    string str8 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'DBN-ABBM' where WHWhseID = 2 and WHStockLink = {0}", inventoryItemID);
                                    string str9 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'GEO-ABBM' where WHWhseID = 4 and WHStockLink = {0}", inventoryItemID);
                                    string str10 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'CPT-ABBM' where WHWhseID = 22 and WHStockLink = {0}", inventoryItemID);
                                    DatabaseContext.ExecuteCommandScalar(str6);
                                    DatabaseContext.ExecuteCommandScalar(str7);
                                    DatabaseContext.ExecuteCommandScalar(str8);
                                    DatabaseContext.ExecuteCommandScalar(str9);
                                    DatabaseContext.ExecuteCommandScalar(str10);
                                    break;
                                case "DABM":
                                    string str11 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'KIM-DABM' where WHWhseID = 5 and WHStockLink = {0}", inventoryItemID);
                                    string str12 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'GAU-DABM' where WHWhseID = 3 and WHStockLink = {0}", inventoryItemID);
                                    string str13 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'DBN-DABM' where WHWhseID = 2 and WHStockLink = {0}", inventoryItemID);
                                    string str14 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'GEO-DABM' where WHWhseID = 4 and WHStockLink = {0}", inventoryItemID);
                                    string str15 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'CPT-DABM' where WHWhseID = 22 and WHStockLink = {0}", inventoryItemID);
                                    DatabaseContext.ExecuteCommandScalar(str11);
                                    DatabaseContext.ExecuteCommandScalar(str12);
                                    DatabaseContext.ExecuteCommandScalar(str13);
                                    DatabaseContext.ExecuteCommandScalar(str14);
                                    DatabaseContext.ExecuteCommandScalar(str15);
                                    break;
                                case "ELEC":
                                    string str16 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'KIM-ELEC' where WHWhseID = 5 and WHStockLink = {0}", inventoryItemID);
                                    string str17 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'GAU-ELEC' where WHWhseID = 3 and WHStockLink = {0}", inventoryItemID);
                                    string str18 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'DBN-ELEC' where WHWhseID = 2 and WHStockLink = {0}", inventoryItemID);
                                    string str19 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'GEO-ELEC' where WHWhseID = 4 and WHStockLink = {0}", inventoryItemID);
                                    string str20 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'CPT-ELEC' where WHWhseID = 22 and WHStockLink = {0}", inventoryItemID);
                                    DatabaseContext.ExecuteCommandScalar(str16);
                                    DatabaseContext.ExecuteCommandScalar(str17);
                                    DatabaseContext.ExecuteCommandScalar(str18);
                                    DatabaseContext.ExecuteCommandScalar(str19);
                                    DatabaseContext.ExecuteCommandScalar(str20);
                                    break;
                                case "FS":
                                    string str21 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'KIM-FS' where WHWhseID = 5 and WHStockLink = {0}", inventoryItemID);
                                    string str22 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'GAU-FS' where WHWhseID = 3 and WHStockLink = {0}", inventoryItemID);
                                    string str23 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'DBN-FS' where WHWhseID = 2 and WHStockLink = {0}", inventoryItemID);
                                    string str24 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'GEO-FS' where WHWhseID = 4 and WHStockLink = {0}", inventoryItemID);
                                    string str25 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'CPT-FS' where WHWhseID = 22 and WHStockLink = {0}", inventoryItemID);
                                    DatabaseContext.ExecuteCommandScalar(str21);
                                    DatabaseContext.ExecuteCommandScalar(str22);
                                    DatabaseContext.ExecuteCommandScalar(str23);
                                    DatabaseContext.ExecuteCommandScalar(str24);
                                    DatabaseContext.ExecuteCommandScalar(str25);
                                    break;
                                case "GFM":
                                    string str26 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'KIM-FS' where WHWhseID = 5 and WHStockLink = {0}", inventoryItemID);
                                    string str27 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'GAU-FS' where WHWhseID = 3 and WHStockLink = {0}", inventoryItemID);
                                    string str28 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'DBN-FS' where WHWhseID = 2 and WHStockLink = {0}", inventoryItemID);
                                    string str29 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'GEO-FS' where WHWhseID = 4 and WHStockLink = {0}", inventoryItemID);
                                    string str30 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'CPT-FS' where WHWhseID = 22 and WHStockLink = {0}", inventoryItemID);
                                    DatabaseContext.ExecuteCommandScalar(str26);
                                    DatabaseContext.ExecuteCommandScalar(str27);
                                    DatabaseContext.ExecuteCommandScalar(str28);
                                    DatabaseContext.ExecuteCommandScalar(str29);
                                    DatabaseContext.ExecuteCommandScalar(str30);
                                    break;
                                case "ILHM":
                                    string str31 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'KIM-ILHM' where WHWhseID = 5 and WHStockLink = {0}", inventoryItemID);
                                    string str32 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'GAU-ILHM' where WHWhseID = 3 and WHStockLink = {0}", inventoryItemID);
                                    string str33 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'DBN-ILHM' where WHWhseID = 2 and WHStockLink = {0}", inventoryItemID);
                                    string str34 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'GEO-ILHM' where WHWhseID = 4 and WHStockLink = {0}", inventoryItemID);
                                    string str35 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'CPT-ILHM' where WHWhseID = 22 and WHStockLink = {0}", inventoryItemID);
                                    DatabaseContext.ExecuteCommandScalar(str31);
                                    DatabaseContext.ExecuteCommandScalar(str32);
                                    DatabaseContext.ExecuteCommandScalar(str33);
                                    DatabaseContext.ExecuteCommandScalar(str34);
                                    DatabaseContext.ExecuteCommandScalar(str35);
                                    break;
                                case "MECH":
                                    string str36 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'KIM-MECH' where WHWhseID = 5 and WHStockLink = {0}", inventoryItemID);
                                    string str37 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'GAU-MECH' where WHWhseID = 3 and WHStockLink = {0}", inventoryItemID);
                                    string str38 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'DBN-MECH' where WHWhseID = 2 and WHStockLink = {0}", inventoryItemID);
                                    string str39 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'GEO-MECH' where WHWhseID = 4 and WHStockLink = {0}", inventoryItemID);
                                    string str40 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'CPT-MECH' where WHWhseID = 22 and WHStockLink = {0}", inventoryItemID);
                                    DatabaseContext.ExecuteCommandScalar(str36);
                                    DatabaseContext.ExecuteCommandScalar(str37);
                                    DatabaseContext.ExecuteCommandScalar(str38);
                                    DatabaseContext.ExecuteCommandScalar(str39);
                                    DatabaseContext.ExecuteCommandScalar(str40);
                                    break;
                                case "MISC":
                                    string str41 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'KIM-MISC' where WHWhseID = 5 and WHStockLink = {0}", inventoryItemID);
                                    string str42 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'GAU-MISC' where WHWhseID = 3 and WHStockLink = {0}", inventoryItemID);
                                    string str43 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'DBN-MISC' where WHWhseID = 2 and WHStockLink = {0}", inventoryItemID);
                                    string str44 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'GEO-MISC' where WHWhseID = 4 and WHStockLink = {0}", inventoryItemID);
                                    string str45 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'CPT-MISC' where WHWhseID = 22 and WHStockLink = {0}", inventoryItemID);
                                    DatabaseContext.ExecuteCommandScalar(str41);
                                    DatabaseContext.ExecuteCommandScalar(str42);
                                    DatabaseContext.ExecuteCommandScalar(str43);
                                    DatabaseContext.ExecuteCommandScalar(str44);
                                    DatabaseContext.ExecuteCommandScalar(str45);
                                    break;
                                case "MSMM":
                                    string str46 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'KIM-MSMM' where WHWhseID = 5 and WHStockLink = {0}", inventoryItemID);
                                    string str47 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'GAU-MSMM' where WHWhseID = 3 and WHStockLink = {0}", inventoryItemID);
                                    string str48 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'DBN-MSMM' where WHWhseID = 2 and WHStockLink = {0}", inventoryItemID);
                                    string str49 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'GEO-MSMM' where WHWhseID = 4 and WHStockLink = {0}", inventoryItemID);
                                    string str50 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'CPT-MSMM' where WHWhseID = 22 and WHStockLink = {0}", inventoryItemID);
                                    DatabaseContext.ExecuteCommandScalar(str46);
                                    DatabaseContext.ExecuteCommandScalar(str47);
                                    DatabaseContext.ExecuteCommandScalar(str48);
                                    DatabaseContext.ExecuteCommandScalar(str49);
                                    DatabaseContext.ExecuteCommandScalar(str50);
                                    break;
                                case "QUANTIS":
                                    string str51 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'KIM-QUANTIS' where WHWhseID = 5 and WHStockLink = {0}", inventoryItemID);
                                    string str52 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'GAU-QUANTIS' where WHWhseID = 3 and WHStockLink = {0}", inventoryItemID);
                                    string str53 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'DBN-QUANTIS' where WHWhseID = 2 and WHStockLink = {0}", inventoryItemID);
                                    string str54 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'GEO-QUANTIS' where WHWhseID = 4 and WHStockLink = {0}", inventoryItemID);
                                    string str55 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'CPT-QUANTIS' where WHWhseID = 22 and WHStockLink = {0}", inventoryItemID);
                                    DatabaseContext.ExecuteCommandScalar(str51);
                                    DatabaseContext.ExecuteCommandScalar(str52);
                                    DatabaseContext.ExecuteCommandScalar(str53);
                                    DatabaseContext.ExecuteCommandScalar(str54);
                                    DatabaseContext.ExecuteCommandScalar(str55);
                                    break;
                                case "RHBM":
                                    string str56 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'KIM-RHBM' where WHWhseID = 5 and WHStockLink = {0}", inventoryItemID);
                                    string str57 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'GAU-RHBM' where WHWhseID = 3 and WHStockLink = {0}", inventoryItemID);
                                    string str58 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'DBN-RHBM' where WHWhseID = 2 and WHStockLink = {0}", inventoryItemID);
                                    string str59 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'GEO-RHBM' where WHWhseID = 4 and WHStockLink = {0}", inventoryItemID);
                                    string str60 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'CPT-RHBM' where WHWhseID = 22 and WHStockLink = {0}", inventoryItemID);
                                    DatabaseContext.ExecuteCommandScalar(str56);
                                    DatabaseContext.ExecuteCommandScalar(str57);
                                    DatabaseContext.ExecuteCommandScalar(str58);
                                    DatabaseContext.ExecuteCommandScalar(str59);
                                    DatabaseContext.ExecuteCommandScalar(str60);
                                    break;
                                default:
                                    string str61 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'KIM-MISC' where WHWhseID = 5 and WHStockLink = {0}", inventoryItemID);
                                    string str62 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'GAU-MISC' where WHWhseID = 3 and WHStockLink = {0}", inventoryItemID);
                                    string str63 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'DBN-MISC' where WHWhseID = 2 and WHStockLink = {0}", inventoryItemID);
                                    string str64 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'GEO-MISC' where WHWhseID = 4 and WHStockLink = {0}", inventoryItemID);
                                    string str65 = string.Format("update [dbo].[WhseStk] set WHStockGroup = 'CPT-MISC' where WHWhseID = 22 and WHStockLink = {0}", inventoryItemID);
                                    DatabaseContext.ExecuteCommandScalar(str61);
                                    DatabaseContext.ExecuteCommandScalar(str62);
                                    DatabaseContext.ExecuteCommandScalar(str63);
                                    DatabaseContext.ExecuteCommandScalar(str64);
                                    DatabaseContext.ExecuteCommandScalar(str65);
                                    break;
                            }

                            MessageBox.Show("Inventory Item Created as: \n\nCode: "+ invNumber + "\nDescription: "+ invDescription + "\nSellingPrice: "+ sellingPrice1 + " ", "Success", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            textBox1.Clear();
                            textBox2.Clear();
                            textBox3.Clear();
                            comboBox1.SelectedIndex = 7;
                        }
                        catch (Exception ex)
                        {
                            EvoDAC.WriteToLogFile(DateTime.Now.ToString() + " - Failed to create inventory item " + invNumber + " to to database using the SDK due to: " + ex.ToString());
                            throw;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Could not create inventory item!", "Item already exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                        comboBox1.SelectedIndex = 7;
                    }
                }
                catch (Exception ex)
                {
                    EvoDAC.WriteToLogFile(DateTime.Now.ToString() + " - Failed to connect to to database using the SDK due to: " + ex.ToString());
                    throw;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar >= 48 && (int)e.KeyChar <= 57 || ((int)e.KeyChar == 46 || (int)e.KeyChar == 8))
                return;
            e.Handled = true;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar != 34 && (int)e.KeyChar != 39)
                return;
            e.Handled = true;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar != 34 && (int)e.KeyChar != 39)
                return;
            e.Handled = true;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //test
    }

}