using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

//Untere nur machen wenn der using import nicht funktioniert!!!

//Vorher die dll MySql.Data.dll zu den Verweisen hinzufügen.
//dann kann man auf den Namespace verweisen.
//Quelle: https://dev.mysql.com/downloads/connector/net/



namespace SimpleDBAccess
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class Laptop
        {
            public int DeviceID { get; set; }
            public string Model { get; set; }
            public string Manufacturer { get; set; }
            public bool haveuser { get; set; }
            public int? EmployeeID { get; set; }
        }
        public MainWindow()
        {
            //MySqlConnection con = new MySqlConnection(@"SERVER = localhost;DATABASE=databs;UID=root;PASSWORD=GZDxfvg49KeqVf+#8AEV3b;");
            //con.Open();

            InitializeComponent();
        }
        private void newbutton_Click(object sender, RoutedEventArgs e)
        {
            newele win2 = new newele();
            win2.Show();
            this.Close();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=databs;User ID=root;Password=GZDxfvg49KeqVf+#8AEV3b;";
            List<Laptop> laptops = new List<Laptop>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                StringBuilder queryBuilder = new StringBuilder("SELECT * FROM laptops WHERE 1=1");

                if (!string.IsNullOrEmpty(DeviceIDTextBox.Text.Trim()))
                {
                    queryBuilder.Append(" AND DeviceID = @DeviceID");
                }
                if (!string.IsNullOrEmpty(ManufacturerTextBox.Text.Trim()))
                {
                    queryBuilder.Append(" AND manufacturer LIKE @Manufacturer");
                }
                if (!string.IsNullOrEmpty(ModelTextBox.Text.Trim()))
                {
                    queryBuilder.Append(" AND Model LIKE @Model");
                }
                if (!string.IsNullOrEmpty(EmployeeIDTextBox.Text.Trim()))
                {
                    queryBuilder.Append(" AND employeeID = @EmployeeID");
                }

                using (MySqlCommand command = new MySqlCommand(queryBuilder.ToString(), connection))
                {
                    if (!string.IsNullOrEmpty(DeviceIDTextBox.Text.Trim()))
                    {
                        command.Parameters.AddWithValue("@DeviceID", DeviceIDTextBox.Text.Trim());
                    }
                    if (!string.IsNullOrEmpty(ManufacturerTextBox.Text.Trim()))
                    {
                        command.Parameters.AddWithValue("@Manufacturer", "%" + ManufacturerTextBox.Text.Trim() + "%");
                    }
                    if (!string.IsNullOrEmpty(ModelTextBox.Text.Trim()))
                    {
                        command.Parameters.AddWithValue("@Model", "%" + ModelTextBox.Text.Trim() + "%");
                    }
                    if (!string.IsNullOrEmpty(EmployeeIDTextBox.Text.Trim()))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", EmployeeIDTextBox.Text.Trim());
                    }

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bool haveuser = reader.IsDBNull(reader.GetOrdinal("haveuser")) ? false : reader.GetBoolean("haveuser");
                            int? employeeID = null;

                            if (haveuser)
                            {
                                employeeID = reader.IsDBNull(reader.GetOrdinal("EmployeeID")) ? (int?)null : reader.GetInt32("EmployeeID");
                            }

                            laptops.Add(new Laptop
                            {
                                DeviceID = reader.GetInt32("DeviceID"),
                                Model = reader.IsDBNull(reader.GetOrdinal("Model")) ? null : reader.GetString("Model"),
                                haveuser = haveuser,
                                Manufacturer = reader.IsDBNull(reader.GetOrdinal("Manufacturer")) ? null : reader.GetString("Manufacturer"),
                                EmployeeID = employeeID
                            });
                        }
                    }
                }


            }
            idlist.Items.Clear();
            manulist.Items.Clear();
            modellist.Items.Clear();
            empidlist.Items.Clear();
            laptops.ForEach(laptop =>
            {
                idlist.Items.Add(laptop.DeviceID);
                manulist.Items.Add(laptop.Manufacturer);
                modellist.Items.Add(laptop.Model);
                if (laptop.haveuser == true)
                {
                    empidlist.Items.Add(laptop.EmployeeID);
                }
                else
                {
                    empidlist.Items.Add("No User");
                }
            });
        }
        private void myListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Ermitteln des angeklickten ListBoxItem
            var item = GetClickedListBoxItem(e.OriginalSource);
            if (item != null && int.TryParse(item.Content.ToString(), out int selectedValue))
            {
                editwin secondWindow = new editwin(selectedValue);
                secondWindow.Show();
                this.Close();
            }
        }
        private void ListBox_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalChange != 0)
            {
                SyncScroll(idlist, manulist);
                SyncScroll(idlist, modellist);
                SyncScroll(idlist, empidlist);
            }
        }

        private void SyncScroll(ListBox source, ListBox target)
        {
            var scrollViewer = GetScrollViewer(target);
            if (scrollViewer != null)
            {
                scrollViewer.ScrollToVerticalOffset(GetScrollViewer(source).VerticalOffset);
            }
        }

        private ScrollViewer GetScrollViewer(DependencyObject depObj)
        {
            if (depObj is ScrollViewer) return depObj as ScrollViewer;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);
                var result = GetScrollViewer(child);
                if (result != null) return result;
            }
            return null;
        }
        private ListBoxItem GetClickedListBoxItem(object originalSource)
        {
            DependencyObject current = originalSource as DependencyObject;
            while (current != null && !(current is ListBoxItem))
            {
                current = VisualTreeHelper.GetParent(current);
            }
            return current as ListBoxItem;
        }
        private void close_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /* private void Button_set_data(object sender, RoutedEventArgs e)
{
    ///Um diese Klassen benutzen zu können, müssen Sie den Namespace MySql.Data in den
    ///using-Direktiven (siehe oben) hinzufügen. 
    ///
    ///Jetzt können wir als Erstes eine Verbindung zur Datenbank herstellen. In einem sog.
    ///Connection-String geben wir die nötigen Verbindungsinformationen an:
    ///- Server-Adresse/IP
    ///- Name der Datenbank
    ///- User, der auf die Datenbank zugreifen darf und
    ///- Passwort (Sicherheitskriterien beachten!)
    ///Natürlich müssen Datenbank und User auf der Datenbank existieren.


    //Nachdem die Zugangsdaten gesetzt wurden, können wir den "Kanal" zur Datenbank öffnen.
    con.Open();  //Jetzt greift unser Programm über das Netzwerk/lokal auf die Datenbank zu.

    //Tabelle erstellen
    string table = "CREATE TABLE Personen_test (name VARCHAR(255), altere INT)";//Alter ist schon ein command in sql weshalb ich für das alter einfach altere geschrieben haben damit es zu keinen Komplikationen führt
    ///Nun können wir einen SQL-Befehl an die DB senden, der Daten in eine Tabelle einfügt. 
    ///Dies geht im einfachsten Fall über SQL-Befehle als String.
    string insert = "INSERT INTO Personen_test (Name, Altere) VALUES ('Morris', 32)"; //Alter ist schon ein command in sql weshalb ich für das alter einfach altere geschrieben haben damit es zu keinen Komplikationen führt
    //Damit der MySQL-Server das SQL-Statement verarbeiten kann, müssen wir es in einen MySqlCommand-Objekt umwandeln.
    //Dafür erstellen wir ein MySqlCommand-Objekt mit new...
    MySqlCommand cmd = new MySqlCommand();
    MySqlCommand cmd2 = new MySqlCommand();
    //..und fügen die nötigen Informationen hinzu:
    cmd2.CommandText = table;
    cmd2.Connection = con;

    cmd.CommandText = insert;
    cmd.Connection = con; //Damit weiß der Command, welche Verbindung er zum DB-Server verwenden soll.

    //Mit ExecuteNonQuery() führen wir den Command auf der DB aus.

    ///cmd2.ExecuteNonQuery();
    cmd2.ExecuteNonQuery();
    cmd.ExecuteNonQuery();

    AusgabeFeld.Text = "Data wurde erfolgreich in die Datenbank eingetragen!";

    //So, wenn alles funktioniert hat, solle nun in der Datenbank ein Eintrag in der Tabelle Personen_test vorhanden sein.
    //Überprüfen Sie dies....

    //Dies war ein Besipiel, hieran kannst du dich für deine Teil 3 Aufgaben orientieren. Die Buttons kannst du nach belieben bearbeiten.
    //Oder sogar neue hinzufügen

}*/


    }
}
