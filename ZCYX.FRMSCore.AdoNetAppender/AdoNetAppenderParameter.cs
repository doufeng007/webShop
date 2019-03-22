using log4net.Core;
using log4net.Layout;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ZCYX.FRMSCore.Logging
{
    public class AdoNetAppenderParameter
    {
        #region Public Instance Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AdoNetAppenderParameter" /> class.
        /// </summary>
        /// <remarks>
        /// Default constructor for the AdoNetAppenderParameter class.
        /// </remarks>
        public AdoNetAppenderParameter()
        {
            Precision = 0;
            Scale = 0;
            Size = 0;
        }

        #endregion // Public Instance Constructors

        #region Public Instance Properties

        /// <summary>
        /// Gets or sets the name of this parameter.
        /// </summary>
        /// <value>
        /// The name of this parameter.
        /// </value>
        /// <remarks>
        /// <para>
        /// The name of this parameter. The parameter name
        /// must match up to a named parameter to the SQL stored procedure
        /// or prepared statement.
        /// </para>
        /// </remarks>
        public string ParameterName
        {
            get { return m_parameterName; }
            set { m_parameterName = value; }
        }

        /// <summary>
        /// Gets or sets the database type for this parameter.
        /// </summary>
        /// <value>
        /// The database type for this parameter.
        /// </value>
        /// <remarks>
        /// <para>
        /// The database type for this parameter. This property should
        /// be set to the database type from the <see cref="DbType"/>
        /// enumeration. See <see cref="IDataParameter.DbType"/>.
        /// </para>
        /// <para>
        /// This property is optional. If not specified the ADO.NET provider 
        /// will attempt to infer the type from the value.
        /// </para>
        /// </remarks>
        /// <seealso cref="IDataParameter.DbType" />
        public DbType DbType
        {
            get { return m_dbType; }
            set
            {
                m_dbType = value;
                m_inferType = false;
            }
        }

        /// <summary>
        /// Gets or sets the precision for this parameter.
        /// </summary>
        /// <value>
        /// The precision for this parameter.
        /// </value>
        /// <remarks>
        /// <para>
        /// The maximum number of digits used to represent the Value.
        /// </para>
        /// <para>
        /// This property is optional. If not specified the ADO.NET provider 
        /// will attempt to infer the precision from the value.
        /// </para>
        /// </remarks>
        /// <seealso cref="IDbDataParameter.Precision" />
        public byte Precision
        {
            get { return m_precision; }
            set { m_precision = value; }
        }

        /// <summary>
        /// Gets or sets the scale for this parameter.
        /// </summary>
        /// <value>
        /// The scale for this parameter.
        /// </value>
        /// <remarks>
        /// <para>
        /// The number of decimal places to which Value is resolved.
        /// </para>
        /// <para>
        /// This property is optional. If not specified the ADO.NET provider 
        /// will attempt to infer the scale from the value.
        /// </para>
        /// </remarks>
        /// <seealso cref="IDbDataParameter.Scale" />
        public byte Scale
        {
            get { return m_scale; }
            set { m_scale = value; }
        }

        /// <summary>
        /// Gets or sets the size for this parameter.
        /// </summary>
        /// <value>
        /// The size for this parameter.
        /// </value>
        /// <remarks>
        /// <para>
        /// The maximum size, in bytes, of the data within the column.
        /// </para>
        /// <para>
        /// This property is optional. If not specified the ADO.NET provider 
        /// will attempt to infer the size from the value.
        /// </para>
        /// <para>
        /// For BLOB data types like VARCHAR(max) it may be impossible to infer the value automatically, use -1 as the size in this case.
        /// </para>
        /// </remarks>
        /// <seealso cref="IDbDataParameter.Size" />
        public int Size
        {
            get { return m_size; }
            set { m_size = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="IRawLayout"/> to use to 
        /// render the logging event into an object for this 
        /// parameter.
        /// </summary>
        /// <value>
        /// The <see cref="IRawLayout"/> used to render the
        /// logging event into an object for this parameter.
        /// </value>
        /// <remarks>
        /// <para>
        /// The <see cref="IRawLayout"/> that renders the value for this
        /// parameter.
        /// </para>
        /// <para>
        /// The <see cref="RawLayoutConverter"/> can be used to adapt
        /// any <see cref="ILayout"/> into a <see cref="IRawLayout"/>
        /// for use in the property.
        /// </para>
        /// </remarks>
        public IRawLayout Layout
        {
            get { return m_layout; }
            set { m_layout = value; }
        }

        #endregion // Public Instance Properties

        #region Public Instance Methods

        /// <summary>
        /// Prepare the specified database command object.
        /// </summary>
        /// <param name="command">The command to prepare.</param>
        /// <remarks>
        /// <para>
        /// Prepares the database command object by adding
        /// this parameter to its collection of parameters.
        /// </para>
        /// </remarks>
        virtual public void Prepare(IDbCommand command)
        {
            // Create a new parameter
            IDbDataParameter param = command.CreateParameter();

            // Set the parameter properties
            param.ParameterName = ParameterName;

            if (!m_inferType)
            {
                param.DbType = DbType;
            }
            if (Precision != 0)
            {
                param.Precision = Precision;
            }
            if (Scale != 0)
            {
                param.Scale = Scale;
            }
            if (Size != 0)
            {
                param.Size = Size;
            }

            // Add the parameter to the collection of params
            command.Parameters.Add(param);
        }

        /// <summary>
        /// Renders the logging event and set the parameter value in the command.
        /// </summary>
        /// <param name="command">The command containing the parameter.</param>
        /// <param name="loggingEvent">The event to be rendered.</param>
        /// <remarks>
        /// <para>
        /// Renders the logging event using this parameters layout
        /// object. Sets the value of the parameter on the command object.
        /// </para>
        /// </remarks>
        virtual public void FormatValue(IDbCommand command, LoggingEvent loggingEvent)
        {
            // Lookup the parameter
            IDbDataParameter param = (IDbDataParameter)command.Parameters[ParameterName];

            // Format the value
            object formattedValue = Layout.Format(loggingEvent);

            // If the value is null then convert to a DBNull
            if (formattedValue == null)
            {
                formattedValue = DBNull.Value;
            }

            param.Value = formattedValue;
        }

        #endregion // Public Instance Methods

        #region Private Instance Fields

        /// <summary>
        /// The name of this parameter.
        /// </summary>
        private string m_parameterName;

        /// <summary>
        /// The database type for this parameter.
        /// </summary>
        private DbType m_dbType;

        /// <summary>
        /// Flag to infer type rather than use the DbType
        /// </summary>
        private bool m_inferType = true;

        /// <summary>
        /// The precision for this parameter.
        /// </summary>
        private byte m_precision;

        /// <summary>
        /// The scale for this parameter.
        /// </summary>
        private byte m_scale;

        /// <summary>
        /// The size for this parameter.
        /// </summary>
        private int m_size;

        /// <summary>
        /// The <see cref="IRawLayout"/> to use to render the
        /// logging event into an object for this parameter.
        /// </summary>
        private IRawLayout m_layout;

        #endregion // Private Instance Fields
    }
}
