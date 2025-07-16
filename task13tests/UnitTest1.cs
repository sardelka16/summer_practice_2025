namespace task13tests;

using task13;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Xunit;
public class StudentSerializationTests
    {
        private readonly Student _testStudent = new Student
        {
            FirstName = "Андрей",
            LastName = "Леонтьев",
            BirthDate = new DateTime(2006, 6, 10),
            Grades = new List<Subject>
            {
                new Subject { Name = "Математика", Grade = 5 },
                new Subject { Name = "Физика", Grade = 4 }
            }
        };

        [Fact]
        public void SerializeDeserialize_ValidStudent_ReturnsEquivalentObject()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                IgnoreNullValues = true
            };

            var json = JsonSerializer.Serialize(_testStudent, options);
            var deserialized = JsonSerializer.Deserialize<Student>(json, options);

            Assert.Equal(_testStudent.FirstName, deserialized.FirstName);
            Assert.Equal(_testStudent.LastName, deserialized.LastName);
            Assert.Equal(_testStudent.BirthDate, deserialized.BirthDate);
            Assert.Equal(_testStudent.Grades.Count, deserialized.Grades.Count);
        }

        [Fact]
        public void ValidateStudent_ValidData_DoesNotThrowException()
        {
            StudentSerializator.ValidateStudent(_testStudent);
        }

        [Fact]
        public void ValidateStudent_EmptyFirstName_ThrowsArgumentException()
        {
            var invalidStudent = new Student
            {
                FirstName = "",
                LastName = "Леонтьев",
                BirthDate = new DateTime(2006, 6, 10),
                Grades = new List<Subject> { new Subject { Name = "Математика", Grade = 5 } }
            };
            Assert.Throws<ArgumentException>(() => StudentSerializator.ValidateStudent(invalidStudent));
        }

        [Fact]
        public void ValidateStudent_FutureBirthDate_ThrowsArgumentException()
        {
            var invalidStudent = new Student
            {
                FirstName = "Андрей",
                LastName = "Леонтьев",
                BirthDate = DateTime.Now.AddDays(1),
                Grades = new List<Subject> { new Subject { Name = "Математика", Grade = 5 } }
            };
            Assert.Throws<ArgumentException>(() => StudentSerializator.ValidateStudent(invalidStudent));
        }

        [Fact]
        public void ValidateStudent_InvalidGrade_ThrowsArgumentException()
        {
            var invalidStudent = new Student
            {
                FirstName = "Андрей",
                LastName = "Леонтьев",
                BirthDate = new DateTime(2006, 6, 10),
                Grades = new List<Subject> { new Subject { Name = "Математика", Grade = 6 } }
            };
            var ex = Assert.Throws<ArgumentException>(() => StudentSerializator.ValidateStudent(invalidStudent));
            Assert.Contains("должна быть от 1 до 5", ex.Message);
        }

        [Fact]
        public void StudentToJson_ValidStudent_CreatesFile()
        {
            var testFile = $"{_testStudent.LastName}_{_testStudent.FirstName}.json";
            if (File.Exists(testFile)) File.Delete(testFile);

            StudentSerializator.StudentToJson(_testStudent);
            Assert.True(File.Exists(testFile));
            File.Delete(testFile);
        }

        [Fact]
        public void DeserializedStudent_FromFile_IsValid()
        {
            var testFile = $"{_testStudent.LastName}_{_testStudent.FirstName}.json";
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                IgnoreNullValues = true
            };
            File.WriteAllText(testFile, JsonSerializer.Serialize(_testStudent, options));
            StudentSerializator.StudentToJson(_testStudent);
            var json = File.ReadAllText(testFile);
            var deserialized = JsonSerializer.Deserialize<Student>(json, options);

            Assert.Equal(_testStudent.FirstName, deserialized.FirstName);
            Assert.Equal(_testStudent.LastName, deserialized.LastName);
            
            File.Delete(testFile);
        }
    }
