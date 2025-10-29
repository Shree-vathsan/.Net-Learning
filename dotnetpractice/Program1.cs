// using System;

// class Program
// {
//     // Using ref: variable must be initialized before passing
//     static void Increment(ref int num)
//     {
//         num = num + 10;
//     }

//     // Using out: variable need not be initialized before passing
//     static void GetValues(out int a, out int b)
//     {
//         a = 100;  // Must assign before using
//         b = 200;
//     }

//     static void Main()
//     {
//         // Example 1: ref
//         int x = 5;
//         Console.WriteLine("Before ref: " + x);
//         Increment(ref x);
//         Console.WriteLine("After ref: " + x);

//         // Example 2: out
//         int p, q; // Not initialized
//         GetValues(out p, out q);
//         Console.WriteLine($"After out: p = {p}, q = {q}");
//     }
// }
