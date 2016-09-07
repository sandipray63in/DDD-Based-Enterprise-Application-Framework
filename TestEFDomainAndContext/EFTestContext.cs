using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Reflection;
using TestEFDomainAndContext.TestDomains;

namespace TestEFDomainAndContext
{
    public class EFTestContext : DbContext
    {
        /// <summary>
        /// Refer http://www.entityframeworktutorial.net/code-first/database-initialization-strategy-in-code-first.aspx 
        /// for more info on Database Initialization Strategies in Code-First
        /// 
        /// For Lazy Loading refer - http://www.entityframeworktutorial.net/EntityFramework4.3/lazy-loading-with-dbcontext.aspx
        /// </summary>
        public EFTestContext()
        : base("EFTestContext")
        {
#if TEST
            Database.SetInitializer<EFTestContext>(new DropCreateDatabaseAlways<EFTestContext>());
#else
            Database.SetInitializer<EFTestContext>(new CreateDatabaseIfNotExists<EFTestContext>());
#endif
            this.Configuration.LazyLoadingEnabled = true;
        }

#if TEST
        public EFTestContext(bool isCalledFromWebService)
            : base("EFTestContext")
        {
            Database.SetInitializer<EFTestContext>(new CreateDatabaseIfNotExists<EFTestContext>());
        }
#endif

        public EFTestContext(string connectionString)
        : base(connectionString)
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        public EFTestContext(DbConnection connection)
        : base(connection, true)
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

#region DBSets
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
#endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove(new PluralizingTableNameConvention());

            ////Adding the entity maps

            //modelBuilder.Configurations.Add(new DepartmentMap());
            //modelBuilder.Configurations.Add(new EmployeeMap());

            //Updated the above code to use AddFromAssembly which is very useful for large number 
            // of entities(although in this case it's only 2 entities)
            // as mentioned here - https://msdn.microsoft.com/en-us/magazine/dn519921.aspx
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
