using AdvertApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace AdvertApi.Services
{
    public class DynamoDBAdvertStorage : IAdvertStorageService
    {
        private readonly IMapper _mapper;

        public DynamoDBAdvertStorage(IMapper mapper)
        {
            this._mapper = mapper;
        }
        public async Task<string> Add(AdvertModel model)
        {
            AdvertDBModel advertDBModel = _mapper.Map<AdvertDBModel>(model);
            advertDBModel.Id = new Guid().ToString();
            advertDBModel.CreationDateTime = DateTime.UtcNow;
            advertDBModel.Status = AdvertStatus.Pending;

            using (var client = new AmazonDynamoDBClient())
            {
                using (var context = new DynamoDBContext (client))
                {
                     await context.SaveAsync(advertDBModel);                    
                }
            }
            return advertDBModel.Id;

        }

        public async Task Confirm(ConfirmAdvertModel model)
        {

            using (var client = new AmazonDynamoDBClient())
            {
                using (var context = new DynamoDBContext(client))
                {
                    var record = await context.LoadAsync<AdvertDBModel>(model.Id);//.SaveAsync(advertDBModel);
                        if(record == null)
                    {
                        throw new KeyNotFoundException($"A record with ID={model.Id} not found");
                    }
                        if (model.Status == AdvertStatus.Active)
                    {
                        record.Status = AdvertStatus.Active;
                        await context.SaveAsync(record);

                    }
                        else
                    {
                        await context.DeleteAsync(record);
                    }
                }
            }

        }
    }
}
