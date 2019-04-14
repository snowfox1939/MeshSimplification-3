using System;

namespace Polynano 
{
    internal class Program 
    {
        [STAThread]
        public static void Main() 
        {
            // Warning: Do not use using(var tk = ...) { using(var app = ..)}.
            // for some reason this causes a memory leak.
            var tk = OpenTK.Toolkit.Init ();
            var app = new Application ();
            var gui = new Polynano.UI.MainApplication(app);

            tk.Dispose ();
        }
    }
}