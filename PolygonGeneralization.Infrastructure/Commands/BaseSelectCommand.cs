﻿using System;
using System.Data;
using System.Data.SqlClient;

namespace PolygonGeneralization.Infrastructure.Commands
{
    public class BaseSelectCommand : BaseCommand
    {
        private readonly string _commandText;

        protected BaseSelectCommand()
        {
            LoggingOn = true;
            Result = new DataTable();
        }

        public BaseSelectCommand(string commandText, string commandName)
            : this()
        {
            _commandText = commandText;
            CommandName = commandName;
        }

        public override string CommandName { get; }
        public virtual DataTable Result { get; }

        public override bool UseLogging => LoggingOn;

        public bool LoggingOn { get; set; }

        protected override void HandleImpl()
        {
            var connection =
                new SqlConnection(ConnectionStringsProvider.PolygonsConnectionString);
            try
            {

                connection.Open();
                using (var command = new SqlCommand(_commandText, connection))
                {
                    using (var dataAdapter = new SqlDataAdapter(command))
                    {
                        dataAdapter.Fill(Result);
                    }
                }
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }
    }
}