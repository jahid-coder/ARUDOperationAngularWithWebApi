set /p server= Enter SQL Server Name:
set db_name="EmployeeDB"


cd "./Table"
del "ResultLog.log"
for %%f in (*.sql) do sqlcmd -S %server% -d %db_name% -U %dbUser_name% -P %dbPassword% /i "%%f" >> "ResultLog.log"
cd "../"