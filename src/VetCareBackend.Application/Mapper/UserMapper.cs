using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.Validations;
using VetCareBackend.Domain.Entities;
using VetCareBackend.Domain.Enums;
using VetCareBackend.Application.Exceptions;

namespace VetCareBackend.Application.Mapper
{
    public static class UserMapper
    {
        public static T ToEntity<T>(this SignUpRequest dto, string dtoRole, Guid id) where T : User, new()
        {
            Enum.TryParse<Role>(dtoRole, out Role role);
            SignUpValidator validation = new SignUpValidator();
            if (!validation.Validate(dto).IsValid)
            {
                throw new ValidationException(validation.Validate(dto).ToString("~"));
            }
            return new T
            {
                Id = id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Dni = dto.Dni,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Password = dto.Password,
                Role = role,
            };
        }

        public static T ToDto<T>(this User Entity) where T : UserResponse, new()
        {
            return new T
            {
                FirstName = Entity.FirstName,
                LastName = Entity.LastName,
                Dni = Entity.Dni,
                Email = Entity.Email,
                PhoneNumber = Entity.PhoneNumber,
            };
        }

        public static T ToEntityUpdate<T>(this T Entity, UserRequest request) where T : User
        {
            UserRequestValidation validation = new UserRequestValidation();
            if (!validation.Validate(request).IsValid)
            {
                throw new ValidationException(validation.Validate(request).ToString("~"));
            }
            Entity.FirstName = string.IsNullOrWhiteSpace(request.FirstName) ? Entity.FirstName : request.FirstName;
            Entity.LastName = string.IsNullOrWhiteSpace(request.LastName) ? Entity.LastName : request.LastName;
            Entity.Dni = string.IsNullOrWhiteSpace(request.Dni) ? Entity.Dni : request.Dni ;
            Entity.Email = string.IsNullOrWhiteSpace(request.Email) ? Entity.Email : request.Email ;
            Entity.PhoneNumber = string.IsNullOrWhiteSpace(request.PhoneNumber) ? Entity.PhoneNumber : request.PhoneNumber;
            return Entity;
        }
    }
}
