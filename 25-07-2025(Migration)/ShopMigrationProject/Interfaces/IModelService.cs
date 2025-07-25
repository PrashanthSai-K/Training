using ChienVHShopOnline.Models;
using ChienVHShopOnline.Models.Dto;

namespace ChienVHShopOnline.Interfaces;

public interface IModelService
{
    Task<IEnumerable<Model>> GetModels(int page, int pageSize);
    Task<Model> GetModel(int id);
    Task<Model> CreateModel(ModelCreateDto modelDto);
    Task<Model> UpdateModel(int id, ModelUpdateDto modelDto);
    Task<Model> DeleteModel(int id);
}