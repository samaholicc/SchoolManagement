using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SchoolManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolManagement.Tests
{
    [TestClass]
    public class ClassSectionServiceTests
    {
        private Mock<IClassSectionRepository> _mockRepository;
        private ClassSectionService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<IClassSectionRepository>();
            _service = new ClassSectionService(_mockRepository.Object);
        }

        [TestMethod]
        public async Task LoadClassesAsync_ReturnsClasses_WhenClassesExist()
        {
            // Arrange
            var classes = new List<ClassSection>
            {
                new ClassSection
                {
                    ClassId = "C001",
                    SubjectId = 35,
                    SubjectName = "Chemistry",
                    TeacherId = "T001",
                    TeacherName = "Teacher1",
                    StartDate = new DateTime(2025, 3, 26),
                    FinishDate = new DateTime(2025, 4, 2),
                    Schedule = "jeudi, 27 mars 2025",
                    NumberOfStudents = 21
                }
            };
            _mockRepository.Setup(r => r.GetClassesAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(classes);

            // Act
            var result = await _service.LoadClassesAsync(1, 10);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("C001", result[0].ClassId);
            Assert.AreEqual("35 - Chemistry", result[0].SubjectDisplay);
            Assert.AreEqual("T001 - Teacher1", result[0].TeacherDisplay);
        }

        [TestMethod]
        public async Task LoadClassesAsync_ReturnsEmptyList_WhenNoClassesExist()
        {
            // Arrange
            var classes = new List<ClassSection>();
            _mockRepository.Setup(r => r.GetClassesAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(classes);

            // Act
            var result = await _service.LoadClassesAsync(1, 10);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task LoadClassesAsync_HandlesInvalidPageSize()
        {
            // Arrange
            var classes = new List<ClassSection>
            {
                new ClassSection { ClassId = "C001" }
            };
            _mockRepository.Setup(r => r.GetClassesAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(classes);

            // Act
            var result = await _service.LoadClassesAsync(-1, 0); // Invalid page and pageSize

            // Assert
            Assert.IsNotNull(result);
            _mockRepository.Verify(r => r.GetClassesAsync(It.Is<int>(p => p >= 1), It.Is<int>(s => s >= 1)), Times.Once());
        }

        [TestMethod]
        public async Task LoadTeachersAsync_ReturnsTeachers_WhenTeachersExist()
        {
            // Arrange
            var teachers = new List<Teacher>
            {
                new Teacher { Id = "T001", FullName = "Teacher1" }
            };
            _mockRepository.Setup(r => r.GetTeachersAsync()).ReturnsAsync(teachers);

            // Act
            var result = await _service.LoadTeachersAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("T001", result[0].Id);
            Assert.AreEqual("Teacher1", result[0].FullName);
        }

        [TestMethod]
        public async Task LoadTeachersAsync_ReturnsEmptyList_WhenNoTeachersExist()
        {
            // Arrange
            var teachers = new List<Teacher>();
            _mockRepository.Setup(r => r.GetTeachersAsync()).ReturnsAsync(teachers);

            // Act
            var result = await _service.LoadTeachersAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task LoadSubjectsAsync_ReturnsSubjects_WhenSubjectsExist()
        {
            // Arrange
            var subjects = new List<Subject>
            {
                new Subject { Id = 35, Name = "Chemistry" }
            };
            _mockRepository.Setup(r => r.GetSubjectsAsync()).ReturnsAsync(subjects);

            // Act
            var result = await _service.LoadSubjectsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(35, result[0].Id);
            Assert.AreEqual("Chemistry", result[0].Name);
        }

        [TestMethod]
        public async Task LoadSubjectsAsync_ReturnsEmptyList_WhenNoSubjectsExist()
        {
            // Arrange
            var subjects = new List<Subject>();
            _mockRepository.Setup(r => r.GetSubjectsAsync()).ReturnsAsync(subjects);

            // Act
            var result = await _service.LoadSubjectsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task SaveClassAsync_AddsClass_WhenIsAddMode()
        {
            // Arrange
            var classSection = new ClassSection
            {
                ClassId = "C001",
                SubjectId = 35,
                TeacherId = "T001",
                StartDate = new DateTime(2025, 3, 26),
                FinishDate = new DateTime(2025, 4, 2),
                Schedule = "jeudi, 27 mars 2025",
                NumberOfStudents = 21
            };
            _mockRepository.Setup(r => r.AddClassAsync(classSection)).ReturnsAsync(true);

            // Act
            var result = await _service.SaveClassAsync(classSection, true);

            // Assert
            Assert.IsTrue(result);
            _mockRepository.Verify(r => r.AddClassAsync(classSection), Times.Once());
        }

        [TestMethod]
        public async Task SaveClassAsync_ReturnsFalse_WhenAddFails()
        {
            // Arrange
            var classSection = new ClassSection
            {
                ClassId = "C001",
                SubjectId = 35,
                TeacherId = "T001",
                StartDate = new DateTime(2025, 3, 26),
                FinishDate = new DateTime(2025, 4, 2),
                Schedule = "jeudi, 27 mars 2025",
                NumberOfStudents = 21
            };
            _mockRepository.Setup(r => r.AddClassAsync(classSection)).ReturnsAsync(false);

            // Act
            var result = await _service.SaveClassAsync(classSection, true);

            // Assert
            Assert.IsFalse(result);
            _mockRepository.Verify(r => r.AddClassAsync(classSection), Times.Once());
        }

        [TestMethod]
        public async Task SaveClassAsync_UpdatesClass_WhenNotAddMode()
        {
            // Arrange
            var classSection = new ClassSection
            {
                ClassId = "C001",
                SubjectId = 35,
                TeacherId = "T001",
                StartDate = new DateTime(2025, 3, 26),
                FinishDate = new DateTime(2025, 4, 2),
                Schedule = "jeudi, 27 mars 2025",
                NumberOfStudents = 21
            };
            _mockRepository.Setup(r => r.UpdateClassAsync(classSection)).ReturnsAsync(true);

            // Act
            var result = await _service.SaveClassAsync(classSection, false);

            // Assert
            Assert.IsTrue(result);
            _mockRepository.Verify(r => r.UpdateClassAsync(classSection), Times.Once());
        }

        [TestMethod]
        public async Task SaveClassAsync_ReturnsFalse_WhenUpdateFails()
        {
            // Arrange
            var classSection = new ClassSection
            {
                ClassId = "C001",
                SubjectId = 35,
                TeacherId = "T001",
                StartDate = new DateTime(2025, 3, 26),
                FinishDate = new DateTime(2025, 4, 2),
                Schedule = "jeudi, 27 mars 2025",
                NumberOfStudents = 21
            };
            _mockRepository.Setup(r => r.UpdateClassAsync(classSection)).ReturnsAsync(false);

            // Act
            var result = await _service.SaveClassAsync(classSection, false);

            // Assert
            Assert.IsFalse(result);
            _mockRepository.Verify(r => r.UpdateClassAsync(classSection), Times.Once());
        }

        [TestMethod]
        public async Task DeleteClassAsync_CallsRepositoryDelete()
        {
            // Arrange
            var classId = "C001";
            _mockRepository.Setup(r => r.DeleteClassAsync(classId)).Returns(Task.CompletedTask);

            // Act
            await _service.DeleteClassAsync(classId);

            // Assert
            _mockRepository.Verify(r => r.DeleteClassAsync(classId), Times.Once());
        }

        [TestMethod]
        public async Task DeleteClassAsync_HandlesNullClassId()
        {
            // Arrange
            string classId = null;
            _mockRepository.Setup(r => r.DeleteClassAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

            // Act
            await _service.DeleteClassAsync(classId);

            // Assert
            _mockRepository.Verify(r => r.DeleteClassAsync(null), Times.Once());
        }

        [TestMethod]
        public async Task SearchClassesAsync_ReturnsFilteredClasses()
        {
            // Arrange
            var searchTerm = "C001";
            var classes = new List<ClassSection>
            {
                new ClassSection
                {
                    ClassId = "C001",
                    SubjectId = 35,
                    SubjectName = "Chemistry",
                    TeacherId = "T001",
                    TeacherName = "Teacher1",
                    StartDate = new DateTime(2025, 3, 26),
                    FinishDate = new DateTime(2025, 4, 2),
                    Schedule = "jeudi, 27 mars 2025",
                    NumberOfStudents = 21
                }
            };
            _mockRepository.Setup(r => r.GetClassesBySearchTermAsync(searchTerm)).ReturnsAsync(classes);

            // Act
            var result = await _service.SearchClassesAsync(searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("C001", result[0].ClassId);
        }

        [TestMethod]
        public async Task SearchClassesAsync_ReturnsEmptyList_WhenNoMatches()
        {
            // Arrange
            var searchTerm = "NonExistent";
            var classes = new List<ClassSection>();
            _mockRepository.Setup(r => r.GetClassesBySearchTermAsync(searchTerm)).ReturnsAsync(classes);

            // Act
            var result = await _service.SearchClassesAsync(searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetAllClassesAsync_ReturnsAllClasses()
        {
            // Arrange
            var classes = new List<ClassSection>
            {
                new ClassSection
                {
                    ClassId = "C001",
                    SubjectId = 35,
                    SubjectName = "Chemistry",
                    TeacherId = "T001",
                    TeacherName = "Teacher1",
                    StartDate = new DateTime(2025, 3, 26),
                    FinishDate = new DateTime(2025, 4, 2),
                    Schedule = "jeudi, 27 mars 2025",
                    NumberOfStudents = 21
                }
            };
            _mockRepository.Setup(r => r.GetAllClassesAsync()).ReturnsAsync(classes);

            // Act
            var result = await _service.GetAllClassesAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("C001", result[0].ClassId);
        }
    }
}