using KP.DataBase;
using System;

namespace KP.Pages
{
    public class Disciplina_Semestr
    {

        public int ID_Program { get; set; }
        public int NomSemestr { get; set; }
        public int ID_RabProg { get; set; }
        public string Type_Zanyatiya { get; set; }
        public DateTime Okonc_Semestra { get; set; }
        public Rab_Programma Rab_Programma { get; set; }

        public string DisplayName
        {
            get
            {
                return Rab_Programma != null ? $"{Rab_Programma.Gruppa?.NomGrup} - {Rab_Programma.Disciplina?.Nazvanie}" : string.Empty;
            }
        }
    }
}