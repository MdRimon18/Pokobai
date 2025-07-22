using Domain.DbContex;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class OrderStageService
    {
        private readonly ApplicationDbContext _context;

        public OrderStageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderStage>> GetAllAsync()
        {
            return await _context.OrderStages
                .OrderBy(o => o.Id)
                .ToListAsync();
        }
    }

}
