# Fully Automated Script to dump ALL tables from EnglishCenterDB to db_seed.sql
# Usage: .\DumpSeedData.ps1

$connectionString = "Server=localhost\SQLEXPRESS;Database=EnglishCenterDB;Trusted_Connection=True;TrustServerCertificate=True;"
$outputFile = "db_seed.sql"

$sb = New-Object System.Text.StringBuilder
[void]$sb.AppendLine("-- EnglishCenterDB Full Data Dump")
[void]$sb.AppendLine("-- Generated at: $(Get-Date)")
[void]$sb.AppendLine("USE [EnglishCenterDB];")
[void]$sb.AppendLine("GO")
[void]$sb.AppendLine("")
[void]$sb.AppendLine("-- Disable all constraints")
[void]$sb.AppendLine("EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';")
[void]$sb.AppendLine("GO")
[void]$sb.AppendLine("")

$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
try {
    $connection.Open()
    
    # 1. Get all user tables (excluding EF migration history)
    $tableQuery = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME NOT LIKE '__EFMigrationsHistory' ORDER BY TABLE_NAME"
    $cmdTables = $connection.CreateCommand()
    $cmdTables.CommandText = $tableQuery
    $adapter = New-Object System.Data.SqlClient.SqlDataAdapter($cmdTables)
    $dtTables = New-Object System.Data.DataTable
    [void]$adapter.Fill($dtTables)

    # 2. Add DELETE statements for all tables
    [void]$sb.AppendLine("-- Clean up existing data")
    foreach ($row in $dtTables) {
        $tableName = $row.TABLE_NAME
        [void]$sb.AppendLine("DELETE FROM [$tableName];")
    }
    [void]$sb.AppendLine("GO")
    [void]$sb.AppendLine("")

    # 3. Dump data for each table
    foreach ($row in $dtTables) {
        $tableName = $row.TABLE_NAME
        $command = $connection.CreateCommand()
        $command.CommandText = "SELECT * FROM [$tableName]"
        
        $reader = $command.ExecuteReader()
        
        if ($reader.HasRows) {
            [void]$sb.AppendLine("-- Dumping data for table $tableName")
            
            # Check if table has identity column
            $hasIdentity = $false
            $idCmd = $connection.CreateCommand()
            $idCmd.CommandText = "SELECT OBJECTPROPERTY(OBJECT_ID('$tableName'), 'TableHasIdentity')"
            # We need to close current reader first or use a new connection
            # But simpler: just try SET IDENTITY_INSERT
            
            [void]$sb.AppendLine("IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('$tableName') AND is_identity = 1) SET IDENTITY_INSERT [$tableName] ON;")
            
            while ($reader.Read()) {
                $columns = @()
                $values = @()
                
                for ($i = 0; $i -lt $reader.FieldCount; $i++) {
                    $colName = $reader.GetName($i)
                    $val = $reader.GetValue($i)
                    
                    $columns += "[$colName]"
                    
                    if ($val -is [System.DBNull]) {
                        $values += "NULL"
                    } elseif ($val -is [string] -or $val -is [System.Guid]) {
                        $values += "N'" + $val.ToString().Replace("'", "''") + "'"
                    } elseif ($val -is [DateTime]) {
                        $values += "'" + $val.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'"
                    } elseif ($val -is [bool]) {
                        $values += if ($val) { "1" } else { "0" }
                    } elseif ($val -is [byte[]]) {
                        $hex = "0x" + [System.BitConverter]::ToString($val).Replace("-", "")
                        $values += $hex
                    } else {
                        $values += $val.ToString().Replace(",", ".") # Đảm bảo dấu chấm thập phân
                    }
                }
                
                $colStr = [string]::Join(", ", $columns)
                $valStr = [string]::Join(", ", $values)
                [void]$sb.AppendLine("INSERT INTO [$tableName] ($colStr) VALUES ($valStr);")
            }
            
            [void]$sb.AppendLine("IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('$tableName') AND is_identity = 1) SET IDENTITY_INSERT [$tableName] OFF;")
            [void]$sb.AppendLine("GO")
            [void]$sb.AppendLine("")
        }
        $reader.Close()
    }
    
    [void]$sb.AppendLine("-- Re-enable all constraints")
    [void]$sb.AppendLine("EXEC sp_MSforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL';")
    [void]$sb.AppendLine("GO")

    $sb.ToString() | Out-File -FilePath $outputFile -Encoding utf8
    Write-Host "Success! ALL tables dumped to $outputFile" -ForegroundColor Green
} catch {
    Write-Error "Error: $_"
} finally {
    $connection.Close()
}
