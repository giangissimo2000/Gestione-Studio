using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for Aggiungi.xaml
    /// </summary>
    public partial class Aggiungi_Verde : Window
    {
        string percorso = "";
        
       
        public Aggiungi_Verde()
        {

            InitializeComponent();
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("it-IT");
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("it-IT");
            
            string data = DateTime.Now.ToString("dd/MM/yyyy");
            scegli_data.SelectedDate  = DateTime.Now.Date;
            Verifica_Database();
           
            Read_Utenti();
            
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



                command.CommandText = "SELECT * FROM utente  ORDER BY NOME;";


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

        private bool IsNumberKey(Key inKey)
        {
            if (inKey < Key.D0 || inKey > Key.D9)
            {
                if (inKey < Key.NumPad0 || inKey > Key.NumPad9)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsActionKey(Key inKey)
        {
            return inKey == Key.Delete || inKey == Key.Back ||  inKey == Key.Return || Keyboard.Modifiers.HasFlag(ModifierKeys.Alt);


        }

        private void importo_block_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9-,]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Salva_Click(object sender, RoutedEventArgs e)
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
                            string data = scegli_data.SelectedDate.Value.ToString("yyyy/MM/dd");



                            //  DateTime.Now.ToString("yyyy/MM/dd");



                            string movimento = "";
                           
                            string s = scegli_data.SelectedDate.Value.ToString("MMMM", new CultureInfo("it-IT"));
                            string mese = new CultureInfo("it-IT").TextInfo.ToTitleCase(s.ToUpper());
                            string descrizione = descrizione_block.Text;
                            string gruppo = "FONDO CASSA";
                            string utente = utenti_combo.Text;
                            string importo = importo_block.Text;
                            decimal number1_;
                            if (Decimal.TryParse(importo, out number1_))
                            {
                                if (number1_ > 0) { movimento = "ENTRATA"; } else { movimento = "USCITA"; }

                            }


                           



                            aggiungi_voce(data, mese, gruppo, descrizione, importo, movimento, utente);
                            Application.Current.Properties["PassGate"] = mese;
                            Application.Current.Properties["PassGate2"] = mese;
                            //var myObject = this.Owner as MainWindow;

                            //   myObject.Read_Database(mese);
                            //   myObject.totale();
                            this.Close();
                        }


                    

                        }
                    else MessageBox.Show("Digitare Importo!!");

                }
                    else MessageBox.Show("Digitare Descrizione!");
                
            }
            else MessageBox.Show("Selezionare utente!");
        }


        private void aggiungi_voce(string data, string mese,string gruppo,string descrizione,string importo,string movimento,string utente)
        {
            try
            {
                string path = Directory.GetCurrentDirectory();

                SQLiteConnection aggiungi = new SQLiteConnection("Data Source = "+ percorso + "; Version = 3; ");
                aggiungi.Open();
                descrizione = descrizione.Replace("'", "''");
                string sql = "insert into fondocassaverde(data,mese,gruppo,descrizione,importo,tipo_mov,utente) values ('" + data + "','" + mese + "','" + gruppo + "','" + descrizione + "','" + importo + "','" + movimento + "','"  + utente + "')";
                // string sqlh = "update Prodotti set Giacenza ='" + quantitanew + "'  where Codice ='" + codice + "'";
                //insert into Prodotti (CodiceAAMS,Prezzo_pacchetto,Tipologia) values ( '" + CodiceAAMS + "','" + Prezzo_pacc + "','" + Tipologia + "')";//


                SQLiteCommand command = new SQLiteCommand(sql, aggiungi);
                command.ExecuteNonQuery();
                aggiungi.Close();

            }
            catch { }

        }

        private void Esci_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Properties["PassGate"] = "";
            string mm = scegli_data.SelectedDate.Value.ToString("MMMM", new CultureInfo("it-IT"));
            Application.Current.Properties["PassGate2"] = mm.ToUpper();
            this.Close();
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




    