using Domain.DbContex;
using Domain.Entity.Settings;
using Domain.RequestModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Settings
{
    public class UserAddressBookService
    {
        private readonly ApplicationDbContext _context;

        public UserAddressBookService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all addresses
        public async Task<List<UserAddressBook>> GetAllAddressesAsync()
        {
            try
            {
                return await _context.UserAddressBooks.ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
           
        }
        public async Task<List<UserAddressBook>> GetAllAddressesByUserIdAsync(long userId)
        {
            try
            {
                return await _context.UserAddressBooks.Where(w=>w.UserID==userId).ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        // Get address by ID
        public async Task<UserAddressBook> GetAddressByIdAsync(int id)
        {
            return await _context.UserAddressBooks.FindAsync(id);
        }

        public async Task<UserAddressBook?> GetAddressByPhoneAsync(string phone)
        {
            return await _context.UserAddressBooks.FirstOrDefaultAsync(w => w.PhoneNumber == phone);
        }

        // Create a new address


        public async Task<UserAddressBook> CreateOrUpdateAddressAsync(UserAddressBook address)
        {
            
            if (address.AddressID > 0)
            {
             var existingAddress = await _context.UserAddressBooks.FindAsync(address.AddressID);

            if (existingAddress != null)
            {
                // Update existing address
                existingAddress.UserID = address.UserID;
                existingAddress.Address = address.Address;
                existingAddress.AddressType = address.AddressType;
                //existingAddress.FullName = address.FullName;
                
                existingAddress.City = address.City;
                existingAddress.State = address.State;
                existingAddress.PostalCode = address.PostalCode;
                existingAddress.Country = address.Country;
                existingAddress.PhoneNumber = address.PhoneNumber;
                existingAddress.IsDefault = address.IsDefault;
                
                _context.UserAddressBooks.Update(existingAddress);
            }
            }
           
            else
            {
                // Create new address
                await _context.UserAddressBooks.AddAsync(address);
            }

            await _context.SaveChangesAsync();
            return address;
        }


        // Delete an address
        public async Task<bool> DeleteAddressAsync(int id)
        {
            var address = await _context.UserAddressBooks.FindAsync(id);
            if (address == null)
            {
                return false; // Address not found
            }

            _context.UserAddressBooks.Remove(address);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
