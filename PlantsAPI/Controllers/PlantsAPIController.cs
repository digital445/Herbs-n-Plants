using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.PlantsAPI.Models.Dto;
using Services.PlantsAPI.Repository;
using System.Runtime.InteropServices;

namespace Services.PlantsAPI.Controllers
{
	[Route("api/plants")]
	[ApiController]
	public class PlantsAPIController : ControllerBase
	{
		private ResponseDto _response;
		private IPlantsRepository _plantsRepository;

		public PlantsAPIController(IPlantsRepository plantsRepository)
		{
			_response = new ResponseDto();
			_plantsRepository = plantsRepository;
		}

		[HttpGet]
		public async Task<object> GetAll()
		{
			try
			{
				IEnumerable<PlantDto> plants = await _plantsRepository.GetAll();
				_response.Result = plants;
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string> { ex.Message };
			}
			return _response;
		}

		[HttpGet]
		[Route("pages")]
		public async Task<object> GetPage([FromQuery] int page, [FromQuery] int pageSize)
		{
			try
			{
				IEnumerable<PlantDto> plants = await _plantsRepository.GetPage(page, pageSize);
				_response.Result = plants;
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string> { ex.Message };
			}
			return _response;
		}

		[HttpGet]
		[Route("{id}")]
		public async Task<object> Get(int id)
		{
			try
			{
				PlantDto? plant = await _plantsRepository.GetPlantById(id);
				_response.Result = plant;
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string> { ex.Message };
			}
			return _response;
		}

		[HttpGet]
		[Route("filter")]
		public async Task<object> GetFiltered(string? name = null, int flowerColorCode = -1, bool? poisonous = null, bool? forHerbalTea = null, bool? pickingProhibited = null)
		{
			try
			{
				PlantDto filter = new PlantDto
				{
					Names = new List<PlantNameDto> { 
						new PlantNameDto { 
							Name = name
						} 
					},
					FlowerColorCode = flowerColorCode,
					Poisonous = poisonous,
					ForHerbalTea = forHerbalTea,
					PickingProhibited = pickingProhibited
				};
				IEnumerable<PlantDto> plants = await _plantsRepository.GetFiltered(filter);
				_response.Result = plants;
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string> { ex.Message };
			}
			return _response;
		}

		[Authorize(Roles = "Admin,Contributor")]
		[HttpPost]
		[HttpPut]
		public async Task<object> CreateUpdate([FromBody] PlantDto plant)
		{
			try
			{
				PlantDto resultPlant = await _plantsRepository.CreateUpdatePlant(plant);
				_response.Result = resultPlant;
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string> { ex.Message };
				if (ex.InnerException != null)
				{
					_response.ErrorMessages.Add(ex.InnerException.Message);
				}
			}
			return _response;
		}

		[Authorize(Roles = "Admin")]
		[HttpDelete]
		[Route("{id}")]
		public async Task<object> Delete(int id)
		{
			try
			{
				bool IsSuccess = await _plantsRepository.DeletePlant(id);
				_response.Result = IsSuccess;
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string> { ex.Message };
			}
			return _response;
		}
	}
}
