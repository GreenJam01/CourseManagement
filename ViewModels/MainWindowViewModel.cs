using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using WpfApp1.Infrastructure.Commands;
using WpfApp1.Models;
using WpfApp1.ViewModels.Base;

namespace WpfApp1.ViewModels
{
    internal class MainWindowViewModel : ViewModel //Вью-модель окна
    {
        public Dispatcher dis = Application.Current.MainWindow.Dispatcher;//сущность, управляющая потоком
        #region Поля
        private string date;
        public string Date { get=>date; set
            {
                Set(ref date, value);

            } }
        public string sum;
        public string Sum
        {
            get => sum;
            set
            {
                Set(ref sum, value);
            }
        }

        private string dates;
        public string Dates
        {
            get => dates;
            set => Set(ref dates, value);
        }

        #endregion
        #region Коллекции
        private ObservableCollection<Course> courses;
        public ObservableCollection<Course> Courses
        {
            get => courses;
            set
            {
                courses = value;
                OnPropertyChanged(nameof(Courses));
            }
        }
        
        private ObservableCollection<Group> groups;
        public ObservableCollection<Group> Groups
        {
            get => groups;
            set
            {
                Set(ref groups, value); 
                OnPropertyChanged(nameof(Groups));
            }
        }

        private ObservableCollection<Student> students;

        public ObservableCollection<Student> Students 
        { 
            get => students;
            set 
            {
                Set(ref students, value);
                OnPropertyChanged(nameof(Students));
            }
        }

        private ObservableCollection<Lesson> lessons;

        public ObservableCollection<Lesson> Lessons { get => lessons; set => Set(ref lessons, value); }
        #endregion 
        public static RequestHandler handl = new RequestHandler(); // обработчик запросов

        #region Выбранные айтемы
        private Course _selectedCourse;

        public Course SelectedCourse{ get => _selectedCourse; set
            {
                //ObservableCollection<Group> groups = new ObservableCollection<Group>(value.groups);
                dis.BeginInvoke((Action)(()=> 
                {
                    Set(ref _selectedCourse, value);
                    Groups = new ObservableCollection<Group>(value.Groups);
                    
                    
                }
                ));
            }
            }

        private Group _selectedGroup;

        public Group SelectedGroup
        {
            get => _selectedGroup;
            set 
            {
                dis.BeginInvoke((Action)(() =>
                {
                    
                    Set(ref _selectedGroup, value);
                    if(SelectedCourse.Groups.Contains(SelectedGroup))
                    Students = new ObservableCollection<Student>(value.students);
                }
                ));
            }
        }
        #endregion

        //Чтобы работало расписание
        private DateTime? myDateTimeProperty = null;
        public DateTime? MyDateTimeProperty
        {
            get
            {
                if (myDateTimeProperty == null)
                {
                    myDateTimeProperty = DateTime.Today;
                }
                return myDateTimeProperty;
            }
            set
            {
                Set(ref myDateTimeProperty, value);
                Dates = MyDateTimeProperty.Value.ToShortDateString();
                Lessons = new ObservableCollection<Lesson>(RequestHandler.lessonslist.Where((i) => i.d == Dates));
                //Lessons = new ObservableCollection<Lesson>(RequestHandler.lessonslist);
            }
        }
        
        

        #region  Window Header
        private string _Title = "Приложение";

        public string Title
        {
            get => _Title;
            set
            {
                Set(ref _Title, value);
            }
        }
        #endregion //Заголовок окна
        #region Команды
        public ICommand CloseApplicationCommand { get; }
        #region Команда закрытия
        private bool CanCloseApplicationCommandExecute(object p) => true;

        private void OnCloseApplicationCommandExecuted(object p)
        {

        }
        #endregion
        #region Show
        public ICommand ShowGroupsOnCalendar { get; }
        private bool CanCShowGroupsOnCalendarCommandExecute(object p) => true;



        private void OnShowGroupsOnCalendarExecuted(object p)
        {



        }
        #endregion

        #region Команда перехода на две недели вперед

        public ICommand StepCommand { get; }
        public bool CanStepCommandExecute(object p) => true;

        private void OnStepCommandExecuted(object p) => dis.Invoke((Action)(() =>
        {
            handl.step();
            Courses = new ObservableCollection<Course>(RequestHandler.cousrcelist);
            if(SelectedCourse != null) Groups = new ObservableCollection<Group>(SelectedCourse.Groups);
            if (SelectedGroup != null) Students = new ObservableCollection<Student>(SelectedGroup.Students);
            //OnPropertyChanged(nameof(Groups));
            handl.DateTime = handl.DateTime.AddDays(14);
            Date = handl.DateTime.ToShortDateString();

            
            foreach (var student in RequestHandler.studentlist) student.UpdatePayment();
            handl.Sum += RequestHandler.studentlist.Count * 500;
            Sum=handl.Sum.ToString();
            

        }));

        //private void OnStepCommandExecuted(object p)
        //{
        //    handl.step();
        //    OnPropertyChanged(nameof(lessons));
        //}




        #endregion
        #endregion

        public MainWindowViewModel()
        {
            Dates = MyDateTimeProperty.ToString();
            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            ShowGroupsOnCalendar = new LambdaCommand(OnShowGroupsOnCalendarExecuted, CanCShowGroupsOnCalendarCommandExecute);
            StepCommand = new LambdaCommand(OnStepCommandExecuted, CanStepCommandExecute);

            









        }
    }

}
