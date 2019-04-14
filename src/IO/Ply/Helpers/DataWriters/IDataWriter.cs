using System;

namespace Polynano.IO.Ply.Helpers.DataWriters
{
    /// <summary>
    /// Interface for the DataWriters(Content writers)
    /// can be for example binary little endian, ascii or binary big endian
    /// </summary>
    internal interface IDataWriter
    {
        /// <summary>
        /// method to write an array of values
        /// </summary>
        /// <param name="data">array of values to write</param>
        void WriteList<T>(params T[] data) where T : IConvertible;

        /// <summary>
        /// method to write single value.
        /// </summary>
        /// <param name="values">value to write</param>
        void WriteValue<T>(T values) where T : IConvertible;

        /// <summary>
        /// Method to finish the writing
        /// </summary>
        void Close();
    }
}
