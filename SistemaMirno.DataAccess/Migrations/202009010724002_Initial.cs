﻿namespace SistemaMirno.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Assistances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        CheckIn = c.String(),
                        CheckOut = c.String(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 30),
                        LastName = c.String(nullable: false, maxLength: 30),
                        DocumentNumber = c.String(nullable: false),
                        BirthDate = c.DateTime(nullable: false),
                        Age = c.Int(nullable: false),
                        Address = c.String(nullable: false, maxLength: 200),
                        PhoneNumber = c.String(nullable: false, maxLength: 20),
                        Profession = c.String(nullable: false, maxLength: 30),
                        BaseSalary = c.Long(nullable: false),
                        SalaryOtherBonus = c.Long(nullable: false),
                        SalaryProductionBonus = c.Long(nullable: false),
                        SalarySalesBonus = c.Long(nullable: false),
                        SalaryWorkOrderBonus = c.Long(nullable: false),
                        SalaryNormalHoursBonus = c.Long(nullable: false),
                        SalaryExtraHoursBonus = c.Long(nullable: false),
                        TotalSalary = c.Long(nullable: false),
                        ReportedIpsSalary = c.Long(nullable: false),
                        ProductionBonusRatio = c.Double(nullable: false),
                        SalesBonusRatio = c.Double(nullable: false),
                        PricePerNormalHour = c.Long(nullable: false),
                        PricePerExtraHour = c.Long(nullable: false),
                        ContractStartDate = c.DateTime(nullable: false),
                        ContractFile = c.String(),
                        IsRegisteredInIps = c.Boolean(nullable: false),
                        IpsStartDate = c.DateTime(),
                        Terminated = c.Boolean(nullable: false),
                        TerminationDate = c.DateTime(),
                        UserId = c.Int(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HistoricalSalaries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        Base = c.Long(nullable: false),
                        ReportedIpsSalary = c.Long(nullable: false),
                        CompanyIpsAmmount = c.Long(nullable: false),
                        EmployeeIpsAmmount = c.Long(nullable: false),
                        ProductionBonusRatio = c.Double(nullable: false),
                        SalesBonusRatio = c.Double(nullable: false),
                        PricePerNormalHour = c.Long(nullable: false),
                        PricePerExtraHour = c.Long(nullable: false),
                        SalesBonus = c.Long(nullable: false),
                        ProductionBonus = c.Long(nullable: false),
                        WorkOrdersBonus = c.Long(nullable: false),
                        NormalHoursBonus = c.Long(nullable: false),
                        ExtraHoursBonus = c.Long(nullable: false),
                        OtherBonus = c.Long(nullable: false),
                        TotalDiscounts = c.Long(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 100),
                        BranchId = c.Int(nullable: false),
                        HasAccessToSales = c.Boolean(nullable: false),
                        HasAccessToProduction = c.Boolean(nullable: false),
                        HasAccessToHumanResources = c.Boolean(nullable: false),
                        HasAccessToAccounting = c.Boolean(nullable: false),
                        HasAccessToLogistics = c.Boolean(nullable: false),
                        IsSystemAdmin = c.Boolean(nullable: false),
                        ProceduresManualPdfFile = c.String(),
                        HasProceduresManual = c.Boolean(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branches", t => t.BranchId)
                .Index(t => t.BranchId);
            
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Address = c.String(nullable: false, maxLength: 100),
                        City = c.String(nullable: false),
                        Department = c.String(nullable: false),
                        Cash = c.Long(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WorkAreas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        BranchId = c.Int(nullable: false),
                        Position = c.String(nullable: false),
                        ResponsibleRoleId = c.Int(nullable: false),
                        SupervisorRoleId = c.Int(nullable: false),
                        IsFirst = c.Boolean(nullable: false),
                        IsLast = c.Boolean(nullable: false),
                        ReportsInProcess = c.Boolean(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branches", t => t.BranchId)
                .ForeignKey("dbo.Roles", t => t.ResponsibleRoleId)
                .ForeignKey("dbo.Roles", t => t.SupervisorRoleId)
                .Index(t => t.BranchId)
                .Index(t => t.ResponsibleRoleId)
                .Index(t => t.SupervisorRoleId);
            
            CreateTable(
                "dbo.WorkAreaConnections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OriginWorkAreaId = c.Int(nullable: false),
                        DestinationWorkAreaId = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WorkAreas", t => t.DestinationWorkAreaId)
                .ForeignKey("dbo.WorkAreas", t => t.OriginWorkAreaId)
                .Index(t => t.OriginWorkAreaId)
                .Index(t => t.DestinationWorkAreaId);
            
            CreateTable(
                "dbo.WorkOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDateTime = c.DateTime(nullable: false),
                        FinishedDateTime = c.DateTime(),
                        OriginWorkAreaId = c.Int(nullable: false),
                        DestinationWorkAreaId = c.Int(nullable: false),
                        ResponsibleEmployeeId = c.Int(nullable: false),
                        SupervisorEmployeeId = c.Int(nullable: false),
                        Observations = c.String(maxLength: 200),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WorkAreas", t => t.DestinationWorkAreaId)
                .ForeignKey("dbo.WorkAreas", t => t.OriginWorkAreaId)
                .ForeignKey("dbo.Employees", t => t.ResponsibleEmployeeId)
                .ForeignKey("dbo.Employees", t => t.SupervisorEmployeeId)
                .Index(t => t.OriginWorkAreaId)
                .Index(t => t.DestinationWorkAreaId)
                .Index(t => t.ResponsibleEmployeeId)
                .Index(t => t.SupervisorEmployeeId);
            
            CreateTable(
                "dbo.WorkOrderUnits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WorkOrderId = c.Int(nullable: false),
                        WorkUnitId = c.Int(nullable: false),
                        Finished = c.Boolean(nullable: false),
                        FinishedDateTime = c.DateTime(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WorkOrders", t => t.WorkOrderId)
                .ForeignKey("dbo.WorkUnits", t => t.WorkUnitId)
                .Index(t => t.WorkOrderId)
                .Index(t => t.WorkUnitId);
            
            CreateTable(
                "dbo.WorkUnits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        MaterialId = c.Int(nullable: false),
                        ColorId = c.Int(nullable: false),
                        CurrentWorkAreaId = c.Int(nullable: false),
                        RequisitionId = c.Int(),
                        CreationDate = c.DateTime(nullable: false),
                        TotalWorkTime = c.Double(nullable: false),
                        Delivered = c.Boolean(nullable: false),
                        Sold = c.Boolean(nullable: false),
                        LatestResponsibleId = c.Int(),
                        LatestSupervisorId = c.Int(),
                        Details = c.String(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Colors", t => t.ColorId)
                .ForeignKey("dbo.WorkAreas", t => t.CurrentWorkAreaId)
                .ForeignKey("dbo.Employees", t => t.LatestResponsibleId)
                .ForeignKey("dbo.Employees", t => t.LatestSupervisorId)
                .ForeignKey("dbo.Materials", t => t.MaterialId)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .ForeignKey("dbo.Requisitions", t => t.RequisitionId)
                .Index(t => t.ProductId)
                .Index(t => t.MaterialId)
                .Index(t => t.ColorId)
                .Index(t => t.CurrentWorkAreaId)
                .Index(t => t.RequisitionId)
                .Index(t => t.LatestResponsibleId)
                .Index(t => t.LatestSupervisorId);
            
            CreateTable(
                "dbo.Colors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Materials",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WorkAreaMovements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FromWorkAreaId = c.Int(),
                        ToWorkAreaId = c.Int(),
                        WorkUnitId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        ResponsibleId = c.Int(),
                        SupervisorId = c.Int(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WorkAreas", t => t.FromWorkAreaId)
                .ForeignKey("dbo.Employees", t => t.ResponsibleId)
                .ForeignKey("dbo.Employees", t => t.SupervisorId)
                .ForeignKey("dbo.WorkAreas", t => t.ToWorkAreaId)
                .ForeignKey("dbo.WorkUnits", t => t.WorkUnitId)
                .Index(t => t.FromWorkAreaId)
                .Index(t => t.ToWorkAreaId)
                .Index(t => t.WorkUnitId)
                .Index(t => t.ResponsibleId)
                .Index(t => t.SupervisorId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 100),
                        Name = c.String(nullable: false, maxLength: 200),
                        ProductCategoryId = c.Int(nullable: false),
                        ProductionValue = c.Long(nullable: false),
                        RetailPrice = c.Long(nullable: false),
                        WholesalerPrice = c.Long(nullable: false),
                        IsCustom = c.Boolean(nullable: false),
                        SketchupFile = c.String(maxLength: 100),
                        TemplateFile = c.String(maxLength: 100),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductCategories", t => t.ProductCategoryId)
                .Index(t => t.ProductCategoryId);
            
            CreateTable(
                "dbo.ProductCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductParts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 300),
                        Material = c.String(nullable: false),
                        RawLength = c.Double(nullable: false),
                        RawWidth = c.Double(nullable: false),
                        RawHeight = c.Double(nullable: false),
                        FinishedLength = c.Double(nullable: false),
                        FinishedWidth = c.Double(nullable: false),
                        FinishedHeight = c.Double(nullable: false),
                        ImageFile = c.String(nullable: false, maxLength: 100),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.ProductPictures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        ImageFile = c.String(nullable: false, maxLength: 100),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.ProductSupplies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        SupplyId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .ForeignKey("dbo.Supplies", t => t.SupplyId)
                .Index(t => t.SupplyId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Supplies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 200),
                        MeasureUnit = c.String(nullable: false, maxLength: 100),
                        SupplyCategoryId = c.Int(nullable: false),
                        RequiresOrderToWithdraw = c.Boolean(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SupplyCategories", t => t.SupplyCategoryId)
                .Index(t => t.SupplyCategoryId);
            
            CreateTable(
                "dbo.SupplyCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SupplyMovements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        SupplyId = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 200),
                        InQuantity = c.Int(nullable: false),
                        OutQuantity = c.Int(nullable: false),
                        ResponsibleId = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.ResponsibleId)
                .ForeignKey("dbo.Supplies", t => t.SupplyId)
                .Index(t => t.SupplyId)
                .Index(t => t.ResponsibleId);
            
            CreateTable(
                "dbo.Requisitions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequestedDate = c.DateTime(nullable: false),
                        Priority = c.String(nullable: false),
                        TargetDate = c.DateTime(),
                        Fulfilled = c.Boolean(nullable: false),
                        FulfilledDate = c.DateTime(),
                        IsForStock = c.Boolean(nullable: false),
                        ClientId = c.Int(),
                        SaleId = c.Int(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.ClientId)
                .ForeignKey("dbo.Sales", t => t.SaleId)
                .Index(t => t.ClientId)
                .Index(t => t.SaleId);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false, maxLength: 100),
                        Ruc = c.String(nullable: false, maxLength: 100),
                        PhoneNumber = c.String(nullable: false, maxLength: 20),
                        Address = c.String(maxLength: 100),
                        City = c.String(nullable: false, maxLength: 30),
                        Department = c.String(maxLength: 20),
                        Email = c.String(maxLength: 30),
                        IsRetail = c.Boolean(nullable: false),
                        IsWholesaler = c.Boolean(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ClientCommunications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        ClientId = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                        ResponsibleId = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.ClientId)
                .ForeignKey("dbo.Employees", t => t.ResponsibleId)
                .Index(t => t.ClientId)
                .Index(t => t.ResponsibleId);
            
            CreateTable(
                "dbo.Sales",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        BranchId = c.Int(nullable: false),
                        ClientId = c.Int(nullable: false),
                        ResponsibleId = c.Int(nullable: false),
                        DeliveryDate = c.DateTime(),
                        EstimatedDeliveryDate = c.DateTime(),
                        Total = c.Long(nullable: false),
                        Discount = c.Long(nullable: false),
                        Tax = c.Long(nullable: false),
                        DeliveryFee = c.Long(nullable: false),
                        InvoiceId = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branches", t => t.BranchId)
                .ForeignKey("dbo.Clients", t => t.ClientId)
                .ForeignKey("dbo.Invoices", t => t.InvoiceId)
                .ForeignKey("dbo.Employees", t => t.ResponsibleId)
                .Index(t => t.BranchId)
                .Index(t => t.ClientId)
                .Index(t => t.ResponsibleId)
                .Index(t => t.InvoiceId);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Ruc = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Code = c.String(nullable: false),
                        Tax10 = c.Long(nullable: false),
                        Tax5 = c.Long(nullable: false),
                        TotalTax = c.Long(nullable: false),
                        Total = c.Long(nullable: false),
                        IsPaid = c.Boolean(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InvoiceUnits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InvoiceId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 200),
                        Price = c.Long(nullable: false),
                        Total = c.Long(nullable: false),
                        Tax10 = c.Boolean(nullable: false),
                        Tax5 = c.Boolean(nullable: false),
                        NoTax = c.Boolean(nullable: false),
                        Discount = c.Boolean(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Invoices", t => t.InvoiceId)
                .Index(t => t.InvoiceId);
            
            CreateTable(
                "dbo.SaleCollections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SaleId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Ammount = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 200),
                        ReceiptNumber = c.String(nullable: false, maxLength: 100),
                        IsDiscount = c.Boolean(nullable: false),
                        DatedCheckId = c.Int(nullable: false),
                        BankAccountId = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branches", t => t.BranchId)
                .ForeignKey("dbo.Sales", t => t.SaleId)
                .Index(t => t.SaleId)
                .Index(t => t.BranchId);
            
            CreateTable(
                "dbo.SalaryDiscounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 200),
                        Ammount = c.Long(nullable: false),
                        Month = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.SalaryPayments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                        Ammount = c.Long(nullable: false),
                        Month = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Username = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                        HasAccessToAccounting = c.Boolean(nullable: false),
                        HasAccessToProduction = c.Boolean(nullable: false),
                        HasAccessToLogistics = c.Boolean(nullable: false),
                        HasAccessToSales = c.Boolean(nullable: false),
                        HasAccessToHumanResources = c.Boolean(nullable: false),
                        IsSystemAdmin = c.Boolean(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.BankAccountMovements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BankAccountId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Description = c.String(nullable: false, maxLength: 200),
                        AmmountIn = c.Long(nullable: false),
                        AmmountOut = c.Long(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BankAccounts", t => t.BankAccountId)
                .Index(t => t.BankAccountId);
            
            CreateTable(
                "dbo.BankAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BankId = c.Int(nullable: false),
                        AccountNumber = c.String(nullable: false),
                        Ammount = c.Long(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Banks", t => t.BankId)
                .Index(t => t.BankId);
            
            CreateTable(
                "dbo.Banks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BuyOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        OrderNumber = c.String(nullable: false, maxLength: 100),
                        ProviderId = c.Int(nullable: false),
                        Total = c.Long(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                        IsPaid = c.Boolean(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeId)
                .ForeignKey("dbo.Providers", t => t.ProviderId)
                .Index(t => t.ProviderId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.BuyOrderUnits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BuyOrderId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Description = c.Int(nullable: false),
                        Price = c.Long(nullable: false),
                        Total = c.Long(nullable: false),
                        SupplyId = c.Int(),
                        HardwareId = c.Int(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BuyOrders", t => t.BuyOrderId)
                .ForeignKey("dbo.Hardwares", t => t.HardwareId)
                .ForeignKey("dbo.Supplies", t => t.SupplyId)
                .Index(t => t.BuyOrderId)
                .Index(t => t.SupplyId)
                .Index(t => t.HardwareId);
            
            CreateTable(
                "dbo.Hardwares",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                        Brand = c.String(nullable: false),
                        SerialNumber = c.String(nullable: false),
                        AssignedEmployeeId = c.Int(),
                        IsWorking = c.Boolean(nullable: false),
                        ExpirationDate = c.DateTime(),
                        HardwareCategoryId = c.Int(nullable: false),
                        Price = c.Long(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.AssignedEmployeeId)
                .ForeignKey("dbo.HardwareCategories", t => t.HardwareCategoryId)
                .Index(t => t.AssignedEmployeeId)
                .Index(t => t.HardwareCategoryId);
            
            CreateTable(
                "dbo.HardwareCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HardwareMaintenanceOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        HardwareId = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                        TechnicianId = c.Int(nullable: false),
                        SupervisorId = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Hardwares", t => t.HardwareId)
                .ForeignKey("dbo.Employees", t => t.SupervisorId)
                .ForeignKey("dbo.Technicians", t => t.TechnicianId)
                .Index(t => t.HardwareId)
                .Index(t => t.TechnicianId)
                .Index(t => t.SupervisorId);
            
            CreateTable(
                "dbo.Technicians",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Providers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DatedChecks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsOwnCheck = c.Boolean(nullable: false),
                        IsFromClient = c.Boolean(nullable: false),
                        BankAccountId = c.Int(nullable: false),
                        Bank = c.String(nullable: false, maxLength: 100),
                        AccountNumber = c.String(nullable: false, maxLength: 100),
                        CheckHolder = c.String(nullable: false),
                        ClientId = c.Int(nullable: false),
                        ToName = c.String(nullable: false, maxLength: 200),
                        CheckNumber = c.String(nullable: false, maxLength: 100),
                        IssueDate = c.DateTime(nullable: false),
                        ExpirationDate = c.DateTime(nullable: false),
                        Ammount = c.Long(nullable: false),
                        Deposited = c.Boolean(nullable: false),
                        DepositBankAccountId = c.Int(nullable: false),
                        UsedAsProviderPayment = c.Boolean(nullable: false),
                        ProviderId = c.Int(nullable: false),
                        DateUsed = c.DateTime(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BankAccounts", t => t.BankAccountId)
                .ForeignKey("dbo.BankAccounts", t => t.DepositBankAccountId)
                .ForeignKey("dbo.Providers", t => t.ProviderId)
                .Index(t => t.BankAccountId)
                .Index(t => t.DepositBankAccountId)
                .Index(t => t.ProviderId);
            
            CreateTable(
                "dbo.Purchases",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        InvoiceId = c.Int(),
                        BuyOrderId = c.Int(),
                        Ammount = c.Long(nullable: false),
                        ProviderId = c.Int(nullable: false),
                        ProviderPaymentId = c.Int(),
                        IsCredit = c.Boolean(nullable: false),
                        IsCash = c.Boolean(nullable: false),
                        IsCard = c.Boolean(nullable: false),
                        CreditCardId = c.Int(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BuyOrders", t => t.BuyOrderId)
                .ForeignKey("dbo.CreditCards", t => t.CreditCardId)
                .ForeignKey("dbo.Invoices", t => t.InvoiceId)
                .ForeignKey("dbo.Providers", t => t.ProviderId)
                .ForeignKey("dbo.ProviderPayments", t => t.ProviderPaymentId)
                .Index(t => t.InvoiceId)
                .Index(t => t.BuyOrderId)
                .Index(t => t.ProviderId)
                .Index(t => t.ProviderPaymentId)
                .Index(t => t.CreditCardId);
            
            CreateTable(
                "dbo.CreditCards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BankAccountId = c.Int(nullable: false),
                        LastDigits = c.String(nullable: false, maxLength: 4),
                        Debt = c.Long(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BankAccounts", t => t.BankAccountId)
                .Index(t => t.BankAccountId);
            
            CreateTable(
                "dbo.CreditCardPayments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreditCardId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Ammount = c.Long(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CreditCards", t => t.CreditCardId)
                .Index(t => t.CreditCardId);
            
            CreateTable(
                "dbo.ProviderPayments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        BranchId = c.Int(),
                        DatedCheckId = c.Int(),
                        CreditCardId = c.Int(),
                        BankAccountId = c.Int(),
                        Ammount = c.Long(nullable: false),
                        ReceiptNumber = c.Long(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BankAccounts", t => t.BankAccountId)
                .ForeignKey("dbo.Branches", t => t.BranchId)
                .ForeignKey("dbo.CreditCards", t => t.CreditCardId)
                .ForeignKey("dbo.DatedChecks", t => t.DatedCheckId)
                .Index(t => t.BranchId)
                .Index(t => t.DatedCheckId)
                .Index(t => t.CreditCardId)
                .Index(t => t.BankAccountId);
            
            CreateTable(
                "dbo.CashMovements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        BranchId = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 200),
                        AmmountIn = c.Long(nullable: false),
                        AmmountOut = c.Long(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branches", t => t.BranchId)
                .Index(t => t.BranchId);
            
            CreateTable(
                "dbo.Deliveries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SaleId = c.Int(nullable: false),
                        DeliveryOrderId = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DeliveryOrders", t => t.DeliveryOrderId)
                .ForeignKey("dbo.Sales", t => t.SaleId)
                .Index(t => t.SaleId)
                .Index(t => t.DeliveryOrderId);
            
            CreateTable(
                "dbo.DeliveryOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        VehicleId = c.Int(nullable: false),
                        ResponsibleId = c.Int(nullable: false),
                        KmBefore = c.Int(nullable: false),
                        KmAfter = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.ResponsibleId)
                .ForeignKey("dbo.Vehicles", t => t.VehicleId)
                .Index(t => t.VehicleId)
                .Index(t => t.ResponsibleId);
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Model = c.String(nullable: false),
                        Year = c.Int(nullable: false),
                        Odometer = c.Int(nullable: false),
                        IsAvailable = c.Boolean(nullable: false),
                        PatentExpiration = c.DateTime(nullable: false),
                        PatentPaid = c.Boolean(nullable: false),
                        DinatranExpiration = c.DateTime(nullable: false),
                        DinatranPaid = c.Boolean(nullable: false),
                        FireExtinguisherExpiration = c.DateTime(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VehicleMaintenanceOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Details = c.String(nullable: false),
                        ResponsibleId = c.Int(nullable: false),
                        Place = c.String(nullable: false, maxLength: 200),
                        VehicleId = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.ResponsibleId)
                .ForeignKey("dbo.Vehicles", t => t.VehicleId)
                .Index(t => t.ResponsibleId)
                .Index(t => t.VehicleId);
            
            CreateTable(
                "dbo.DeliveryUnits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DeliveryId = c.Int(nullable: false),
                        WorkUnitId = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Deliveries", t => t.DeliveryId)
                .ForeignKey("dbo.WorkUnits", t => t.WorkUnitId)
                .Index(t => t.DeliveryId)
                .Index(t => t.WorkUnitId);
            
            CreateTable(
                "dbo.IntermediateProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        ManufacturingWorkAreaId = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WorkAreas", t => t.ManufacturingWorkAreaId)
                .Index(t => t.ManufacturingWorkAreaId);
            
            CreateTable(
                "dbo.IntermediateWorkOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDateTime = c.DateTime(nullable: false),
                        FinisheDateTime = c.DateTime(),
                        ResponsibleId = c.Int(nullable: false),
                        SupervisorId = c.Int(nullable: false),
                        IntermediateProductId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IntermediateProducts", t => t.IntermediateProductId)
                .ForeignKey("dbo.Employees", t => t.ResponsibleId)
                .ForeignKey("dbo.Employees", t => t.SupervisorId)
                .Index(t => t.ResponsibleId)
                .Index(t => t.SupervisorId)
                .Index(t => t.IntermediateProductId);
            
            CreateTable(
                "dbo.IntermediateWorkUnitMovements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IntermediateProductId = c.Int(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                        QuantityIn = c.Int(nullable: false),
                        QuantityOut = c.Int(nullable: false),
                        ResponsibleId = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IntermediateProducts", t => t.IntermediateProductId)
                .ForeignKey("dbo.Employees", t => t.ResponsibleId)
                .Index(t => t.IntermediateProductId)
                .Index(t => t.ResponsibleId);
            
            CreateTable(
                "dbo.IntermediateWorkUnits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        IntermediateProductId = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IntermediateProducts", t => t.IntermediateProductId)
                .Index(t => t.IntermediateProductId);
            
            CreateTable(
                "dbo.RoleEmployees",
                c => new
                    {
                        Role_Id = c.Int(nullable: false),
                        Employee_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_Id, t.Employee_Id })
                .ForeignKey("dbo.Roles", t => t.Role_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.Employee_Id, cascadeDelete: true)
                .Index(t => t.Role_Id)
                .Index(t => t.Employee_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IntermediateWorkUnits", "IntermediateProductId", "dbo.IntermediateProducts");
            DropForeignKey("dbo.IntermediateWorkUnitMovements", "ResponsibleId", "dbo.Employees");
            DropForeignKey("dbo.IntermediateWorkUnitMovements", "IntermediateProductId", "dbo.IntermediateProducts");
            DropForeignKey("dbo.IntermediateWorkOrders", "SupervisorId", "dbo.Employees");
            DropForeignKey("dbo.IntermediateWorkOrders", "ResponsibleId", "dbo.Employees");
            DropForeignKey("dbo.IntermediateWorkOrders", "IntermediateProductId", "dbo.IntermediateProducts");
            DropForeignKey("dbo.IntermediateProducts", "ManufacturingWorkAreaId", "dbo.WorkAreas");
            DropForeignKey("dbo.Deliveries", "SaleId", "dbo.Sales");
            DropForeignKey("dbo.DeliveryUnits", "WorkUnitId", "dbo.WorkUnits");
            DropForeignKey("dbo.DeliveryUnits", "DeliveryId", "dbo.Deliveries");
            DropForeignKey("dbo.DeliveryOrders", "VehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.VehicleMaintenanceOrders", "VehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.VehicleMaintenanceOrders", "ResponsibleId", "dbo.Employees");
            DropForeignKey("dbo.DeliveryOrders", "ResponsibleId", "dbo.Employees");
            DropForeignKey("dbo.Deliveries", "DeliveryOrderId", "dbo.DeliveryOrders");
            DropForeignKey("dbo.CashMovements", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.BuyOrders", "ProviderId", "dbo.Providers");
            DropForeignKey("dbo.Purchases", "ProviderPaymentId", "dbo.ProviderPayments");
            DropForeignKey("dbo.ProviderPayments", "DatedCheckId", "dbo.DatedChecks");
            DropForeignKey("dbo.ProviderPayments", "CreditCardId", "dbo.CreditCards");
            DropForeignKey("dbo.ProviderPayments", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.ProviderPayments", "BankAccountId", "dbo.BankAccounts");
            DropForeignKey("dbo.Purchases", "ProviderId", "dbo.Providers");
            DropForeignKey("dbo.Purchases", "InvoiceId", "dbo.Invoices");
            DropForeignKey("dbo.Purchases", "CreditCardId", "dbo.CreditCards");
            DropForeignKey("dbo.CreditCardPayments", "CreditCardId", "dbo.CreditCards");
            DropForeignKey("dbo.CreditCards", "BankAccountId", "dbo.BankAccounts");
            DropForeignKey("dbo.Purchases", "BuyOrderId", "dbo.BuyOrders");
            DropForeignKey("dbo.DatedChecks", "ProviderId", "dbo.Providers");
            DropForeignKey("dbo.DatedChecks", "DepositBankAccountId", "dbo.BankAccounts");
            DropForeignKey("dbo.DatedChecks", "BankAccountId", "dbo.BankAccounts");
            DropForeignKey("dbo.BuyOrders", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.BuyOrderUnits", "SupplyId", "dbo.Supplies");
            DropForeignKey("dbo.BuyOrderUnits", "HardwareId", "dbo.Hardwares");
            DropForeignKey("dbo.HardwareMaintenanceOrders", "TechnicianId", "dbo.Technicians");
            DropForeignKey("dbo.HardwareMaintenanceOrders", "SupervisorId", "dbo.Employees");
            DropForeignKey("dbo.HardwareMaintenanceOrders", "HardwareId", "dbo.Hardwares");
            DropForeignKey("dbo.Hardwares", "HardwareCategoryId", "dbo.HardwareCategories");
            DropForeignKey("dbo.Hardwares", "AssignedEmployeeId", "dbo.Employees");
            DropForeignKey("dbo.BuyOrderUnits", "BuyOrderId", "dbo.BuyOrders");
            DropForeignKey("dbo.BankAccountMovements", "BankAccountId", "dbo.BankAccounts");
            DropForeignKey("dbo.BankAccounts", "BankId", "dbo.Banks");
            DropForeignKey("dbo.Users", "Id", "dbo.Employees");
            DropForeignKey("dbo.SalaryPayments", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.SalaryDiscounts", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.RoleEmployees", "Employee_Id", "dbo.Employees");
            DropForeignKey("dbo.RoleEmployees", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.WorkAreas", "SupervisorRoleId", "dbo.Roles");
            DropForeignKey("dbo.WorkAreas", "ResponsibleRoleId", "dbo.Roles");
            DropForeignKey("dbo.WorkOrderUnits", "WorkUnitId", "dbo.WorkUnits");
            DropForeignKey("dbo.WorkUnits", "RequisitionId", "dbo.Requisitions");
            DropForeignKey("dbo.SaleCollections", "SaleId", "dbo.Sales");
            DropForeignKey("dbo.SaleCollections", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.Sales", "ResponsibleId", "dbo.Employees");
            DropForeignKey("dbo.Requisitions", "SaleId", "dbo.Sales");
            DropForeignKey("dbo.Sales", "InvoiceId", "dbo.Invoices");
            DropForeignKey("dbo.InvoiceUnits", "InvoiceId", "dbo.Invoices");
            DropForeignKey("dbo.Sales", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.Sales", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.Requisitions", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.ClientCommunications", "ResponsibleId", "dbo.Employees");
            DropForeignKey("dbo.ClientCommunications", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.WorkUnits", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductSupplies", "SupplyId", "dbo.Supplies");
            DropForeignKey("dbo.SupplyMovements", "SupplyId", "dbo.Supplies");
            DropForeignKey("dbo.SupplyMovements", "ResponsibleId", "dbo.Employees");
            DropForeignKey("dbo.Supplies", "SupplyCategoryId", "dbo.SupplyCategories");
            DropForeignKey("dbo.ProductSupplies", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductPictures", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductParts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Products", "ProductCategoryId", "dbo.ProductCategories");
            DropForeignKey("dbo.WorkAreaMovements", "WorkUnitId", "dbo.WorkUnits");
            DropForeignKey("dbo.WorkAreaMovements", "ToWorkAreaId", "dbo.WorkAreas");
            DropForeignKey("dbo.WorkAreaMovements", "SupervisorId", "dbo.Employees");
            DropForeignKey("dbo.WorkAreaMovements", "ResponsibleId", "dbo.Employees");
            DropForeignKey("dbo.WorkAreaMovements", "FromWorkAreaId", "dbo.WorkAreas");
            DropForeignKey("dbo.WorkUnits", "MaterialId", "dbo.Materials");
            DropForeignKey("dbo.WorkUnits", "LatestSupervisorId", "dbo.Employees");
            DropForeignKey("dbo.WorkUnits", "LatestResponsibleId", "dbo.Employees");
            DropForeignKey("dbo.WorkUnits", "CurrentWorkAreaId", "dbo.WorkAreas");
            DropForeignKey("dbo.WorkUnits", "ColorId", "dbo.Colors");
            DropForeignKey("dbo.WorkOrderUnits", "WorkOrderId", "dbo.WorkOrders");
            DropForeignKey("dbo.WorkOrders", "SupervisorEmployeeId", "dbo.Employees");
            DropForeignKey("dbo.WorkOrders", "ResponsibleEmployeeId", "dbo.Employees");
            DropForeignKey("dbo.WorkOrders", "OriginWorkAreaId", "dbo.WorkAreas");
            DropForeignKey("dbo.WorkOrders", "DestinationWorkAreaId", "dbo.WorkAreas");
            DropForeignKey("dbo.WorkAreaConnections", "OriginWorkAreaId", "dbo.WorkAreas");
            DropForeignKey("dbo.WorkAreaConnections", "DestinationWorkAreaId", "dbo.WorkAreas");
            DropForeignKey("dbo.WorkAreas", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.Roles", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.HistoricalSalaries", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Assistances", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.RoleEmployees", new[] { "Employee_Id" });
            DropIndex("dbo.RoleEmployees", new[] { "Role_Id" });
            DropIndex("dbo.IntermediateWorkUnits", new[] { "IntermediateProductId" });
            DropIndex("dbo.IntermediateWorkUnitMovements", new[] { "ResponsibleId" });
            DropIndex("dbo.IntermediateWorkUnitMovements", new[] { "IntermediateProductId" });
            DropIndex("dbo.IntermediateWorkOrders", new[] { "IntermediateProductId" });
            DropIndex("dbo.IntermediateWorkOrders", new[] { "SupervisorId" });
            DropIndex("dbo.IntermediateWorkOrders", new[] { "ResponsibleId" });
            DropIndex("dbo.IntermediateProducts", new[] { "ManufacturingWorkAreaId" });
            DropIndex("dbo.DeliveryUnits", new[] { "WorkUnitId" });
            DropIndex("dbo.DeliveryUnits", new[] { "DeliveryId" });
            DropIndex("dbo.VehicleMaintenanceOrders", new[] { "VehicleId" });
            DropIndex("dbo.VehicleMaintenanceOrders", new[] { "ResponsibleId" });
            DropIndex("dbo.DeliveryOrders", new[] { "ResponsibleId" });
            DropIndex("dbo.DeliveryOrders", new[] { "VehicleId" });
            DropIndex("dbo.Deliveries", new[] { "DeliveryOrderId" });
            DropIndex("dbo.Deliveries", new[] { "SaleId" });
            DropIndex("dbo.CashMovements", new[] { "BranchId" });
            DropIndex("dbo.ProviderPayments", new[] { "BankAccountId" });
            DropIndex("dbo.ProviderPayments", new[] { "CreditCardId" });
            DropIndex("dbo.ProviderPayments", new[] { "DatedCheckId" });
            DropIndex("dbo.ProviderPayments", new[] { "BranchId" });
            DropIndex("dbo.CreditCardPayments", new[] { "CreditCardId" });
            DropIndex("dbo.CreditCards", new[] { "BankAccountId" });
            DropIndex("dbo.Purchases", new[] { "CreditCardId" });
            DropIndex("dbo.Purchases", new[] { "ProviderPaymentId" });
            DropIndex("dbo.Purchases", new[] { "ProviderId" });
            DropIndex("dbo.Purchases", new[] { "BuyOrderId" });
            DropIndex("dbo.Purchases", new[] { "InvoiceId" });
            DropIndex("dbo.DatedChecks", new[] { "ProviderId" });
            DropIndex("dbo.DatedChecks", new[] { "DepositBankAccountId" });
            DropIndex("dbo.DatedChecks", new[] { "BankAccountId" });
            DropIndex("dbo.HardwareMaintenanceOrders", new[] { "SupervisorId" });
            DropIndex("dbo.HardwareMaintenanceOrders", new[] { "TechnicianId" });
            DropIndex("dbo.HardwareMaintenanceOrders", new[] { "HardwareId" });
            DropIndex("dbo.Hardwares", new[] { "HardwareCategoryId" });
            DropIndex("dbo.Hardwares", new[] { "AssignedEmployeeId" });
            DropIndex("dbo.BuyOrderUnits", new[] { "HardwareId" });
            DropIndex("dbo.BuyOrderUnits", new[] { "SupplyId" });
            DropIndex("dbo.BuyOrderUnits", new[] { "BuyOrderId" });
            DropIndex("dbo.BuyOrders", new[] { "EmployeeId" });
            DropIndex("dbo.BuyOrders", new[] { "ProviderId" });
            DropIndex("dbo.BankAccounts", new[] { "BankId" });
            DropIndex("dbo.BankAccountMovements", new[] { "BankAccountId" });
            DropIndex("dbo.Users", new[] { "Id" });
            DropIndex("dbo.SalaryPayments", new[] { "EmployeeId" });
            DropIndex("dbo.SalaryDiscounts", new[] { "EmployeeId" });
            DropIndex("dbo.SaleCollections", new[] { "BranchId" });
            DropIndex("dbo.SaleCollections", new[] { "SaleId" });
            DropIndex("dbo.InvoiceUnits", new[] { "InvoiceId" });
            DropIndex("dbo.Sales", new[] { "InvoiceId" });
            DropIndex("dbo.Sales", new[] { "ResponsibleId" });
            DropIndex("dbo.Sales", new[] { "ClientId" });
            DropIndex("dbo.Sales", new[] { "BranchId" });
            DropIndex("dbo.ClientCommunications", new[] { "ResponsibleId" });
            DropIndex("dbo.ClientCommunications", new[] { "ClientId" });
            DropIndex("dbo.Requisitions", new[] { "SaleId" });
            DropIndex("dbo.Requisitions", new[] { "ClientId" });
            DropIndex("dbo.SupplyMovements", new[] { "ResponsibleId" });
            DropIndex("dbo.SupplyMovements", new[] { "SupplyId" });
            DropIndex("dbo.Supplies", new[] { "SupplyCategoryId" });
            DropIndex("dbo.ProductSupplies", new[] { "ProductId" });
            DropIndex("dbo.ProductSupplies", new[] { "SupplyId" });
            DropIndex("dbo.ProductPictures", new[] { "ProductId" });
            DropIndex("dbo.ProductParts", new[] { "ProductId" });
            DropIndex("dbo.Products", new[] { "ProductCategoryId" });
            DropIndex("dbo.WorkAreaMovements", new[] { "SupervisorId" });
            DropIndex("dbo.WorkAreaMovements", new[] { "ResponsibleId" });
            DropIndex("dbo.WorkAreaMovements", new[] { "WorkUnitId" });
            DropIndex("dbo.WorkAreaMovements", new[] { "ToWorkAreaId" });
            DropIndex("dbo.WorkAreaMovements", new[] { "FromWorkAreaId" });
            DropIndex("dbo.WorkUnits", new[] { "LatestSupervisorId" });
            DropIndex("dbo.WorkUnits", new[] { "LatestResponsibleId" });
            DropIndex("dbo.WorkUnits", new[] { "RequisitionId" });
            DropIndex("dbo.WorkUnits", new[] { "CurrentWorkAreaId" });
            DropIndex("dbo.WorkUnits", new[] { "ColorId" });
            DropIndex("dbo.WorkUnits", new[] { "MaterialId" });
            DropIndex("dbo.WorkUnits", new[] { "ProductId" });
            DropIndex("dbo.WorkOrderUnits", new[] { "WorkUnitId" });
            DropIndex("dbo.WorkOrderUnits", new[] { "WorkOrderId" });
            DropIndex("dbo.WorkOrders", new[] { "SupervisorEmployeeId" });
            DropIndex("dbo.WorkOrders", new[] { "ResponsibleEmployeeId" });
            DropIndex("dbo.WorkOrders", new[] { "DestinationWorkAreaId" });
            DropIndex("dbo.WorkOrders", new[] { "OriginWorkAreaId" });
            DropIndex("dbo.WorkAreaConnections", new[] { "DestinationWorkAreaId" });
            DropIndex("dbo.WorkAreaConnections", new[] { "OriginWorkAreaId" });
            DropIndex("dbo.WorkAreas", new[] { "SupervisorRoleId" });
            DropIndex("dbo.WorkAreas", new[] { "ResponsibleRoleId" });
            DropIndex("dbo.WorkAreas", new[] { "BranchId" });
            DropIndex("dbo.Roles", new[] { "BranchId" });
            DropIndex("dbo.HistoricalSalaries", new[] { "EmployeeId" });
            DropIndex("dbo.Assistances", new[] { "EmployeeId" });
            DropTable("dbo.RoleEmployees");
            DropTable("dbo.IntermediateWorkUnits");
            DropTable("dbo.IntermediateWorkUnitMovements");
            DropTable("dbo.IntermediateWorkOrders");
            DropTable("dbo.IntermediateProducts");
            DropTable("dbo.DeliveryUnits");
            DropTable("dbo.VehicleMaintenanceOrders");
            DropTable("dbo.Vehicles");
            DropTable("dbo.DeliveryOrders");
            DropTable("dbo.Deliveries");
            DropTable("dbo.CashMovements");
            DropTable("dbo.ProviderPayments");
            DropTable("dbo.CreditCardPayments");
            DropTable("dbo.CreditCards");
            DropTable("dbo.Purchases");
            DropTable("dbo.DatedChecks");
            DropTable("dbo.Providers");
            DropTable("dbo.Technicians");
            DropTable("dbo.HardwareMaintenanceOrders");
            DropTable("dbo.HardwareCategories");
            DropTable("dbo.Hardwares");
            DropTable("dbo.BuyOrderUnits");
            DropTable("dbo.BuyOrders");
            DropTable("dbo.Banks");
            DropTable("dbo.BankAccounts");
            DropTable("dbo.BankAccountMovements");
            DropTable("dbo.Users");
            DropTable("dbo.SalaryPayments");
            DropTable("dbo.SalaryDiscounts");
            DropTable("dbo.SaleCollections");
            DropTable("dbo.InvoiceUnits");
            DropTable("dbo.Invoices");
            DropTable("dbo.Sales");
            DropTable("dbo.ClientCommunications");
            DropTable("dbo.Clients");
            DropTable("dbo.Requisitions");
            DropTable("dbo.SupplyMovements");
            DropTable("dbo.SupplyCategories");
            DropTable("dbo.Supplies");
            DropTable("dbo.ProductSupplies");
            DropTable("dbo.ProductPictures");
            DropTable("dbo.ProductParts");
            DropTable("dbo.ProductCategories");
            DropTable("dbo.Products");
            DropTable("dbo.WorkAreaMovements");
            DropTable("dbo.Materials");
            DropTable("dbo.Colors");
            DropTable("dbo.WorkUnits");
            DropTable("dbo.WorkOrderUnits");
            DropTable("dbo.WorkOrders");
            DropTable("dbo.WorkAreaConnections");
            DropTable("dbo.WorkAreas");
            DropTable("dbo.Branches");
            DropTable("dbo.Roles");
            DropTable("dbo.HistoricalSalaries");
            DropTable("dbo.Employees");
            DropTable("dbo.Assistances");
        }
    }
}