using System;

namespace associet_backend
{
    public class CommonVeriables
    {
        public string GetPaginatedSQL(int startRow, int numberOfRows, string sql, string orderingClause)
        {
            // Ordering clause is mandatory!
            if (String.IsNullOrEmpty(orderingClause))
                throw new ArgumentNullException("orderingClause");

            // numberOfRows here is checked of disable building paginated/limited query
            // in case is not greater than 0. In this case we simply return the
            // query with its ordering clause appended to it. 
            // If ordering is not spe
            if (numberOfRows <= 0)
            {
                return String.Format("{0} {1}", sql, orderingClause);
            }
            // Extract the SELECT from the beginning.
            String partialSQL = sql.Remove(0, "SELECT ".Length);

            // Build the limited query...
            return String.Format(
                "SELECT * FROM ( SELECT ROW_NUMBER() OVER ({0}) AS rn, {1} ) AS SUB WHERE rn > {2} AND rn <= {3}",
                orderingClause,
                partialSQL,
                startRow.ToString(),
                (startRow + numberOfRows).ToString()
            );
        }

        public class ResponseMeta
        {
            public int current_page { get; set; }
            public int per_page { get; set; }
            public int last_page { get; set; }
            public int total { get; set; }
            public int current_page_record { get; set; }

        }
    }
}