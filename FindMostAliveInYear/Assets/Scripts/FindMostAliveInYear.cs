using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class Person
{
    public string FirstName;
    public string LastName;
    public DateTime BirthDate;
    public DateTime DeathDate;

    public Person()
    {
        FirstName = "Bob"; // TODO: Randomize
        LastName = "Dude"; // TODO: Randomize
        BirthDate = RandomDate();
        DeathDate = GetDeathDate();
    }

    DateTime RandomDate()
    {
        int randomYear = UnityEngine.Random.Range(1900, 2000);
        return new DateTime(randomYear, 1, 1);
    }

    DateTime RandomDate(DateTime date)
    {
        int randomYear = UnityEngine.Random.Range(date.Year, 2000);
        return new DateTime(randomYear, 1, 1);
    }

    DateTime GetDeathDate()
    {
        if(BirthDate.Year < 1950)
        {
            return default(DateTime);  // DateTime.MinValue
        }

        return RandomDate(BirthDate);
    }
}

public class CommonYear
{
    public int Year;
    public int Count;
}

public class FindMostAliveInYear : MonoBehaviour
{
    // Text Labels in-game
    public Text m_PeopleInList;
    public Text m_PeopleAlive;
    public Text m_CommonYears;
    public Text m_CommonYear;
    public Text m_CommonCount;

    void Start()
    {
        Execute();
    }

    // Called by Start() and by a button in the scene...
    public void Execute()
    {
        List<Person> people = InitializePeople();

        if(people == null)
        {
            Debug.Log("m_People is null");
            return;
        }

        m_PeopleInList.text = "People in list: " + people.Count;
        Debug.Log("People in list: " + people.Count);

        List<Person> peopleAlive = GetPeopleAlive(people);

        m_PeopleAlive.text = "People alive: " + peopleAlive.Count;
        Debug.Log("People alive: " + peopleAlive.Count);

        Dictionary<int, int> commonYears = GetCommonYears(peopleAlive);

        m_CommonYears.text = "Common years: " + commonYears.Count;
        Debug.Log("Common years: " + commonYears.Count);

        CommonYear commonYear = GetHighestYearCount(commonYears);

        m_CommonYear.text = "Common year: " + commonYear.Year;
        m_CommonCount.text = " Count: " + commonYear.Count;
        Debug.Log("Common year: " + commonYear.Year + " Count: " + commonYear.Count);
    }

    // Initalize the list of people with random birth/death dates
    public List<Person> InitializePeople()
    {
        print("Initailze people");

        List<Person> people = new List<Person>();

        for(int i = 0; i < 1000; i++)
        {
            Person p = new Person();
            people.Add(p);

            if(i < 20)
            {
                Debug.Log("person birthdate: " + p.BirthDate + " Death: " + p.DeathDate);
            }
        }

        return people;
    }

    public List<Person> GetPeopleAlive(List<Person> people)
    {
        IEnumerable<Person> alive = new List<Person>();

        alive = people.Where(x => x.DeathDate != DateTime.MinValue);

        return alive.ToList();
    }

    public Dictionary<int, int> GetCommonYears(List<Person> people)
    {
        Dictionary<int, int> commonYears = new Dictionary<int, int>();

        foreach(Person p in people)
        {
            int year = p.BirthDate.Year;

            if(commonYears.ContainsKey(year))
            {
                commonYears[p.BirthDate.Year] += 1;
            }
            else
            {
                commonYears.Add(year, 1);
            }
        }

        return commonYears;
    }

    public CommonYear GetHighestYearCount(Dictionary<int, int> years)
    {
        int year = 0;
        int count = 0;

        foreach(KeyValuePair<int, int> y in years)
        {
            // If there is a tie, then the first date wins.
            if(count < y.Value)
            {
                year = y.Key;
                count = y.Value;
            }
        }

        CommonYear highestYear = new CommonYear();

        highestYear.Year = year;
        highestYear.Count = count;

        return highestYear;
    }
}

