using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModels
{
    public class AuditLog
    {
        // private fields
        private readonly int _id;
        private readonly int _employeeId;
        private readonly string _actionType;
        private readonly DateTime _actionDate;
        private readonly string _oldValues;
        private readonly string _newValues;

        // constructor
        public AuditLog(int id, int employeeId, string actionType, DateTime actionDate, string oldValues, string newValues)
        {
            _id = id;
            _employeeId = employeeId;
            _actionType = actionType;
            _actionDate = actionDate;
            _oldValues = oldValues;
            _newValues = newValues;
        }

        // properties
        public int Id { get { return _id; } }
        public int EmployeeId { get { return _employeeId; } }
        public string ActionType { get { return _actionType; } }
        public DateTime ActionDate { get { return _actionDate; } }
        public string OldValues { get { return _oldValues; } }
        public string NewValues { get { return _newValues; } }

    }
}
