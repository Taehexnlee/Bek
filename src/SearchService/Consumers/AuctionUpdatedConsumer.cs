using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers
{
    public class AuctionUpdatedConsumer : IConsumer<AuctionUpdated>
    {
        private readonly IMapper _mapper;

        public AuctionUpdatedConsumer(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<AuctionUpdated> context)
        {
            Console.WriteLine($"Consuming auction updated: {context.Message.Id}");

            var item = _mapper.Map<Item>(context.Message);

            var result = await DB.Update<Item>()
                .MatchID(item.ID)
                .ModifyOnly(x => new
                {
                    x.Make,
                    x.Model,
                    x.Color,
                    x.Year,
                    x.Mileage,
                    x.ImageUrl
                }, item)
                .ExecuteAsync();

            if (!result.IsAcknowledged || result.MatchedCount == 0)
            {
                throw new MessageException(typeof(AuctionUpdated), "Problem updating MongoDb");
            }
        }
    }
}
