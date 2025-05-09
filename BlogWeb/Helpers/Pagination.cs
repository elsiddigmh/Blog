﻿namespace BlogWeb.Helpers
{
    public class Pagination
    {
        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize {  get; private set; }

        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }


        public Pagination()
        {
            
        }

        public Pagination(int totalItems, int page, int pageSize = 10)
        {
            int totalPages = (int)Math.Ceiling( (decimal)totalItems / (decimal)pageSize);
            int currentPage = page;
        }

    }
}
