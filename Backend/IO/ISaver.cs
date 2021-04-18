using Backend.Model;

namespace Backend.IO
{
    public interface ISaver
    {
        void Save(Container container, string path);
        string Filter();
    }
}