﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DevIO.Data.Repository;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
{
    protected readonly MeuDbContext Db;
    protected readonly DbSet<TEntity> DbSet;

    protected Repository(MeuDbContext db)
    {
        Db = db;
        DbSet = db.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate) => await DbSet.AsNoTracking().Where(predicate).ToListAsync();

    public virtual async Task<TEntity> ObterPorId(Guid id) => await DbSet.FindAsync(id);

    public virtual async Task<List<TEntity>> ObterTodos() => await DbSet.ToListAsync();

    public virtual async Task Adicionar(TEntity entity)
    {
        DbSet.Add(entity);
        await SaveChanges();
    }

    public virtual async Task Atualizar(TEntity entity)
    {
        DbSet.Update(entity);
        await SaveChanges();
    }

    public virtual async Task Remover(Guid id)
    {
        DbSet.Remove(new TEntity { ID = id });
        await SaveChanges();
    }

    public async Task<int> SaveChanges() => await Db.SaveChangesAsync();

    public void Dispose() => Db?.Dispose();
}