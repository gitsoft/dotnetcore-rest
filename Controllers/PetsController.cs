using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiSample.Models;

namespace WebApiSample.Controllers
{
    #region snippet_Inherit
    [Produces(MediaTypeNames.Application.Json)]
    [Route("[controller]")]
    public class PetsController : MyControllerBase
    #endregion
    {
        private static readonly List<Pet> _petsInMemoryStore = new List<Pet>();

        public PetsController()
        {
            if (_petsInMemoryStore.Count == 0)
            {
                _petsInMemoryStore.AddRange(new List<Pet> { 
                    new Pet
                    {
                        Breed = "Collie",
                        Id = 1,
                        Name = "Christoffer",
                        PetType = PetType.Dog
                    }, new Pet
                    {
                        Breed = "Tax",
                        Id = 2,
                        Name = "Magnus",
                        PetType = PetType.Dog
                    }, new Pet
                    {
                        Breed = "Schäfer",
                        Id = 3,
                        Name = "André",
                        PetType = PetType.Dog
                    }, new Pet
                    {
                        Breed = "Helig Birma",
                        Id = 4,
                        Name = "Minou",
                        PetType = PetType.Cat
                    }, new Pet
                    {
                        Breed = "Bengal",
                        Id = 5,
                        Name = "Ejub",
                        PetType = PetType.Cat
                    }
                });
            }
        }

        [HttpGet]
        public ActionResult<List<Pet>> GetAll() => _petsInMemoryStore;

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Pet> GetById(int id)
        {
            var pet = _petsInMemoryStore.FirstOrDefault(p => p.Id == id);

            #region snippet_ProblemDetailsStatusCode
            if (pet == null)
            {
                return NotFound();
            }
            #endregion

            return pet;
        }

        #region snippet_400And201
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Pet> Create(Pet pet)
        {
            pet.Id = _petsInMemoryStore.Any() ? 
                     _petsInMemoryStore.Max(p => p.Id) + 1 : 1;
            _petsInMemoryStore.Add(pet);

            return CreatedAtAction(nameof(GetById), new { id = pet.Id }, pet);
        }
        #endregion
    }
}
