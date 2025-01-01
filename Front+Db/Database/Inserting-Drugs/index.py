import pandas as pd
import pyodbc

# Step 1: File Path to Your CSV
csv_file_path = r'E:\Banyan\2023\Dec\25-12\db\drugs.csv'  # Update with your CSV file path

# Step 2: Read and Clean the CSV Data
try:
    # Read the CSV file into a DataFrame
    data = pd.read_csv(csv_file_path)
    
    # Rename CSV columns to match database columns
    data = data.rename(columns={
        'drug_name': 'DrugName',
        'ndc': 'NDC',
        'form': 'Form',
        'strength': 'Strength',
        'mfg': 'Manufacturer',
        'acq': 'AcquisitionCost',
        'awp': 'AWP',
        'dispensed': 'Dispensed',
        'p_update': 'PreviousUpdate',
        'rxcui': 'RxCUI',
        'epc_class': 'EPCClass',
        'drug_class': 'DrugClass'
    })
    
    # Clean and preprocess numeric columns
    data['AcquisitionCost'] = pd.to_numeric(data['AcquisitionCost'], errors='coerce').fillna(0).round(2)
    data['AWP'] = pd.to_numeric(data['AWP'], errors='coerce').fillna(0).round(2)

    # Ensure PreviousUpdate is in datetime format
    data['PreviousUpdate'] = pd.to_datetime(data['PreviousUpdate'], errors='coerce')

    # Fill any remaining NaN values in string columns with empty strings
    string_columns = ['DrugName', 'NDC', 'Form', 'Strength', 'Manufacturer', 'Dispensed', 'RxCUI', 'EPCClass', 'DrugClass']
    for col in string_columns:
        data[col] = data[col].fillna('')

    print("Data cleaned and ready for insertion.")
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
    "DrugName", "Manufacturer", "AcquisitionCost", "PreviousUpdate", "AWP",
    "Dispensed", "DrugClass", "EPCClass", "Form", "NDC", "RxCUI", "Strength"
]
columns_str = ', '.join(columns)
placeholders = ', '.join(['?'] * len(columns))  # Create placeholders for parameterized query
insert_query = f"INSERT INTO dbo.Drugs ({columns_str}) VALUES ({placeholders})"

# Step 5: Insert Data into the Database
try:
    for row in data.itertuples(index=False):
        cursor.execute(insert_query, 
                       row.DrugName, row.Manufacturer, row.AcquisitionCost, row.PreviousUpdate,
                       row.AWP, row.Dispensed, row.DrugClass, row.EPCClass, row.Form,
                       row.NDC, row.RxCUI, row.Strength)
    
    # Commit the transaction
    conn.commit()
    print(f"{len(data)} rows inserted successfully into dbo.Drugs.")
except Exception as e:
    print(f"Error during data insertion: {e}")
    conn.rollback()
finally:
    # Close the connection
    cursor.close()
    conn.close()
    print("Database connection closed.")
