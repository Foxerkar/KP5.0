using KP.DataBase;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace KP.Pages
{
    public partial class GruppaControl
    {
        private const string ConnectionString = "Data Source=Ucheb_Raspisnie.db;Version=3;";
        public ObservableCollection<Gruppa> Groups { get; set; }
        public bool IsEditor { get; set; }

        public GruppaControl(bool isEditor)
        {
            InitializeComponent();
            IsEditor = isEditor;
            Groups = new ObservableCollection<Gruppa>();
            GroupsListView.ItemsSource = Groups;
            DataContext = this;
            LoadData();
        }


        private void LoadData()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM Gruppa", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Groups.Add(new Gruppa
                        {
                            ID_gruppa = reader.GetInt32(0),
                            NomGrup = reader.GetString(1),
                            Specialnost = reader.GetString(2)
                        });
                    }
                }
            }
        }

        private void AddGroup_Click(object sender, RoutedEventArgs e)
        {
            var newGroup = new Gruppa
            {
                NomGrup = NomGrupTextBox.Text,
                Specialnost = SpecialnostTextBox.Text
            };
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("INSERT INTO Gruppa (NomGrup, Specialnost) VALUES (@NomGrup, @Specialnost)", connection);
                command.Parameters.AddWithValue("@NomGrup", newGroup.NomGrup);
                command.Parameters.AddWithValue("@Specialnost", newGroup.Specialnost);
                command.ExecuteNonQuery();

            }

            Groups.Add(newGroup);

            // Refresh the user list
            Groups.Clear();
            LoadData();
        }


        private void DeleteGroup_Click(object sender, RoutedEventArgs e)
        {
            if (GroupsListView.SelectedItem is Gruppa selectedGroup)
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    var command = new SQLiteCommand("DELETE FROM Gruppa WHERE ID_gruppa = @ID_gruppa", connection);
                    command.Parameters.AddWithValue("@ID_gruppa", selectedGroup.ID_gruppa);
                    command.ExecuteNonQuery();
                }
                Groups.Remove(selectedGroup);
            }
        }

        private void GroupsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GroupsListView.SelectedItem is Gruppa selectedGroup)
            {
                NomGrupTextBox.Text = selectedGroup.NomGrup;
                SpecialnostTextBox.Text = selectedGroup.Specialnost;
            }
        }

    }
}
