namespace Saber.TestTask
{
    public interface IFileManager
    {
        void Write(byte[] bytes);
        byte[] Read();
    }
}