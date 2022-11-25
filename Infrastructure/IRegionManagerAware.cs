// -----------------------------------------------------------------------
// <copyright file="IRegionManagerAware.cs" company="Techint">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Microsoft.Practices.Prism.Regions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IRegionManagerAware
    {
        /// <summary>
        IRegionManager RegionManager { get; set; }
        /// Region manager
        /// </summary>
    }
}
