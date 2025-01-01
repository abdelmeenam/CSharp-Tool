import pandas as pd

# Load the CSV file
file_path = r'E:\\Banyan\\2023\\Dec\\25-12\\db\\29-12\\scripts.csv'  # Update with your actual file name
data = pd.read_csv(file_path)

# Convert the 'Date' column to datetime and format it
data['Date'] = pd.to_datetime(data['Date'], dayfirst=True).dt.strftime('%Y-%m-%d %H:%M:%S')

# Define the SQL CREATE TABLE statement
create_table_sql = """
CREATE TABLE Prescriptions (
    Date DATETIME,
    Script INT,
    RNumber INT,
    RA INT,
    DrugName NVARCHAR(255),
    Ins NVARCHAR(50),
    PF NVARCHAR(50),
    Prescriber NVARCHAR(255),
    Qty FLOAT,
    ACQ FLOAT,
    Discount INT,
    InsPay FLOAT,
    PatPay FLOAT,
    NDC BIGINT,
    RxCui FLOAT,
    Class NVARCHAR(255),
    Net FLOAT
);
"""

# Replace NaN values with NULL for SQL compatibility
data.fillna(value="NULL", inplace=True)

# Prepare the SQL INSERT statements for the rows
insert_statements = []
for index, row in data.iterrows():
    insert_statements.append(f"""
    INSERT INTO Prescriptions (
        Date, Script, RNumber, RA, DrugName, Ins, PF, Prescriber, Qty, ACQ, Discount, InsPay, PatPay, NDC, RxCui, Class, Net
    ) VALUES (
        '{row['Date']}', {row['Script']}, {row['R#']}, {row['RA']}, 
        '{row['Drug Name']}', '{row['Ins']}', '{row['PF']}', '{row['Prescriber']}',
        {row['Qty']}, {row['ACQ']}, {row['Discount']}, {row['Ins Pay']}, 
        {row['Pat Pay']}, {row['NDC']}, {row['RxCui']}, '{row['class']}', {row['NET']}
    );
    """)

# Combine all statements into a single SQL script
insert_script = "\n".join(insert_statements)

# Save the CREATE TABLE and INSERT script to a file
output_path = r'E:\\Banyan\\2023\\Dec\\25-12\\db\\29-12\\SQL_Script-date.sql'
with open(output_path, "w") as file:
    file.write(create_table_sql + "\n" + insert_script)

# Log the number of insert statements
print(f"SQL script has been saved to {output_path}")
print(f"Total number of INSERT statements generated: {len(insert_statements)}")
