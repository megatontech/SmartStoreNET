// Customized version of CsvHelper from Josh Close:
// ------------------------------------------------
// Copyright 2009-2015 Josh Close and Contributors
// This file is a part of CsvHelper and is dual licensed under MS-PL and Apache 2.0.
// See LICENSE.txt for details or visit http://www.opensource.org/licenses/ms-pl.html for MS-PL and http://opensource.org/licenses/Apache-2.0 for Apache 2.0.
// http://csvhelper.com

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SmartStore.Services.DataExchange.Csv
{
    /// <summary>
    /// SImple utility class used to write CSV files.
    /// </summary>
    public class CsvWriter : DisposableObject
    {
        #region Private Fields

        private readonly IList<string> _currentRow = new List<string>();

        private int? _fieldCount;

        private TextWriter _writer;

        #endregion Private Fields

        #region Public Constructors

        public CsvWriter(TextWriter writer)
            : this(writer, new CsvConfiguration())
        {
        }

        public CsvWriter(TextWriter writer, CsvConfiguration configuration)
        {
            Guard.NotNull(writer, nameof(writer));
            Guard.NotNull(configuration, nameof(configuration));

            _writer = writer;
            this.Configuration = configuration;
        }

        #endregion Public Constructors



        #region Public Properties

        public CsvConfiguration Configuration
        {
            get;
            private set;
        }

        #endregion Public Properties



        #region Public Methods

        public string CurrentRawValue()
        {
            var row = string.Join(Configuration.Delimiter.ToString(), _currentRow);
            return row;
        }

        /// <summary>
        /// Ends writing of the current row
        /// and starts a new row.
        /// </summary>
        public virtual void NextRow()
        {
            WriteRow(_currentRow.ToArray());
            _currentRow.Clear();
        }

        /// <summary>
        /// Writes the field to the CSV file. The field
        /// may get quotes added to it.
        /// When all fields are written for a row,
        /// <see cref="CsvWriter.NextRow" /> must be called
        /// to complete writing of the current row.
        /// </summary>
        /// <param name="field">The field to write.</param>
        public virtual void WriteField(string field)
        {
            var shouldQuote = Configuration.QuoteAllFields;

            if (!string.IsNullOrEmpty(field))
            {
                if (Configuration.TrimValues)
                {
                    field = field.Trim();
                }

                if (!Configuration.SupportsMultiline)
                {
                    field = field.Replace('\r', ' ').Replace('\n', ' ');
                }

                if (shouldQuote
                    || field[0] == ' '
                    || field[field.Length - 1] == ' '
                    || field.IndexOfAny(Configuration.QuotableChars) > -1)
                {
                    shouldQuote = true;
                }
            }

            WriteField(field, shouldQuote);
        }

        /// <summary>
        /// Writes the field to the CSV file. This will
        /// ignore any need to quote and ignore the
        /// <see cref="CsvConfiguration.QuoteAllFields"/>
        /// and just quote based on the shouldQuote
        /// parameter.
        /// When all fields are written for a row,
        /// <see cref="CsvWriter.NextRow" /> must be called
        /// to complete writing of the current row.
        /// </summary>
        /// <param name="field">The field to write.</param>
        /// <param name="shouldQuote">True to quote the field, otherwise false.</param>
        public virtual void WriteField(string field, bool shouldQuote)
        {
            // All quotes must be escaped.
            if (shouldQuote && !string.IsNullOrEmpty(field))
            {
                field = field.Replace(Configuration.Quote.ToString(), Configuration.QuoteString);
            }

            if (shouldQuote)
            {
                field = Configuration.Quote + (field ?? string.Empty) + Configuration.Quote;
            }

            _currentRow.Add(field ?? string.Empty);
        }

        /// <summary>
        /// Writes a sequence of fields to the CSV file. The fields
        /// may get quotes added to it.
        /// When all fields are written for a row,
        /// <see cref="CsvWriter.NextRow" /> must be called
        /// to complete writing of the current row.
        /// </summary>
        /// <param name="fields">The fields to write.</param>
        public virtual void WriteFields(IEnumerable<string> fields)
        {
            Guard.NotNull(fields, nameof(fields));
            fields.Each(x => WriteField(x));
        }

        /// <summary>
        /// Writes a sequence of fields to the CSV file. This will
        /// ignore any need to quote and ignore the
        /// <see cref="CsvConfiguration.QuoteAllFields"/>
        /// and just quote based on the shouldQuote
        /// parameter.
        /// When all fields are written for a row,
        /// <see cref="CsvWriter.NextRow" /> must be called
        /// to complete writing of the current row.
        /// </summary>
        /// <param name="fields">The fields to write.</param>
        /// <param name="shouldQuote">True to quote the fields, otherwise false.</param>
        public virtual void WriteFields(IEnumerable<string> fields, bool shouldQuote)
        {
            Guard.NotNull(fields, nameof(fields));
            fields.Each(x => WriteField(x, shouldQuote));
        }

        #endregion Public Methods



        #region Protected Methods

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                if (_writer != null)
                {
                    _writer.Dispose();
                    _writer = null;
                }
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void WriteRow(string[] fields)
        {
            CheckDisposed();

            if (fields.Length == 0)
            {
                throw new SmartException("Cannot write an empty row to the CSV file.");
            }

            if (!_fieldCount.HasValue)
            {
                _fieldCount = fields.Length;
            }

            if (_fieldCount.Value != fields.Length)
            {
                throw new SmartException("The field count of the current row does not match the previous row's field count.");
            }

            var row = string.Join(Configuration.Delimiter.ToString(), fields);
            _writer.WriteLine(row);
        }

        #endregion Private Methods
    }
}