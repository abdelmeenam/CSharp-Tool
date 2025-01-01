import pandas as pd
import pyodbc

# Step 1: File Path to Your CSV
csv_file_path = r'E:\Banyan\2023\Dec\25-12\db\scripts.csv'  # Update with your CSV file path

# Step 2: Read and Clean the CSV Data
try:
    # Read the CSV file into a DataFrame
    data = pd.read_csv(csv_file_path)

    # Rename CSV columns to match database columns
    data = data.rename(columns={
        'Date': 'Date',
        'Script': 'ScriptNumber',
        'R#': 'RxData',
        'RA': 'RaData',
        'Drug Name': 'DrugName',
        'Ins': 'Insurance',
        'PF': 'PF',
        'Prescriber': 'Prescriber',
        'Qty': 'Qty',
        'ACQ': 'ACQ',
        'Discount': 'Discount',
        'Ins Pay': 'InsPay',
        'Pat Pay': 'PatPay',
        'NDC': 'NDC',
        'RxCui': 'RxCul',
        'class': 'Class',
        'NET': 'NET'
    })

    # Ensure Date is in datetime format and adjust format
    data['Date'] = pd.to_datetime(data['Date'], errors='coerce').dt.strftime('%Y-%m-%d %H:%M:%S')

    # Ensure NET column values are valid
    data['NET'] = pd.to_numeric(data['NET'], errors='coerce')  # Convert invalid values to NaN
    data['NET'] = data['NET'].apply(lambda x: round(x, 2) if pd.notnull(x) else x)  # Round to 2 decimal places

    # Replace NaN with None for SQL NULL
    data = data.where(pd.notnull(data), None)

except Exception as e:
    print(f"Error reading or cleaning the CSV file: {e}")
    exit()

# Step 3: Connect to SQL Server
try:
    connection_string = (
        "Driver={SQL Server};"
        "Server=PC\\SQLEXPRESS;"  # Update with your SQL Server details
        "Database=DrugsDb;"       # Your database name
        "Trusted_Connection=yes;" # Use Windows authentication
    )
    conn = pyodbc.connect(connection_string)
    cursor = conn.cursor()
    print("Database connection successful.")
except Exception as e:
    print(f"Error connecting to the database: {e}")
    exit()

# Step 4: Prepare the Insert Query
columns = [
    "Date", "ScriptNumber", "RxData", "RaData", "DrugName", "Insurance", "PF",
    "Prescriber", "Qty", "ACQ", "Discount", "InsPay", "PatPay", "NDC",
    "RxCul", "Class", "NET"
]
columns_str = ', '.join(columns)
placeholders = ', '.join(['?'] * len(columns))  # Create placeholders for parameterized query
insert_query = f"INSERT INTO dbo.Scripts ({columns_str}) VALUES ({placeholders})"

# Step 5: Insert Data into the Database
try:
    failed_rows = []  # To track rows that fail insertion
    for row in data.itertuples(index=False):
        try:
            cursor.execute(insert_query,
                           row.Date, row.ScriptNumber, row.RxData, row.RaData,
                           row.DrugName, row.Insurance, row.PF, row.Prescriber,
                           row.Qty, row.ACQ, row.Discount, row.InsPay,
                           row.PatPay, row.NDC, row.RxCul, row.Class, row.NET)
        except Exception as e:
            print(f"Error inserting row: {row}")
            print(f"Error: {e}")
            failed_rows.append(row)

    # Commit the transaction
    conn.commit()
    print(f"{len(data) - len(failed_rows)} rows inserted successfully into dbo.Scripts.")
    if failed_rows:
        pd.DataFrame(failed_rows).to_csv('failed_rows.csv', index=False)
        print(f"Failed rows saved to 'failed_rows.csv' for review.")
except Exception as e:
    print(f"Error during data insertion: {e}")
    conn.rollback()
finally:
    # Close the connection
    cursor.close()
    conn.close()
    print("Database connection closed.")