using WpfApp1.Factories;

namespace WpfApp1
{
    public interface IShapePlugin
    {
        string ShapeName { get; }
        ShapeCreator GetShapeCreator();
        bool SupportsCorners { get; }
    }
}
