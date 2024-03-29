﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.PlantsAPI.Models.Dto;
using Services.PlantsAPI.Repository;

namespace Services.PlantsAPI.Controllers
{
	[Route("api/plants")]
	[ApiController]
	public class PlantsAPIController : ControllerBase
	{
		private readonly ResponseDto _response;
		private readonly IPlantsRepository _plantsRepository;

		public PlantsAPIController(IPlantsRepository plantsRepository)
		{
			_response = new ResponseDto();
			_plantsRepository = plantsRepository;
		}


		[HttpGet]
		public async Task<object> GetPage([FromQuery] int page, [FromQuery] int pageSize)
		{
			try
			{
				PageResultDto result = await _plantsRepository.GetPage(page, pageSize);
				_response.Result = result;
		}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages.Add(ex.Message);
			}
			return _response;
		}

		[HttpGet]
		[Route("{id}")]
		public async Task<object> GetPlantById(int id)
		{
			try
			{
				PlantDto plant = await _plantsRepository.GetPlantById(id);
				_response.Result = plant;
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages.Add(ex.Message);
			}
			return _response;
		}

		[HttpGet]
		[Route("filter")]
		public async Task<object> GetFilteredPage([FromQuery] FilterDto filter, [FromQuery] int page, [FromQuery] int pageSize)
		{
			try
			{
				PageResultDto result = await _plantsRepository.GetFilteredPage(filter, page, pageSize);
				_response.Result = result;
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages.Add(ex.Message);
			}
			return _response;
		}

		[HttpGet]
		[Route("palette")]
		public async Task<object> GetPalette()
		{
			try
			{
				IEnumerable<ColorDto> result = await _plantsRepository.GetPalette();
				_response.Result = result;
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages.Add(ex.Message);
			}
			return _response;
		}


		//[Authorize(Roles = "Admin,Contributor")]
		[HttpPost]
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
				_response.ErrorMessages.Add(ex.Message);
				if (ex.InnerException != null)
				{
					_response.ErrorMessages.Add(ex.InnerException.Message);
				}
			}
			return _response;
		}

		//[Authorize(Roles = "Admin")]
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
				_response.ErrorMessages.Add(ex.Message);
			}
			return _response;
		}
	}
}
