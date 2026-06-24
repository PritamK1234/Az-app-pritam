using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Az_app_pritam.Data;

namespace Az_app_pritam.Pages_Persons
{
    public class IndexModel : PageModel
    {
        private readonly Az_app_pritam.Data.AppDbContext _context;

        public IndexModel(Az_app_pritam.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<Person> Person { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Person = await _context.Persons.ToListAsync();
        }
    }
}
