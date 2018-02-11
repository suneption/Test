namespace Saber.TestTask
{
    public interface IConverter<in T>
    {
        string ToString(T source);
    }
}
