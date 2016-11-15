using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Common;
using Contracts;
using DAL;
using DAL.Base;
using Helpers;
using Infrastructure;
using Services;

namespace OrderVehicle.Configuration
{
    public class Configuration
    {
        private static readonly IDatabaseInitializer<WfCustomDatabaseContext> WfDbInitializer = new WfCustomDatabaseContextInitializer();

        public static void Configure()
        {
            Register();
            SchemaSynchronizer();
            CreatePeristanceDatabase();
        }

        public static void Register()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(Configuration).Assembly);

            builder.RegisterType<Services.EmailService>().As<IEmailService>();
            builder.RegisterType<EmployeeService>().As<IEmployeeService>();
            builder.RegisterType<VehicleService>().As<IVehicleService>();
            builder.RegisterType<ConfigurationService>().As<IConfigurationService>();
            builder.RegisterType<RepositoryBase>().As<IRepositoryBase>().InstancePerRequest();

            builder.RegisterGeneric(typeof(VehicleOrderService<>)).As(typeof(IVehicleOrderService<>));
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));

            builder.Update(IoC.Container);

            DependencyResolver.SetResolver(new AutofacDependencyResolver(IoC.Container));
        }

        private static void CreatePeristanceDatabase()
        {
            var runScript = false;

            try
            {
                var persistanceConn = new SqlConnectionStringBuilder
                {
                    ConnectionString = IoC.Resolve<IConfigurationService>().InstanceStore
                };

                var masterConn = new SqlConnectionStringBuilder
                {
                    ConnectionString = persistanceConn.ConnectionString,
                    InitialCatalog = "master"
                };

                var sqlCreateDbQuery = $"SELECT database_id FROM sys.databases WHERE Name = '{persistanceConn.InitialCatalog}'";

                using (var sqlConn = new SqlConnection(masterConn.ConnectionString))
                {
                    using (var sqlCmd = new SqlCommand(sqlCreateDbQuery, sqlConn))
                    {
                        sqlConn.Open();

                        var resultObj = sqlCmd.ExecuteScalar();

                        var databaseId = 0;

                        if (resultObj != null)
                        {
                            int.TryParse(resultObj.ToString(), out databaseId);
                        }

                        if (!(databaseId > 0))
                        {
                            sqlCmd.CommandText = $"CREATE DATABASE {persistanceConn.InitialCatalog}";
                            sqlCmd.ExecuteNonQuery();

                            runScript = true;
                        }
                    }
                }

                if (!runScript) return;

                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");

                foreach (var file in Directory.GetFiles(path, "*.sql", SearchOption.AllDirectories))
                {
                    using (var sqlConn = new SqlConnection(persistanceConn.ConnectionString))
                    {
                        var content = File.ReadAllText(file);

                        var sqlSplit = RegexHelpers.SplitSqlStatements(content);

                        sqlConn.Open();
                        foreach (var sql in sqlSplit)
                        {
                            using (var sqlCmd = new SqlCommand(sql, sqlConn))
                            {
                                sqlCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO manage exception
            }
        }

        private static void SchemaSynchronizer()
        {
            Database.SetInitializer(WfDbInitializer);
        }
    }
}