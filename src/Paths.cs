using static System.Environment;
using static System.Environment.SpecialFolder;
using static System.IO.Path;

namespace SPV3
{
  public class Paths
  {
    public static readonly string Directory     = Combine(GetFolderPath(ApplicationData), "SPV3");
    public static readonly string Exception     = Combine(Directory,                      "exception.log");
    public static readonly string Configuration = Combine(Directory,                      "loader.bin");
  }
}