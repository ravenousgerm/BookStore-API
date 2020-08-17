﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStore_API.Conracts;
using BookStore_API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookStore_API.Controllers
{
	/// <summary>
	/// Endpoint used to interact witht he Authors in the book store's database
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public class AuthorsController : ControllerBase
	{
		private readonly IAuthorRepository _authorRepository;
		private readonly ILoggerService _logger;
		private readonly IMapper _mapper;

		public AuthorsController(IAuthorRepository authorRepository, ILoggerService logger, IMapper mapper)
		{
			_authorRepository = authorRepository;
			_logger = logger;
		}

		/// <summary>
		/// Get all authors
		/// </summary>
		/// <returns>List of authors</returns>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetAuthoros()
		{
			try
			{
				_logger.LogInfo("Attempted get all authors");
				var authors = await _authorRepository.FindAll();
				var response = _mapper.Map<IList<AuthorDTO>>(authors);
				_logger.LogInfo("Successfully got all authors");

				return Ok(response);
			}
			catch (Exception ex)
			{
				return HandleInternalServericeError(ex);
			}
		}

		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetAuthorById(int id)
		{
			try
			{
				_logger.LogInfo($"Attempted get author with Id:  {id}");
				var author = await _authorRepository.FindById(id);

				if (author == null)
				{
					_logger.LogWarn($"Author with Id:{id} not found");
					return NotFound();
				}

				var response = _mapper.Map<IList<AuthorDTO>>(author);
				_logger.LogInfo($"Successfully got author with Id: {id}");

				return Ok(response);
			}
			catch (Exception ex)
			{
				return HandleInternalServericeError(ex);
			}
		}

		private ObjectResult HandleInternalServericeError(Exception ex)
		{
			_logger.LogError($"{ex.Message} ~ {ex.InnerException}");
			return StatusCode(500, "Something when wrong.  Please contact the administrator");
		}
	}
}
