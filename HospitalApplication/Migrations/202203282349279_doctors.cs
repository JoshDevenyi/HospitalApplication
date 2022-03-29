namespace HospitalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class doctors : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Doctors",
                c => new
                    {
                        DoctorId = c.Int(nullable: false, identity: true),
                        DoctorFirstName = c.String(),
                        DoctorLastName = c.String(),
                        StaffNumber = c.Int(nullable: false),
                        Department = c.String(),
                        DoctorPhone = c.String(),
                        DoctorEmail = c.String(),
                    })
                .PrimaryKey(t => t.DoctorId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Doctors");
        }
    }
}
