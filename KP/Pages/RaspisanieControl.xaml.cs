using System.Windows.Controls;

namespace KP.Pages
{
    /// <summary>
    /// Логика взаимодействия для RaspisanieControl.xaml
    /// </summary>
    public partial class RaspisanieControl : Page
    {
        public RaspisanieControl()
        {
            InitializeComponent();
        }

        //private void LoadGroups()                     // Закоментировать Ctrl + K + C   Откоментировать Ctrl + K + U
        //{
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        SqlCommand command = new SqlCommand("SELECT GroupName FROM Groups", connection);
        //        SqlDataReader reader = command.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            GroupComboBox.Items.Add(reader["GroupName"].ToString());
        //        }
        //    }
        //}

    }
}
