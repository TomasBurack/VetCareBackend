using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Domain.Entities;
using VetCareBackend.Domain.Enums;

namespace VetCareBackend.Application.Mapper
{
    public static class UserMapper
    {
        public static T ToEntity<T>(this UserRequest dto) where T : User, new()
        {
            Enum.TryParse<Role>(dto.Role, out Role role);

            return new T
            {
                Id = Guid.NewGuid(),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Dni = dto.Dni,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Password = dto.Password,
                Role = role,
            };
        }

    }
}
