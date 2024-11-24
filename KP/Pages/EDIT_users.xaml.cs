using KP.DataBase;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace KP.Pages
{
    public partial class EDIT_users : Page
    {

        private const string ConnectionString = "Data Source=Ucheb_Raspisnie.db;Version=3;";
        public ObservableCollection<Polzovateli> Users { get; set; }

        public EDIT_users()
        {
            InitializeComponent();
            Users = new ObservableCollection<Polzovateli>();
            DataContext = this;
            LoadUsers();
        }

        private void LoadUsers()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                var command = new SQLiteCommand("SELECT * FROM Polzovateli", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Users.Add(new Polzovateli
                        {
                            ID_Polzovatel = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            FName = reader.GetString(2),
                            LOGIN = reader.GetString(3),
                            //PASSWORD = reader.GetString(4)
                        });
                    }
                }
            }
        }

        private void ButtonAddUser_Click(object sender, RoutedEventArgs e)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string sql = "INSERT INTO Polzovateli (Name, FName, LOGIN, PASSWORD) VALUES (@Name, @FName, @LOGIN, @PASSWORD)";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", textBoxName.Text);
                    command.Parameters.AddWithValue("@FName", textBoxFName.Text);
                    command.Parameters.AddWithValue("@LOGIN", textBoxLogin.Text);
                    command.Parameters.AddWithValue("@PASSWORD", passwordBox.Password);

                    command.ExecuteNonQuery();
                }
            }

            // Refresh the user list
            Users.Clear();
            LoadUsers();

            // Clear the input fields
            ClearInputFields();
        }

        private void ButtonDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = listViewUsers.SelectedItem as Polzovateli;
            if (selectedUser != null)
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();

                    string sql = "DELETE FROM Polzovateli WHERE ID_Polzovatel = @ID_Polzovatel";
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ID_Polzovatel", selectedUser.ID_Polzovatel);
                        command.ExecuteNonQuery();
                    }
                }

                // Refresh the user list
                Users.Clear();
                LoadUsers();
            }
            else
            {
                MessageBox.Show("Выберите пользователя для удаления!", "Ошибка!");
            }
        }


        private void ClearInputFields()
        {
            textBoxName.Text = string.Empty;
            textBoxFName.Text = string.Empty;
            textBoxLogin.Text = string.Empty;
            passwordBox.Password = string.Empty;
        }
    }
}
