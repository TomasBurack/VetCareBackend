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
       public VeterinarianResponse Create(VeterinarianRequest request);
       public List<VeterinarianResponse> GetAll();
       public VeterinarianResponse GetById(string Sub);
       public VeterinarianResponse GetByEnrollment(string enrollment);
       public void Update( string Sub, VeterinarianRequest request);
       public void Delete( string Sub );
    }
}
