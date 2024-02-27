using Bogus;
using Domain.Projecten;
using Domain.Users;
using Domain.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Fakers.User
{
    public static class UserFaker
    {
        private static int id = 1;

        public class Klant : Faker<Domain.Users.Klant>
        {

            private List<Domain.Users.Klant> _klanten = new();

            private static Klant _instance;
            public static Klant Instance
            {
                get
                {
                    if (_instance is null)
                    {
                        _instance = new Klant();
                    }
                    return _instance;
                }
            }



            public Klant()
            {
           //     RuleFor(e => e.Id, _ => id++);

                CustomInstantiator(f =>
                {   
                    int randomNumber = f.Random.Int(0, 2);
                    if (randomNumber == 0)
                    {
                        return new InterneKlant(
                            f.Person.LastName,
                            f.Person.FirstName,
                            GeneratePhoneNumber(),
                            f.Person.Email,
                            PasswordUtilities.HashPassword("Klant.1"),
                            f.PickRandom<Course>()
                        );
                    }
                    else
                    {
                        //create faker obj, for different values everytime. 
                        var _faker = new Faker();
                        var faker = new Faker();

                        return new ExterneKlant(
                            f.Person.LastName,
                            f.Person.FirstName,
                            GeneratePhoneNumber(),
                            f.Person.Email,
                            PasswordUtilities.HashPassword("Klant.1"),
                            f.Company.CompanyName(),
                            f.PickRandom<BedrijfType>(),
                            new Domain.Common.ContactDetails(GeneratePhoneNumber(), faker.Person.Email, faker.Person.FirstName, faker.Person.LastName),
                            new Domain.Common.ContactDetails(GeneratePhoneNumber(), _faker.Person.Email, _faker.Person.FirstName, _faker.Person.LastName)
                        );
                    }
                });

            }

            public override List<Domain.Users.Klant> Generate(int count, string ruleSets = null)
            {
                List<Domain.Users.Klant> output;

                if (_klanten.Count() < count)
                {
                    output = base.Generate(count, ruleSets);
                    output.ForEach(e => _klanten.Add(e));
                }
                else
                {
                    if (count == 1)
                    {
                        output = new List<Domain.Users.Klant>() { _klanten[RandomNumberGenerator.GetInt32(0, _klanten.Count())] };
                    }
                    else
                    {
                        output = _klanten.GetRange(0, count);
                    }
                }

                return output;
            }

        }

        public class Administrators : Faker<Administrator>
        {
            private static Administrators _instance = null;

            public static Administrators Instance
            {
                get
                {
                    if (_instance is null)
                    {
                        _instance = new Administrators();
                    }
                    return _instance;
                }
            }

            public Administrators()
            {
                //RuleFor(e => e.Id, _ => id++);
                CustomInstantiator(e => new Administrator(
                    e.Person.LastName,
                    e.Person.FirstName,
                    GeneratePhoneNumber(),
                    e.Person.Email,
                    PasswordUtilities.HashPassword("Admin.1"),
                    e.PickRandom<AdminRole>()
                    )); ;
            }

        }

        private static string GeneratePhoneNumber()
        {
            string output = "0497";


            for (int i = 0; i < 6; i++)
            {
                output += (new Random().Next(0, 10)).ToString();
            }

            return output;
        }




    }
}