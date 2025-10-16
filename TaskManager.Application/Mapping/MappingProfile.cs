using AutoMapper;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Project, ProjectDto>().ReverseMap();
        CreateMap<TaskItem, TaskItemDto>().ReverseMap();
    }
}