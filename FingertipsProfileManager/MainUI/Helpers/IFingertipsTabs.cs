﻿namespace Fpm.MainUI.Helpers
{
    /// <summary>
    /// The visualisation tabs that are displayed in Fingertips.
    /// </summary>
    public interface IFingertipsTabs
    {
        bool IsMapTab { get; set; }
        bool IsScatterPlotTab { get; set; }
        bool IsEnglandTab { get; set; }
        bool IsPopulationTab { get; set; }
        bool IsReportsTab { get; set; }
        bool IsBoxPlotTab { get; set; }
        bool IsInequalitiesTab { get; set; }
        bool IsCompareAreasTab { get; set; }
    }
}