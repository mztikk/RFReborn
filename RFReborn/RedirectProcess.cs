using System;
using System.Diagnostics;
using System.IO;

namespace RFReborn
{
    /// <summary>
    /// Process that redirect std and err output to a TextWriter and enables writing to its input.
    /// </summary>
    public class RedirectProcess : Process
    {
        private bool _started;
        private readonly TextWriter _writer;

        /// <summary>
        /// Create a <see cref="RedirectProcess"/> with a <see cref="TextWriter"/> which gets the std and err output.
        /// </summary>
        /// <param name="redirectTo"><see cref="TextWriter"/> to which we redirect.</param>
        public RedirectProcess(TextWriter redirectTo)
        {
            _writer = redirectTo;
            SetupProcess();
        }

        /// <summary>
        /// Creates a <see cref="RedirectProcess"/> with the <see cref="Console.Out"/> as its <see cref="TextWriter"/>.
        /// </summary>
        public RedirectProcess() : this(Console.Out) { }

        /// <summary>
        /// Overwrites the <see cref="Process.StartInfo"/> to redirect streams and not create a new cmd window, then starts the process.
        /// Starts the process resource that is specified by the StartInfo property of this Process component and associates it with the component.
        /// Does nothing if it already started.
        /// </summary>
        public new void Start()
        {
            // if its already started return early
            if (_started)
            {
                return;
            }

            // set started to false if the process exits
            Exited += (_, _1) => _started = false;

            OverwriteStartInfo();

            // start the process and begin reading the std and err ouput
            base.Start();
            BeginOutputReadLine();
            BeginErrorReadLine();
            _started = true;
        }

        private void OverwriteStartInfo()
        {
            // overwrite startinfo to redirect streams and start inside our process
            StartInfo.RedirectStandardError = true;
            StartInfo.RedirectStandardOutput = true;
            StartInfo.RedirectStandardInput = true;
            StartInfo.UseShellExecute = false;
            StartInfo.CreateNoWindow = true;
            StartInfo.WorkingDirectory = Environment.CurrentDirectory;
        }

        private void SetupProcess()
        {
            // write the std and err output to our console
            OutputDataReceived += (sender, args) => _writer.WriteLine(args.Data);
            ErrorDataReceived += (sender, args) => _writer.WriteLine(args.Data);
        }

        protected override void Dispose(bool disposing)
        {
            _writer.Dispose();

            base.Dispose(disposing);
        }
    }
}
