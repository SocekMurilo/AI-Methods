using System.Xml.Serialization;

namespace MazeAI;

public class Space
{
    public int    X          { get; init; }
    public int    Y          { get; init; }
    public Space? Top        { get; set; }
    public Space? Left       { get; set; }
    public Space? Right      { get; set; }
    public Space? Bottom     { get; set; }
    public bool   Visited    { get; set; } = false;
    public bool   IsSolution { get; set; } = false;
    public bool   Exit       { get; set; } = false;

    public void Reset()
    {
        IsSolution = false;
        Visited = false;
    }
}