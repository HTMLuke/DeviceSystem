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
using static System.Net.Mime.MediaTypeNames;

namespace SimpleDBAccess
{
    /// <summary>
    /// Interaktionslogik für newele.xaml
    /// </summary>
    public partial class newele : Window
    {
        public newele()
        {
            InitializeComponent();
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
                string query = "INSERT INTO laptops (DeviceID, Manufacturer, Model, Haveuser, EmployeeID, Hadproblems) VALUES (@DeviceID, @Manufacturer, @Model, @Haveuser, @EmployeeID, @Hadproblems)";

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
                            command.Parameters.AddWithValue("@HaveUser", true);
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
                        command.Parameters.AddWithValue("@HaveUser", false);
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
                            //MessageBox.Show("Datensatz erfolgreich hinzugefügt.");
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