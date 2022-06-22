﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ConsoleApp1.Utils
{
    internal class QueryOutput
    {
        public static int Counter { get; set; }

        public QueryOutput()
        {
            Counter = 1;
        }

        public void DisplayResult<T>(IEnumerable<T> result, string queryTitle)
        {
            Console.WriteLine($"Query #{Counter++}\n{queryTitle}");
            foreach (var r in result)
            {
                Console.WriteLine(r.ToString());
            }
            Console.WriteLine();
        }

        public void DisplayGroupedStudentsResult(IEnumerable<IGrouping<string, XElement>> groups)
        {
            Console.WriteLine($"Query #{Counter++}\nGroup students by their group names");
            foreach (var group in groups)
            {
                Console.WriteLine(group.Key);
                foreach (var student in group)
                {
                    Console.WriteLine(student.Element("name").Value);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void DisplayGroupedTeachersResult(IEnumerable<IGrouping<int, XElement>> teachers)
        {
            Console.WriteLine($"Query #{Counter++}\nGroup teachers by students amount");
            foreach (var group in teachers)
            {
                Console.WriteLine("Amount of students: " + group.Key);
                foreach (var t in group)
                {
                    Console.WriteLine(t.Element("name").Value);
                }
                Console.WriteLine();
            }
        }
    }
}
