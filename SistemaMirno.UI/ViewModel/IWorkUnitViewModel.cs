﻿using System.Threading.Tasks;

namespace SistemaMirno.UI.ViewModel
{
    public interface IWorkUnitViewModel
    {
        Task LoadAsync(int productionAreaId);
    }
}