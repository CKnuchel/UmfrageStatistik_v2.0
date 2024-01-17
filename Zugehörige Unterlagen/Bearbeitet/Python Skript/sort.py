# Normalisieren von Daten aus CSV für die Verwendung in der Datenbank

import csv

#```
# 
# `

skip = True
id = 0
row_coounter = 1
entry_number = 1

def writeNewRow(date, moduleId, responseId, value):
    global entry_number
    # Erstellen einer neuen Zeile in der CSV-Datei
    with open('Response.csv', 'a', newline='') as csvfile:
        writer = csv.writer(csvfile, delimiter=';')
        writer.writerow([entry_number, responseId, moduleId, value, date])
        entry_number += 1  # Inkrementierung der fortlaufenden Nummer

# Kopzeile der CSV-Datei schreiben
with open('Response.csv', 'w', newline='') as csvfile:
    writer = csv.writer(csvfile, delimiter=';')
    writer.writerow(['ResponseId', 'AnswerId', 'ModuleId', 'Value', 'DateTime'])

# Einlesen der CSV-Datei
with open('n1.CSV', 'r') as csvfile:
    reader = csv.reader(csvfile, delimiter=';')

    for row in reader:
        date = row[0]
        moduleId = row[1]
        # Frage 1
        if row[3].isdigit():
            writeNewRow(date, moduleId, 1, 1)
        if row[4].isdigit():
            writeNewRow(date, moduleId, 2, 1)
        if row[5].isdigit():
            writeNewRow(date, moduleId, 3, 1)

        # Frage 2
        if row[7].isdigit():
            writeNewRow(date, moduleId, 4, 1)
        if row[8].isdigit():
            writeNewRow(date, moduleId, 5, 1)
        if row[9].isdigit():
            writeNewRow(date, moduleId, 6, 1)
        # Frage 3
        if row[11].isdigit():
            writeNewRow(date, moduleId, 7, 1)
        if row[12].isdigit():
            writeNewRow(date, moduleId, 8, 1)
        if row[13].isdigit():
            writeNewRow(date, moduleId, 9, 1)
        # Frage 4
        if row[15].isdigit():
            writeNewRow(date, moduleId, 10, 1)
        if row[16].isdigit():
            writeNewRow(date, moduleId, 11, 1)
        if row[17].isdigit():
            writeNewRow(date, moduleId, 12, 1)
        # Frage 5
        if row[19].isdigit():
            writeNewRow(date, moduleId, 13, 1)
        if row[20].isdigit():
            writeNewRow(date, moduleId, 14, 1)
        if row[21].isdigit():
            writeNewRow(date, moduleId, 15, 1)
        if row[22].isdigit():
            writeNewRow(date, moduleId, 16, 1)
        if row[23].isdigit():
            writeNewRow(date, moduleId, 17, 1)
        # Frage 6
        if row[25].isdigit():
            writeNewRow(date, moduleId, 18, 1)
        if row[26].isdigit():
            writeNewRow(date, moduleId, 19, 1)
        if row[27].isdigit():
            writeNewRow(date, moduleId, 20, 1)
        if row[28].isdigit():
            writeNewRow(date, moduleId, 21, 1)
        # Frage 7
        if row[30].isdigit():
            writeNewRow(date, moduleId, 22, 1)
        if row[31].isdigit():
            writeNewRow(date, moduleId, 23, 1)
        if row[32].isdigit():
            writeNewRow(date, moduleId, 24, 1)
        if row[33].isdigit():
            writeNewRow(date, moduleId, 25, 1)
        # Frage 8
        if row[35].isdigit():
            writeNewRow(date, moduleId, 26, 1)
        if row[36].isdigit():
            writeNewRow(date, moduleId, 27, 1)
        if row[37].isdigit():
            writeNewRow(date, moduleId, 28, 1)
        if row[38].isdigit():
            writeNewRow(date, moduleId, 29, 1)
        # Frage 9
        if row[40].isdigit():
            writeNewRow(date, moduleId, 30, 1)
        if row[41].isdigit():
            writeNewRow(date, moduleId, 31, 1)
        if row[42].isdigit():
            writeNewRow(date, moduleId, 32, 1)
        if row[43].isdigit():
            writeNewRow(date, moduleId, 33, 1)
        # Frage 10
        if row[45].isdigit():
            writeNewRow(date, moduleId, 34, 1)
        if row[46].isdigit():
            writeNewRow(date, moduleId, 35, 1)
        if row[47].isdigit():
            writeNewRow(date, moduleId, 36, 1)
        if row[48].isdigit():
            writeNewRow(date, moduleId, 37, 1)
        # Frage 11
        if row[50].isdigit():
            writeNewRow(date, moduleId, 38, 1)
        if row[51].isdigit():
            writeNewRow(date, moduleId, 39, 1)
        if row[52].isdigit():
            writeNewRow(date, moduleId, 40, 1)
        if row[53].isdigit():
            writeNewRow(date, moduleId, 41, 1)
        # Frage 12
        if row[55].isdigit():
            writeNewRow(date, moduleId, 42, 1)
        if row[56].isdigit():
            writeNewRow(date, moduleId, 43, 1)
        if row[57].isdigit():
            writeNewRow(date, moduleId, 44, 1)
        if row[58].isdigit():
            writeNewRow(date, moduleId, 45, 1)
        # Frage 13
        if row[60].isdigit():
            writeNewRow(date, moduleId, 46, 1)
        if row[61].isdigit():
            writeNewRow(date, moduleId, 47, 1)
        if row[62].isdigit():
            writeNewRow(date, moduleId, 48, 1)
        if row[63].isdigit():
            writeNewRow(date, moduleId, 49, 1)
        # Frage 14
        if row[64].isdigit():
            if row[64] != 0:
                writeNewRow(date, moduleId, 50, row[64])
            
        # Frage 15
        if row[65].isdigit():
            if row[65] != 0:
                writeNewRow(date, moduleId, 51, row[65])

        # Frage 16
        if row[66].isdigit():
            if row[66] != 0:
                writeNewRow(date, moduleId, 52, row[66])

        print('Row: ' + str(row_coounter) + ' abgeschlossen')    
        row_coounter += 1

print("*******************************************************************************************************")
print(f'Fertig! \nEs wurden {row_coounter} Zeilen verarbeitet. \nEs wurden {entry_number} Einträge erstellt.')
print("*******************************************************************************************************")