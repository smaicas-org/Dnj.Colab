using AutoMapper;
using Dnj.Colab.Samples.CustomersRestFul.Dtos;
using Dnj.Colab.Samples.CustomersRestFul.Models;

namespace Dnj.Colab.Samples.CustomersRestFul.Mapper;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        //Post
        CreateMap<CustomerPostDto, Customer>(); // Origen - destino
        //Put
        CreateMap<CustomerPutDto, Customer>();
        //Get
        CreateMap<CustomerGetDto, Customer>();
    }
}