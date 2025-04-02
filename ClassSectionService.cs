using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolManagement
{
    public class ClassSectionService
    {
        private readonly IClassSectionRepository _classRepository;

        public ClassSectionService(IClassSectionRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public async Task<List<ClassSection>> LoadClassesAsync(int page, int pageSize)
        {
            // Sanitize page and pageSize
            page = page < 1 ? 1 : page; // Default to page 1 if invalid
            pageSize = pageSize < 1 ? 10 : pageSize; // Default to 10 if invalid

            var classes = await _classRepository.GetClassesAsync(page, pageSize);

            // Update display properties for UI
            

            return classes;
        }

        public async Task<List<Teacher>> LoadTeachersAsync()
        {
            return await _classRepository.GetTeachersAsync();
        }

        public async Task<List<Subject>> LoadSubjectsAsync()
        {
            return await _classRepository.GetSubjectsAsync();
        }

        public async Task<bool> SaveClassAsync(ClassSection classSection, bool isAddMode)
        {
            return isAddMode
                ? await _classRepository.AddClassAsync(classSection)
                : await _classRepository.UpdateClassAsync(classSection);
        }

        public async Task DeleteClassAsync(string classId)
        {
            await _classRepository.DeleteClassAsync(classId);
        }

        public async Task<List<ClassSection>> SearchClassesAsync(string searchTerm)
        {
            return await _classRepository.GetClassesBySearchTermAsync(searchTerm);
        }

        public async Task<List<ClassSection>> GetAllClassesAsync()
        {
            return await _classRepository.GetAllClassesAsync();
        }
    }
}