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
        public static T ToEntity<T>(this UserRequest dto, string dtoRole, Guid id) where T : User, new()
        {
            Enum.TryParse<Role>(dtoRole, out Role role);
            UserRequestValidator validation = new UserRequestValidator();
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
    }
}
