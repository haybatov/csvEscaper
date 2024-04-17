## CSV Escaper for files with new lines in fields

One of the large 4 banks in Australia, ANZ, produces CSV files that has new lines in fields without escaping. This is a problem for most CSV parsers, including Excel. It is ironic that ANZ calls this format "Microsoft Excel (CSV)" and yet it is not compatible with Excel.

This is a simple CSV escaper for files with new lines in fields. It expects the data lines to start with a date (ANZ does put headers in their CSV files).

