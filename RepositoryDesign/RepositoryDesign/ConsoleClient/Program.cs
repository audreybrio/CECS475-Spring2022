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
           
            Console.WriteLine("Enter a teacher name:");
            string name = Console.ReadLine();

            Teacher temp = new Teacher
            {
                TeacherName = name
            };
            temp.EntityState = EntityState.Added;
            businessLayer.AddTeacher(temp);


        }

        /// <summary>
        /// Update the name of a teacher.
        /// </summary>
        public static void updateTeacher()
        {
            
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

                Console.WriteLine("Enter Teacher Name: ");
                string name = Console.ReadLine();
                Teacher temp = businessLayer.GetTeacherByName(name);
                if (temp == null)
                {
                    Console.WriteLine("Teacher does not exist");
                }
                else
                {
                    Console.WriteLine("Change this teacher's name to: ");
                    temp.TeacherName = Console.ReadLine();
                    temp.EntityState = EntityState.Modified;
                    businessLayer.UpdateTeacher(temp);
                }

            }
            //find by a teacher's id
            else if (input == 2)
            {
                int id = Validator.getId();
                Teacher temp = businessLayer.GetTeacherById(id);
                if (temp != null)
                {
                    Console.WriteLine("Change this teacher's name to: ");
                    temp.TeacherName = Console.ReadLine();
                    temp.EntityState = EntityState.Modified;
                    businessLayer.UpdateTeacher(temp);
                }
                else
                {
                    Console.WriteLine("Teacher does not exist.");
                };
            }
        }

        /// <summary>
        /// Remove a teacher from the database.
        /// </summary>
        public static void removeTeacher()
        {
           
            listTeachers();
            int id = Validator.getId();
            //YOUR CODE TO REMOVE A TEACHER THE DATABASE
            //Get the teacher. If the teacher object is not null, display the message that
            //the teacher has been removed. Remove the teacher from the database.
            Teacher teacher = businessLayer.GetTeacherById(id);
            if (teacher == null)
            {
                Console.WriteLine("Teacher does not exist");
            }
            else
            {
                Console.WriteLine("{0} has been removed.", teacher.TeacherName);
                teacher.EntityState = EntityState.Deleted;
                businessLayer.RemoveTeacher(teacher);
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

            IList<Teacher> teachers = businessLayer.GetAllTeachers();
            foreach (Teacher teacher in teachers)
                Console.WriteLine("Teacher ID: {0}, Name: {1}", teacher.TeacherId, teacher.TeacherName);


        }

        /// <summary>
        /// List the courses of a specified teacher.
        /// </summary>
        public static void listTeacherCourses()
        {
          
            listTeachers();
            int id = Validator.getId();

            Teacher teacher = businessLayer.GetTeacherById(id);
            if (teacher != null)
            {
                Console.WriteLine("Listing courses for [ID: {0}, Name: {1}]:", teacher.TeacherId, teacher.TeacherName);
                if (teacher.Courses.Count > 0)
                {
                    foreach (Course course in teacher.Courses)
                        Console.WriteLine("Course ID: {0}, Name: {1}", course.CourseId, course.CourseName);
                }
                else
                {
                    Console.WriteLine("No courses for [ID: {0}, Name: {1}]:", teacher.TeacherId, teacher.TeacherName);
                };

            }
            else
            {
                Console.WriteLine("Teacher does not exist.");
            };

        }

        

        //CRUD for courses

        /// <summary>
        /// Add a course to a teacher.
        /// </summary>
        public static void addCourse()
        {
           
            Console.WriteLine("Enter a course name: ");
            string courseName = Console.ReadLine();

            listTeachers();
            Console.WriteLine("Select a teacher for this course. ");
            int id = Validator.getId();
            //Get the teacher object using the id
            //your code

            Teacher teacher = businessLayer.GetTeacherById(id);
            if (teacher == null)
            {
                Console.WriteLine("Teacher not found!");
            }
            else
            {
                Course course = new Course()
                {
                    CourseName = courseName,
                    TeacherId = teacher.TeacherId,
                    EntityState = EntityState.Added
                };

                //add course to teacher
                teacher.EntityState = EntityState.Modified;
                foreach (Course c in teacher.Courses)
                    c.EntityState = EntityState.Unchanged;
                teacher.Courses.Add(course);
                businessLayer.UpdateTeacher(teacher);
                Console.WriteLine("{0} has been added to the database.", courseName);
            }
        }

        /// <summary>
        /// Update the name of a course.
        /// </summary>
        public static void updateCourse()
        {
           
            Menu.displaySearchOptions2();
           
            int input = Validator.getOptionInput();
            listCourses();

            //find course by name
            if (input == 1)
            {
                Console.WriteLine("Enter a course's name: ");
                Course course = businessLayer.GetCourseByName(Console.ReadLine());
                if (course != null)
                {
                    Console.WriteLine("Change this course's name to: ");
                    course.CourseName = Console.ReadLine();
                    course.EntityState = EntityState.Modified;
                    businessLayer.UpdateCourse(course);
                    


                }
                else
                {
                    Console.WriteLine("Course does not exist.");
                };
            }
            //find course by id
            else if (input == 2)
            {
                int id = Validator.getId();
                Course course = businessLayer.GetCourseById(id);
                if (course != null)
                {
                    Console.WriteLine("Change this course's name to: ");
                    course.CourseName = Console.ReadLine();
                    course.EntityState = EntityState.Modified;
                    businessLayer.UpdateCourse(course);
                }
                else
                {
                    Console.WriteLine("Course does not exist.");
                };
            }

            else if (input == 3)
            {
                int id = Validator.getId();
                Course course = businessLayer.GetCourseById(id);
                if (course != null)
                {
                    reassignCourse(course);
                }
                else
                {
                    Console.WriteLine("Course does not exist.");
                };

            }

        }
            
        

        /// <summary>
        /// Remove a course in the database.
        /// </summary>
        public static void removeCourse()
        {

            listCourses();
            int id = Validator.getId();
            Course course = businessLayer.GetCourseById(id);
            if (course != null)
            {
                Console.WriteLine("{0} has been removed.", course.CourseName);
                course.EntityState = EntityState.Deleted;
                businessLayer.RemoveCourse(course);
            }
            else
            {
                Console.WriteLine("Course does not exist.");
            };
        }


        /// <summary>
        /// List all courses in the database.
        /// </summary>
        public static void listCourses()
        {   //List all the courses by id and name
            //Display course id and course name
            //Your code
            IList<Course> courses = businessLayer.GetAllCourses();
            foreach (Course course in courses)
                Console.WriteLine("Course ID: {0}, Name: {1}", course.CourseId, course.CourseName);

        }


        public static void reassignCourse(Course course)
        {;
            
            string courseName = course.CourseName;

            listTeachers();
            Console.WriteLine("Select a teacher for this course. ");
            int t_id = Validator.getId();

            Teacher teacher = businessLayer.GetTeacherById(t_id);

            if (teacher == null)
            {
                Console.WriteLine("Teacher not found!");
            }
            else
            {

                course.EntityState = EntityState.Deleted;
                businessLayer.RemoveCourse(course);

                Course temp = new Course()
                {
                    CourseName = courseName,
                    TeacherId = teacher.TeacherId,
                    EntityState = EntityState.Added
                };

                //add course to teacher
                teacher.EntityState = EntityState.Modified;
                foreach (Course c in teacher.Courses)
                    c.EntityState = EntityState.Unchanged;
                teacher.Courses.Add(temp);
                businessLayer.UpdateTeacher(teacher);
            }
        }
    }
}