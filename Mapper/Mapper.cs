﻿namespace cattoapi.Mapper
{
    using AutoMapper;
    using cattoapi.DTOS;
    using cattoapi.Models;
    using Microsoft.Identity.Client;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Account, AccountDTO> ();
            CreateMap<Post, PostDTO> ();
            CreateMap<Account, ProfileDTO> ();
            CreateMap<Comment, CommentDTO> ();
            CreateMap<Conversation, ConversationDTO>();
            CreateMap<Message, MessageDTO>();
        }
    }

}
