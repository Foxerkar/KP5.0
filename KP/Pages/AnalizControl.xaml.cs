using KP.DataBase;
using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KP.Pages
{
    /// <summary>
    /// Логика взаимодействия для AnalizControl.xaml
    /// </summary>
    public partial class AnalizControl : Page
    {
        private const string ConnectionString = "Data Source=Ucheb_Raspisnie.db;Version=3;";
        public bool IsEditor { get; set; }
        public ObservableCollection<Gruppa> Gruppy { get; set; }
        public ObservableCollection<Disciplina> Discipliny { get; set; }
        public ObservableCollection<Rab_Programma> RabProgrammy { get; set; }
        public AnalizControl(bool isEditor)
        {
            InitializeComponent();
            IsEditor = isEditor;
            DataContext = this;

            Gruppy = new ObservableCollection<Gruppa>();
            Discipliny = new ObservableCollection<Disciplina>();
            RabProgrammy = new ObservableCollection<Rab_Programma>();

            LoadData();
        }

        // Загрузка данных
        private void LoadData()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("SELECT * FROM Gruppa", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Gruppy.Add(new Gruppa
                            {
                                ID_gruppa = reader.GetInt32(0),
                                NomGrup = reader.GetString(1),
                                Specialnost = reader.GetString(2)
                            });
                        }
                    }
                }

                using (var command = new SQLiteCommand("SELECT * FROM Disciplina", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Discipliny.Add(new Disciplina
                            {
                                ID_Disciplina = reader.GetInt32(0),
                                Nazvanie = reader.GetString(1),
                                Opisanie = reader.GetString(2)
                            });
                        }
                    }
                }

                using (var command = new SQLiteCommand("SELECT * FROM Rab_Programma", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RabProgrammy.Add(new Rab_Programma
                            {
                                ID_RabProg = reader.GetInt32(0),
                                ID_grup = reader.GetInt32(1),
                                ID_Disciplina = reader.GetInt32(2),
                                FAK_HOURS = reader.GetInt32(3),
                                AUDIT_HOURS = reader.GetInt32(4)
                            });
                        }
                    }
                }

                connection.Close();
            }

            // Ставим группу и дисциплину в раб программу
            foreach (var rabProgramma in RabProgrammy)
            {
                rabProgramma.Gruppa = Gruppy.FirstOrDefault(g => g.ID_gruppa == rabProgramma.ID_grup);
                rabProgramma.Disciplina = Discipliny.FirstOrDefault(d => d.ID_Disciplina == rabProgramma.ID_Disciplina);
            }
        }

        private void AddRabProgramma_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Rab_Programma rabProgramma = new Rab_Programma
                {
                    ID_grup = (int)GruppaComboBox.SelectedValue,
                    ID_Disciplina = (int)DisciplinaComboBox.SelectedValue,
                    FAK_HOURS = int.Parse(FAK_HOURS.Text),
                    AUDIT_HOURS = int.Parse(AUDIT_HOURS.Text)
                };

                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();

                    using (var command = new SQLiteCommand(connection))
                    {
                        command.CommandText = "INSERT INTO Rab_Programma (ID_grup, ID_Disciplina, FAK_HOURS, AUDIT_HOURS) VALUES (@ID_grup, @ID_Disciplina, @FAK_HOURS, @AUDIT_HOURS)";
                        command.Parameters.AddWithValue("@ID_grup", rabProgramma.ID_grup);
                        command.Parameters.AddWithValue("@ID_Disciplina", rabProgramma.ID_Disciplina);
                        command.Parameters.AddWithValue("@FAK_HOURS", rabProgramma.FAK_HOURS);
                        command.Parameters.AddWithValue("@AUDIT_HOURS", rabProgramma.AUDIT_HOURS);
                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }

                rabProgramma.ID_RabProg = GetLastInsertedId();
                rabProgramma.Gruppa = Gruppy.FirstOrDefault(g => g.ID_gruppa == rabProgramma.ID_grup);
                rabProgramma.Disciplina = Discipliny.FirstOrDefault(d => d.ID_Disciplina == rabProgramma.ID_Disciplina);

                RabProgrammy.Add(rabProgramma);

                MessageBox.Show("Рабочая программа успешно добавлена!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }

            // Обновляем лист Программ
            RabProgrammy.Clear();
            LoadData();

            //Очистка текстбоксов
            ClearInputFields();
        }

        private int GetLastInsertedId()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("SELECT last_insert_rowid()", connection))
                {
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        private void DeleteRabProgramma_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Rab_Programma selectedRabProgramma = RabProgrammaListView.SelectedItem as Rab_Programma;

                if (selectedRabProgramma != null)
                {
                    using (var connection = new SQLiteConnection(ConnectionString))
                    {
                        connection.Open();

                        using (var command = new SQLiteCommand(connection))
                        {
                            command.CommandText = "DELETE FROM Rab_Programma WHERE ID_RabProg = @ID_RabProg";
                            command.Parameters.AddWithValue("@ID_RabProg", selectedRabProgramma.ID_RabProg);
                            command.ExecuteNonQuery();
                        }

                        connection.Close();
                    }

                    RabProgrammy.Remove(selectedRabProgramma);

                    MessageBox.Show("Рабочая программа успешно удалена!");
                }
                else
                {
                    MessageBox.Show("Выберите рабочую программу для удаления!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
            // Обновляем лист Программ
            RabProgrammy.Clear();
            LoadData();
        }

        private void ClearInputFields()
        {
            FAK_HOURS.Text = string.Empty;
            AUDIT_HOURS.Text = string.Empty;
        }

    }
}

