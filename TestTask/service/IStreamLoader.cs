using System.Text;

namespace TestTask.service
{
    internal interface IStreamLoader
    {
        public int Load(Stream stream, Encoding encoding, char lineSplitter, char columnSplitter);
    }
}
