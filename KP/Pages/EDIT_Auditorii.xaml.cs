using KP.DataBase;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace KP.Pages
{
    /// <summary>
    /// Логика взаимодействия для EDIT_Auditorii.xaml
    /// </summary>
    public partial class EDIT_Auditorii : Page
    {
        private const string ConnectionString = "Data Source=Ucheb_Raspisnie.db;Version=3;";
        public ObservableCollection<Auditoria> Auditorii { get; set; }
        public bool IsEditor { get; set; }
        public EDIT_Auditorii(bool isEditor)
        {
            InitializeComponent();
            Auditorii = new ObservableCollection<Auditoria>();
            DataContext = this;
            IsEditor = isEditor;
            LoadAudit();
        }

        private void LoadAudit()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                var command = new SQLiteCommand("SELECT * FROM Auditoria", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Auditorii.Add(new Auditoria
                        {
                            ID_Auditor = reader.GetInt32(0),
                            NomAud = reader.GetString(1),
                            Type = reader.GetString(2),

                        });
                    }
                }
            }
        }

        private void AddAudit_Click(object sender, RoutedEventArgs e)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string sql = "INSERT INTO Auditoria (NomAud, Type) VALUES (@NomAud, @Type)";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@NomAud", NomAud.Text);
                    command.Parameters.AddWithValue("@Type", Type.Text);

                    command.ExecuteNonQuery();
                }
            }
            Auditorii.Clear();
            LoadAudit();

            // Очистить введенные данные
            ClearInputFields();
        }

        private void DeleteAudit_Click(object sender, RoutedEventArgs e)
        {
            var selectedAud = PrepodListView.SelectedItem as Auditoria;
            if (selectedAud != null)
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();

                    string sql = "DELETE FROM Auditoria WHERE ID_Auditor = @ID_Auditor";
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ID_Auditor", selectedAud.ID_Auditor);
                        command.ExecuteNonQuery();
                    }
                }

                // Обновляем лист аудиторий
                Auditorii.Clear();
                LoadAudit();
            }
            else
            {
                MessageBox.Show("Выберите Аудиторию для удаления!", "Ошибка!");
            }
        }
        private void ClearInputFields()
        {
            NomAud.Text = string.Empty;
            Type.Text = string.Empty;
        }


    }
}
