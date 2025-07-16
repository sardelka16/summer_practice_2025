namespace task13;

using System;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;


public class Subject
{
  public string Name {get; set; }
  public int Grade {get; set; }
}

public class Student
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public List<Subject> Grades { get; set; }
}
public class StudentSerializator
{
    private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
    {
        WriteIndented = true, 
        IgnoreNullValues = true,
    };

    public static void StudentToJson(Student student)
    {
        Console.WriteLine("Демонстрация работы с System.Text.Json");
        try
        {
            string jsonString = JsonSerializer.Serialize(student, jsonOptions);

            string filePath = $"{student.LastName}_{student.FirstName}.json";
            File.WriteAllText(filePath, jsonString, Encoding.UTF8);

            string jsonFromFile = File.ReadAllText(filePath, Encoding.UTF8);;

            Student deserializedStudent = JsonSerializer.Deserialize<Student>(jsonFromFile, jsonOptions);

            ValidateStudent(deserializedStudent);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
    public static void ValidateStudent(Student student)
    {
        if (string.IsNullOrWhiteSpace(student.FirstName))
            throw new ArgumentException("Имя студента не может быть пустым");

        if (string.IsNullOrWhiteSpace(student.LastName))
            throw new ArgumentException("Фамилия студента не может быть пустой");

        if (student.BirthDate == default)
            throw new ArgumentException("Дата рождения должна быть указана");

        if (student.BirthDate > DateTime.Now)
            throw new ArgumentException("Дата рождения не может быть в будущем");

        if (student.Grades == null || student.Grades.Count == 0)
            throw new ArgumentException("Список оценок не может быть пустым");

        foreach (var subject in student.Grades)
        {
            if (string.IsNullOrWhiteSpace(subject.Name))
                throw new ArgumentException("Название предмета не может быть пустым");

            if (subject.Grade < 1 || subject.Grade > 5)
                throw new ArgumentException($"Оценка по предмету {subject.Name} должна быть от 1 до 5");
        }
    }
}
