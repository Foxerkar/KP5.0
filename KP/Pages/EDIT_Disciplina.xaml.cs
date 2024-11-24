using KP.DataBase;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace KP.Pages
{
    /// <summary>
    /// Логика взаимодействия для EDIT_Disciplina.xaml
    /// </summary>
    public partial class EDIT_Disciplina : Page
    {
        private const string ConnectionString = "Data Source=Ucheb_Raspisnie.db;Version=3;";
        public ObservableCollection<Disciplina> Disciplini { get; set; }
        public bool IsEditor { get; set; }
        public EDIT_Disciplina(bool isEditor)
        {
            InitializeComponent();
            Disciplini = new ObservableCollection<Disciplina>();
            IsEditor = isEditor;
            DataContext = this;
            LoadDisciplin();
        }

        private void LoadDisciplin()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                var command = new SQLiteCommand("SELECT * FROM Disciplina", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Disciplini.Add(new Disciplina
                        {
                            ID_Disciplina = reader.GetInt32(0),
                            Nazvanie = reader.GetString(1),
                            Opisanie = reader.GetString(2),

                        });
                    }
                }
            }
        }

        private void AddDisciplina_Click(object sender, RoutedEventArgs e)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string sql = "INSERT INTO Disciplina (Nazvanie, Opisanie) VALUES (@Nazvanie, @Opisanie)";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Nazvanie", Nazvanie.Text);
                    command.Parameters.AddWithValue("@Opisanie", Opisanie.Text);

                    command.ExecuteNonQuery();
                }
            }
            Disciplini.Clear();
            LoadDisciplin();
            // Clear the input fields
            ClearInputFields();
        }

        private void DeleteDiciplina_Click(object sender, RoutedEventArgs e)
        {
            var selectedDis = PrepodListView.SelectedItem as Disciplina;
            if (selectedDis != null)
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();

                    string sql = "DELETE FROM Disciplina WHERE ID_Disciplina = @ID_Disciplina";
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ID_Disciplina", selectedDis.ID_Disciplina);
                        command.ExecuteNonQuery();
                    }
                }

                // Обновляем лист аудиторий
                Disciplini.Clear();
                LoadDisciplin();
            }
            else
            {
                MessageBox.Show("Выберите Аудиторию для удаления!", "Ошибка!");
            }
        }

        private void ClearInputFields()
        {
            Nazvanie.Text = string.Empty;
            Opisanie.Text = string.Empty;
        }
    }
}
