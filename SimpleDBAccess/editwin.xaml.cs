using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace SimpleDBAccess
{
    /// <summary>
    /// Interaktionslogik für editwin.xaml
    /// </summary>
    public partial class editwin : Window
    {


        private int _selectedValue;

        public class Edit
        {
            public int DeviceID { get; set; }
            public string Model { get; set; }
            public string Manufacturer { get; set; }
            public bool HaveUser { get; set; }
            public int? EmployeeID { get; set; }
        }

        public editwin(int selectedValue)
        {
            InitializeComponent();
            _selectedValue = selectedValue;
            LoadData();
        }

        private void LoadData()
        {
            string connectionString = "Server=localhost;Database=databs;User ID=root;Password=GZDxfvg49KeqVf+#8AEV3b;";
            string query = "SELECT Model, Manufacturer, Haveuser, EmployeeID FROM laptops WHERE DeviceID = @DeviceID";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@DeviceID", _selectedValue);

                try
                {
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int haveUserOrdinal = reader.GetOrdinal("Haveuser");
                            bool haveUser = reader.IsDBNull(haveUserOrdinal) ? false : reader.GetBoolean(haveUserOrdinal);

                            int? employeeID = null;
                            if (haveUser)
                            {
                                int employeeIDOrdinal = reader.GetOrdinal("EmployeeID");
                                employeeID = reader.IsDBNull(employeeIDOrdinal) ? (int?)null : reader.GetInt32(employeeIDOrdinal);
                            }

                            DeviceIDTextBox.Text = _selectedValue.ToString();
                            modleTextBox.Text = reader.IsDBNull(reader.GetOrdinal("Model")) ? null : reader.GetString(reader.GetOrdinal("Model"));
                            manuTextBox.Text = reader.IsDBNull(reader.GetOrdinal("Manufacturer")) ? null : reader.GetString(reader.GetOrdinal("Manufacturer"));
                            checknox12.IsChecked = haveUser;
                            empid_TextBox.Text = employeeID?.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler beim Laden der Daten: {ex.Message}");
                }
            }
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            empid_TextBox.IsEnabled = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            empid_TextBox.IsEnabled = false;
            empid_TextBox.Text = string.Empty;
            uiderror.Visibility = Visibility.Hidden;
            empid_TextBox.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#FFABADB3");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool exit = false;
            string connectionString = "Server=localhost;Database=databs;User ID=root;Password=GZDxfvg49KeqVf+#8AEV3b;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE laptops SET Manufacturer = @Manufacturer, Model = @Model, Haveuser = @Haveuser, EmployeeID = @EmployeeID, Hadproblems = @Hadproblems WHERE DeviceID = @DeviceID";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    bool isnum = int.TryParse(DeviceIDTextBox.Text.Trim(), out _);
                    if (!string.IsNullOrEmpty(DeviceIDTextBox.Text.Trim()) && isnum)
                    {
                        command.Parameters.AddWithValue("@DeviceID", DeviceIDTextBox.Text.Trim());
                        diderror.Visibility = Visibility.Hidden;
                        DeviceIDTextBox.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#FFABADB3");
                    }
                    else
                    {
                        diderror.Visibility = Visibility.Visible;
                        DeviceIDTextBox.BorderBrush = Brushes.Red;
                        exit = true;
                    }

                    if (checknox12.IsChecked == true)
                    {
                        isnum = int.TryParse(empid_TextBox.Text.Trim(), out _);
                        if (!string.IsNullOrEmpty(empid_TextBox.Text.Trim()) && isnum)
                        {
                            command.Parameters.AddWithValue("@EmployeeID", empid_TextBox.Text.Trim());
                            command.Parameters.AddWithValue("@Haveuser", true);
                            uiderror.Visibility = Visibility.Hidden;
                            empid_TextBox.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#FFABADB3");
                        }
                        else
                        {
                            uiderror.Visibility = Visibility.Visible;
                            empid_TextBox.BorderBrush = Brushes.Red;
                            exit = true;
                        }
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Haveuser", false);
                        command.Parameters.AddWithValue("@EmployeeID", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(manuTextBox.Text.Trim()))
                    {
                        command.Parameters.AddWithValue("@Manufacturer", manuTextBox.Text.Trim());
                        manuTextBox.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#FFABADB3");
                    }
                    else
                    {
                        manuTextBox.BorderBrush = Brushes.Red;
                        exit = true;
                    }

                    if (!string.IsNullOrEmpty(modleTextBox.Text.Trim()))
                    {
                        command.Parameters.AddWithValue("@Model", modleTextBox.Text.Trim());
                        modleTextBox.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#FFABADB3");
                    }
                    else
                    {
                        modleTextBox.BorderBrush = Brushes.Red;
                        exit = true;
                    }

                    command.Parameters.AddWithValue("@Hadproblems", false);

                    if (!exit)
                    {
                        try
                        {
                            command.ExecuteNonQuery();
                            //MessageBox.Show("Datensatz erfolgreich aktualisiert.");
                            MainWindow win3 = new MainWindow();
                            win3.Show();
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ein Fehler ist aufgetreten: " + ex.Message);
                        }
                    }
                }
            }
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow win3 = new MainWindow();
            win3.Show();
            this.Close();
        }
    }
}