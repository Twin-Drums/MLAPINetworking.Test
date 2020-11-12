using System;

namespace Twindrums.TheWagaduChronicles.Visibility
{
    public interface IVisibilityGridObject
    {
        VisibilityGrid.Position Position { get; }        
        VisibilityGrid.Cell Cell { get; set; }
    }
}

