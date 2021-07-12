using System.Collections.Generic;
using AutoMapper;
using CustomerOrdersService.Dtos.Create;
using CustomerOrdersService.Dtos.Read;
using CustomerOrdersService.Models;

namespace CustomerOrdersService.Infrastructure
{
    public class CustomerOrdersMappingProfile : Profile
    {
        public CustomerOrdersMappingProfile()
        {
            CreateMap<Customer, ReadCustomerOrdersDto>();
            CreateMap<List<Order>, List<ReadOrderDto>>();
        }

        public override string ProfileName
        {
            get { return this.GetType().Name; }
        }
    }

    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            CreateMap<Customer, ReadCustomerDto>();
        }

        public override string ProfileName
        {
            get { return this.GetType().Name; }
        }
    }

    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<Order, ReadOrderDto>();
        }

        public override string ProfileName
        {
            get { return this.GetType().Name; }
        }
    }

    public class OrderAllFieldsMappingProfile : Profile
    {
        public OrderAllFieldsMappingProfile()
        {
            CreateMap<Order, ReadOrderAllFieldsDto>();
        }

        public override string ProfileName
        {
            get { return this.GetType().Name; }
        }
    }

    public class CreateOrderMappingProfile : Profile
    {
        public CreateOrderMappingProfile()
        {
            CreateMap<CreateOrderDto, Order>();
        }

        public override string ProfileName
        {
            get { return this.GetType().Name; }
        }
    }

    public class CreateCustomerMappingProfile : Profile
    {
        public CreateCustomerMappingProfile()
        {
            CreateMap<CreateCustomerDto, Customer>();
        }

        public override string ProfileName
        {
            get { return this.GetType().Name; }
        }
    }
}
