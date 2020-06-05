using SmartStore.Core.Domain.Directory;
using System.Collections.Generic;

namespace SmartStore.Services.Directory
{
    /// <summary>
    /// Measure dimension service interface
    /// </summary>
    public partial interface IMeasureService
    {
        #region Public Methods

        /// <summary>
        /// Converts dimension
        /// </summary>
        /// <param name="quantity">Quantity</param>
        /// <param name="sourceMeasureDimension">Source dimension</param>
        /// <param name="targetMeasureDimension">Target dimension</param>
        /// <param name="round">A value indicating whether a result should be rounded</param>
        /// <returns>Converted value</returns>
        decimal ConvertDimension(decimal quantity,
            MeasureDimension sourceMeasureDimension, MeasureDimension targetMeasureDimension, bool round = true);

        /// <summary>
        /// Converts from primary dimension
        /// </summary>
        /// <param name="quantity">Quantity</param>
        /// <param name="targetMeasureDimension">Target dimension</param>
        /// <returns>Converted value</returns>
        decimal ConvertFromPrimaryMeasureDimension(decimal quantity,
            MeasureDimension targetMeasureDimension);

        /// <summary>
        /// Converts from primary weight
        /// </summary>
        /// <param name="quantity">Quantity</param>
        /// <param name="targetMeasureWeight">Target weight</param>
        /// <returns>Converted value</returns>
        decimal ConvertFromPrimaryMeasureWeight(decimal quantity,
            MeasureWeight targetMeasureWeight);

        /// <summary>
        /// Converts to primary measure dimension
        /// </summary>
        /// <param name="quantity">Quantity</param>
        /// <param name="sourceMeasureDimension">Source dimension</param>
        /// <returns>Converted value</returns>
        decimal ConvertToPrimaryMeasureDimension(decimal quantity,
            MeasureDimension sourceMeasureDimension);

        /// <summary>
        /// Converts to primary measure weight
        /// </summary>
        /// <param name="quantity">Quantity</param>
        /// <param name="sourceMeasureWeight">Source weight</param>
        /// <returns>Converted value</returns>
        decimal ConvertToPrimaryMeasureWeight(decimal quantity, MeasureWeight sourceMeasureWeight);

        /// <summary>
        /// Converts weight
        /// </summary>
        /// <param name="quantity">Quantity</param>
        /// <param name="sourceMeasureWeight">Source weight</param>
        /// <param name="targetMeasureWeight">Target weight</param>
        /// <param name="round">A value indicating whether a result should be rounded</param>
        /// <returns>Converted value</returns>
        decimal ConvertWeight(decimal quantity,
            MeasureWeight sourceMeasureWeight, MeasureWeight targetMeasureWeight, bool round = true);

        /// <summary>
        /// Deletes measure dimension
        /// </summary>
        /// <param name="measureDimension">Measure dimension</param>
        void DeleteMeasureDimension(MeasureDimension measureDimension);

        /// <summary>
        /// Deletes measure weight
        /// </summary>
        /// <param name="measureWeight">Measure weight</param>
        void DeleteMeasureWeight(MeasureWeight measureWeight);

        /// <summary>
        /// Gets all measure dimensions
        /// </summary>
        /// <returns>Measure dimension collection</returns>
        IList<MeasureDimension> GetAllMeasureDimensions();

        /// <summary>
        /// Gets all measure weights
        /// </summary>
        /// <returns>Measure weight collection</returns>
        IList<MeasureWeight> GetAllMeasureWeights();

        /// <summary>
        /// Gets a measure dimension by identifier
        /// </summary>
        /// <param name="measureDimensionId">Measure dimension identifier</param>
        /// <returns>Measure dimension</returns>
        MeasureDimension GetMeasureDimensionById(int measureDimensionId);

        /// <summary>
        /// Gets a measure dimension by system keyword
        /// </summary>
        /// <param name="systemKeyword">The system keyword</param>
        /// <returns>Measure dimension</returns>
        MeasureDimension GetMeasureDimensionBySystemKeyword(string systemKeyword);

        /// <summary>
        /// Gets a measure weight by identifier
        /// </summary>
        /// <param name="measureWeightId">Measure weight identifier</param>
        /// <returns>Measure weight</returns>
        MeasureWeight GetMeasureWeightById(int measureWeightId);

        /// <summary>
        /// Gets a measure weight by system keyword
        /// </summary>
        /// <param name="systemKeyword">The system keyword</param>
        /// <returns>Measure weight</returns>
        MeasureWeight GetMeasureWeightBySystemKeyword(string systemKeyword);

        /// <summary>
        /// Inserts a measure dimension
        /// </summary>
        /// <param name="measure">Measure dimension</param>
        void InsertMeasureDimension(MeasureDimension measure);

        /// <summary>
        /// Inserts a measure weight
        /// </summary>
        /// <param name="measure">Measure weight</param>
        void InsertMeasureWeight(MeasureWeight measure);

        /// <summary>
        /// Updates the measure dimension
        /// </summary>
        /// <param name="measure">Measure dimension</param>
        void UpdateMeasureDimension(MeasureDimension measure);

        /// <summary>
        /// Updates the measure weight
        /// </summary>
        /// <param name="measure">Measure weight</param>
        void UpdateMeasureWeight(MeasureWeight measure);

        #endregion Public Methods
    }
}