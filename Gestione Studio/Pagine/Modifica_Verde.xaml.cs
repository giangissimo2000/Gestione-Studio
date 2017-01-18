using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Gestione_Studio
{
    /// <summary>
    /// Interaction logic for Modifica.xaml
    /// </summary>
    public partial class Modifica_Verde : Window
    {
        string id;
        string data3;
        string data4;
        string percorso = "";
        
        string mese;
        string gruppo;
        string descrizione;
        string importo;
       
        string tipo;
        string utente;
        public Modifica_Verde()
        {
            InitializeComponent();
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("it-IT");
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("it-IT");

           
           
            Verifica_Database();
           
            Read_Utenti();
            leggi_tabella();
        }

        private void Verifica_Database()
        {
            try
            {
                string path = Directory.GetCurrentDirectory();
                var parser = new FileIniDataParser();
                IniData data = parser.ReadFile(path + "\\" + "Config.ini");
                percorso = data["Generale"]["Percorso"];
               
            }
            catch
            {

                MessageBox.Show("Impossibile trovare il file Config.ini!");

            }


            try
            {
                if (File.Exists(percorso))
                {
                    percorso = percorso.Replace(@"\\", @"\\\");
                }
                else
                {
                    MessageBox.Show("Impossibile trovare il Database!");
                }
            }
            catch
            {
                MessageBox.Show("Impossibile trovare il Database!");

            }

        }
        public void trova_id (string data, string mese, string gruppo, string descrizione, string importo, string movimento,  string utente)
        {

            try
            {
                
                    
                    string path = Directory.GetCurrentDirectory();
                string ConString = "Data Source=" + percorso + ";Version=3;";



                SQLiteConnection connection = new SQLiteConnection(ConString);
                SQLiteCommand command = connection.CreateCommand();
                SQLiteDataReader Reader;

                descrizione = descrizione.Replace("'", "''");

                command.CommandText  = "select id from fondocassaverde where data ='" + data + "' and mese='" + mese + "' and gruppo='" + gruppo + "' and descrizione='" + descrizione + "' and importo='" + importo + "' and tipo_mov='" + movimento +  "' and utente='" + utente + "'";


                    connection.Open();
                    Reader = command.ExecuteReader();
                    if (Reader.HasRows)
                    {

                        while (Reader.Read())
                        {
                            
                            id = Reader["id"].ToString();
                           
                        }

                        //gruppi_combo.ItemsSource = dt.DefaultView;
                    }
                    Reader.Close();




                }
                catch (Exception e)
                {

                    MessageBox.Show("ERRORE!: ", e.ToString());

                }
                
                

           




        }


        public void leggi_tabella()
        {
            var myObject = this.Owner as MainWindow;
            string data = Application.Current.Properties["data_mod"].ToString();
            scegli_data.SelectedDate  = Convert.ToDateTime(data);
            data3 = DateTime.ParseExact(data, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy/MM/dd");
            data4 = Application.Current.Properties["data_mod"].ToString();
            mese = Application.Current.Properties["mese_mod"].ToString();
            gruppo = "FONDO CASSA";
           
            descrizione = Application.Current.Properties["descrizione_mod"].ToString();
            descrizione_block.Text = Application.Current.Properties["descrizione_mod"].ToString();
            importo = Application.Current.Properties["importo_mod"].ToString();
            importo_block.Text = Application.Current.Properties["importo_mod"].ToString();
           
            tipo  = Application.Current.Properties["tipo_mod"].ToString();
            utente = Application.Current.Properties["utente_mod"].ToString();
            utenti_combo.SelectedValue = Application.Current.Properties["utente_mod"].ToString();
           

            trova_id(data3, mese, gruppo, descrizione, importo, tipo, utente);

        }



        private void aggiorna_database(string data, string mese, string gruppo, string descrizione, string importo, string movimento,  string utente)
        {

            try
            {
                string path = Directory.GetCurrentDirectory();

                SQLiteConnection modifica = new SQLiteConnection("Data Source = "+ percorso + "; Version = 3; ");
                modifica.Open();
                descrizione = descrizione.Replace("'", "''");
                string sql = "update fondocassaverde set data ='" + data + "', mese='" + mese + "', gruppo='" + gruppo + "', descrizione='" + descrizione + "', importo='" + importo + "', tipo_mov='"  + movimento +  "', utente='" + utente + "'  where id='" + id + "'";
                // string sqlh = "update Prodotti set Giacenza ='" + quantitanew + "'  where Codice ='" + codice + "'";
                //insert into Prodotti (CodiceAAMS,Prezzo_pacchetto,Tipologia) values ( '" + CodiceAAMS + "','" + Prezzo_pacc + "','" + Tipologia + "')";//


                SQLiteCommand command = new SQLiteCommand(sql, modifica);
                command.ExecuteNonQuery();
                modifica.Close();

            }
            catch { }



        }


      

        private void Esci_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Properties["PassGate"] = "";
            this.Close();
        }

        private void Modifica_Click(object sender, RoutedEventArgs e)
        {
            if (utenti_combo.Text != "")
            {
                
                    if (descrizione_block.Text != "")
                    {
                        if (importo_block.Text != "")

                        {

                            
                            





                                string number = importo_block.Text;
                                decimal number_;
                                if (!Decimal.TryParse(number, out number_))
                                {
                                    MessageBox.Show("Importo non coretto!");
                                }

                                else
                                {




                                    string movimento = "";
                                   
                                  //  string s = scegli_data.SelectedDate.Value.ToString("MMMM", new CultureInfo("it-IT"));
                                  //  string mese1 = new CultureInfo("it-IT").TextInfo.ToTitleCase(s.ToUpper());
                                    string data3 = scegli_data.SelectedDate.Value.ToString("yyyy/MM/dd");
                                    string s = scegli_data.SelectedDate.Value.ToString("MMMM", new CultureInfo("it-IT"));
                                    string mese = new CultureInfo("it-IT").TextInfo.ToTitleCase(s.ToUpper());
                                    //string mese = 
                                    string descrizione = descrizione_block.Text;
                                    string gruppo = "FONDO CASSA";
                                    string utente = utenti_combo.Text;
                                    string importo = importo_block.Text;
                                    decimal number1_;
                                    if (Decimal.TryParse(importo, out number1_))
                                    {
                                        if (number1_ > 0) { movimento = "ENTRATA"; } else { movimento = "USCITA"; }

                                    }


                                   



                                    aggiorna_database(data3, mese, gruppo, descrizione, importo, movimento,  utente);
                                    var myObject = this.Owner as MainWindow;
                                    Application.Current.Properties["PassGate"] = mese;
                                    Application.Current.Properties["PassGate2"] = mese;
                                    // myObject.Read_Database(mese);
                                    //  myObject.totale();
                                    this.Close();
                                }


                            

                        }
                        else MessageBox.Show("Digitare Importo!!");
                    }
                    else MessageBox.Show("Digitare Descrizione!");
             

            }
            else MessageBox.Show("Selezionare utente!");





        }


        private void Read_Utenti()
        {
            try
            {
                DataTable dt = new DataTable();

                dt.Columns.Add("utente");
                string path = Directory.GetCurrentDirectory();
                string ConString = "Data Source=" + percorso + ";Version=3;";

                SQLiteConnection connection = new SQLiteConnection(ConString);
                SQLiteCommand command = connection.CreateCommand();
                SQLiteDataReader Reader;



                command.CommandText = "SELECT * FROM utente ORDER BY NOME";


                connection.Open();
                Reader = command.ExecuteReader();
                if (Reader.HasRows)
                {

                    while (Reader.Read())
                    {
                        DataRow ne = dt.NewRow();
                        string utenti = Reader["nome"].ToString();
                        utenti_combo.Items.Add(utenti);
                    }

                    //gruppi_combo.ItemsSource = dt.DefaultView;
                }
                Reader.Close();




            }
            catch (Exception e)
            {

                MessageBox.Show("ERRORE!: ", e.ToString());

            }
        }

        private void importo_block_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void importo_block_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9-,]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var picker = sender as DatePicker;

            // ... Get nullable DateTime from SelectedDate.
            DateTime? date = picker.SelectedDate;
            if (date == null)
            {
                // ... A null object.
                this.Title = "No date";
            }
            else
            {


                // ... No need to display the time.
                this.Title = date.Value.ToShortDateString();
            }
        }
    }
}
