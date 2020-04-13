using System;
using GamersHub.Services.Data.Parties;
using GamersHub.Web.ViewModels;
using GamersHub.Web.ViewModels.Administration.Parties;
using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Areas.Administration.Controllers
{
    public class PartiesController : AdministrationController
    {
        private const int PartiesPerPage = 14;

        private readonly IPartiesService partiesService;

        public PartiesController(IPartiesService partiesService)
        {
            this.partiesService = partiesService;
        }

        public IActionResult Index(int id = 1)
        {
            var parties = this.partiesService.GetAll<PartyAdministrationViewModel>(PartiesPerPage, (id - 1) * PartiesPerPage);

            var viewModel = new PartyAdministrationIndexViewModel { Parties = parties};

            var count = this.partiesService.GetCount();

            var pagination = new PaginationViewModel();

            pagination.PagesCount = (int) Math.Ceiling((double) count / PartiesPerPage);
            if (pagination.PagesCount == 0)
            {
                pagination.PagesCount = 1;
            }

            pagination.CurrentPage = id;

            viewModel.Pagination = pagination;


            return this.View(viewModel);
        }
    }
}