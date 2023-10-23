using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Minesweeper.Models;
using Minesweeper.Service;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Minesweeper.Controllers
{
	[Route("/miner")]
	[ApiController]
	[EnableCors()]
	public class MinerSweeperControler : ControllerBase
	{
		private GameInfoResponse _gameInfoResponse;
		private FieldRepository _repository;
		private static int[,] _array;

        public MinerSweeperControler(GameInfoResponse gameInfoResponse, FieldRepository repository)
        {
            _gameInfoResponse = gameInfoResponse;
			_repository = repository;
        }

        [HttpPost("new")]
		public ActionResult<GameInfoResponse> StartNewGame(NewGameRequest request)
		{
			if (request.width > 30 || request.height > 30 || request.mines_count > request.width * request.height - 1)
				return BadRequest(new ErrorResponse("кол-во мин больше или равно кол-ву ячеек"));

			_gameInfoResponse.game_id = Random.Shared.Next(90).ToString();
			_gameInfoResponse.width = request.width;
			_gameInfoResponse.height = request.height;
			_gameInfoResponse.mines_count = request.mines_count;
			_gameInfoResponse.completed = false;
			_gameInfoResponse.field = _repository.CreateField(request.width, request.height);
			_array = _repository.GenerateMines(_repository.CreateFieldInt(request.width, request.height), _gameInfoResponse.mines_count, _gameInfoResponse.width, _gameInfoResponse.height);
			return Ok(_gameInfoResponse);
		}
		[HttpPost("turn")]
		public ActionResult<GameInfoResponse> TurnUser(GameTurnRequest gameTurnRequest)
		{			
			if (_gameInfoResponse.completed == true) return BadRequest(new ErrorResponse("игра завершена"));
			if (gameTurnRequest.game_id != _gameInfoResponse.game_id) return BadRequest(new ErrorResponse("ошибка"));

			if (_array[gameTurnRequest.row, gameTurnRequest.col] == 9)
			{
				_gameInfoResponse.field = _repository.ArrayProjection(_gameInfoResponse.field, _array);
				_gameInfoResponse.completed = true;				
				return Ok(_gameInfoResponse);
			}
			if (_array[gameTurnRequest.row, gameTurnRequest.col] == 0 || Math.Abs(_array[gameTurnRequest.row, gameTurnRequest.col]) == 10)
			{
				if (_gameInfoResponse.field[gameTurnRequest.row, gameTurnRequest.col].Contains("0"))
				{
					return BadRequest(new ErrorResponse("уже открытая ячейка"));
				}
				else
				{
					_repository.UncoverField(gameTurnRequest.row, gameTurnRequest.col, _array, _gameInfoResponse.field);
					if (_repository.isEqual(_gameInfoResponse))
					{
						_gameInfoResponse.completed = true;
						_gameInfoResponse.field = _repository.ArrayProjection(_gameInfoResponse.field, _array);
						for (int x = 0; x < _gameInfoResponse.field.GetLength(0); x++)
						{
							for (int y = 0; y < _gameInfoResponse.field.GetLength(1); y++)
							{
								if (_gameInfoResponse.field[x, y].Contains("X"))
								{
									_gameInfoResponse.field[x, y] = "M";
								}
							}
						}
					};
				}			
				return Ok(_gameInfoResponse);
			}
			if (_array[gameTurnRequest.row, gameTurnRequest.col] != 9)
			{
				_gameInfoResponse.field[gameTurnRequest.row, gameTurnRequest.col] = Math.Abs(_array[gameTurnRequest.row, gameTurnRequest.col]).ToString();
				if (_repository.isEqual(_gameInfoResponse))
				{
					_gameInfoResponse.completed = true;
					_gameInfoResponse.field = _repository.ArrayProjection(_gameInfoResponse.field, _array);
					for (int x = 0; x < _gameInfoResponse.field.GetLength(0); x++)
					{
						for (int y = 0; y < _gameInfoResponse.field.GetLength(1); y++)
						{
							if (_gameInfoResponse.field[x, y].Contains("X"))
							{
								_gameInfoResponse.field[x, y] = "M";
							}
						}
					}
				};
				return Ok(_gameInfoResponse);
			}

			return Ok(_gameInfoResponse);
		}
	}
}