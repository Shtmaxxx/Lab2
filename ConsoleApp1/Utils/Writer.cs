using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleApp1.Utils
{
    static class Writer
    {
        static public void SaveStudentsToXml()
        {
            List<Student> students = Data.Students;

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            using (XmlWriter xmlWriter = XmlWriter.Create("students.xml", settings))
            {
                xmlWriter.WriteStartElement("students");
                foreach (var student in students)
                {
                    xmlWriter.WriteStartElement("student");
                    xmlWriter.WriteElementString("name", student.Name);
                    xmlWriter.WriteElementString("birthday", student.BirthDay.ToString());
                    xmlWriter.WriteElementString("group", student.Group);
                    xmlWriter.WriteElementString("average_mark", student.AverageMark.ToString());
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
            }
        }

        static public void DisplayStudents()
        {
            int counter = 0;
            XmlDocument doc = new XmlDocument();
            doc.Load("students.xml");
            foreach(XmlNode node in doc.DocumentElement)
            {
                counter++;
                var name = node["name"].InnerText;
                var birthday = node["birthday"].InnerText;
                var group = node["group"].InnerText;
                var averageMark = node["average_mark"].InnerText;
                var student = new Student(name, group, DateTime.Parse(birthday), int.Parse(averageMark));
                Console.WriteLine($"Student #{counter}");
                Console.WriteLine(student.ToString() + "\n");
            }
            Console.WriteLine();
        }

        static public void SaveTeachersToXml()
        {
            List<Teacher> teachers = Data.Teachers;

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            using (XmlWriter xmlWriter = XmlWriter.Create("teachers.xml", settings))
            {
                xmlWriter.WriteStartElement("teachers");
                foreach (var teacher in teachers)
                {
                    xmlWriter.WriteStartElement("teacher");
                    xmlWriter.WriteElementString("name", teacher.Name);
                    xmlWriter.WriteElementString("position", teacher.Position);

                    xmlWriter.WriteStartElement("students");
                    foreach (var student in teacher.Students)
                    {
                        xmlWriter.WriteStartElement("student");
                        xmlWriter.WriteElementString("name", student.Name);
                        xmlWriter.WriteElementString("birthday", student.BirthDay.ToString());
                        xmlWriter.WriteElementString("group", student.Group);
                        xmlWriter.WriteElementString("average_mark", student.AverageMark.ToString());
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
            }
        }

        static public void DisplayTeachers()
        {
            int counter = 0;
            XmlDocument doc = new XmlDocument();
            doc.Load("teachers.xml");
            foreach (XmlNode node in doc.DocumentElement)
            {
                counter++;
                var name = node["name"].InnerText;
                var position = node["position"].InnerText;
                List<Student> studentsList = new List<Student>();

                foreach (XmlNode student in node["students"].ChildNodes)
                {
                    var studentName = student["name"].InnerText;
                    var birthday = student["birthday"].InnerText;
                    var group = student["group"].InnerText;
                    var averageMark = student["average_mark"].InnerText;
                    var studentInstance = new Student(studentName, group, DateTime.Parse(birthday), int.Parse(averageMark));
                    studentsList.Add(studentInstance);
                }

                var teacher = new Teacher(name, position, studentsList);
                Console.WriteLine($"Teacher #{counter}");
                Console.WriteLine(teacher.ToString());

                Console.WriteLine("Students list:");
                foreach (var student in studentsList)
                {
                    Console.WriteLine(student.ToString());
                }
                Console.WriteLine();
            }
        }
    }
}
