using System;

namespace Twindrums.TheWagaduChronicles.Visibility
{
    public interface IVisibilityGridObject
    {
        Cell.Position Position { get; }        
        ICell Cell { get; set; }
        bool ShouldUpdate { get; }        
    }
}