using KP.DataBase;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace KP.Pages
{
    /// <summary>
    /// Логика взаимодействия для EDIT_Prepodavateli.xaml
    /// </summary>
    public partial class EDIT_Prepodavateli : Page
    {
        private const string ConnectionString = "Data Source=Ucheb_Raspisnie.db;Version=3;";
        public ObservableCollection<Prepodavateli> Prepodi { get; set; }
        public bool IsEditor { get; set; }
        public EDIT_Prepodavateli(bool isEditor)
        {
            InitializeComponent();
            Prepodi = new ObservableCollection<Prepodavateli>();
            DataContext = this;
            IsEditor = isEditor;
            LoadPrepodi();
        }

        private void LoadPrepodi()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                var command = new SQLiteCommand("SELECT * FROM Prepodavateli", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Prepodi.Add(new Prepodavateli
                        {
                            ID_Prepod = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            FName = reader.GetString(2),
                            Otchestvo = reader.GetString(3),
                            Specialnost = reader.GetString(4)
                        });
                    }
                }
            }
        }

        private void AddPrep_Click(object sender, RoutedEventArgs e)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string sql = "INSERT INTO Prepodavateli (Name, FName, Otchestvo, Specialnost) VALUES (@Name, @FName, @Otchestvo, @Specialnost)";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", Name.Text);
                    command.Parameters.AddWithValue("@FName", FName.Text);
                    command.Parameters.AddWithValue("@Otchestvo", Otchestvo.Text);
                    command.Parameters.AddWithValue("@Specialnost", Specialnost.Text);

                    command.ExecuteNonQuery();
                }
            }
            Prepodi.Clear();
            LoadPrepodi();

            // Clear the input fields
            ClearInputFields();
        }

        private void DeletePrep_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = PrepodListView.SelectedItem as Prepodavateli;
            if (selectedUser != null)
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();

                    string sql = "DELETE FROM Prepodavateli WHERE ID_Prepod = @ID_Prepod";
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ID_Prepod", selectedUser.ID_Prepod);
                        command.ExecuteNonQuery();
                    }
                }

                // Refresh the user list
                Prepodi.Clear();
                LoadPrepodi();
            }
            else
            {
                MessageBox.Show("Выберите Преподавателя для удаления!", "Ошибка!");
            }
        }


        private void ClearInputFields()
        {
            Name.Text = string.Empty;
            FName.Text = string.Empty;
            Otchestvo.Text = string.Empty;
            Specialnost.Text = string.Empty;
        }
    }
}
