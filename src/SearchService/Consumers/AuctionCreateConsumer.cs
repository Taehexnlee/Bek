using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers
{
    public class AuctionCreateConsumer : IConsumer<AuctionCreated>
    {
        private readonly IMapper _mapper;
        public AuctionCreateConsumer(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task Consume(ConsumeContext<AuctionCreated> context)
        {
            Console.WriteLine($"Auction Created: {context.Message.Id}, {context.Message.Make}, {context.Message.Model}, {context.Message.Year}, {context.Message.Color}, {context.Message.Mileage}, {context.Message.ImageUrl}");
            var item = _mapper.Map<Item>(context.Message);
            if(item.Model == "Foo") throw new ArgumentException("Model cannot be Foo");
            await item.SaveAsync();
        }
    }
}