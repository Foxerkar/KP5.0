using KP.DataBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
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

namespace KP.Pages
{
    /// <summary>
    /// Логика взаимодействия для Disciplina_semestr.xaml
    /// </summary>
    public partial class Disciplina_semestr : Page
    {
        private const string ConnectionString = "Data Source=Ucheb_Raspisnie.db;Version=3;";

        // Collection of Rab_Programma objects
        public ObservableCollection<Rab_Programma> RabProgrammy { get; set; }

        // Collection of Disciplina_Semestr objects
        public ObservableCollection<Disciplina_Semestr> DisciplinaSemestry { get; set; }

        // Currently selected Disciplina_Semestr object
        private Disciplina_Semestr _selectedDisciplinaSemestr;

        public Disciplina_semestr()
        {
            InitializeComponent();

            RabProgrammy = new ObservableCollection<Rab_Programma>();
            DisciplinaSemestry = new ObservableCollection<Disciplina_Semestr>();

            // Load the data into the collections
            LoadRabProgrammy();
            LoadDisciplinaSemestry();
            LoadTypeZanyatiya();
        }

        public Disciplina_Semestr SelectedDisciplinaSemestr
        {
            get { return _selectedDisciplinaSemestr; }
            set
            {
                _selectedDisciplinaSemestr = value;

                // If a Disciplina_Semestr object is selected, set its ID_RabProg property
                // to the ID_RabProg property of the selected Rab_Programma object
                if (_selectedDisciplinaSemestr != null && RabProgrammaComboBox.SelectedItem != null)
                {
                    _selectedDisciplinaSemestr.ID_RabProg = ((Rab_Programma)RabProgrammaComboBox.SelectedItem).ID_RabProg;
                }
            }
        }

        private void LoadRabProgrammy()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                var command = new SQLiteCommand("SELECT * FROM Rab_Programma", connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var rabProgramma = new Rab_Programma
                    {
                        ID_RabProg = reader.GetInt32(0),
                        ID_grup = reader.GetInt32(1),
                        ID_Disciplina = reader.GetInt32(2),
                        FAK_HOURS = reader.GetInt32(3),
                        AUDIT_HOURS = reader.GetInt32(4),
                        Gruppa = GetGruppa(reader.GetInt32(1)),
                        Disciplina = GetDisciplina(reader.GetInt32(2))
                    };

                    RabProgrammy.Add(rabProgramma);
                }

                connection.Close();
            }
        }

        // Method to load the data for the DisciplinaSemestry collection
        private void LoadDisciplinaSemestry()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                var command = new SQLiteCommand("SELECT * FROM Disciplina_Semestr", connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var disciplinaSemestr = new Disciplina_Semestr
                    {
                        ID_Program = reader.GetInt32(0),
                        NomSemestr = reader.GetInt32(1),
                        ID_RabProg = reader.GetInt32(2),
                        Type_Zanyatiya = reader.GetString(3),
                        Okonc_Semestra = reader.GetDateTime(4),
                        Rab_Programma = GetRabProgramma(reader.GetInt32(2))
                    };

                    DisciplinaSemestry.Add(disciplinaSemestr);
                }

                connection.Close();
            }
        }

        private Gruppa GetGruppa(int idGruppa)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                var command = new SQLiteCommand("SELECT * FROM Gruppa WHERE ID_gruppa = @ID_gruppa", connection);
                command.Parameters.AddWithValue("@ID_gruppa", idGruppa);

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Gruppa
                    {
                        ID_gruppa = reader.GetInt32(0),
                        NomGrup = reader.GetString(1),
                        Specialnost = reader.GetString(2)
                    };
                }

                connection.Close();
            }

            return null;
        }

        // Method to retrieve a Disciplina object by its ID
        private Disciplina GetDisciplina(int idDisciplina)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                var command = new SQLiteCommand("SELECT * FROM Disciplina WHERE ID_Disciplina = @ID_Disciplina", connection);
                command.Parameters.AddWithValue("@ID_Disciplina", idDisciplina);

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Disciplina
                    {
                        ID_Disciplina = reader.GetInt32(0),
                        Nazvanie = reader.GetString(1),
                        Opisanie = reader.GetString(2)
                    };
                }

                connection.Close();
            }

            return null;
        }

        // Method to retrieve a Rab_Programma object by its ID
        private Rab_Programma GetRabProgramma(int idRabProgramma)
        {
            return RabProgrammy.FirstOrDefault(rp => rp.ID_RabProg == idRabProgramma);
        }

        // Method to load the data for the Type_ZanyatiyaComboBox
        private void LoadTypeZanyatiya()
        {
            Type_ZanyatiyaComboBox.Items.Add("Lecture");
            Type_ZanyatiyaComboBox.Items.Add("Lab");
            Type_ZanyatiyaComboBox.Items.Add("Seminar");
            Type_ZanyatiyaComboBox.Items.Add("Practice");
        }

        // Event handler for the Click event of the AddDisciplinaSemestr_Click button
        private void AddDisciplinaSemestr_Click(object sender, RoutedEventArgs e)
        {
            var disciplinaSemestr = new Disciplina_Semestr
            {
                NomSemestr = int.Parse(NomSemestrComboBox.SelectedItem.ToString()),
                ID_RabProg = ((Rab_Programma)RabProgrammaComboBox.SelectedItem).ID_RabProg,
                Type_Zanyatiya = Type_ZanyatiyaComboBox.SelectedItem.ToString(),
                Okonc_Semestra = Okonc_Semestra.SelectedDate.Value
            };

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                var command = new SQLiteCommand("INSERT INTO Disciplina_Semestr (NomSemestr, ID_RabProg, Type_Zanyatiya, Okonc_Semestra) VALUES (@NomSemestr, @ID_RabProg, @Type_Zanyatiya, @Okonc_Semestra)", connection);
                command.Parameters.AddWithValue("@NomSemestr", disciplinaSemestr.NomSemestr);
                command.Parameters.AddWithValue("@ID_RabProg", disciplinaSemestr.ID_RabProg);
                command.Parameters.AddWithValue("@Type_Zanyatiya", disciplinaSemestr.Type_Zanyatiya);
                command.Parameters.AddWithValue("@Okonc_Semestra", disciplinaSemestr.Okonc_Semestra);

                command.ExecuteNonQuery();

                connection.Close();
            }

            DisciplinaSemestry.Add(disciplinaSemestr);
        }

        private void RabProgrammaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If a Disciplina_Semestr object is selected, set its ID_RabProg property
            // to the ID_RabProg property of the selected Rab_Programma object
            if (SelectedDisciplinaSemestr != null)
            {
                SelectedDisciplinaSemestr.ID_RabProg = ((Rab_Programma)RabProgrammaComboBox.SelectedItem)?.ID_RabProg ?? 0;
            }
        }

        // Event handler for the Click event of the DeleteDisciplinaSemestr_Click button
        private void DeleteDisciplinaSemestr_Click(object sender, RoutedEventArgs e)
        {
            var selectedDisciplinaSemestr = DisciplinaSemestrListView.SelectedItem as Disciplina_Semestr;

            if (selectedDisciplinaSemestr != null)
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();

                    var command = new SQLiteCommand("DELETE FROM Disciplina_Semestr WHERE ID_Program = @ID_Program", connection);
                    command.Parameters.AddWithValue("@ID_Program", selectedDisciplinaSemestr.ID_Program);

                    command.ExecuteNonQuery();

                    connection.Close();
                }

                DisciplinaSemestry.Remove(selectedDisciplinaSemestr);
            }
        }
    }
}