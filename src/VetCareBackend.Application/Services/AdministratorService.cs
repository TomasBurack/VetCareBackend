using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Application.Exceptions;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Application.Mapper;
using VetCareBackend.Application.Validations;
using VetCareBackend.Domain.Entities;
using VetCareBackend.Domain.Enums;


namespace VetCareBackend.Application.Services
{
    public class AdministratorService : IAdministratorService
    {
        private readonly IAdministratorRepository _AdminRep;
        private readonly IClientRepository _ClientRep;
        private readonly IVeterinarianRepository _VetRep;
        private readonly IPasswordHash _hash;
        private readonly ISysadminRepository _SysadminRep;
        public AdministratorService(IAdministratorRepository repository,IClientRepository ClientRep, IVeterinarianRepository VetRep, IPasswordHash hash, ISysadminRepository sysadmin)
        {
            _AdminRep = repository;
            _ClientRep = ClientRep;
            _VetRep = VetRep;
            _hash = hash;
            _SysadminRep = sysadmin;
        }

        public UserResponse Create(SignUpRequest request)
        {
            if (_AdminRep.FindEmail(request.Email) || _ClientRep.FindEmail(request.Email) || _VetRep.FindEmail(request.Email) || _SysadminRep.FindEmail(request.Email)) {
                throw new ConflictException($"The email {request.Email} is already in use");
            } else if (_AdminRep.FindDni(request.Dni) || _ClientRep.FindDni(request.Dni) || _VetRep.FindDni(request.Dni) || _SysadminRep.FindDni(request.Dni)) {
                throw new ConflictException($"The DNI {request.Dni} is already in use");
            } else if (_AdminRep.FindPN(request.PhoneNumber) || _ClientRep.FindPN(request.PhoneNumber) || _VetRep.FindPN(request.PhoneNumber) || _SysadminRep.FindPN(request.PhoneNumber))
            {
                throw new ConflictException($"The Phone Number {request.PhoneNumber} is already in use");
            }
            //pasar las validaciones del mapper aca
            SignUpValidator validation = new SignUpValidator();
            if (!validation.Validate(request).IsValid)
            {
                throw new ValidationException(validation.Validate(request).ToString("~"));
            }
            request.Password = _hash.Hash(request.Password);
            Guid id = Guid.NewGuid();
            string dtoRole = "Administrator";
            var admin = UserMapper.ToEntity<Administrator>(request, dtoRole, id);
            _AdminRep.Add(admin);
            return UserMapper.ToDto<UserResponse>(admin);
        }

        public UserResponse Get(string id)
        {
            bool isGuid = Guid.TryParse(id, out Guid Id);
            if (!isGuid)
            {
                throw new ValidationException("Invalid ID format");
            }
            var admin = _AdminRep.Get(Id);
            if (admin == null)
            {
                throw new NotFoundException("Administrator not found");
            }
            return UserMapper.ToDto<UserResponse>(admin);
        }

        public void Update(string id, UserRequest request)
        {
            bool isGuid = Guid.TryParse(id, out Guid Id);
            if (!isGuid)
            {
                throw new ValidationException("Invalid ID format");
            }
            if (_AdminRep.FindEmail(request.Email) || _ClientRep.FindEmail(request.Email) || _VetRep.FindEmail(request.Email) || _SysadminRep.FindEmail(request.Email))
            {
                throw new ConflictException($"The email {request.Email} is already in use");
            }
            else if (_AdminRep.FindDni(request.Dni) || _ClientRep.FindDni(request.Dni) || _VetRep.FindDni(request.Dni) || _SysadminRep.FindDni(request.Dni))
            {
                throw new ConflictException($"The DNI {request.Dni} is already in use");
            }
            else if (_AdminRep.FindPN(request.PhoneNumber) || _ClientRep.FindPN(request.PhoneNumber) || _VetRep.FindPN(request.PhoneNumber) || _SysadminRep.FindPN(request.PhoneNumber))
            {
                throw new ConflictException($"The Phone Number {request.PhoneNumber} is already in use");
            }
            var admin = _AdminRep.Get(Id);
            if (admin == null)
            {
                throw new NotFoundException("Administrator not found");
            }

            UserRequestValidation validation = new UserRequestValidation();
            if (!validation.Validate(request).IsValid)
            {
                throw new ValidationException(validation.Validate(request).ToString("~"));
            }
            admin = UserMapper.ToEntityUpdate<Administrator>(admin, request);
            _AdminRep.Update(admin);
        }

        public void Delete(string id)
        {
            bool isGuid = Guid.TryParse(id, out Guid Id);
            if (!isGuid)
            {
                 throw new ValidationException("Invalid ID format");
            }

            _AdminRep.Delete(Id);
        }

        public List<UserResponse> GetAll()
        {
            var list = _AdminRep.GetAll();
            return list.Select(adm => UserMapper.ToDto<UserResponse>(adm)).ToList();
        }

        public void AddRoleToUser(string Email, string RoleName, string enrollment)
        {
            string patron = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            bool emailVal = Regex.IsMatch(Email, patron);
            bool RoleParse = Enum.TryParse<Role>(RoleName, out Role role);
            if (!emailVal)
                throw new ValidationException("Invalid email");
            if (!RoleParse)
                throw new ValidationException("Invalid role");

            var client = _ClientRep.GetByEmail(Email);
            var vet = _VetRep.GetByEmail(Email);
            /*var admin = _AdminRep.GetByEmail(Email);
            var sysadmin = _SysadminRep.GetByEmail(Email);*/

            User user = (User)client ?? (User)vet/*??(User) admin ?? (User)sysadmin*/;

            if (user == null)
                throw new NotFoundException("User not found");

            

            if (role == Role.Client)
            {
                if (client != null)
                    throw new ConflictException("You already have this role");
                Client newClient = new Client();
                newClient.Id = Guid.NewGuid();
                newClient.Role = role;
                newClient.FirstName = user.FirstName;
                newClient.LastName = user.LastName;
                newClient.PhoneNumber = user.PhoneNumber;
                newClient.Dni = user.Dni;
                newClient.Email = user.Email;
                newClient.Password = user.Password;
                newClient.IsDeleted = user.IsDeleted;
                newClient.UpdateDate = user.UpdateDate;
                newClient.DeleteDate = user.DeleteDate;

                _ClientRep.Add(newClient);
            }
            else if (role == Role.Veterinarian)
            {
                if (vet != null)
                    throw new ConflictException("You already have this role");

                if (!enrollment.All(char.IsDigit) || enrollment.Length != 4)
                    throw new ValidationException("The enrollment is invalid");
                
                if (_VetRep.FindEnrollment(enrollment))
                    throw new ConflictException("The enrollment is already in use");

                Veterinarian newVet = new Veterinarian();
                newVet.Id = Guid.NewGuid();
                newVet.Role = role;
                newVet.FirstName = user.FirstName;
                newVet.LastName = user.LastName;
                newVet.PhoneNumber = user.PhoneNumber;
                newVet.Dni = user.Dni;
                newVet.Email = user.Email;
                newVet.Password = user.Password;
                newVet.IsDeleted = user.IsDeleted;
                newVet.UpdateDate = user.UpdateDate;
                newVet.DeleteDate = user.DeleteDate;
                newVet.Enrollment = enrollment;
                _VetRep.Add(newVet);
            }/*else if(role == Role.Administrator)
            {
                Administrator newAdmin = new Administrator();
                newAdmin.Id = Guid.NewGuid();
                newAdmin.Role = role;
                newAdmin.FirstName = user.FirstName;
                newAdmin.LastName = user.LastName;
                newAdmin.PhoneNumber = user.PhoneNumber;
                newAdmin.Dni = user.Dni;
                newAdmin.Email = user.Email;
                newAdmin.Password = user.Password;
                newAdmin.IsDeleted = user.IsDeleted;
                newAdmin.UpdateDate = user.UpdateDate;
                newAdmin.DeleteDate = user.DeleteDate;

                _AdminRep.Add(newAdmin);
            }
            else if (role == Role.SysAdmin)
            {
                Sysadmin newSysadmin = new Sysadmin();
                newSysadmin.Id = Guid.NewGuid();
                newSysadmin.Role = role;
                newSysadmin.FirstName = user.FirstName;
                newSysadmin.LastName = user.LastName;
                newSysadmin.PhoneNumber = user.PhoneNumber;
                newSysadmin.Dni = user.Dni;
                newSysadmin.Email = user.Email;
                newSysadmin.Password = user.Password;
                newSysadmin.IsDeleted = user.IsDeleted;
                newSysadmin.UpdateDate = user.UpdateDate;
                newSysadmin.DeleteDate = user.DeleteDate;

                _SysadminRep.Add(newSysadmin);
            }*/
        }
    }
}
