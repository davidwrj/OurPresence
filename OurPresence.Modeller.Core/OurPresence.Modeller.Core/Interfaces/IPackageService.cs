using System.Collections.Generic;

namespace OurPresence.Modeller.Interfaces
{
    public interface IPackageService
    {
        IEnumerable<IPackage> Items { get; }

        void Refresh(string targetFile);
    }
}
