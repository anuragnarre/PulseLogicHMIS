Get-ChildItem -Path '.' -Filter '*DbContext.cs' -Recurse | ForEach-Object {
    $content = Get-Content $_.FullName -Raw
    $modified = $false
    
    if ($content -match '\.WithRequired\(') {
        $content = $content -replace '\.WithRequired\(', '.WithOne('
        $modified = $true
    }
    if ($content -match '\.WithOptional\(') {
        $content = $content -replace '\.WithOptional\(', '.WithOne('
        $modified = $true
    }
    
    # Fix DbContext Constructor
    $className = $_.BaseName
    $regexConstructor = "public\s+$className\s*\(\s*string\s+[a-zA-Z0-9_]+\s*\)\s*:\s*base\s*\(\s*[a-zA-Z0-9_]+\s*\)\s*\{[^}]*\}"
    if ($content -match $regexConstructor) {
        $newConstructor = "private readonly string _connString;`n`n        public $className(string conn)`n        {`n            _connString = conn;`n        }`n`n        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)`n        {`n            if (!optionsBuilder.IsConfigured && !string.IsNullOrEmpty(_connString))`n            {`n                optionsBuilder.UseSqlServer(_connString);`n            }`n        }"
        $content = $content -replace $regexConstructor, $newConstructor
        $modified = $true
    }

    # Fix Decimal Property Convention
    if ($content -match 'modelBuilder\.Conventions\.Remove<DecimalPropertyConvention>\(\);') {
        $decimalFix = "foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetProperties()).Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?))) { property.SetColumnType(`"decimal(16,4)`"); }"
        $content = $content -replace 'modelBuilder\.Conventions\.Remove<DecimalPropertyConvention>\(\);[\s\S]*?modelBuilder\.Conventions\.Add\(new DecimalPropertyConvention\([^)]+\)\);', $decimalFix
        $modified = $true
    }

    if ($content -match 'using System\.Data\.Entity\.ModelConfiguration\.Conventions;') {
        $content = $content -replace 'using System\.Data\.Entity\.ModelConfiguration\.Conventions;', ''
        $modified = $true
    }

    if ($content -match 'using System\.Data\.Entity;') {
        $content = $content -replace 'using System\.Data\.Entity;', ''
        $modified = $true
    }

    if ($modified) {
        Set-Content -Path $_.FullName -Value $content -Encoding UTF8
    }
}
