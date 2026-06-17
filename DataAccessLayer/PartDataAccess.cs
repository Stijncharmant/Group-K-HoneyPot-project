using DomainModels;

namespace DataAccessLayer
{
    public class PartDataAccess
    {
        private readonly AppDbContext _context;

        public PartDataAccess(AppDbContext context)
        {
            _context = context;
        }

        #region CREATE

        public void AddPart(Part part)
        {
            _context.Parts.Add(part);
            _context.SaveChanges();
        }

        #endregion


        #region READ

        public List<Part> GetAllParts()
        {
            return _context.Parts
                .Where(p => !p.IsArchived)
                .ToList();
        }

        public Part? GetPartById(int id)
        {
            return _context.Parts
                .FirstOrDefault(p => p.Id == id && !p.IsArchived);
        }

        #endregion

        #region UPDATE

        public void UpdatePart(Part updatedPart)
        {
            var existingPart = _context.Parts.FirstOrDefault(p => p.Id == updatedPart.Id);

            if (existingPart == null)
                throw new Exception("Part not found");

            _context.Entry(existingPart).CurrentValues.SetValues(updatedPart);

            _context.SaveChanges();
        }

        #endregion

        #region DELETE

        public void DeletePart(int id)
        {
            var part = _context.Parts.FirstOrDefault(p => p.Id == id);

            if (part == null)
                throw new Exception("Part not found");

            part.Archive();

            _context.SaveChanges();
        }

        #endregion
    }
}
