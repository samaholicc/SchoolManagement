using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SchoolManagement.ClassSectionManager;

namespace SchoolManagement
{
    public interface IClassSectionRepository
    {
        Task<List<ClassSection>> GetClassesAsync(int page, int pageSize);
        Task<List<ClassSection>> GetClassesBySearchTermAsync(string searchTerm);
        Task<List<Teacher>> GetTeachersAsync();
        Task<List<Subject>> GetSubjectsAsync();
        Task<bool> AddClassAsync(ClassSection classSection);
        Task<bool> UpdateClassAsync(ClassSection classSection);
        Task DeleteClassAsync(string classId);
        Task<List<ClassSection>> GetAllClassesAsync(); // For export

    

    }
}

