using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace SenlaTest
{
    public sealed class Program
    {
        static void Main(string[] args)
        {
            var act = new List<Action<Stopwatch>>();
            act.AddRange(new Action<Stopwatch>[] { task1, task2, task3, task4, task5, task6 });
            Stopwatch timer = Stopwatch.StartNew();
            foreach (var item in act)
            {
                item.Invoke(timer);
                Console.WriteLine(timer.ElapsedMilliseconds.ToString() + "msec. has gone from the start");
            }
        }
        static void task1(Stopwatch timer) // simple numbers
        {
            Console.WriteLine("// Simple numbers");
            Console.WriteLine("Please enter upper limit");
            timer.Stop();
            int n = Convert.ToInt32(Console.ReadLine());
            timer.Start();
            var li = new List<int>();
            for (int i = 1; i < n; i++)
            {
                if (IsSimple(i)) li.Add(i);
            }
            if (li.Count > 0)
            {
                foreach (var item in li)
                {
                    Console.WriteLine("{0} is a simple number", item.ToString());
                }
            }
            else Console.WriteLine("You've entered a wrong number! It must be greater than 1.");
        }
        static void task2(Stopwatch timer) // fibonacci numbers
        {
            Console.WriteLine("// Fibonacci numbers");
            Console.WriteLine("Please enter upper limit");
            timer.Stop();
            int n = Convert.ToInt32(Console.ReadLine());
            timer.Start();
            if (n < 1) throw new ArgumentOutOfRangeException("Upper limit must be strictly positive");
            var li = new List<int>();
            li.Add(1); li.Add(1); // getting 2 first elem. by definition
            Console.WriteLine("There are {0} fibonacci numbers on the (1, {1}) They are:", getFiboMaxNum(n, 1, 1, ref li), n);
            foreach (var item in li)
            {
                Console.WriteLine(item.ToString());
            }

        }
        static void task3(Stopwatch timer) // segments intersection: 2D case
        {
            Console.WriteLine("// Segments intersecton");
            Console.WriteLine("Define 2 segments in 2 strings. Please use brackets for each e.g. A(2 3), B(4, 5)");
            timer.Stop();
            Segment s1 = new Segment(Console.ReadLine()), s2 = new Segment(Console.ReadLine());
            timer.Start();
            if (s1.Intersect(s2))
            {
                Console.WriteLine("Segments are intersecting at " + Segment.Intersect(s1, s2));
            }
            else Console.WriteLine("Segments are{0} intersecting {1}", (s1.Intersect(s2))? "" : " not", 
                (s1.Intersect(s2)) ? $"at {Segment.Intersect(s1, s2).ToString()}" : "");
            Console.WriteLine(s1.ToString()); Console.WriteLine(s2);
        }
        static void task4(Stopwatch timer) // Greatest common divisor and least common multiple
        {
            Console.WriteLine("// Greatest common divisor and least common multiple");
            Console.WriteLine("Enter 2 integer numbers.");
            timer.Stop();
            string s = Console.ReadLine();
            timer.Start();
            int x = 0, y;
            string temp = null;
            for (int i = 0; i < s.Length; i++) // divide a string into 2 numbers
            {
                if (s[i] != ' ') temp += s[i];
                else
                {
                    x = Convert.ToInt32(temp);
                    temp = null;
                }
            }
            y = Convert.ToInt32(temp);
            if ((x < 1) || (y < 1)) throw new FormatException("You have entered wrong numbers. They must be positive. e.g. 5 15");
            Console.WriteLine($"Their greatest common divisor is {GCD(x, y)} and the least common multiple is {LCM(x, y)}");
        }
        static void task5(Stopwatch timer) // string palindrome (not space-sensetive and not case-sensetive)
        {
            Console.WriteLine("// String palindrome");
            Console.WriteLine("Enter a string to check");
            timer.Stop();
            string inp = Console.ReadLine();
            timer.Start();
            string s = null;
            foreach (var item in inp)
            {
                if (item != ' ') s += char.ToLower(item);
            }
            string rev = null;
            for (int i = s.Length-1; i >= 0; i--)
            {
                rev += s[i];
            }
            var b = string.Equals(s, rev);
            Console.WriteLine("{0} is {1}the same as {2}", inp, (b)? "" : "not ", (b)? inp : rev);
        }
        static void task6(Stopwatch timer) // delete numbers from string
        {
            Console.WriteLine("// Delete numbers from string");
            Console.WriteLine("Enter a sting");
            timer.Stop();
            string s = Console.ReadLine();
            timer.Start();
            var numbers = "0123456789";
            string output = null;
            bool IsNumber(char c)
            {
                foreach (var item in numbers)
                {
                    if (c == item) return true;
                }
                return false;
            }
            foreach (var item in s)
            {
                if (!IsNumber(item))
                {
                    output += item;
                }
            }
            Console.WriteLine(output);
        }
        static int LCM(int a, int b) // Least common multiple
        {
            return (a * b) / GCD(a, b);
        }
        static int GCD(int a, int b) // Greatest common divisor
        {
            if (a < b) (a, b) = (b, a);
            for (int i = a; i > 1; i--)
            {
                if ((a % i == 0) && (b % i == 0)) return i;
            }
            return 1;
        }
        static int getFiboMaxNum(int lim, int arg, int prev, ref List<int> dest)
        {
            int i = 0;
            if ((arg += prev) < lim)
            {
                dest.Add(arg);
                getFiboMaxNum(lim, arg, arg - prev, ref dest);
            }
            return ++i;
        }
        static bool IsSimple(int x)
        {
            for (int i = 2; i < x; i++)
            {
                if ((x % i) == 0) return false;
            }
            return true;
        }
    }
    public class Point
    {
        public Point(double a, double b)
        {
            x = a; y = b;
        }
        public Point(string s)
        {
            string temp = null;
            for (int i = 0; i < s.Length; i++) // divide string into 2 coordinates 
            {
                if (s[i] != ' ') temp += s[i];
                else
                {
                    x = Convert.ToDouble(temp);
                    temp = null;
                }
            }
            y = Convert.ToDouble(temp);
        }
        double x, y;
        public double X { get { return x; } } // incapsulation
        public double Y { get { return y; } } // incapsulation
        public override string ToString()
        {
            return "(" + x.ToString() + ", " + y.ToString() + ")";
        }
    }
    public class Segment
    {
        public Segment(Point a, Point b)
        {
            start = a; end = b;
            k = (start.Y - end.Y)/(start.X - end.X);
            this.b = start.Y - start.X * k;
        }
        public Segment(string s)
        {
            int l1 = s.IndexOf('('), l2 = s.LastIndexOf('(');
            int r1 = s.IndexOf(')'), r2 = s.LastIndexOf(')');
            start = new Point(s.Substring(++l1, r1 - l1));
            end = new Point(s.Substring(++l2, r2 - l2));
            k = Math.Abs(start.Y - end.Y) / Math.Abs(start.X - end.X);
            b = start.Y - start.X * k;
        }
        Point start, end;
        public readonly double k; // from the line equation y = kx + b
        public readonly double b;
        public Point Start { get { return start; } } // incapsulation
        public Point End { get { return end; } } // incapsulation
        public static Point Intersect(Segment A, Segment B)
        {
            if (A.Intersect(B)) return new Point((A.b - B.b) / (B.k - A.k), A.k * (A.b - B.b) / (B.k - A.k) + A.b); else return null;
        }
        public bool Intersect(Segment B)
        {
            if (k == B.k) return false; // for parallel
            var x0 = (b - B.b) / (B.k - k);
            var y0 = k * x0 + b;
            if ((x0 >= Min(start.X, end.X)) && (x0 <= Max(start.X, end.X)) && (y0 >= Min(start.Y, end.Y)) && (y0 <= Max(start.Y, end.Y)))
                return true; else return false;
        }
        static double Min(double a, double b)
        {
            return (a > b) ? b : a;
        }
        static double Max(double a, double b)
        {
            return (a < b) ? b : a;
        }
        public override string ToString()
        {
            return k.ToString() + "x + " + b.ToString();
        }
    }
}
