using log4net.Appender;
using log4net.Core;
using log4net.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace ZCYX.FRMSCore.Logging
{
    public class AdoNetAppender : BufferingAppenderSkeleton
    {
        #region Public Instance Constructors

        /// <summary> 
        /// Initializes a new instance of the <see cref="AdoNetAppender" /> class.
        /// </summary>
        /// <remarks>
        /// Public default constructor to initialize a new instance of this class.
        /// </remarks>
        public AdoNetAppender()
        {
            //			ConnectionType = "System.Data.OleDb.OleDbConnection, System.Data, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            ConnectionType = typeof(SqlConnection).AssemblyQualifiedName;//"System.Data.SqlClient.SqlConnection, System.Data.SqlClient, Version=4.2.0.2, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
            UseTransactions = true;
            CommandType = System.Data.CommandType.Text;
            m_parameters = new ArrayList();
            ReconnectOnError = false;
        }

        #endregion // Public Instance Constructors

        #region Public Instance Properties

        /// <summary>
        /// Gets or sets the database connection string that is used to connect to 
        /// the database.
        /// </summary>
        /// <value>
        /// The database connection string used to connect to the database.
        /// </value>
        /// <remarks>
        /// <para>
        /// The connections string is specific to the connection type.
        /// See <see cref="ConnectionType"/> for more information.
        /// </para>
        /// </remarks>
        /// <example>Connection string for MS Access via ODBC:
        /// <code>"DSN=MS Access Database;UID=admin;PWD=;SystemDB=C:\data\System.mdw;SafeTransactions = 0;FIL=MS Access;DriverID = 25;DBQ=C:\data\train33.mdb"</code>
        /// </example>
        /// <example>Another connection string for MS Access via ODBC:
        /// <code>"Driver={Microsoft Access Driver (*.mdb)};DBQ=C:\Work\cvs_root\log4net-1.2\access.mdb;UID=;PWD=;"</code>
        /// </example>
        /// <example>Connection string for MS Access via OLE DB:
        /// <code>"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Work\cvs_root\log4net-1.2\access.mdb;User Id=;Password=;"</code>
        /// </example>
        public string ConnectionString
        {
            get { return m_connectionString; }
            set { m_connectionString = value; }
        }

        /// <summary>
        /// The appSettings key from App.Config that contains the connection string.
        /// </summary>
        public string AppSettingsKey
        {
            get { return m_appSettingsKey; }
            set { m_appSettingsKey = value; }
        }

#if NET_2_0 || NETSTANDARD1_3
        /// <summary>
        /// The connectionStrings key from App.Config that contains the connection string.
        /// </summary>
        /// <remarks>
        /// This property requires at least .NET 2.0.
        /// </remarks>
        public string ConnectionStringName
		{
			get { return m_connectionStringName; }
			set { m_connectionStringName = value; }
		}
#endif
#if NETSTANDARD1_3
	    /// <summary>
        /// The connectionStrings key from App.Config that contains the connection string.
        /// </summary>
        /// <remarks>
        /// This property requires at least .NET 2.0.
        /// </remarks>
        public string ConnectionStringFile
		{
			get { return m_connectionStringFile; }
			set { m_connectionStringFile = value; }
		}
#endif

        /// <summary>
        /// Gets or sets the type name of the <see cref="IDbConnection"/> connection
        /// that should be created.
        /// </summary>
        /// <value>
        /// The type name of the <see cref="IDbConnection"/> connection.
        /// </value>
        /// <remarks>
        /// <para>
        /// The type name of the ADO.NET provider to use.
        /// </para>
        /// <para>
        /// The default is to use the OLE DB provider.
        /// </para>
        /// </remarks>
        /// <example>Use the OLE DB Provider. This is the default value.
        /// <code>System.Data.OleDb.OleDbConnection, System.Data, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</code>
        /// </example>
        /// <example>Use the MS SQL Server Provider. 
        /// <code>System.Data.SqlClient.SqlConnection, System.Data, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</code>
        /// </example>
        /// <example>Use the ODBC Provider. 
        /// <code>Microsoft.Data.Odbc.OdbcConnection,Microsoft.Data.Odbc,version=1.0.3300.0,publicKeyToken=b77a5c561934e089,culture=neutral</code>
        /// This is an optional package that you can download from 
        /// <a href="http://msdn.microsoft.com/downloads">http://msdn.microsoft.com/downloads</a> 
        /// search for <b>ODBC .NET Data Provider</b>.
        /// </example>
        /// <example>Use the Oracle Provider. 
        /// <code>System.Data.OracleClient.OracleConnection, System.Data.OracleClient, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</code>
        /// This is an optional package that you can download from 
        /// <a href="http://msdn.microsoft.com/downloads">http://msdn.microsoft.com/downloads</a> 
        /// search for <b>.NET Managed Provider for Oracle</b>.
        /// </example>
        public string ConnectionType
        {
            get { return m_connectionType; }
            set { m_connectionType = value; }
        }

        /// <summary>
        /// Gets or sets the command text that is used to insert logging events
        /// into the database.
        /// </summary>
        /// <value>
        /// The command text used to insert logging events into the database.
        /// </value>
        /// <remarks>
        /// <para>
        /// Either the text of the prepared statement or the
        /// name of the stored procedure to execute to write into
        /// the database.
        /// </para>
        /// <para>
        /// The <see cref="CommandType"/> property determines if
        /// this text is a prepared statement or a stored procedure.
        /// </para>
        /// <para>
        /// If this property is not set, the command text is retrieved by invoking
        /// <see cref="GetLogStatement(LoggingEvent)"/>.
        /// </para>
        /// </remarks>
        public string CommandText
        {
            get { return m_commandText; }
            set { m_commandText = value; }
        }

        /// <summary>
        /// Gets or sets the command type to execute.
        /// </summary>
        /// <value>
        /// The command type to execute.
        /// </value>
        /// <remarks>
        /// <para>
        /// This value may be either <see cref="System.Data.CommandType.Text"/> (<c>System.Data.CommandType.Text</c>) to specify
        /// that the <see cref="CommandText"/> is a prepared statement to execute, 
        /// or <see cref="System.Data.CommandType.StoredProcedure"/> (<c>System.Data.CommandType.StoredProcedure</c>) to specify that the
        /// <see cref="CommandText"/> property is the name of a stored procedure
        /// to execute.
        /// </para>
        /// <para>
        /// The default value is <see cref="System.Data.CommandType.Text"/> (<c>System.Data.CommandType.Text</c>).
        /// </para>
        /// </remarks>
        public CommandType CommandType
        {
            get { return m_commandType; }
            set { m_commandType = value; }
        }

        /// <summary>
        /// Should transactions be used to insert logging events in the database.
        /// </summary>
        /// <value>
        /// <c>true</c> if transactions should be used to insert logging events in
        /// the database, otherwise <c>false</c>. The default value is <c>true</c>.
        /// </value>
        /// <remarks>
        /// <para>
        /// Gets or sets a value that indicates whether transactions should be used
        /// to insert logging events in the database.
        /// </para>
        /// <para>
        /// When set a single transaction will be used to insert the buffered events
        /// into the database. Otherwise each event will be inserted without using
        /// an explicit transaction.
        /// </para>
        /// </remarks>
        public bool UseTransactions
        {
            get { return m_useTransactions; }
            set { m_useTransactions = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="SecurityContext"/> used to call the NetSend method.
        /// </summary>
        /// <value>
        /// The <see cref="SecurityContext"/> used to call the NetSend method.
        /// </value>
        /// <remarks>
        /// <para>
        /// Unless a <see cref="SecurityContext"/> specified here for this appender
        /// the <see cref="SecurityContextProvider.DefaultProvider"/> is queried for the
        /// security context to use. The default behavior is to use the security context
        /// of the current thread.
        /// </para>
        /// </remarks>
        public SecurityContext SecurityContext
        {
            get { return m_securityContext; }
            set { m_securityContext = value; }
        }

        /// <summary>
        /// Should this appender try to reconnect to the database on error.
        /// </summary>
        /// <value>
        /// <c>true</c> if the appender should try to reconnect to the database after an
        /// error has occurred, otherwise <c>false</c>. The default value is <c>false</c>, 
        /// i.e. not to try to reconnect.
        /// </value>
        /// <remarks>
        /// <para>
        /// The default behaviour is for the appender not to try to reconnect to the
        /// database if an error occurs. Subsequent logging events are discarded.
        /// </para>
        /// <para>
        /// To force the appender to attempt to reconnect to the database set this
        /// property to <c>true</c>.
        /// </para>
        /// <note>
        /// When the appender attempts to connect to the database there may be a
        /// delay of up to the connection timeout specified in the connection string.
        /// This delay will block the calling application's thread. 
        /// Until the connection can be reestablished this potential delay may occur multiple times.
        /// </note>
        /// </remarks>
        public bool ReconnectOnError
        {
            get { return m_reconnectOnError; }
            set { m_reconnectOnError = value; }
        }

        #endregion // Public Instance Properties

        #region Protected Instance Properties

        /// <summary>
        /// Gets or sets the underlying <see cref="IDbConnection" />.
        /// </summary>
        /// <value>
        /// The underlying <see cref="IDbConnection" />.
        /// </value>
        /// <remarks>
        /// <see cref="AdoNetAppender" /> creates a <see cref="IDbConnection" /> to insert 
        /// logging events into a database.  Classes deriving from <see cref="AdoNetAppender" /> 
        /// can use this property to get or set this <see cref="IDbConnection" />.  Use the 
        /// underlying <see cref="IDbConnection" /> returned from <see cref="Connection" /> if 
        /// you require access beyond that which <see cref="AdoNetAppender" /> provides.
        /// </remarks>
        protected IDbConnection Connection
        {
            get { return m_dbConnection; }
            set { m_dbConnection = value; }
        }

        #endregion // Protected Instance Properties

        #region Implementation of IOptionHandler

        /// <summary>
        /// Initialize the appender based on the options set
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is part of the <see cref="IOptionHandler"/> delayed object
        /// activation scheme. The <see cref="ActivateOptions"/> method must 
        /// be called on this object after the configuration properties have
        /// been set. Until <see cref="ActivateOptions"/> is called this
        /// object is in an undefined state and must not be used. 
        /// </para>
        /// <para>
        /// If any of the configuration properties are modified then 
        /// <see cref="ActivateOptions"/> must be called again.
        /// </para>
        /// </remarks>
        override public void ActivateOptions()
        {
            base.ActivateOptions();

            if (SecurityContext == null)
            {
                SecurityContext = SecurityContextProvider.DefaultProvider.CreateSecurityContext(this);
            }

            InitializeDatabaseConnection();
        }

        #endregion

        #region Override implementation of AppenderSkeleton

        /// <summary>
        /// Override the parent method to close the database
        /// </summary>
        /// <remarks>
        /// <para>
        /// Closes the database command and database connection.
        /// </para>
        /// </remarks>
        override protected void OnClose()
        {
            base.OnClose();
            DiposeConnection();
        }

        #endregion

        #region Override implementation of BufferingAppenderSkeleton

        /// <summary>
        /// Inserts the events into the database.
        /// </summary>
        /// <param name="events">The events to insert into the database.</param>
        /// <remarks>
        /// <para>
        /// Insert all the events specified in the <paramref name="events"/>
        /// array into the database.
        /// </para>
        /// </remarks>
        override protected void SendBuffer(LoggingEvent[] events)
        {
            if (ReconnectOnError && (Connection == null || Connection.State != ConnectionState.Open))
            {
                LogLog.Debug(declaringType, "Attempting to reconnect to database. Current Connection State: " + ((Connection == null) ? SystemInfo.NullText : Connection.State.ToString()));

                InitializeDatabaseConnection();
            }

            // Check that the connection exists and is open
            if (Connection != null && Connection.State == ConnectionState.Open)
            {
                if (UseTransactions)
                {
                    // Create transaction
                    // NJC - Do this on 2 lines because it can confuse the debugger
                    using (IDbTransaction dbTran = Connection.BeginTransaction())
                    {
                        try
                        {
                            SendBuffer(dbTran, events);

                            // commit transaction
                            dbTran.Commit();
                        }
                        catch (Exception ex)
                        {
                            // rollback the transaction
                            try
                            {
                                dbTran.Rollback();
                            }
                            catch (Exception)
                            {
                                // Ignore exception
                            }

                            // Can't insert into the database. That's a bad thing
                            ErrorHandler.Error("Exception while writing to database", ex);
                        }
                    }
                }
                else
                {
                    // Send without transaction
                    SendBuffer(null, events);
                }
            }
        }

        #endregion // Override implementation of BufferingAppenderSkeleton

        #region Public Instance Methods

        /// <summary>
        /// Adds a parameter to the command.
        /// </summary>
        /// <param name="parameter">The parameter to add to the command.</param>
        /// <remarks>
        /// <para>
        /// Adds a parameter to the ordered list of command parameters.
        /// </para>
        /// </remarks>
        public void AddParameter(AdoNetAppenderParameter parameter)
        {
            m_parameters.Add(parameter);
        }


        #endregion // Public Instance Methods

        #region Protected Instance Methods

        /// <summary>
        /// Writes the events to the database using the transaction specified.
        /// </summary>
        /// <param name="dbTran">The transaction that the events will be executed under.</param>
        /// <param name="events">The array of events to insert into the database.</param>
        /// <remarks>
        /// <para>
        /// The transaction argument can be <c>null</c> if the appender has been
        /// configured not to use transactions. See <see cref="UseTransactions"/>
        /// property for more information.
        /// </para>
        /// </remarks>
        virtual protected void SendBuffer(IDbTransaction dbTran, LoggingEvent[] events)
        {
            // string.IsNotNullOrWhiteSpace() does not exist in ancient .NET frameworks
            if (CommandText != null && CommandText.Trim() != "")
            {
                using (IDbCommand dbCmd = Connection.CreateCommand())
                {
                    // Set the command string
                    dbCmd.CommandText = CommandText;

                    // Set the command type
                    dbCmd.CommandType = CommandType;
                    // Send buffer using the prepared command object
                    if (dbTran != null)
                    {
                        dbCmd.Transaction = dbTran;
                    }
                    // prepare the command, which is significantly faster
                    dbCmd.Prepare();
                    // run for all events
                    foreach (LoggingEvent e in events)
                    {
                        // clear parameters that have been set
                        dbCmd.Parameters.Clear();

                        // Set the parameter values
                        foreach (AdoNetAppenderParameter param in m_parameters)
                        {
                            param.Prepare(dbCmd);
                            param.FormatValue(dbCmd, e);
                        }

                        // Execute the query
                        dbCmd.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                // create a new command
                using (IDbCommand dbCmd = Connection.CreateCommand())
                {
                    if (dbTran != null)
                    {
                        dbCmd.Transaction = dbTran;
                    }
                    // run for all events
                    foreach (LoggingEvent e in events)
                    {
                        // Get the command text from the Layout
                        string logStatement = GetLogStatement(e);

                        LogLog.Debug(declaringType, "LogStatement [" + logStatement + "]");

                        dbCmd.CommandText = logStatement;
                        dbCmd.ExecuteNonQuery();
                    }
                }
            }
        }

        /// <summary>
        /// Formats the log message into database statement text.
        /// </summary>
        /// <param name="logEvent">The event being logged.</param>
        /// <remarks>
        /// This method can be overridden by subclasses to provide 
        /// more control over the format of the database statement.
        /// </remarks>
        /// <returns>
        /// Text that can be passed to a <see cref="System.Data.IDbCommand"/>.
        /// </returns>
        virtual protected string GetLogStatement(LoggingEvent logEvent)
        {
            if (Layout == null)
            {
                ErrorHandler.Error("AdoNetAppender: No Layout specified.");
                return "";
            }
            else
            {
                StringWriter writer = new StringWriter(System.Globalization.CultureInfo.InvariantCulture);
                Layout.Format(writer, logEvent);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Creates an <see cref="IDbConnection"/> instance used to connect to the database.
        /// </summary>
        /// <remarks>
        /// This method is called whenever a new IDbConnection is needed (i.e. when a reconnect is necessary).
        /// </remarks>
        /// <param name="connectionType">The <see cref="Type"/> of the <see cref="IDbConnection"/> object.</param>
        /// <param name="connectionString">The connectionString output from the ResolveConnectionString method.</param>
        /// <returns>An <see cref="IDbConnection"/> instance with a valid connection string.</returns>
        virtual protected IDbConnection CreateConnection(Type connectionType, string connectionString)
        {
            IDbConnection connection = (IDbConnection)Activator.CreateInstance(connectionType);
            connection.ConnectionString = connectionString;
            return connection;
        }

        /// <summary>
        /// Resolves the connection string from the ConnectionString, ConnectionStringName, or AppSettingsKey
        /// property.
        /// </summary>
        /// <remarks>
        /// ConnectiongStringName is only supported on .NET 2.0 and higher.
        /// </remarks>
        /// <param name="connectionStringContext">Additional information describing the connection string.</param>
        /// <returns>A connection string used to connect to the database.</returns>
        virtual protected string ResolveConnectionString(out string connectionStringContext)
        {
            if (ConnectionString != null && ConnectionString.Length > 0)
            {
                connectionStringContext = "ConnectionString";
                return ConnectionString;
            }

#if NET_2_0
			if (!String.IsNullOrEmpty(ConnectionStringName))
			{
				ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[ConnectionStringName];
				if (settings != null)
				{
					connectionStringContext = "ConnectionStringName";
					return settings.ConnectionString;
				}
				else
				{
					throw new LogException("Unable to find [" + ConnectionStringName + "] ConfigurationManager.ConnectionStrings item");
				}
			}
#endif
#if NETSTANDARD1_3
            if (!string.IsNullOrWhiteSpace(ConnectionStringFile))
		    {
		        var configFile = new FileInfo(ConnectionStringFile);
		        if (configFile.Exists)
		        {
		            var configurationBuilder = new ConfigurationBuilder();
		            if (configFile.Extension.ToLowerInvariant() == ".json")
		            {
		                configurationBuilder.AddJsonFile(configFile.FullName, false);
		            }
		            else
		            {
                        throw new LogException($"Unsupported configuration format \"{configFile.Extension}\"");
                    }
                    var configuration = configurationBuilder.Build();
		            connectionStringContext = $"ConnectionStringFile: {configFile.FullName}";
		            return configuration.GetConnectionString(ConnectionStringName);
		        }
		        throw new LogException($"Unable to find [{ConnectionStringFile}] at \"{configFile.FullName}\"");
            }
#endif

            if (AppSettingsKey != null && AppSettingsKey.Length > 0)
            {
                connectionStringContext = "AppSettingsKey";
                string appSettingsConnectionString = SystemInfo.GetAppSetting(AppSettingsKey);
                if (appSettingsConnectionString == null || appSettingsConnectionString.Length == 0)
                {
                    throw new LogException("Unable to find [" + AppSettingsKey + "] AppSettings key.");
                }
                return appSettingsConnectionString;
            }

            connectionStringContext = "Unable to resolve connection string from ConnectionString, ConnectionStrings, or AppSettings.";
            return string.Empty;
        }

        /// <summary>
        /// Retrieves the class type of the ADO.NET provider.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Gets the Type of the ADO.NET provider to use to connect to the
        /// database. This method resolves the type specified in the 
        /// <see cref="ConnectionType"/> property.
        /// </para>
        /// <para>
        /// Subclasses can override this method to return a different type
        /// if necessary.
        /// </para>
        /// </remarks>
        /// <returns>The <see cref="Type"/> of the ADO.NET provider</returns>
        virtual protected Type ResolveConnectionType()
        {
            try
            {
#if NETSTANDARD1_3
                return SystemInfo.GetTypeFromString(GetType().GetTypeInfo().Assembly, ConnectionType, true, false);
#else
                return SystemInfo.GetTypeFromString(GetType().Assembly, ConnectionType, true, false);
#endif
            }
            catch (Exception ex)
            {
                ErrorHandler.Error("Failed to load connection type [" + ConnectionType + "]", ex);
                throw;
            }
        }

        #endregion // Protected Instance Methods

        #region Private Instance Methods

        /// <summary>
        /// Connects to the database.
        /// </summary>		
        private void InitializeDatabaseConnection()
        {
            string connectionStringContext = "Unable to determine connection string context.";
            string resolvedConnectionString = string.Empty;

            try
            {
                DiposeConnection();

                // Set the connection string
                resolvedConnectionString = ResolveConnectionString(out connectionStringContext);

                Connection = CreateConnection(ResolveConnectionType(), resolvedConnectionString);

               using (SecurityContext.Impersonate(this))
                {
                    // Open the database connection
                    Connection.Open();
                }
            }
            catch (Exception e)
            {
                // Sadly, your connection string is bad.
                ErrorHandler.Error("Could not open database connection [" + resolvedConnectionString + "]. Connection string context [" + connectionStringContext + "].", e);

                Connection = null;
            }
        }

        /// <summary>
        /// Cleanup the existing connection.
        /// </summary>
        /// <remarks>
        /// Calls the IDbConnection's <see cref="IDbConnection.Close"/> method.
        /// </remarks>
        private void DiposeConnection()
        {
            if (Connection != null)
            {
                try
                {
                    Connection.Close();
                }
                catch (Exception ex)
                {
                    LogLog.Warn(declaringType, "Exception while disposing cached connection object", ex);
                }
                Connection = null;
            }
        }

        #endregion // Private Instance Methods

        #region Protected Instance Fields

        /// <summary>
        /// The list of <see cref="AdoNetAppenderParameter"/> objects.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The list of <see cref="AdoNetAppenderParameter"/> objects.
        /// </para>
        /// </remarks>
        protected ArrayList m_parameters;

        #endregion // Protected Instance Fields

        #region Private Instance Fields

        /// <summary>
        /// The security context to use for privileged calls
        /// </summary>
        private SecurityContext m_securityContext;

        /// <summary>
        /// The <see cref="IDbConnection" /> that will be used
        /// to insert logging events into a database.
        /// </summary>
        private IDbConnection m_dbConnection;

        /// <summary>
        /// Database connection string.
        /// </summary>
        private string m_connectionString;

        /// <summary>
        /// The appSettings key from App.Config that contains the connection string.
        /// </summary>
        private string m_appSettingsKey;

#if NET_2_0 || NETSTANDARD1_3
        /// <summary>
        /// The connectionStrings key from App.Config that contains the connection string.
        /// </summary>
        private string m_connectionStringName;
#endif

#if NETSTANDARD1_3
	    /// <summary>
	    /// Points to configuration file containing connectionstrings
	    /// </summary>
	    /// <remarks>Currently only .json files supported</remarks>
	    private string m_connectionStringFile;
#endif

        /// <summary>
        /// String type name of the <see cref="IDbConnection"/> type name.
        /// </summary>
        private string m_connectionType;

        /// <summary>
        /// The text of the command.
        /// </summary>
        private string m_commandText;

        /// <summary>
        /// The command type.
        /// </summary>
        private CommandType m_commandType;

        /// <summary>
        /// Indicates whether to use transactions when writing to the database.
        /// </summary>
        private bool m_useTransactions;

        /// <summary>
        /// Indicates whether to reconnect when a connection is lost.
        /// </summary>
        private bool m_reconnectOnError;

        #endregion // Private Instance Fields

        #region Private Static Fields

        /// <summary>
        /// The fully qualified type of the AdoNetAppender class.
        /// </summary>
        /// <remarks>
        /// Used by the internal logger to record the Type of the
        /// log message.
        /// </remarks>
        private readonly static Type declaringType = typeof(AdoNetAppender);

        #endregion Private Static Fields
    }
}
