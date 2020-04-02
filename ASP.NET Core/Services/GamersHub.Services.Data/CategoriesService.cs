﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamersHub.Common;
using GamersHub.Data.Common.Repositories;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace GamersHub.Services.Data
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepository;

        public CategoriesService(IDeletableEntityRepository<Category> categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
        }

        public IEnumerable<T> GetAll<T>(int? count = null)
        {
            IQueryable<Category> query =
                this.categoriesRepository.All();
            if (count.HasValue)
            {
                query = query.Take(count.Value);
            }

            return query.To<T>().ToList();
        }

        public async Task<int> CreateAsync(string name, string description)
        {
            var alreadyExists = this.CheckIfExistsByName(name);

            if (alreadyExists)
            {
                return 0;
            }

            var category = new Category
            {
                Name = name,
                Description = description,
            };

            await this.categoriesRepository.AddAsync(category);
            await this.categoriesRepository.SaveChangesAsync();

            return category.Id;
        }

        public string GetNormalisedName(string name)
        {
            var categories = this.categoriesRepository.All().Select(x => x.Name).ToList();

            var categoryName = categories.FirstOrDefault(x => UrlParser.ParseToUrl(x) == name);

            return categoryName;
        }

        private bool CheckIfExistsByName(string name)
        {
            bool alreadyExists = false;
            var category = this.categoriesRepository.All().FirstOrDefault(x => x.Name == name);

            if (category != null)
            {
                alreadyExists = true;
            }

            return alreadyExists;
        }
    }
}