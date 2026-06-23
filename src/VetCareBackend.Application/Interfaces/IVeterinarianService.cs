using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using VetCareBackend.Application.dtos.Requests;
using VetCareBackend.Application.dtos.Responses;
using VetCareBackend.Domain.Entities;

namespace VetCareBackend.Application.Interfaces
{
    public interface IVeterinarianService
    {
       Task<VeterinarianResponse> Create(VeterinarianRequest request);
       Task<List<VeterinarianResponse>> GetAll();
       Task<VeterinarianResponse> GetById(string Sub);
       Task<VeterinarianResponse> GetByEnrollment(string enrollment);
       Task Update(string Sub, VeterinarianUpdateRequest request);
       Task Delete(string Sub);
    }
}
