// using System;

// namespace AccessModifier
// {
//     class Students
//     {
//         public string Name = "Gokul";
//         private int age = 23;
//         internal int RollNumber = 1;
//         protected string Address = "Chennai";
//         private protected string Email = "gokul123@gmail.com";
//         protected internal string Phonenumber = "9876543210";

//         public void Details()
//         {
//             Console.WriteLine("Show the Students Details... ");
//             Console.WriteLine("Name: " + Name);
//             Console.WriteLine("Age: " + age);
//             Console.WriteLine("RollNumber: " + RollNumber);
//             Console.WriteLine("Address: " + Address);
//             Console.WriteLine("MailId: " + Email);
//             Console.WriteLine("MobileNo: " + Phonenumber);
//         }
//     }

//     class StudentDetails : Students
//     {
//         public void STD()
//         {
//             Console.WriteLine("\nShow the STD Details... ");
//             Console.WriteLine("Name: " + Name);
//             // Console.WriteLine("Age: " + age); // ❌ Private, not accessible
//             Console.WriteLine("RollNumber: " + RollNumber);
//             Console.WriteLine("Address: " + Address);
//             Console.WriteLine("MailId: " + Email);
//             Console.WriteLine("MobileNo: " + Phonenumber);
//         }
//     }

//     class Program
//     {
//         public void College()
//         {
//             Students s = new Students();
//             s.Details();

//             StudentDetails cs = new StudentDetails();
//             cs.STD();

//             Console.WriteLine("\nShow the Details from Program....");
//             Console.WriteLine("Name: " + s.Name);
//             // Console.WriteLine("Age: " + s.age); // ❌ Private
//             Console.WriteLine("RollNumber: " + s.RollNumber);
//             // Console.WriteLine("Address: " + s.Address); // ❌ Protected
//             // Console.WriteLine("MailId: " + s.Email);    // ❌ Private Protected
//             Console.WriteLine("MobileNo: " + s.Phonenumber);

//             Console.WriteLine("\nPress any key to exit...");
//             Console.ReadKey();
//         }

//         public static void Main(string[] args)
//         {
//             Program p = new Program();
//             p.College();
//         }
//     }
// }
