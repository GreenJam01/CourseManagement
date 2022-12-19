using Syncfusion.Windows.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Controls;

namespace WpfApp1.Models
{
    #region Перечисления
    enum Language
    {
       Английский,
       Японский,
       Русский,
       Французкий,
    }

    enum Level
    {
        Начальный,
        Средний,
        Продвинутый
    }

    enum Intensity
    {
        Интенсив, Обычное, Поддерживающее 
    }
    #endregion

    #region Классы
    internal class Course:IComparable<Course>
    {
        private string name;

        public string Name { get { return name; } }
        Language language;
        public Language Language { get=>language; }

        private Level level;

        public Level Level { get => level; }

        private Intensity intensity;

        public Intensity Intensity { get => intensity; }

        private int price;

        public int Price { get => price; }

        public List<Group> Groups { get=>groups; set { groups = value; } }
        public List<Group> groups;


        public Group individgroup;
        public Course(Language language, Level level, Intensity intensity, int price=500)
        {
            
            this.language = language;
            this.level = level;
            this.intensity = intensity;
            this.name += language.ToString()+ " " + level.ToString()+ " " + intensity.ToString() ;
            groups = new List<Group>();
            individgroup = new Group(this, "Индивидуальная группа");
            groups.Add(individgroup);
            this.price = price;
           
        }

        public int CompareTo(Course? course)
        {
            if (course is null) throw new ArgumentException("Некорректное значение параметра");
            else if (language.ToString() != course.language.ToString()) return -1; else return 0;
            
        }
    }
    internal class Group
    {
       
        private Course course;
        private string name;
        public string Name { get => name; set => name = value; }
        
        public Course Course { get => course; }
        public List<Student> students;

        public List<Student> Students { get => students; set => students = value; }

        public void AddStudent(Student student)
        {
            students.Add(student);
        }

        public void KickStudent(Student student)
        {
            students.Remove(student);
        }

        public Group(Course course, int name)
        {
            this.course = course;
            this.name += "Группа " + name;
            students = new List<Student>();
        }
        public Group(Course course, string name)
        {
            this.course = course;
            this.name = name;
            students = new List<Student>();
        }
        
    }

    // Please let me study, I will be a good student!!!!!!!
    internal class Request
    {
        public Language language;
        public Level level;
        public Intensity intensity;
        public Request(string course, string level, string intensity)
        {
            switch (course)
            {
                case "английский":
                    this.language = Language.Английский;
                    break;
                case "японский":
                    this.language = Language.Японский;
                    break;
                case "русский":
                    this.language = Language.Русский;
                    break;
                case "французский":
                    this.language = Language.Французкий;
                    break;
            }
            switch (level)
            {
                case "начальный":
                    this.level = Level.Начальный;
                    break;
                case "средний":
                    this.level = Level.Средний;
                    break;
                case "Продвинутый":
                    this.level = Level.Продвинутый;
                    break;
            }
            switch (intensity)
            {
                case "интенсив":
                    this.intensity = Intensity.Интенсив;
                    break;
                case "обычное":
                    this.intensity = Intensity.Обычное;
                    break;
                case "поддерживающее":
                    this.intensity = Intensity.Поддерживающее;
                    break;
            }
        }
    }

    internal class Student
    {

        
        string name;

        public string Name { get =>name; }

        public string IsPaid 
        { 
            get 
            {
                if (group.Item2) return "Да";
                else return "Нет";
            } 
        }
        public (Group,bool) group;
        private Request request;

        public void AddToGroup(Group gr)
        {
            bool isPaid=true;
            group = (gr, isPaid);
            gr.AddStudent(this);
        }

        public void KickFromGroup(Group gr)
        {
            group.Item1 = null;
            gr.KickStudent(this);
            
        }

        public Request Request(string course, string level, string intensity)
        {
            request = new Request(course, level, intensity);
            return request;
        }

        public void DeleteRequest()
        {
            
        }

        public Student(string name)
        {
            this.name = name;
            //this.request = new List<Request>();
        }

        public void UpdatePayment()
        {
            bool paid;
            Random r = new Random();         
            if (r.Next(1, 100) < 99) paid = true;
            else paid = false;
            this.group = (this.group.Item1, paid);
            
        }
    }

    internal class Lesson
    {
        
        public string d;
        public Group gr;
        public Student stud;

        public bool single=true;

        private string name;
        public string Name { get=>name; set=>name=value; }

        public Lesson(DateTime date, Student stud,Group group )
        {
            
            this.d = date.ToShortDateString();
            this.stud = stud;
            this.gr = group;
            this.single = false;
            if(stud.group.Item1!=null) this.name = stud.Name +" "+ stud.group.Item1.Course.Name + " " + stud.group.Item1.Name;
        }

        public Lesson(DateTime d, Student stud)
        {
            
            this.d = d.ToShortDateString();
            this.stud = stud;
            this.single = true;
            if (stud.group.Item1 != null) this.name =  stud.Name + stud.group.Item1.Course.Name + " " + stud.group.Item1.Name;
        }
    }

    //internal class Comp<Course>: ICo
        
    //{
    //    public int Compare(T x, T y)
    //    {
    //        if (x.Language.ToString() != y.Language.ToString())
    //        {
    //            return 1;
    //        }
    //        else return 0;
    //    }
    //}
    internal struct DataPoint
    {
        public double XValue { get; set; }

        public double YValue { get; set; }

    }

    #endregion
}
