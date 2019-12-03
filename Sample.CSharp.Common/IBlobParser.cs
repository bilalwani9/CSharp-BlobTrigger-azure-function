namespace Sample.CSharp.Common
{
    using System.IO;
    using System.Threading.Tasks;

    public interface IBlobParser<out T> 
    {
        T ReadAndParse(Stream stream);

        T Parse(string input);
    }
}
