using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Context;
using backend.Models;
using backend.Repository;
using Microsoft.Extensions.Logging;
using backend.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
   
    public class CardsController : ControllerBase
    {
        private readonly MeuDbContext _context;
        private readonly ICardsRepository _cardsRepository;
        private readonly ILogger _logger;

        public CardsController(MeuDbContext context, ILogger<LoginController> logger, ICardsRepository cardsRepository)
        {
            _context = context;
            _cardsRepository = cardsRepository;
            _logger = logger;
        }


//       5-  Os entrypoints da aplicação devem usar a porta 5000 e ser:
//(GET) http://0.0.0.0:5000/cards/
//(POST) http://0.0.0.0:5000/cards/
//(PUT) http://0.0.0.0:5000/cards/{id}
//(DELETE) http://0.0.0.0:5000/cards/{id}

        // GET: api/Cards
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Cards>>> Getcards()
        {
            return await _cardsRepository.ObterTodos();
        }

        // GET: api/Cards/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Cards>> GetCards(Guid id)
        {
            var card = await _cardsRepository.ObterPorId(id);
            return card;
        }

        // PUT: api/Cards/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        //7- Para alterar um card, o entrypoint deve receber um id pela URL
        //     e um card pelo corpo da requisição.

        public async Task<IActionResult> PutCards(Guid id, Cards card)
        {

            if (id != card.Id)
            {
                //7-   Caso o id não exista retorne 404
                return NotFound();
            }

            //7-  Na alteração todos os campos são alterados.
            //Caso inválido, retorne status 400.
            //    .Se tudo correu bem, 
            //     retorne o card alterado.

            try
            {
                await _cardsRepository.Atualizar(id, card);

                
                //13- escreva no console sempre que os entrypoints de alteração ou remoção forem usados, indicando o horário formatado como o datetime a seguir: 01 / 01 / 2021 13:45:00.

                  _logger.LogInformation(DateTime.Now + " - Card " + id + " - " + card.titulo +" - Alterado");

                //6- Ao inserir retorne o card completo incluindo o id atribuído com o statusCode 
                //    apropriado. 
                return Ok(card);          

          
        }
            catch (Exception)
            {
                //6-   Caso inválido, retorne status 400.
                return BadRequest(new
                {
                    success = false,
                    error = "Ocorreu um erro ao atualizar o card"
                });
            }

           
        }

        // POST: api/Cards
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Cards>> PostCards(Cards card)
        {
            try
            {
                await _cardsRepository.Adicionar(card);

                //6- Ao inserir retorne o card completo incluindo o id atribuído com o statusCode 
                //    apropriado. 

                return Ok(CreatedAtAction("GetCards", new { id = card.Id }, card).Value);

               
            }
            catch (Exception)
            {
                //6-   Caso inválido, retorne status 400.
                return BadRequest(new
                {
                    success = false,
                    error = "Ocorreu um erro ao inserir o card"
                });
            }      
        }

        // DELETE: api/Cards/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Cards>>> DeleteCards(Guid id)
        {

            //8- Para remover um card, o entrypoint deve receber um id pela URL. 
            //    Caso o id não exista retorne 404.
            //    Se a remoção for bem sucedida retorne a lista de cards.

            var cards = await _context.cards.FindAsync(id);
            if (cards == null)
            {
                return NotFound();
            }


            await _cardsRepository.Remover(id);


            //13- escreva no console sempre que os entrypoints de alteração ou remoção forem usados, indicando o horário formatado como o datetime a seguir: 01 / 01 / 2021 13:45:00.

            _logger.LogInformation(DateTime.Now + " - Card " + id + " - " + cards.titulo + " - Removido");

            return await _cardsRepository.ObterTodos();
        }

    }
}
