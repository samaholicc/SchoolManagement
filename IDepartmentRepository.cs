using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SchoolManagement.DepartmentManager;

namespace SchoolManagement
{
   
        public interface IDepartmentRepository
        {
            Task<List<Department>> GetAllDepartmentsAsync();
            Task<bool> AddDepartmentAsync(Department department);
            Task<bool> UpdateDepartmentAsync(Department department);
            Task DeleteDepartmentAsync(string id); // Already string
        }
    
}
