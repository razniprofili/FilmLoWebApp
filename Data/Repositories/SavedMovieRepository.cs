﻿using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public class SavedMovieRepository : GenericRepository<SavedMovie>, ISavedMovieRepository
    {
        public SavedMovieRepository(DbContext context) : base(context)
        {

        }
    }
}
