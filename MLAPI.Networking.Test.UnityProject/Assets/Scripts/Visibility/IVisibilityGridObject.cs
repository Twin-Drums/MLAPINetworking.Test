using System;

namespace Twindrums.TheWagaduChronicles.Visibility
{
    public interface IVisibilityGridObject
    {
        VisibilityGrid.Position Position { get; }
        ulong CellId { get; set; }
    }
}

