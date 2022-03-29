namespace HospitalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class procedures : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Procedures",
                c => new
                    {
                        ProcedureId = c.Int(nullable: false, identity: true),
                        ProcedureName = c.String(),
                        ProcedureDoctor = c.Int(nullable: false),
                        ProcedurePatient = c.Int(nullable: false),
                        ProcedureRoom = c.Int(nullable: false),
                        Duration = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ProcedureId)
                .ForeignKey("dbo.Doctors", t => t.ProcedureDoctor, cascadeDelete: true)
                .ForeignKey("dbo.Patients", t => t.ProcedurePatient, cascadeDelete: true)
                .ForeignKey("dbo.Rooms", t => t.ProcedureRoom, cascadeDelete: true)
                .Index(t => t.ProcedureDoctor)
                .Index(t => t.ProcedurePatient)
                .Index(t => t.ProcedureRoom);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Procedures", "ProcedureRoom", "dbo.Rooms");
            DropForeignKey("dbo.Procedures", "ProcedurePatient", "dbo.Patients");
            DropForeignKey("dbo.Procedures", "ProcedureDoctor", "dbo.Doctors");
            DropIndex("dbo.Procedures", new[] { "ProcedureRoom" });
            DropIndex("dbo.Procedures", new[] { "ProcedurePatient" });
            DropIndex("dbo.Procedures", new[] { "ProcedureDoctor" });
            DropTable("dbo.Procedures");
        }
    }
}
