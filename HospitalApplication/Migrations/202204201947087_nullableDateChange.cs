namespace HospitalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullableDateChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Procedures", "Time", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Procedures", "Time", c => c.DateTime(nullable: false));
        }
    }
}
