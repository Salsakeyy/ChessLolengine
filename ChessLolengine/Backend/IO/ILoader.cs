using Backend.Model;

namespace Backend.IO
{
    public interface ILoader
    {
        Container Load(string path);

        string Filter();
    }
}