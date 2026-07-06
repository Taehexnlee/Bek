using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionService.Dtos;
using AuctionService.Entities;
using AutoMapper;
using Contracts;

namespace AuctionService.RequestHelpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Auction , AuctionDto>().IncludeMembers(src => src.Item);
            CreateMap<Item, AuctionDto>();
            CreateMap<CreateAuctionDto, Auction>()
                .ForMember(dest => dest.Item, opt => opt.MapFrom(src => src));
            CreateMap<CreateAuctionDto, Item>();
            CreateMap<AuctionDto, AuctionCreated>();
            CreateMap<Auction, AuctionUpdated>()
                .IncludeMembers(src => src.Item)
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
            CreateMap<Item, AuctionUpdated>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            
        }
        
    }
}
