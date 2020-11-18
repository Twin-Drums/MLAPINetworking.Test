using System;

namespace Twindrums.TheWagaduChronicles.Visibility
{
    public interface IVisibilityGridObject
    {
        Cell.Position Position { get; }        
        Cell Cell { get; set; }
        bool ShouldUpdate { get; }        
    }
}