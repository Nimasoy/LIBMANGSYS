using AutoMapper;
using Application.DTOs;
using Domain.Entities;
using Application.Commands;

namespace Application.Mapping
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t => t.Name).ToList()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<AddBookCommand, Book>();

            CreateMap<Lending, BorrowingHistoryDto>()
            .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
            .ForMember(dest => dest.BookAuthor, opt => opt.MapFrom(src => src.Book.Author));

            CreateMap<Lending, OverdueDto>()
           .ForMember(dest => dest.LendingId, opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
           .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
           .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email));

            CreateMap<Lending, UserOverdueDto>()
                .ForMember(dest => dest.LendingId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title));

        }
    }
}
