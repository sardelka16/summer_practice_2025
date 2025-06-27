namespace task02;

public class Student
{
    public required string Name { get; set; }
    public required string Faculty { get; set; }
    public required List<int> Grades { get; set; }
}
public class StudentService
{
    private readonly List<Student> _students;

    public StudentService(List<Student> students) => _students = students;

    // 1. Возвращает студентов указанного факультета
    public IEnumerable<Student> GetStudentsByFaculty(string faculty)
    {
        var studentsByFaculty = from stud in _students
                                where stud.Faculty == faculty
                                select stud;
        return studentsByFaculty;
    }


    // 2. Возвращает студентов со средним баллом >= minAverageGrade
    public IEnumerable<Student> GetStudentsWithMinAverageGrade(double minAverageGrade)
    {
        var studentsWithMinAverageGrade = from stud in _students
                                          where stud.Grades.Average() >= minAverageGrade
                                          select stud;
        return studentsWithMinAverageGrade;

    }

    // 3. Возвращает студентов, отсортированных по имени (A-Z)
    public IEnumerable<Student> GetStudentsOrderedByName()
    {
        var sortedStudents = from stud in _students
                             orderby stud.Name
                             select stud;
        return sortedStudents;
    }

    // 4. Группировка по факультету
    public ILookup<string, Student> GroupStudentsByFaculty()
    {
        return _students.ToLookup(stud => stud.Faculty);
    }

    // 5. Находит факультет с максимальным средним баллом
    public string GetFacultyWithHighestAverageGrade()
    {
        var averageGradeByFaculty = (from stud in _students
                                    group stud by stud.Faculty into facultyGroup
                                    let avgGrade = facultyGroup.SelectMany(s => s.Grades).Average()
                                    orderby avgGrade descending
                                    select facultyGroup.Key);
        
        return averageGradeByFaculty.FirstOrDefault() ?? "";
                                    
                                       
    }
}