using AutoMapper;
using Contract.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Profiles
{
   public class CustomProfile : Profile
    {
        public CustomProfile()
        {
            // File mapping: DTO -> Entity
            CreateMap<FileDto, TaskAttachment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // File mapping: Entity -> DTO
            CreateMap<TaskAttachment, FileDto>();

            // Task mapping: DTO -> Entity
            CreateMap<TaskDTO, TaskItem>()
                .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Files))
                .ForMember(dest => dest.TaskComments, opt => opt.Ignore())
                .ForMember(dest => dest.AssignedToUser, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUser, opt => opt.Ignore());

            // Task mapping: Entity -> DTO
            CreateMap<TaskItem, TaskDTO>()
                .ForMember(dest => dest.Files, opt => opt.MapFrom(src => src.Attachments))
                .ForMember(dest => dest.TaskCommentId, opt => opt.MapFrom(src => src.TaskComments != null
                    ? src.TaskComments.Select(c => c.Id).ToList()
                    : null));
            CreateMap<TaskComment, TaskCommentDTO>().ReverseMap();
           
        }
        
    }
}
