
using System;                             // For general utilities like DateTime
using System.Collections.Generic;         // For List<T> and other collections
using System.IO;                          // For handling file I/O (e.g., JSON key file)
using GoogleSheetsWrapper;                // For the wrapper itself


class Spackler
{
    static void Main()
    {
        var string googleSheetsID = "1uUDWwGnRBfa-cOX7M14uqcuO9z6T3M_PfqF5juhru_4";
        var string userAccountEmail = "spackler@blakeassignmentspackler.iam.gserviceaccount.com"

        // Instantiate the wrapper with the necessary credentials
        var googleSheets = new GoogleSheets($"{googleSheetsID}", "Assignments", $"{userAccountEmail}");

        // Get the assignments
        // List<Assignment> assignmentList = Assignments.GetAssignments();

        // Prepare data to write to Google Sheets
        List<List<object>> sheetData = new List<List<object>>();

        // Add header row (optional)
        sheetData.Add(new List<object> { "Class Code", "Name", "URL", "Due Date" });

        // Populate data rows
        foreach (var assignment in assignmentList)
        {
            sheetData.Add(new List<object>
            {
                assignment.ClassCode,
                assignment.Name,
                assignment.Url,
                assignment.DueDate.ToString("MM/dd/yyyy") // Double click this in sheets to pop up a calendar
            });
        }

        // Write to Google Sheets
        googleSheets.WriteRows(sheetData);

        Console.WriteLine("Data written to Google Sheets successfully.");
    }
}





