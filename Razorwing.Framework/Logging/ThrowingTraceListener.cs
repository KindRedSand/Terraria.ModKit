using System.Diagnostics;

namespace Razorwing.Framework.Logging
{
    /// <summary>
    /// A <see cref="TraceListener"/> that throws exceptions when a trace is hit.
    /// This allows consistent behaviour across runtimes (ie. under Mono where no winforms dialog is displayed on encountering an exception).
    /// </summary>
    public class ThrowingTraceListener : TraceListener
    {
        public override void Write(string message)
        {
        }

        public override void WriteLine(string message)
        {
        }

        public override void Fail(string message) => throw new System.Exception(message);

        public override void Fail(string message1, string message2) => throw new System.Exception($"{message1}: {message2}");
    }
}
