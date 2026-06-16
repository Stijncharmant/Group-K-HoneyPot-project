using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DomainModels;
using ZstdSharp.Unsafe;

namespace LogicLayer.Management
{
    public class PartManagement
    {
        private readonly PartDataAccess _partDataAccess;

        public PartManagement(PartDataAccess partDataAccess)
        {
            _partDataAccess = partDataAccess;
        }

        #region CREATE

        public void AddPart(int sku, string name, string description, decimal price, int quantity, short year, int categoryId, int manufacturerId)
        {
            Part part = new Part(sku, name, description, price, quantity, year, categoryId, manufacturerId);

            _partDataAccess.AddPart(part);
        }

        #endregion

        #region READ

        public List<Part> GetAllParts()
        {
            return _partDataAccess.GetAllParts();
        }

        public Part? GetPartById(int id)
        {
            return _partDataAccess.GetPartById(id);
        }
        #endregion

        #region UPDATE

        public void UpdatePart(Part part)
        {
            _partDataAccess.UpdatePart(part);
        }

        #endregion

        #region DELETE

        public void DeletePart(int id)
        {
            _partDataAccess.DeletePart(id);
        }

        #endregion
    }
}
