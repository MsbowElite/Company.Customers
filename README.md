# Company.Customers

# DDD project
	crqs, repository, dependency injection, clean, solid, docker compose.

# Database Sql Server EntityFramework Commands
	Concept of DATABASE FIRST is applied

		dotnet ef dbcontext scaffold "Server=localhost;Database=Company;Uid=sa;Pwd=Insecure!12345" Microsoft.EntityFrameworkCore.SqlServer

# Sql Create Script
	File in the base of solution, "Create.sql"

# Free Commands

  docker run -e "ACCEPT_EULA=Y" -e "Insecure!12345" `
   -p 1433:1433 --name companyMSQL -h companyMSQL `
   -d microsoft/mssql-server-linux

   docker container commit 016e5c9cbbf5 companyMSQL
   docker image tag company-msql msbowelite/company-msql
   docker push msbowelite/company-msql