using DomainModel;
using Mm.BusinessLayer;
using System;
using System.Collections.Generic;

namespace ConsoleClient
{
    class Program
    {
        private static IBusinessLayer businessLayer = new BuinessLayer();

        static void Main(string[] args)
        {
            run();
        }

        /// <summary>
        /// Display the menu and get user selection until exit.
        /// </summary>
        public static void run()
        {
            
            bool repeat = true;
            int input;

            do
            {
                Menu.displayMenu();
                input = Validator.getMenuInput();

                switch (input)
                {
                    case 0:
                        repeat = false;
                        break;
                    case 1:
                        Menu.clearMenu();
                        addTeacher();
                        break;
                    case 2:
                        Menu.clearMenu();
                        updateTeacher();
                        break;
                    case 3:
                        Menu.clearMenu();
                        removeTeacher();
                        break;
                    case 4:
                        Menu.clearMenu();
                        listTeachers();
                        break;
                    case 5:
                        Menu.clearMenu();
                        listTeacherCourses();
                        break;
                    case 6:
                        Menu.clearMenu();
                        addCourse();
                        break;
                    case 7:
                        Menu.clearMenu();
                        updateCourse();
                        break;
                    case 8:
                        Menu.clearMenu();
                        removeCourse();
                        break;
                    case 9:
                        Menu.clearMenu();
                        listCourses();
                        break;
                }
            } while (repeat);
        }

        //CRUD for teachers

        /// <summary>
        /// Add a teacher to the database.
        /// </summary>
        public static void addTeacher()
        {   //YOUR CODE TO ADD A TEACHER THE DATABASE
            //Create a teacher object, set EntityState to Added, add the teacher 
            //object to the database using the businessLayer object, and display
            //a message to the console window that the teacher has been added
            //to the database.
            Mm.BusinessLayer.BuinessLayer bl = new Mm.BusinessLayer.BuinessLayer();
            Console.WriteLine("Teacher Name ?");
            string name = Console.ReadLine();

            Teacher temp = new Teacher
            {
                TeacherName = name
            };

            bl.AddTeacher(temp);


        }

        /// <summary>
        /// Update the name of a teacher.
        /// </summary>
        public static void updateTeacher()
        {
            Mm.BusinessLayer.BuinessLayer bl = new Mm.BusinessLayer.BuinessLayer();
            Menu.displaySearchOptions();
            int input = Validator.getOptionInput();
            listTeachers();

            //Find by a teacher's name
            if (input == 1)
            {   //YOUR CODE TO UPDATE A TEACHER THE DATABASE
                //Create a teacher object, input the name of the teacher,
                //and get the teacher by name using a method in the class BusinessLayer.
                //If the teacher object is not null, change the teacher's based on the input user
                //enters, set EntityState to Modified, update the teacher 
                //object to the database using the businessLayer object.
                //If teacher is null, display a message "Teacher does not exist"
                //to the database.

                Console.WriteLine("Teacher Name ?");
                string name = Console.ReadLine();
                Teacher temp = bl.GetTeacherByName(name);
                if (temp == null)
                {
                    Console.WriteLine("Teacher not found!");
                }
                else
                {
                    int id = temp.TeacherId;
                    Teacher teacher = bl.GetTeacherById(id);
                    if (teacher == null)
                    {
                        Console.WriteLine("Teacher not found!");
                    }
                    else
                    {
                        Console.WriteLine("Enter new teacher name: ");
                        teacher.TeacherName = Console.ReadLine();
                        bl.UpdateTeacher(teacher);
                    }
                }

            }
            //find by a teacher's id
            else if (input == 2)
            {
                Console.WriteLine("Teacher ID ?");
                int id = Convert.ToInt32(Console.ReadLine());
                Teacher teacher = bl.GetTeacherById(id);
                if (teacher == null)
                {
                    Console.WriteLine("Teacher not found!");
                }
                else
                {
                    Console.WriteLine("Enter new teacher name: ");
                    teacher.TeacherName = Console.ReadLine();
                    bl.UpdateTeacher(teacher);
                }
            }
        }

        /// <summary>
        /// Remove a teacher from the database.
        /// </summary>
        public static void removeTeacher()
        {
            Mm.BusinessLayer.BuinessLayer bl = new Mm.BusinessLayer.BuinessLayer();
            listTeachers();
            int id = Validator.getId();
            //YOUR CODE TO REMOVE A TEACHER THE DATABASE
            //Get the teacher. If the teacher object is not null, display the message that
            //the teacher has been removed. Remove the teacher from the database.
            Teacher teacher = bl.GetTeacherById(id);
            if (teacher == null)
            {
                Console.WriteLine("Teacher not found !");
            }
            else
            {
                Teacher temp = (Teacher)bl.GetCoursesByTeacherId(id);
                foreach (var i in temp.Courses)
                {
                    Course c = bl.GetCourseById(i.CourseId);
                    c.TeacherId = null;
                    bl.UpdateCourse(c);
                }

                bl.RemoveTeacher(teacher);
            }

        }

        /// <summary>
        /// List all teachers in the database.
        /// </summary>
        public static void listTeachers()
        {   //Call a method from the class BusinessLayer to get all the teacher and assign
            //the return to an object of type IList<Teacher>
            //Display the all the teacher id and name.
            //Your code
            Mm.BusinessLayer.BuinessLayer bl = new Mm.BusinessLayer.BuinessLayer();
            IList<Teacher> allTeachers = bl.GetAllTeachers();
            Console.WriteLine();
            Console.WriteLine("ID Teacher Name");
            foreach (Teacher temp in allTeachers)
            {
                Console.WriteLine(temp.TeacherId + "  " + temp.TeacherName);
            }
            Console.WriteLine();


        }

        /// <summary>
        /// List the courses of a specified teacher.
        /// </summary>
        public static void listTeacherCourses()
        {
            Mm.BusinessLayer.BuinessLayer bl = new Mm.BusinessLayer.BuinessLayer();
            listTeachers();
            int id = Validator.getId();
            //Get a Teacher object by on the teacher id input
            //If the teacher object is not null
            //   Display teacher id and teacher name
            //   List all the course the teacher teaches
            //Else
            //Display a message " No course for the teacher id and name". Display
            //the teacher's id and name
            Teacher teacher = (Teacher)bl.GetCoursesByTeacherId(id);
            if (teacher == null || teacher.Courses.Count == 0)
            {
                Console.WriteLine("No courses associated with this Teacher ID");
            }
            else
            {
                foreach (Course c in teacher.Courses)
                    Console.WriteLine("- " + c.CourseName);
            }

        }

        

        //CRUD for courses

        /// <summary>
        /// Add a course to a teacher.
        /// </summary>
        public static void addCourse()
        {
            Mm.BusinessLayer.BuinessLayer bl = new Mm.BusinessLayer.BuinessLayer();
            Console.WriteLine("Enter a course name: ");
            string courseName = Console.ReadLine();

            listTeachers();
            Console.WriteLine("Select a teacher for this course. ");
            int id = Validator.getId();
            //Get the teacher object using the id
            //your code

            Teacher teacher = bl.GetTeacherById(id);
            if (teacher == null)
            {
                Console.WriteLine("Teacher not found!");
            }
            else
            {
                Console.WriteLine("Course Name? ");
                string cName = Console.ReadLine();
                //Console.WriteLine("Course Location? ");
                //string cLocation = Console.ReadLine();
                Course temp = new Course()
                {
                    CourseName = cName,
                    TeacherId = id
                };
                bl.AddCourse(temp);
            }
        }

        /// <summary>
        /// Update the name of a course.
        /// </summary>
        public static void updateCourse()
        {
            Mm.BusinessLayer.BuinessLayer bl = new Mm.BusinessLayer.BuinessLayer();
            Menu.displaySearchOptions();
            int input = Validator.getOptionInput();
            listCourses();

            //find course by name
            if (input == 1)
            {
                Console.WriteLine("Enter a course's name: ");
                string name = Console.ReadLine();
                Course tempCourse = bl.GetCourseByName(name);
                if (tempCourse == null)
                {
                    Console.WriteLine("Course not found!");
                }
                else
                {
                    int id = tempCourse.CourseId;
                    Course course = bl.GetCourseById(id);
                    if (course == null)
                    {
                        Console.WriteLine("Course not found!");
                    }
                    else
                    {
                        Console.WriteLine("Enter new course name: ");
                        course.CourseName = Console.ReadLine();
                        IEnumerable<Teacher> allTeachers = bl.GetAllTeachers();
                        Console.WriteLine("");
                        Console.WriteLine("Current available Teachers");
                        foreach (Teacher temp in allTeachers)
                        {
                            Console.WriteLine(temp.TeacherId + "  " + temp.TeacherName);
                        }

                        Console.WriteLine("Current teacher id is " + course.TeacherId + " . Please enter new teacher id: ");
                        int idTemp = Convert.ToInt32(Console.ReadLine());
                        bool flag = false;
                        foreach (Teacher temp in allTeachers)
                        {
                            if (idTemp == temp.TeacherId)
                            {
                                flag = true;
                            }
                        }
                        if (flag == true)
                        {
                            course.TeacherId = idTemp;
                            bl.UpdateCourse(course);
                        }
                        else
                        {
                            Console.WriteLine("Teacher ID is invalid");
                        }
                    }
                }
            }
            //find course by id
            else if (input == 2)
            {
                Console.WriteLine("Course ID ?");
                int id = Convert.ToInt32(Console.ReadLine());
                Course course = bl.GetCourseById(id);
                if (course == null)
                {
                    Console.WriteLine("Course not found");
                }
                else
                {
                    Console.WriteLine("Enter new course name: ");
                    course.CourseName = Console.ReadLine();

                    IEnumerable<Teacher> allTeachers = bl.GetAllTeachers();
                    Console.WriteLine("");
                    Console.WriteLine("Current available Teachers");
                    foreach (Teacher temp in allTeachers)
                    {
                        Console.WriteLine(temp.TeacherId + "  " + temp.TeacherName);
                    }

                    Console.WriteLine("Current teacher id is " + course.TeacherId + " . Please enter new teacher id: ");
                    int idTemp = Convert.ToInt32(Console.ReadLine());
                    bool flag = false;
                    foreach (Teacher temp in allTeachers)
                    {
                        if (idTemp == temp.TeacherId)
                        {
                            flag = true;
                        }
                    }
                    if (flag == true)
                    {
                        course.TeacherId = idTemp;
                        bl.UpdateCourse(course);
                    }
                    else
                    {
                        Console.WriteLine("Teacher ID is invalid");
                    }

                }
            }
        }

        /// <summary>
        /// Remove a course in the database.
        /// </summary>
        public static void removeCourse()
        {
            Mm.BusinessLayer.BuinessLayer bl = new Mm.BusinessLayer.BuinessLayer();
            listCourses();
            Console.WriteLine("Course ID to be deleted?");
            int id = Convert.ToInt32(Console.ReadLine());

            Course course = bl.GetCourseById(id);
            if (course == null)
            {
                Console.WriteLine("Course not found!");
            }
            else
            {
                bl.RemoveCourse(course);
            }
        }


        /// <summary>
        /// List all courses in the database.
        /// </summary>
        public static void listCourses()
        {   //List all the courses by id and name
            //Display course id and course name
            //Your code
            Mm.BusinessLayer.BuinessLayer bl = new Mm.BusinessLayer.BuinessLayer();
            IList<Course> allCourses = bl.GetAllCourses();
            Console.WriteLine();
            Console.WriteLine($"\t  {"ID",-5}" + $"{"Course name",-15}" + $"{"TeacherID",-15}");
            //Console.WriteLine("ID Course Name  TeacherID");
            foreach (Course temp in allCourses)
            {
                Console.WriteLine($"\t  {temp.CourseId,-5}" + $"{temp.CourseName,-15}" + $"{temp.TeacherId,-15}");
                //Console.WriteLine(temp.CourseId + "  " + temp.CourseName+ "       " + temp.TeacherId);
            }
            Console.WriteLine();

        }
    }
}