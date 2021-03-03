using OurPresence.Modeller.Interfaces;

namespace OurPresence.Modeller.Tests.TestFiles
{
    internal class TestFileWriter : IFileWriter
    {
        public string Output { get; private set; }

        void IFileWriter.Write(IFile file)
        {
            Output = file.Content;
        }
    }
}
