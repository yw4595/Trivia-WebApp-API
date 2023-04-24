using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json; // Library for working with JSON data
using PeopleAppGlobals; // Custom library for working with people data
using PeopleLib; // Custom library for defining people objects
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Net;

namespace JsonHtml
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Create sample data for people
            Globals.AddPeopleSampleData();

            // Create empty lists to hold teachers and students
            List<Teacher> teachers = new List<Teacher>();
            List<Student> students = new List<Student>();

            // Loop through all people and add them to the appropriate list
            foreach (KeyValuePair<string, Person> keyValuePair in Globals.people.sortedList)
            {
                if (keyValuePair.Value.GetType() == typeof(Teacher))
                {
                    teachers.Add((Teacher)keyValuePair.Value);
                }
                else
                {
                    students.Add((Student)keyValuePair.Value);
                }
            }

            // Convert the student and teacher lists to JSON strings
            string s = JsonConvert.SerializeObject(students);
            string t = JsonConvert.SerializeObject(teachers);

            // Write the student JSON string to a file
            StreamWriter writer = new StreamWriter("C:/Windows/Temp/students.json");
            writer.Write(s);
            writer.Close();

            // Write the teacher JSON string to a file
            writer = new StreamWriter("C:/Windows/Temp/teachers.json");
            writer.Write(t);
            writer.Close();

            // Read the student JSON file
            StreamReader reader = new StreamReader("C:/Windows/Temp/students.json");
            s = reader.ReadToEnd();
            reader.Close();

            // Read the teacher JSON file
            reader = new StreamReader("C:/Windows/Temp/teachers.json");
            t = reader.ReadToEnd();
            reader.Close();

            // Convert the student JSON string to a list of Student objects
            students = JsonConvert.DeserializeObject<List<Student>>(s);

            // Convert the teacher JSON string to a list of Teacher objects
            teachers = JsonConvert.DeserializeObject<List<Teacher>>(t);

            // Create a new empty sorted list of people
            SortedList<string, Person> people = new SortedList<string, Person>();

            // Loop through all students and add them to the sorted list
            foreach (Student student in students)
            {
                people[student.email] = student;
            }
            
            // Loop through all teachers and add them to the sorted list
            foreach (Teacher teacher in teachers)
            {
                people[teacher.email] = teacher;
            }

            // URL of a web service that returns JSON data
            string url = "http://people.rit.edu/dxsigm/json.php";

            // Create a request object for the web service
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            // Get the response from the web service
            HttpWebResponse response = (HttpWebResponse)request.GetResponse(); 

            // Read the JSON data from the response
            reader = new StreamReader(response.GetResponseStream());
            t = reader.ReadToEnd();
            reader.Close(); response.Close();

            // Convert the JSON string to a list of Teacher objects
            teachers = JsonConvert.DeserializeObject<List<Teacher>>(t);
        }
    }
}
