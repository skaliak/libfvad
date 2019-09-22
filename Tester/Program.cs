using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libfvadWrapper;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            var dummy_frame = Enumerable.Repeat((Int16)0, 160).ToArray();
            var fvad = new Libfvad();

            var result = fvad.HasActiveVoice(dummy_frame, dummy_frame.Length);

            System.Threading.Thread.Sleep(5000);

            fvad.Dispose();
        }
    }
}
