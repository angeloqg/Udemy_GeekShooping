using GeekShopping.Email.Messages;
using GeekShopping.Email.Model;
using GeekShopping.Email.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Email.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly DbContextOptions<MySqlContext> _context;

        public EmailRepository(DbContextOptions<MySqlContext> context)
        {
            _context = context;
        }

        public async Task<bool> LogEmail(UpdatePaymentResultMessage message)
        {
            try
            {
                EmailLog email = new()
                {
                    Email = message.Email,
                    SentDate = DateTime.Now,
                    Log = $"Order = {message.OrderId} has been created successfully!"
                };

                await using var _db = new MySqlContext(_context);

                _db.EmailLogs.Add(email);

                await _db.SaveChangesAsync();

                return await Task.FromResult(true);
            }
            catch (Exception)
            {
                return await Task.FromResult(false);
            }
        }
    }
}
