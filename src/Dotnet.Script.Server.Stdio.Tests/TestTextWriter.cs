using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Dotnet.Script.Server.Stdio.Tests
{
    public class TestTextWriter : TextWriter
    {
        private readonly IEnumerator<Action<string>> _enumerator;

        public TestTextWriter(params Action<string>[] callbacks)
        {
            _enumerator = ((IEnumerable<Action<string>>) callbacks).GetEnumerator();
            _enumerator.MoveNext();
        }

        public override Encoding Encoding { get; }

        public override void WriteLine(string value)
        {
            _enumerator.Current(value);
            _enumerator.MoveNext();
        }
    }
}