namespace KP.DataBase
{
    public class Raspisanie
    {
        public Raspisanie() { }
        public int ID_Rasp { get; set; }
        public int ID_prepod { get; set; }
        public int ID_auditor { get; set; }
        public int ID_Program { get; set; }
        public string Date { get; set; }
        public int Nom_Pari { get; set; }
        public int Hours { get; set; }
        public int Podgruppa { get; set; }
        public int Week { get; set; }

        public Prepodavateli Prepodavateli { get; set; }
        public Auditoria Auditoria { get; set; }
        //public Disciplina_Semestr Disciplina_Semestr { get; set; }

    }
}
