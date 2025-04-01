using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApp1.Models.Shapes.Base
{
    // Абстрактный базовый класс для ломаных линий
    public abstract class PolylineShapeBase : ShapeBase
    {
        public PointCollection Points { get; protected set; }
    }
}
