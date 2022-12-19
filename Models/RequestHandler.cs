using Syncfusion.UI.Xaml.Schedule;
using Syncfusion.Windows.Controls;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.Linq;

namespace WpfApp1.Models
{
    internal class RequestHandler
    {
        public DateTime DateTime = DateTime.UtcNow;
        FileStream fs;
        StreamReader sr;
        static Dictionary<Student, Request> requests = new Dictionary<Student, Request>();
        public static List<Course> cousrcelist = new List<Course>();
        public static List<Lesson> lessonslist = new List<Lesson>();
        public static List<Student> studentlist = new List<Student>();
        public int Sum=0;


        public RequestHandler()
        {
            
            this.fs = File.OpenRead("Data.txt");
            this.sr = new StreamReader(this.fs);
        }

        public void step()
        {
            string line;
            if (!sr.EndOfStream) cousrcelist.Sort();

            while ((line = sr.ReadLine()) != "!")
            {
                if (sr.EndOfStream) break;
                string[] words = line.Split(' ');
                string name = words[0];
                string lang = words[1];
                string level = words[2];
                string intensity = words[3];
                Student stud = new Student(name);
                studentlist.Add(stud);
                var req = stud.Request(lang, level, intensity);
                stud.Request(lang, level, intensity);
                requests.Add(stud, req);
            }
            //sr.ReadLine();
            //sr.ReadLine();
            
            RequestHandling();
            LessonsManagement();

            for (int i=0;i<studentlist.Count;i++)
            {
                //studentlist[i].UpdatePayment();
                if (studentlist[i].group.Item2 == false) {
                    if (studentlist[i].group.Item1 != null)
                    {
                        studentlist[i].KickFromGroup(studentlist[i].group.Item1);
                        studentlist.Remove(studentlist[i]);
                    }
                }
                
            }
            this.DateTime = this.DateTime.AddDays(14);
            
            
        }
        void RequestHandling()
        {
            //request handling
            foreach (var req in requests)
            {
                var c = new Course(req.Value.language, req.Value.level, req.Value.intensity);
                if (cousrcelist.Find(i => i.Name == c.Name) == null)
                    cousrcelist.Add(c);
                else
                    c = cousrcelist.Find(i => i.Name == c.Name);

                int let = c.groups.Count;
                if (c.groups.Count == 1) { Group group = new(c,let);c.groups.Add(group);let++; }
                for(int i = 1; i < c.groups.Count; i++)
                {
                    if (c.groups[i].students.Count <= 30)
                    {
                        if (requests.Keys.Contains(req.Key)) requests.Remove(req.Key);
                        req.Key.AddToGroup(c.groups[i]);
                        //c.groups[i].students.Add(req.Key);
                        
                    }
                    else
                    {
                        c.groups.Add(new Group(c,let));
                        let++;
                        
                    }
                }
            }
            //If people are enough students will be deleted from requests list
            foreach (var course in cousrcelist)
            {
                for(int i = 1;i<course.groups.Count;i++)
                {
                    if (course.groups[i].students.Count > 15)
                    {
                        foreach(var student in course.groups[i].students)
                        {
                            if (requests.Keys.Contains(student)) requests.Remove(student);
                            if (course.individgroup.students.Contains(student)) student.KickFromGroup(course.individgroup);
                        }
                    }
                    else
                    {

                        int j = 0;
                        while(course.groups[i].students.Count > 0)
                        {
                            Student t = course.groups[i].students[j];
                            t.KickFromGroup(course.groups[i]);
                            t.AddToGroup(course.individgroup);
                        }
                        course.groups.RemoveAt(i);
                        
                    }
                }
            }
        }
        void LessonsManagement()
        {
            foreach(Course course in cousrcelist)
            {
                foreach(var group in course.groups)
                {
                    foreach (var student in group.students) {
                        switch (group.Course.Intensity) 
                        {
                            case Intensity.Поддерживающее: new Lesson(DateTime, student, group); break;

                            case Intensity.Обычное:
                                lessonslist.Add(new Lesson(DateTime, student, group));
                                lessonslist.Add(new Lesson(DateTime.AddDays(1), student, group));
                                lessonslist.Add(new Lesson(DateTime.AddDays(2), student, group));

                                lessonslist.Add(new Lesson(DateTime.AddDays(7), student, group));
                                lessonslist.Add(new Lesson(DateTime.AddDays(8), student, group));
                                lessonslist.Add(new Lesson(DateTime.AddDays(9), student, group)); 
                                break;

                            case Intensity.Интенсив:
                                lessonslist.Add(new Lesson(DateTime, student, group));
                                lessonslist.Add(new Lesson(DateTime.AddDays(1), student, group));
                                lessonslist.Add(new Lesson(DateTime.AddDays(2), student, group));
                                lessonslist.Add(new Lesson(DateTime.AddDays(3), student, group));
                                lessonslist.Add(new Lesson(DateTime.AddDays(4), student, group));

                                lessonslist.Add(new Lesson(DateTime.AddDays(7), student, group));
                                lessonslist.Add(new Lesson(DateTime.AddDays(8), student, group));
                                lessonslist.Add(new Lesson(DateTime.AddDays(9), student, group));
                                lessonslist.Add(new Lesson(DateTime.AddDays(10), student, group));
                                lessonslist.Add(new Lesson(DateTime.AddDays(11), student, group));
                                break;
                        }
                    }
                }
            }
            foreach(var req in requests)
            {
                Student student = req.Key;
                switch (req.Value.intensity)
                {
                    case Intensity.Поддерживающее: new Lesson(DateTime, student); break;

                    case Intensity.Обычное:
                        lessonslist.Add(new Lesson(DateTime, student));
                        lessonslist.Add(new Lesson(DateTime.AddDays(1), student));
                        lessonslist.Add(new Lesson(DateTime.AddDays(2), student));

                        lessonslist.Add(new Lesson(DateTime.AddDays(7), student));
                        lessonslist.Add(new Lesson(DateTime.AddDays(8), student));
                        lessonslist.Add(new Lesson(DateTime.AddDays(9), student));
                        break;

                    case Intensity.Интенсив:
                        lessonslist.Add(new Lesson(DateTime, student));
                        lessonslist.Add(new Lesson(DateTime.AddDays(1), student));
                        lessonslist.Add(new Lesson(DateTime.AddDays(2), student));
                        lessonslist.Add(new Lesson(DateTime.AddDays(3), student));
                        lessonslist.Add(new Lesson(DateTime.AddDays(4), student));

                        lessonslist.Add(new Lesson(DateTime.AddDays(7), student));
                        lessonslist.Add(new Lesson(DateTime.AddDays(8), student));
                        lessonslist.Add(new Lesson(DateTime.AddDays(9), student));
                        lessonslist.Add(new Lesson(DateTime.AddDays(10), student));
                        lessonslist.Add(new Lesson(DateTime.AddDays(11), student));
                        break;
                }
            }
        }

    }
    
}
