// using System;

// namespace Polymorphism
// {
//     class Animal
//     {
//         public virtual void Sound()
//         {
//             Console.WriteLine("Animal makes a Sound");
//         }
//     }

//     class Dog : Animal
//     {
//         public override void Sound()
//         {
//             Console.WriteLine("Dog Barks");
//         }
//     }

//     class Lion : Animal
//     {
//         public override void Sound()
//         {
//             Console.WriteLine("Lion Roars");
//         }
//     }

//     class Cat : Animal
//     {
//         public override void Sound()
//         {
//             Console.WriteLine("Cat Meows");
//         }
//     }

//     class Program
//     {
//         static void Main()
//         {
//             Animal a;

//             a = new Dog();
//             a.Sound();

//             a = new Lion();
//             a.Sound();

//             a = new Cat();
//             a.Sound();
//         }
//     }
// }
