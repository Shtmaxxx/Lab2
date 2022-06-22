using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ConsoleApp1.Utils
{
    internal class Queries
    {
        private XDocument _studentsDoc = XDocument.Load("students.xml");
        private XDocument _teachersDoc = XDocument.Load("teachers.xml");

        private IEnumerable<XElement> _students;
        private IEnumerable<XElement> _teachers;

        public Queries()
        {
            _students = _studentsDoc.Element("students").Elements("student");
            _teachers = _teachersDoc.Element("teachers").Elements("teacher");
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return from x in _students
                   select new Student(
                       x.Element("name").Value,
                       x.Element("group").Value,
                       DateTime.Parse(x.Element("birthday").Value),
                       int.Parse(x.Element("average_mark").Value)
                   );
        }

        public IEnumerable<string> GetAllTeachersPositions()
        {
            return from teacher in _teachers
                   select teacher.Element("position").Value;
        }

        public IEnumerable<Student> GetStudentsOfSpecificYear(int year)
        {
            return from student in _students
                   where DateTime.Parse(student.Element("birthday").Value).Year == year
                   select new Student(
                       student.Element("name").Value,
                       student.Element("group").Value,
                       DateTime.Parse(student.Element("birthday").Value),
                       int.Parse(student.Element("average_mark").Value)
                   );
        }

        public IEnumerable<Student> GetStudentsByFirstLetter(char letter)
        {
            return from student in _students
                   where student.Element("name").Value[0] == letter
                   orderby DateTime.Parse(student.Element("birthday").Value) ascending
                   select new Student(
                       student.Element("name").Value,
                       student.Element("group").Value,
                       DateTime.Parse(student.Element("birthday").Value),
                       int.Parse(student.Element("average_mark").Value)
                   );
        }

        public IEnumerable<Teacher> GetTeacherWithMostStudents()
        {
            return from teacher in _teachers
                   where teacher.Element("students").Elements("student").ToList().Count ==
                        _teachers.Max(x => x.Element("students").Elements("student").ToList().Count)
                   select new Teacher
                   {
                       Name = teacher.Element("name").Value,
                       Position = teacher.Element("position").Value,
                   };
        }

        public IEnumerable<object> GetStudentsByGroupName(string groupName)
        {
            return from student in _students
                   where student.Element("group").Value == groupName
                   select new 
                   { 
                       Name = student.Element("name").Value, 
                       Group = student.Element("group").Value 
                   };
        }

        public IEnumerable<object> GetSortedStudentNames()
        {
            return _students.OrderBy(student => student.Element("name").Value).Select(x => new { x.Element("name").Value });
        }

        public IEnumerable<object> GetStudentsSortedByMarks()
        {
            return _students.OrderByDescending(student => student.Element("average_mark").Value).
                        Select(x => new 
                        {
                            Name = x.Element("name").Value,
                            AverageMark = int.Parse(x.Element("average_mark").Value)
                        });
        }

        public IEnumerable<object> GetStudentsSortedByBirthDay()
        {
            return _students.OrderByDescending(s => DateTime.Parse(s.Element("birthday").Value)).
                        Select(y => new 
                        { 
                            Name = y.Element("name").Value, 
                            Birthday = DateTime.Parse(y.Element("birthday").Value),
                        });
        }

        // Get teacher with the most instances of students from the given group 
        public IEnumerable<object> GetTeacherByMostStudentGroupName(string groupName)
        {
            return _teachers.Where(t => t.Element("students").Elements("student").ToList().Where(s => s.Element("group").Value == groupName).Count() ==
                       _teachers.Max(t => t.Element("students").Elements("student").Where(s => s.Element("group").Value == groupName).Count()))
                           .Select(t => t.Element("name").Value);
        }

        public IEnumerable<object> GetStudentsByGroupNameAndMarks(string groupName, int mark)
        {
            return _students.Where(s => s.Element("group").Value == groupName && 
                        int.Parse(s.Element("average_mark").Value) > mark).
                        Select(x => new
                        {
                            Name = x.Element("name").Value, 
                            Group = x.Element("group").Value, 
                            AverageMark = x.Element("average_mark").Value,
                        });
        }


        // Get teachers with students who have the highest average mark
        public IEnumerable<object> GetTeachersWithBestStudents()
        {
            return _teachers.Where(t => t.Element("students").Elements("student").
                    Where(s => int.Parse(s.Element("average_mark").Value) ==
                        _students.Max(x => int.Parse(x.Element("average_mark").Value))).
                            Count() > 0).Select(a => a.Element("name").Value);
        }

        // Get teachers with students who have the lowest average mark
        public IEnumerable<object> GetTeachersWithWorstStudents()
        {
            return _teachers.Where(t => t.Element("students").Elements("student").
                        Where(s => int.Parse(s.Element("average_mark").Value) ==
                        _students.Min(x => int.Parse(x.Element("average_mark").Value))).Count() > 0).
                            Select(a => a.Element("name").Value);
        }

        public IEnumerable<IGrouping<string, XElement>> GroupStudentsByGroup()
        {
            return _students.GroupBy(x => x.Element("group").Value);
        }

        public IEnumerable<IGrouping<int, XElement>> GroupTeachersByStudentsAmount()
        {
            return _teachers.GroupBy(t => t.Element("students").Elements("student").ToList().Count);
        }
    }
}
